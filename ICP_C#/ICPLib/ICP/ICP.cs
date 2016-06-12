

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Windows.Media;
using System.Diagnostics;
using OpenTK;
using OpenTKLib;

namespace ICPLib
{


    public partial class IterativeClosestPointTransform
    {
        //int NumberPointsSolution = 100;
        public static List<Vector3d> NormalsSource;
        public static List<Vector3d> NormalsTarget;
        

        int MaxNumberSolutions = 10;
        public int NumberOfStartTrialPoints = 100000;


        public static ICP_VersionUsed ICPVersion = ICP_VersionUsed.Scaling_Umeyama;

        public static bool SimulatedAnnealing = false;
        public static bool NormalsCheck = false;
        public static bool FixedTestPoints = false;
        public static int MaximumNumberOfIterations = 100;
        public static bool ResetVertexToOrigin = true;
        public static bool DistanceOptimization = false;

        private static IterativeClosestPointTransform instance;
       

        public Matrix4d Matrix;
        public int NumberOfIterations;

        private List<Vertex> pointsTransformed;

        //int CheckMeanDistance;
        public double MeanDistance;


        double MaximumMeanDistance;
        double ThresholdOutlier = 10;


        public LandmarkTransform LandmarkTransform;
        List<ICPSolution> solutionList;
    

        public List<Vertex> PSource;
        public List<Vertex> PTarget;

        public static void Reset()
        {
            ICPVersion = ICP_VersionUsed.Scaling_Umeyama;
            SimulatedAnnealing = false;
            NormalsCheck = false;
            FixedTestPoints = false;
            MaximumNumberOfIterations = 100;
            ResetVertexToOrigin = true;
            IterativeClosestPointTransform.DistanceOptimization = false;

        }
        public IterativeClosestPointTransform()//:base(PointerUtils.GetIntPtr(new double[3]), true, true)
        {
            this.PSource = null;
            this.PTarget = null;

            this.LandmarkTransform = new LandmarkTransform();



            this.MaximumMeanDistance = 1.0E-3;

            this.NumberOfIterations = 0;
            this.MeanDistance = 0.0;

        }

        public List<Vertex> PerformICP(List<Vertex> myPointsTarget, List<Vertex> mypointsSource)
        {
            this.PTarget = myPointsTarget;
            this.PSource = mypointsSource;


            if (ICPVersion == ICP_VersionUsed.UsingStitchData)
                return PerformICP_Stitching();
            else if (ICPVersion == ICP_VersionUsed.RandomPoints)
                return PerformICP();
            else
                return PerformICP();
        }
        public static IterativeClosestPointTransform Instance
        {
            get
            {
                if (instance == null)
                    instance = new IterativeClosestPointTransform();
                return instance;

            }
        }
        public List<Vertex> PTransformed
        {
            get
            {
                return pointsTransformed;
            }
            set
            {
                pointsTransformed = value;
            }
        }



        public void Inverse()
        {
            List<Vertex> tmp1 = this.PSource;
            this.PSource = this.PTarget;
            this.PTarget = tmp1;
            //this.Modified();
        }

