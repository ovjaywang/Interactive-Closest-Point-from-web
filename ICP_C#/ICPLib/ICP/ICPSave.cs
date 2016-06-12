/*=========================================================================

  Program:   Visualization Toolkit
  Module:    $RCSfile: vtkIterativeClosestPointTransform.cxx,v $

  Copyright (c) Ken Martin, Will Schroeder, Bill Lorensen
  All rights reserved.
  See Copyright.txt or http://www.kitware.com/Copyright.htm for details.

     This software is distributed WITHOUT ANY WARRANTY; without even
     the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
     PURPOSE.  See the above copyright notice for more information.

=========================================================================*/
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

namespace OpenTKLib
{
  
   
    public partial class IterativeClosestPointTransform //: vtkLinearTransform //csharpLinearTransform
    {
        public static ICP_VersionUsed ICPVersion = ICP_VersionUsed.Scaling_Umeyama;
        public static bool FixedTestPoints = false;
        public static int MaximumNumberOfIterations = 100;
        public static bool ResetVertexToOrigin = true;

        private static IterativeClosestPointTransform instance;
        public ushort NumberOfStartTrialPoints = 6;
        
        public Matrix4d Matrix;
        public int NumberOfIterations;

        private List<Vertex> pointsTransformed;

        //int CheckMeanDistance;
        public double MeanDistance;
               

        double MaximumMeanDistance;
        double ThresholdOutlier = 10;
        
       
        public LandmarkTransform LandmarkTransform;


        public List<Vertex> PSource;
        public List<Vertex> PTarget;
    
        public IterativeClosestPointTransform()//:base(PointerUtils.GetIntPtr(new double[3]), true, true)
        {
            this.PSource = null;
            this.PTarget = null;

            this.LandmarkTransform = new LandmarkTransform();

            

            this.MaximumMeanDistance = 1.0E-3;

            this.NumberOfIterations = 0;
            this.MeanDistance = 0.0;

        }

        public List<Vertex> PerformICP(List<Vertex> myPointsTarget, List<Vertex> myPointsToBeMatched)
        {
            this.PTarget = myPointsTarget;
            this.PSource = myPointsToBeMatched;


            if (ICPVersion == ICP_VersionUsed.UsingStitchData)
                return PerformICP_Stitching();
            else
                return PerformICP_New();
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



     
        public List<Vertex> PerformICP_New()
        {
            
            int iter = 0;
            try
            {
                if (!CheckSourceTarget(PTarget, PSource))
                    return null;
                if (ResetVertexToOrigin)
                {

                    Vertices.ResetVertexToOrigin(PTarget);
                    Vertices.ResetVertexToOrigin(PSource);
                }

                List<Vertex> pointsTargetIterate = Vertices.CopyVertices(PTarget);
                List<Vertex> pointsSourceIterate = Vertices.CopyVertices(PSource);
                Matrix4d myMatrix ;
              
                if(!FixedTestPoints)
                    BuildKDTree(pointsTargetIterate, pointsSourceIterate);
                
                this.Matrix = Matrix4d.Identity;
                double oldMeanDistance = 0;
                
                
                for (iter = 0; iter < MaximumNumberOfIterations; iter++ )
                {
                    //pointsSourceIterate = PSource;
                    if (!FixedTestPoints)
                    {
                        pointsTargetIterate = SearchNearestNeighbours(pointsSourceIterate, pointsTargetIterate);

                        if (pointsTargetIterate.Count != pointsSourceIterate.Count)
                        {
                            MessageBox.Show("Error finding neighbours, found " + pointsSourceIterate.Count.ToString() + " out of " + pointsTargetIterate.ToString());
                            break;
                        }
                    }
                    List<Vertex> myPointsTransformed = null;
                    if (ICPVersion == ICP_VersionUsed.Quaternions)
                    {
                        TransformPointsUtils.FindTransformationMatrix(Vertices.Vector3dListFromVertexList(pointsSourceIterate), Vertices.Vector3dListFromVertexList(pointsTargetIterate), this.LandmarkTransform);
                        myMatrix = LandmarkTransform.Matrix;
                        myPointsTransformed = MathUtils.TransformPoints(pointsSourceIterate, myMatrix);
                        DebugWriteUtils.WriteTestOutputVertex("Quaternion Method", myMatrix, pointsSourceIterate, myPointsTransformed, pointsTargetIterate);
                    }
                    else
                    {

                        myMatrix = SVD.FindTransformationMatrix(Vertices.Vector3dListFromVertexList(pointsTargetIterate), Vertices.Vector3dListFromVertexList(pointsSourceIterate), ICPVersion);
                        //DebugWriteUtils.WriteMatrix("Solution with scaling", trialM);

                        myPointsTransformed = MathUtils.TransformPoints(pointsSourceIterate, myMatrix);
                        DebugWriteUtils.WriteTestOutputVertex("New Method WITH Scale", myMatrix, pointsSourceIterate, myPointsTransformed, pointsTargetIterate);
                       
                    }
                    double totaldist = PointUtils.CalculateTotalDistance(pointsTargetIterate, myPointsTransformed);
                    this.MeanDistance = totaldist / Convert.ToDouble(pointsTargetIterate.Count);
                    Debug.WriteLine("--------------Iteration: " + iter.ToString() + " : Mean Distance: " + MeanDistance.ToString("0.00000000000"));

                    //accumulate resulting matrix
                    Matrix4d.Mult(ref myMatrix, ref this.Matrix, out this.Matrix);

                    if (MeanDistance < this.MaximumMeanDistance) //< Math.Abs(MeanDistance - oldMeanDistance) < this.MaximumMeanDistance)
                        break;
                    oldMeanDistance = MeanDistance;
                 
                    DebugWriteUtils.WriteMatrix("Concatenated matrix", Matrix);

                    myPointsTransformed = MathUtils.TransformPoints(PSource, myMatrix);

                    //Matrix4.Mult(ref preMatrix, ref myMatrix, out preMatrix);
                    pointsSourceIterate = myPointsTransformed;
                    pointsTargetIterate = Vertices.CopyVertices(PTarget);
                }

                Debug.WriteLine("--------****** Solution of ICP after : " + iter.ToString() + " iterations, and Mean Distance: " + MeanDistance.ToString("0.00000000000"));
                this.PTransformed = MathUtils.TransformPoints(PSource, Matrix);

                DebugWriteUtils.WriteTestOutputVertex("Solution of ICP", Matrix, PSource, PTransformed, PTarget);

             

                return PTransformed;
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in Update ICP at iteration: " + iter.ToString() + " : " + err.Message);
                return null;

            }
         
        }
        private static bool CheckSourceTarget(List<Vertex> myPointsTarget, List<Vertex> myPointsToBeMatched)
        {
            // Check source, target
            if (myPointsToBeMatched == null || myPointsToBeMatched.Count == 0)
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
        Quaternions,
        Scaling_Umeyama,
        Scaling_Zensser,
        Scaling_Du,
        UsingStitchData
    }
}