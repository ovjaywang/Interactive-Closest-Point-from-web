
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
        
     
       
        
        private bool CheckExitOnIterations()
        {
            this.NumberOfIterations++;
            System.Diagnostics.Debug.WriteLine("Iteration: " + this.NumberOfIterations);
            if (this.NumberOfIterations >= MaximumNumberOfIterations)
            {
                return true;
            }
            return false;
        }



        private void SetStartPoints(ref List<Vertex> points1, ref List<Vertex> points2, List<Vertex> pointsInput1, List<Vertex> pointsInput2)
        {

            List<int> randomIndices = RandomUtils.UniqueRandomIndices(3, pointsInput1.Count);


            points1 = RandomUtils.ExtractPoints(pointsInput1, randomIndices);
            points2 = RandomUtils.ExtractPoints(pointsInput2, randomIndices);

        }


        private static Matrix4d TryoutNewPoint(int iPoint, List<Vertex> pointsTarget, List<Vertex> pointsSource, List<Vertex> pointsTargetTrial, List<Vertex> pointsSourceTrial, LandmarkTransform myLandmarkTransform)
        {

            Vertex p1 = pointsTarget[iPoint];
            Vertex p2 = pointsSource[iPoint];
            pointsTargetTrial.Add(p1);
            pointsSourceTrial.Add(p2);



            MatrixUtilsNew.FindTransformationMatrix(Vertices.ConvertToVector3dList(pointsSourceTrial), Vertices.ConvertToVector3dList(pointsTargetTrial), myLandmarkTransform);//, accumulate);
     
            Matrix4d myMatrix = myLandmarkTransform.Matrix;
          

            return myMatrix;
        }
        public static Matrix4d TryoutPoints(List<Vertex> pointsTarget, List<Vertex> pointsSource, ICPSolution res, LandmarkTransform myLandmarkTransform)
        {
            res.PointsTarget = RandomUtils.ExtractPoints(pointsTarget, res.RandomIndices);
            res.PointsSource = RandomUtils.ExtractPoints(pointsSource, res.RandomIndices);

            //transform:
            MatrixUtilsNew.FindTransformationMatrix(Vertices.ConvertToVector3dList(res.PointsSource), Vertices.ConvertToVector3dList(res.PointsTarget), myLandmarkTransform);//, accumulate);

            res.Matrix = myLandmarkTransform.Matrix;

            return res.Matrix;

        }
        private static ICPSolution IterateStartPoints(List<Vertex> pointsTarget, List<Vertex> pointsSource, int myNumberPoints, LandmarkTransform myLandmarkTransform, int maxNumberOfIterations)
        {
            int maxIterationPoints = pointsSource.Count;
            int currentIteration = 0;
            try
            {
                if (myNumberPoints > pointsSource.Count)
                    myNumberPoints = pointsSource.Count;

                List<ICPSolution> solutionList = new List<ICPSolution>();

                for (currentIteration = 0; currentIteration < maxNumberOfIterations; currentIteration++)
                {

                    ICPSolution res = ICPSolution.SetRandomIndices(myNumberPoints, maxIterationPoints, solutionList);


                    res.Matrix = TryoutPoints(pointsTarget, pointsSource, res, myLandmarkTransform);//, accumulate);
                    res.PointsTransformed = MathUtils.TransformPoints(res.PointsSource, res.Matrix);

                    double totaldist = PointUtils.CalculateTotalDistance(res.PointsTarget, res.PointsTransformed);
                    res.MeanDistance = totaldist / Convert.ToDouble(res.PointsSource.Count);
                  
                    solutionList.Add(res);

                  
                }


                if (solutionList.Count > 0)
                {
                    solutionList.Sort(new ICPSolutionComparer());
                    RemoveSolutionIfMatrixContainsNaN(solutionList);
                    if(solutionList.Count == 0)
                        System.Windows.Forms.MessageBox.Show("No start solution could be found !");


                    Debug.WriteLine("Solutions found after: " + currentIteration.ToString() + " iterations, number of solution " + solutionList.Count.ToString());

                    if (solutionList.Count > 0)
                    {
                        ICPSolution result = solutionList[0];
                        //write solution to debug ouput
                        //System.Diagnostics.Debug.WriteLine("Solution of start sequence is: ");
                        DebugWriteUtils.WriteTestOutputVertex("Solution of start sequence", result.Matrix, result.PointsSource, result.PointsTransformed, result.PointsTarget);
                        return result;
                   
                    }

                }
                return null;
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in IterateStartPoints of ICP at: " + currentIteration.ToString() + " : " + err.Message);
                return null;
            }


        }
        private static bool CheckIfMatrixIsOK(Matrix4d myMatrix)
        {
            //ContainsNaN
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (double.IsNaN( myMatrix[i, 0]  ))
                        return false;

                }
            }
            return true;

        }
        private static void RemoveSolutionIfMatrixContainsNaN(List<ICPSolution> solutionList)
        {
            int iTotal = 0;
            for (int i = solutionList.Count - 1; i >= 0; i--)
            {
                if (!CheckIfMatrixIsOK(solutionList[i].Matrix))
                {
                    iTotal++;
                    
                    solutionList.RemoveAt(i);
                }
            }
           // Debug.WriteLine("-->Removed a total of: " + iTotal.ToString() + " solutions - because invalid matrixes");
        }
        /// <summary>
        /// calculates a start solution set in total of "myNumberPoints" points
        /// </summary>
        /// <param name="pointsTargetSubset"></param>
        /// <param name="pointsSourceSubset"></param>
        /// <returns></returns>
        private static ICPSolution CalculateStartSolution(ref List<Vertex> pointsTargetSubset, ref  List<Vertex> pointsSourceSubset, int myNumberPoints,
            LandmarkTransform myLandmarkTranform, List<Vertex> pointsTarget, List<Vertex> pointsSource, int maxNumberOfIterations)
        {
            try
            {
                if (CheckSourceTarget(pointsTarget, pointsSource))
                    return null;
                pointsTargetSubset = Vertices.CopyVertices(pointsTarget);
                pointsSourceSubset = Vertices.CopyVertices(pointsSource);

                ICPSolution res = IterateStartPoints(pointsTargetSubset, pointsSourceSubset, myNumberPoints, myLandmarkTranform, maxNumberOfIterations);
                if (res == null)
                {
                    System.Windows.Forms.MessageBox.Show("Could not find starting points for ICP Iteration - bad matching");
                    return null;
                }
                PointUtils.RemoveVertex(ref pointsTargetSubset, ref pointsSourceSubset, res.RandomIndices);

                return res;
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in CalculateStartSolution of ICP: " + err.Message);
                return null;
            }
        }
  
        private double CheckNewPointDistance(int iPoint, Matrix4d myMatrix, List<Vertex> pointsTarget, List<Vertex> pointsSource)
        {
            Vertex p1 = pointsTarget[iPoint];
            Vertex p2 = pointsSource[iPoint];
            List<Vertex> tempPointReference = new List<Vertex>();
            List<Vertex> tempPointToBeMatched = new List<Vertex>();

            tempPointReference.Add(p1);
            tempPointToBeMatched.Add(p2);

            List<Vertex> tempPointRotate = MathUtils.TransformPoints(tempPointToBeMatched, myMatrix);
            double dist = PointUtils.CalculateTotalDistance(tempPointReference, tempPointRotate);
            return dist;

        }
        public List<Vertex> PerformICP_Stitching()
        {
            int iPoint = 0;
            try
            {
               
                List<Vertex> pointsTarget = null;
                List<Vertex> pointsSource = null;

                ICPSolution res = CalculateStartSolution(ref pointsTarget, ref  pointsSource, NumberOfStartTrialPoints, this.LandmarkTransform, this.PTarget, this.PSource, MaximumNumberOfIterations);
                if (res == null)
                    return null;

                Matrix4d myMatrix = res.Matrix;
                
               

                double oldMeanDistance = 0;
                //now try all points and check if outlier
                for (iPoint = (pointsTarget.Count - 1); iPoint >= 0; iPoint--)
                {
                    double distanceOfNewPoint = CheckNewPointDistance(iPoint, myMatrix, pointsTarget, pointsSource);

                    ////experimental

                    ////--compare this distance to:
                    //pointsTargetTrial.Add[pointsTargetTrial.Count, p1[0], p1[1], p1[2]);
                    //pointsSourceTrial.Add[pointsSourceTrial.Count, p2[0], p2[1], p2[2]);
                    //List<Vertex> tempPointRotateAll = TransformPoints(pointsSourceTrial, myMatrix, pointsSourceTrial.Count);


                    //dist = CalculateTotalDistance(pointsTargetTrial, tempPointRotateAll);
                    //DebugWriteUtils.WriteTestOutput(myMatrix, pointsSourceTrial, tempPointRotateAll, pointsTargetTrial, pointsTargetTrial.Count);
                    Debug.WriteLine("------>ICP Iteration Trial: " + iPoint.ToString() + " : Mean Distance: " + distanceOfNewPoint.ToString());
                    if (Math.Abs(distanceOfNewPoint - res.MeanDistance) < ThresholdOutlier)
                    {
                        List<Vertex> pointsTargetTrial = PointUtils.CopyVertices(res.PointsTarget);
                        List<Vertex> pointsSourceTrial = PointUtils.CopyVertices(res.PointsSource);


                        myMatrix = TryoutNewPoint(iPoint, pointsTarget, pointsSource, pointsTargetTrial, pointsSourceTrial, this.LandmarkTransform);

                        List<Vertex> myPointsTransformed = MathUtils.TransformPoints(pointsSourceTrial, myMatrix);
                        double totaldist = PointUtils.CalculateTotalDistance(pointsTargetTrial, myPointsTransformed);
                        this.MeanDistance = totaldist / Convert.ToDouble(pointsTargetTrial.Count);


                        DebugWriteUtils.WriteTestOutputVertex("Iteration " + iPoint.ToString(),  myMatrix, pointsSourceTrial, myPointsTransformed, pointsTargetTrial);

                        //could also remove this check...
                        if (Math.Abs(oldMeanDistance - this.MeanDistance) < ThresholdOutlier)
                        {

                            res.PointsTarget = pointsTargetTrial;
                            res.PointsSource = pointsSourceTrial;
                            res.Matrix = myMatrix;
                            res.PointsTransformed = myPointsTransformed;
                            oldMeanDistance = this.MeanDistance;

                            //Debug.WriteLine("************* Point  OK : ");
                            DebugWriteUtils.WriteTestOutputVertex("************* Point  OK :" , myMatrix, res.PointsSource, myPointsTransformed, res.PointsTarget);

                        }
                        //remove point from point list
                        pointsTarget.RemoveAt(iPoint);
                        pointsSource.RemoveAt(iPoint);
                       

                    }


                }
                this.Matrix = res.Matrix;
                //System.Diagnostics.Debug.WriteLine("Solution of ICP is : ");
                DebugWriteUtils.WriteTestOutputVertex("Solution of ICP", Matrix, res.PointsSource, res.PointsTransformed, res.PointsTarget);
                pointsTransformed = res.PointsTransformed;

                return pointsTransformed;
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in Update ICP at point: " + iPoint.ToString() + " : " + err.Message);
                return null;

            }
            //Matrix4d newMatrix = accumulate.GetMatrix();
            //this.Matrix = newMatrix;

        }

        private List<Vertex> ICPOnPoints_WithSubset(List<Vertex> myVerticesTarget, List<Vertex> myVerticesToBeMatched, List<Vertex> myPointsTargetSubset, List<Vertex> mypointsSourceSubset)
        {

            List<Vector3d> myVectorsTransformed = null;
            List<Vertex> myVerticesTransformed = null;

            try
            {
                Matrix4d m;


                PerformICP(myPointsTargetSubset, mypointsSourceSubset);
                myVectorsTransformed = Vertices.ConvertToVector3dList(PTransformed);
                m = Matrix;

                //DebugWriteUtils.WriteTestOutput(m, mypointsSourceSubset, myPointsTransformed, myPointsTargetSubset);
                //extend points:
                //myPointsTransformed = icpSharp.TransformPointsToPointsData(mypointsSourceSubset, m);
                //-----------------------------
                //DebugWriteUtils.WriteTestOutput(m, mypointsSourceSubset, myPointsTransformed, myPointsTargetSubset);

                //now with all other points as well...
                myVectorsTransformed = new List<Vector3d>();

                myVectorsTransformed = MathUtils.TransformPoints(Vertices.ConvertToVector3dList(myVerticesToBeMatched), m);
                myVerticesTransformed = Vertices.ConvertVector3DListToVertexList(myVectorsTransformed);
                //write all results in debug output
                DebugWriteUtils.WriteTestOutputVertex("Soluation of Points With Subset", m, myVerticesToBeMatched, myVerticesTransformed, myVerticesTarget);

            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Error in ICP : " + err.Message);
                return null;
            }
            //for output:
           

            return myVerticesTransformed;

        }


        private List<Vertex> ICPOnPoints_WithSubset_PointsData(List<List<Vertex>> PointsDataList, List<System.Drawing.Point> pointsLeft, List<System.Drawing.Point> pointsRight)
        {

            List<Vertex> myPointsTarget = PointsDataList[0];
            List<Vertex> mypointsSource = PointsDataList[1];
            if (PointsDataList.Count > 1)
            {
                if (pointsLeft != null)
                {
                    List<Vertex> mySubsetLeft = PointCloudUtils.CreateVerticesFromDrawingPoints_IncludingCheck(pointsLeft, PointsDataList[0], pointsRight);
                    List<Vertex> mySubsetRight = PointCloudUtils.CreateVerticesFromDrawingPoints_IncludingCheck(pointsRight, PointsDataList[1], pointsLeft);

                    if (mySubsetLeft.Count == mySubsetRight.Count)
                    {

                        List<Vertex> myPointsTransformed = ICPOnPoints_WithSubset(myPointsTarget, mypointsSource, mySubsetLeft, mySubsetRight);
                        return myPointsTransformed;


                    }
                    else
                    {
                        MessageBox.Show("Error in identifying stitched points ");

                    }
                }
            }

            return null;
        }
        public List<Vertex> ICPOnPointss_WithSubset_Vector3d(List<Vertex> myVector3Reference, List<Vertex> myVector3ToBeMatched, List<System.Drawing.Point> pointsLeft2D, List<System.Drawing.Point> pointsRight2D)
        {
            List<List<Vertex>> PointsDataList = new List<List<Vertex>>();
            List<Vertex> myPointsTarget = myVector3Reference;
            PointsDataList.Add(myPointsTarget);

            List<Vertex> mypointsSource = myVector3ToBeMatched;
            PointsDataList.Add(mypointsSource);


            List<Vertex> myPointsTransformed = ICPOnPoints_WithSubset_PointsData(PointsDataList, pointsLeft2D, pointsRight2D);
            if (myPointsTransformed != null)
            {

                //PointsTarget = myPointsTarget;
                //pointsSource = mypointsSource;
                //PointsTransformed = myPointsTransformed;

              
                return myPointsTransformed;
            }
            return null;

        }
        
    }
   
}