        private KDTreeVertex Helper_CreateTree(List<Vertex> pointsTarget)
        {
            KDTreeVertex kdTree = new KDTreeVertex();
            if (!FixedTestPoints)
            {
                if (KDTreeVertex.KDTreeMode == KDTreeMode.Stark)
                    kdTree.BuildKDTree_Stark(pointsTarget);
                else if (KDTreeVertex.KDTreeMode == KDTreeMode.Rednaxala)
                    kdTree.BuildKDTree_Rednaxela(pointsTarget);
                //else if(KDTreeMode == KDTreeMode.BruteForce)

            }
            return kdTree;
        }
        private bool Helper_FindNeighbours(ref List<Vertex> pointsTarget, ref List<Vertex> pointsSource, KDTreeVertex kdTree, int keepOnlyPoints)
        {

            if (!FixedTestPoints)
            {
                if (KDTreeVertex.KDTreeMode == KDTreeMode.Stark)
                    pointsTarget = kdTree.FindNearest_Stark(pointsSource, pointsTarget);
                else if (KDTreeVertex.KDTreeMode == KDTreeMode.Rednaxala)
                    pointsTarget = kdTree.FindNearest_Rednaxela(pointsSource, pointsTarget, keepOnlyPoints);
                else if (KDTreeVertex.KDTreeMode == KDTreeMode.BruteForce)
                    pointsTarget = kdTree.FindNearest_BruteForce(pointsSource, pointsTarget);

                if(NormalsCheck)
                {
                    //adjust normals - because of Search, the number of PointTarget my be different

                    int pointsRemoved = 0;
                    for(int i = pointsTarget.Count -1; i >=0 ; i--)
                    {
                        Vector3d vT = pointsTarget[i].Vector;
                        Vector3d vS = pointsSource[i].Vector;
                        //double angle = Vector3d.CalculateAngle(vT, vS);
                        int indexVec = pointsTarget[i].IndexInModel;
                        Vector3d vTNormal = NormalsTarget[pointsTarget[i].IndexInModel];
                        //Vector3d vTNormal = NormalsTarget[i];
                        Vector3d vSNormal = NormalsSource[i];

                        double angle = vTNormal.AngleInDegrees(vSNormal);
                        //double angle = vT.AngleInDegrees(vS);
                        if(Math.Abs(angle) > 30)
                        {
                            pointsTarget.RemoveAt(i);
                            pointsSource.RemoveAt(i);
                            pointsRemoved++;

                        }
                    }
                    Debug.WriteLine("--NormalCheck: Removed a total of: " + pointsRemoved.ToString());

                }

                if (pointsTarget.Count != pointsSource.Count)
                {
                    MessageBox.Show("Error finding neighbours, found " + pointsTarget.Count.ToString() + " out of " + pointsSource.Count.ToString());
                    return false;
                }
            }
            else
            {
                //adjust number of points - for the case if there are outliers
                int min = pointsSource.Count;
                if (pointsTarget.Count < min)
                {
                    min = pointsTarget.Count;
                    pointsSource.RemoveRange(pointsTarget.Count, pointsSource.Count - min);

                }
                else
                {
                    pointsTarget.RemoveRange(pointsSource.Count, pointsTarget.Count - min);
                }

            }
            return true;

        }
        private static double TransformPoints(ref List<Vertex> myPointsTransformed, List<Vertex> pointsTarget, List<Vertex> pointsSource, Matrix4d myMatrix)
        {
            myPointsTransformed = MathUtils.TransformPoints(pointsSource, myMatrix);
            double totaldist = PointUtils.CalculateTotalDistance(pointsTarget, myPointsTransformed);
            double meanDistance = totaldist / Convert.ToDouble(pointsTarget.Count);
            return meanDistance;

        }
        private Matrix4d Helper_FindTransformationMatrix(List<Vertex> pointsTarget, List<Vertex> pointsSource)
        {
            Matrix4d myMatrix;

            if (ICPVersion == ICP_VersionUsed.Horn)
            {
                MatrixUtilsNew.FindTransformationMatrix(Vertices.ConvertToVector3dList(pointsSource), Vertices.ConvertToVector3dList(pointsTarget), this.LandmarkTransform);
                myMatrix = LandmarkTransform.Matrix;
             
            }
            else
            {

                myMatrix = SVD.FindTransformationMatrix(Vertices.ConvertToVector3dList(pointsTarget), Vertices.ConvertToVector3dList(pointsSource), ICPVersion);
                
            }
            return myMatrix;

           

        }
        private void Helper_SetNewInterationSets(ref List<Vertex> pointsTarget, ref List<Vertex> pointsSource, List<Vertex> PT, List<Vertex> PS)
        {
            List<Vertex> myPointsTransformed = MathUtils.TransformPoints(PS, Matrix);
            pointsSource = myPointsTransformed;
            pointsTarget = Vertices.CopyVertices(PT);
       
        }
        /// <summary>
        /// A single ICP Iteration
        /// </summary>
        /// <param name="pointsTarget"></param>
        /// <param name="pointsSource"></param>
        /// <param name="PT"></param>
        /// <param name="PS"></param>
        /// <param name="kdTree"></param>
        /// <returns></returns>
        private bool Helper_ICP_Iteration(ref List<Vertex> pointsTarget, ref List<Vertex> pointsSource, List<Vertex> PT, List<Vertex> PS, KDTreeVertex kdTree, int keepOnlyPoints)
        {
            if (!Helper_FindNeighbours(ref pointsTarget, ref pointsSource, kdTree, keepOnlyPoints))
                return true;

            Matrix4d myMatrix = Helper_FindTransformationMatrix(pointsTarget, pointsSource);
            List<Vertex> myPointsTransformed = MathUtils.TransformPoints(pointsSource, myMatrix);

            if (SimulatedAnnealing)
            {
                this.Matrix = myMatrix;

                double totaldist = PointUtils.CalculateTotalDistance(pointsTarget, myPointsTransformed);
                this.MeanDistance = totaldist / Convert.ToDouble(pointsTarget.Count);

                //new set:
                pointsSource = myPointsTransformed;
                pointsTarget = Vertices.CopyVertices(PT);

               
                
            }
            else
            {
                Matrix4d.Mult(ref myMatrix, ref this.Matrix, out this.Matrix);
                double totaldist = PointUtils.CalculateTotalDistance(pointsTarget, myPointsTransformed);
                this.MeanDistance = totaldist / Convert.ToDouble(pointsTarget.Count);
                //Debug.WriteLine("--------------Iteration: " + iter.ToString() + " : Mean Distance: " + MeanDistance.ToString("0.00000000000"));

                if (MeanDistance < this.MaximumMeanDistance) //< Math.Abs(MeanDistance - oldMeanDistance) < this.MaximumMeanDistance)
                    return true;

                Helper_SetNewInterationSets(ref pointsTarget, ref pointsSource, PT, PS);
            }
            return false;

        }
        private bool Helper_ICP_Iteration_SA(List<Vertex> PT, List<Vertex> PS, KDTreeVertex kdTree, int keepOnlyPoints)
        {
            try
            {

                //first iteration
                if (solutionList == null)
                {
                    solutionList = new List<ICPSolution>();


                    if (NumberOfStartTrialPoints > PS.Count)
                        NumberOfStartTrialPoints = PS.Count;
                    if (NumberOfStartTrialPoints == PS.Count)
                        NumberOfStartTrialPoints = PS.Count * 80/100;
                    if (NumberOfStartTrialPoints < 3)
                        NumberOfStartTrialPoints = 3;



                    for (int i = 0; i < MaxNumberSolutions; i++)
                    {
                        ICPSolution myTrial = ICPSolution.SetRandomIndices(NumberOfStartTrialPoints, PS.Count, solutionList);
                        
                        if (myTrial != null)
                        {
                            myTrial.PointsSource = RandomUtils.ExtractPoints(PS, myTrial.RandomIndices);
                            solutionList.Add(myTrial);
                        }
                    }
                    ////test....
                    ////maxNumberSolutions = 1;
                    //ICPSolution myTrial1 = new ICPSolution();
                    //for (int i = 0; i < NumberPointsSolution; i++)
                    //{
                    //    myTrial1.RandomIndices.Add(i);
                    //}
                    //myTrial1.PointsSource = RandomUtils.ExtractPoints(PS, myTrial1.RandomIndices);
                    //solutionList[0] = myTrial1;


                }


                for (int i = 0; i < solutionList.Count; i++)
                {
                    List<Vertex> transformedPoints = null;

                    ICPSolution myTrial = solutionList[i];
                    Helper_ICP_Iteration(ref myTrial.PointsTarget, ref myTrial.PointsSource, PT, PS, kdTree, keepOnlyPoints);
                    myTrial.Matrix = Matrix4d.Mult(myTrial.Matrix, this.Matrix);
                    myTrial.MeanDistanceSubset = this.MeanDistance;
                                      
                    myTrial.MeanDistance = TransformPoints(ref transformedPoints, PT, PS, myTrial.Matrix);

                   // solutionList[i] = myTrial;

                }
                if (solutionList.Count > 0)
                {
                    solutionList.Sort(new ICPSolutionComparer());
                    RemoveSolutionIfMatrixContainsNaN(solutionList);
                    if (solutionList.Count == 0)
                        System.Windows.Forms.MessageBox.Show("No solution could be found !");
                    
                    this.Matrix = solutionList[0].Matrix;
                    this.MeanDistance = solutionList[0].MeanDistance;

                    if (solutionList[0].MeanDistance < this.MaximumMeanDistance)
                    {
                        return true;
                    }
                   
                    
                }
                
            }
            catch (Exception err)
            {
                MessageBox.Show("Error in Helper_ICP_Iteration_SA: " + err.Message);
                return false;
            }

            return false;


        }
        private void CalculateNormals(List<Vertex> pointsSource, List<Vertex> pointsTarget)
        {
            Model3D myModelTarget = new Model3D("Target");
            myModelTarget.VertexList = pointsTarget;
            myModelTarget.CalculateNormals_Triangulation();

            Model3D myModelSource = new Model3D("Source");
            myModelSource.VertexList = pointsSource;
            myModelSource.CalculateNormals_Triangulation();

            NormalsTarget = myModelTarget.Normals;
            NormalsSource = myModelSource.Normals;
        }
        public List<Vertex> PerformICP()
        {
            List<Vertex> PT = Vertices.CopyVertices(PTarget);
            List<Vertex> PS = Vertices.CopyVertices(PSource);
            Vertex pSOrigin = null;
            Vertex pTOrigin = null;

            if (ResetVertexToOrigin)
            {
                pTOrigin = Vertices.ResetVertexToOrigin(PT);
                pSOrigin = Vertices.ResetVertexToOrigin(PS);
            }

            int keepOnlyPoints = 0;
            if(DistanceOptimization)
                keepOnlyPoints = 3;
            int iter = 0;
            try
            {
                if (!CheckSourceTarget(PT, PS))
                    return null;
                
                List<Vertex> pointsTarget = Vertices.CopyVertices(PT);
                List<Vertex> pointsSource = Vertices.CopyVertices(PS);
                
                this.Matrix = Matrix4d.Identity;
                double oldMeanDistance = 0;
                
                KDTreeVertex kdTreee = Helper_CreateTree(pointsTarget);
                              
                for (iter = 0; iter < MaximumNumberOfIterations; iter++)
                {
                    if(NormalsCheck)
                    {

                        CalculateNormals(pointsSource, pointsTarget);

                    }
                    if (SimulatedAnnealing)
                    {
                        if (Helper_ICP_Iteration_SA(PT, PS, kdTreee, keepOnlyPoints))
                            break;
                    }
                    else
                    {
                        if (Helper_ICP_Iteration(ref pointsTarget, ref pointsSource, PT, PS, kdTreee, keepOnlyPoints))
                            break;                        
                    }
                    oldMeanDistance = MeanDistance;
                    Debug.WriteLine("--------------Iteration: " + iter.ToString() + " : Mean Distance: " + MeanDistance.ToString("0.00000000000"));

                }

                Debug.WriteLine("--------****** Solution of ICP after : " + iter.ToString() + " iterations, and Mean Distance: " + MeanDistance.ToString("0.00000000000"));

                PTransformed = MathUtils.TransformPoints(PS, Matrix);
                //re-reset vector 
                if (ResetVertexToOrigin)
                {
                    Vertices.AddVector(PTransformed, pTOrigin);
                    //Vertices.AddVector(PSource, pSOrigin);

                }
               
                DebugWriteUtils.WriteTestOutputVertex("Solution of ICP", Matrix, this.PSource, PTransformed, PTarget);

                return PTransformed;
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in Update ICP at iteration: " + iter.ToString() + " : " + err.Message);
                return null;

            }

        }
        private static bool CheckSourceTarget(List<Vertex> myPointsTarget, List<Vertex> mypointsSource)
        {
            // Check source, target
            if (mypointsSource == null || mypointsSource.Count == 0)
            {
                MessageBox.Show("Source point set is empty");
                System.Diagnostics.Debug.WriteLine("Can't execute with null or empty input");
                return false;
            }

            if (myPointsTarget == null || myPointsTarget.Count == 0)
            {
                MessageBox.Show("Target point set is empty");
                System.Diagnostics.Debug.WriteLine("Can't execute with null or empty target");
                return false;
            }
            return true;
        }


    }
    public enum ICP_VersionUsed
    {
        Horn,
        Scaling_Umeyama,
        Scaling_Zinsser,
        Scaling_Du,
        UsingStitchData,
        RandomPoints
    }
}