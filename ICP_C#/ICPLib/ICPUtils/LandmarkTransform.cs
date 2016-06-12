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
using OpenTK;
using System.Windows.Media.Media3D;
using OpenTKLib;

namespace ICPLib
{


    public class LandmarkTransform 
    {
       
        public OpenTK.Matrix4d Matrix;
      
        public List<Vector3d> SourceLandmarks;
        public List<Vector3d> TargetLandmarks;
   

        //----------------------------------------------------------------------------
        public LandmarkTransform()
        {
            Matrix = new Matrix4d();
        
        }

     
      
        private void FindCentroids(int N_PTS, double[] source_centroid, double[] target_centroid)
        {
           
            Vector3d p = new Vector3d();
           
            for (int i = 0; i < N_PTS; i++)
            {
                p = this.SourceLandmarks[i];
                source_centroid[0] += p[0];
                source_centroid[1] += p[1];
                source_centroid[2] += p[2];
                p = this.TargetLandmarks[i];
                target_centroid[0] += p[0];
                target_centroid[1] += p[1];
                target_centroid[2] += p[2];
            }
            source_centroid[0] /= N_PTS;
            source_centroid[1] /= N_PTS;
            source_centroid[2] /= N_PTS;
            target_centroid[0] /= N_PTS;
            target_centroid[1] /= N_PTS;
            target_centroid[2] /= N_PTS;
        }
        private void UpdateCorrelationMatrix(int pointIndex, double[] source_centroid, double[] target_centroid, double[,] M, ref double scale)
        {

            double sSum = 0.0F, tSum = 0.0F;

           
            Vector3d s = this.SourceLandmarks[pointIndex];
            s[0] -= source_centroid[0];
            s[1] -= source_centroid[1];
            s[2] -= source_centroid[2];
          
            Vector3d t = this.TargetLandmarks[pointIndex];
            t[0] -= target_centroid[0];
            t[1] -= target_centroid[1];
            t[2] -= target_centroid[2];
            // accumulate the products s*T(t) into the matrix M
            for (int i = 0; i < 3; i++)
            {
                M[i, 0] += s[i] * t[0];
                M[i, 1] += s[i] * t[1];
                M[i, 2] += s[i] * t[2];

                // for the affine transform, compute ((a.a^t)^-1 . a.b^t)^t.
                // a.b^t is already in M.  here we put a.a^t in AAT.
                //if (this.Mode == VTK_LANDMARK_AFFINE)
                //  {
                //  AAT[i,0] += a[i]*a[0];
                //  AAT[i,1] += a[i]*a[1];
                //  AAT[i,2] += a[i]*a[2];
                //  }
                //}
                // accumulate scale factors (if desired)
                sSum += s[0] * s[0] + s[1] * s[1] + s[2] * s[2];
                tSum += t[0] * t[0] + t[1] * t[1] + t[2] * t[2];
            }


            scale = (float)Math.Sqrt(tSum / sSum);
        }
        private double[,] CreateMatrixForDiag(int pointIndex, double[] source_centroid, double[] target_centroid, double[,] M, ref double scale)
        {
            //updates M
            UpdateCorrelationMatrix(pointIndex, source_centroid, target_centroid, M, ref scale);

            // -- build the 4x4 matrix N --

            double[,] Ndata = new double[4, 4];
            double[,] N = new double[4, 4];
            //      double *N[4];
            for (int i = 0; i < 4; i++)
            {
                //for (int j = 0; j < 4; j++)
                //{
                //    N[i, j] = Ndata[i, j];
                //}
                // fill N with zeros
                for (int j = 0; j < 4; j++)
                {
                    N[i, j] = 0.0F;
                }

            }
            // on-diagonal elements
            N[0, 0] = M[0, 0] + M[1, 1] + M[2, 2];
            N[1, 1] = M[0, 0] - M[1, 1] - M[2, 2];
            N[2, 2] = -M[0, 0] + M[1, 1] - M[2, 2];
            N[3, 3] = -M[0, 0] - M[1, 1] + M[2, 2];
            // off-diagonal elements
            N[0, 1] = N[1, 0] = M[1, 2] - M[2, 1];
            N[0, 2] = N[2, 0] = M[2, 0] - M[0, 2];
            N[0, 3] = N[3, 0] = M[0, 1] - M[1, 0];

            N[1, 2] = N[2, 1] = M[0, 1] + M[1, 0];
            N[1, 3] = N[3, 1] = M[2, 0] + M[0, 2];
            N[2, 3] = N[3, 2] = M[1, 2] + M[2, 1];

            // -- eigen-decompose N (is symmetric) --
            return N;

        }
        private void ChooseFirstFourEigenvalues(ref double w, ref double x, ref double y, ref double z, double[,] eigenvectors, double[] eigenvalues, int N_PTS)
        {
            
            // first: if points are collinear, choose the quaternion that 
            // results in the smallest rotation.
            if (eigenvalues[0] == eigenvalues[1] || N_PTS == 2)
            {
                Vector3d s0 = this.SourceLandmarks[0];
                Vector3d t0 = this.TargetLandmarks[0];
                Vector3d s1 = this.SourceLandmarks[1];
                Vector3d t1 = this.TargetLandmarks[1];

          

                double[] ds = new double[3];
                double[] dt = new double[3];

                double rs = 0;
                double rt = 0;
                for (int i = 0; i < 3; i++)
                {
                    ds[i] = s1[i] - s0[i];      // vector between points
                    rs += ds[i] * ds[i];
                    dt[i] = t1[i] - t0[i];
                    rt += dt[i] * dt[i];
                }

                // normalize the two vectors
                rs = Math.Sqrt(rs);
                ds[0] /= rs; ds[1] /= rs; ds[2] /= rs;
                rt = Math.Sqrt(rt);
                dt[0] /= rt; dt[1] /= rt; dt[2] /= rt;

                // take dot & cross product
                w = ds[0] * dt[0] + ds[1] * dt[1] + ds[2] * dt[2];
                x = ds[1] * dt[2] - ds[2] * dt[1];
                y = ds[2] * dt[0] - ds[0] * dt[2];
                z = ds[0] * dt[1] - ds[1] * dt[0];

                double r = Math.Sqrt(x * x + y * y + z * z);
                double theta = Math.Atan2(r, w);

                // construct quaternion
                w = Math.Cos(theta / 2);
                if (r != 0)
                {
                    r = Math.Sin(theta / 2) / r;
                    x = x * r;
                    y = y * r;
                    z = z * r;
                }
                else // rotation by 180 degrees: special case
                {
                    // rotate around a vector perpendicular to ds
                    MathUtils.Perpendiculars(ds, dt, null, 0);
                    r = Math.Sin(theta / 2);
                    x = dt[0] * r;
                    y = dt[1] * r;
                    z = dt[2] * r;
                }
            }
            else // points are not collinear
            {
                w = eigenvectors[0, 0];
                x = eigenvectors[1, 0];
                y = eigenvectors[2, 0];
                z = eigenvectors[3, 0];
            }
        }
        private Matrix4d CreateMatrixOutOfDiagonalizationResult(double[,] eigenvectors, double[] eigenvalues, int N_PTS, double[] source_centroid, double[] target_centroid, double scale)
        {
            // the eigenvector with the largest eigenvalue is the quaternion we want
            // (they are sorted in decreasing order for us by JacobiN)

            double w = 0;
            double x = 0;
            double y = 0;
            double z = 0;

            //only important if points are collinear - otherwise they are the first largest eigenvalues
            ChooseFirstFourEigenvalues(ref w, ref x, ref y, ref z, eigenvectors, eigenvalues, N_PTS);


            // convert quaternion to a rotation matrix

            double ww = w * w;
            double wx = w * x;
            double wy = w * y;
            double wz = w * z;

            double xx = x * x;
            double yy = y * y;
            double zz = z * z;

            double xy = x * y;
            double xz = x * z;
            double yz = y * z;

            this.Matrix[0,0] = ww + xx - yy - zz;
            this.Matrix[1, 0] = 2.0 * (wz + xy);
            this.Matrix[2, 0] =  2.0 * (-wy + xz);

            this.Matrix[0, 1] =  2.0 * (-wz + xy);
            this.Matrix[1, 1] =  ww - xx + yy - zz;
            this.Matrix[2, 1] =  2.0 * (wx + yz);

            this.Matrix[0, 2] =  2.0 * (wy + xz);
            this.Matrix[1, 2] =  2.0 * (-wx + yz);
            this.Matrix[2, 2] =  ww - xx - yy + zz;

            //if (this.Mode != VTK_LANDMARK_RIGIDBODY)
            //  { // add in the scale factor (if desired)
            for (int i = 0; i < 3; i++)
            {
                double val = this.Matrix[i, 0]  * scale;
                this.Matrix[i, 0] =  val;
                val = this.Matrix[i, 1] * scale;
                this.Matrix[i, 1] =  val;

                val = this.Matrix[i, 2]  * scale;
                this.Matrix[i, 2] =  val;

            }

            //}

            // the translation is given by the difference in the transformed source
            // centroid and the target centroid
            double sx, sy, sz;

            sx = this.Matrix[0, 0]  * source_centroid[0] +
                 this.Matrix[0, 1]   * source_centroid[1] +
                 this.Matrix[0, 2] * source_centroid[2];
            sy = this.Matrix[1, 0]* source_centroid[0] +
                 this.Matrix[1, 1] * source_centroid[1] +
                 this.Matrix[1, 2] * source_centroid[2];
            sz = this.Matrix[2, 0] * source_centroid[0] +
                 this.Matrix[2, 1] * source_centroid[1] +
                 this.Matrix[2, 2] * source_centroid[2];

            this.Matrix[0, 3] =  target_centroid[0] - sx;
            this.Matrix[1, 3] = target_centroid[1] - sy;
            this.Matrix[2, 3] = target_centroid[2] - sz;

            // fill the bottom row of the 4x4 matrix
            this.Matrix[3, 0] = 0.0;
            this.Matrix[3, 1] = 0.0;
            this.Matrix[3, 2] = 0.0;
            this.Matrix[3, 3] = 1.0;
            return this.Matrix;

        }
        public bool Update()
        {
            /*
           The solution is based on
           Berthold K. P. Horn (1987),
           "Closed-form solution of absolute orientation using unit quaternions,"
           Journal of the Optical Society of America A, 4:629-642
         */

            // Original python implementation by David G. Gobbi

            if (this.SourceLandmarks == null || this.TargetLandmarks == null)
            {
                //Identity Matrix
                this.Matrix = new Matrix4d(Vector4d.UnitX, Vector4d.UnitY, Vector4d.UnitZ, Vector4d.UnitW);
                return false;
            }

            // --- compute the necessary transform to match the two sets of landmarks ---

         

            int N_PTS = this.SourceLandmarks.Count;
            if (N_PTS != this.TargetLandmarks.Count)
            {
                System.Diagnostics.Debug.WriteLine("Error:  Source and Target Landmarks contain a different number of points");
                return false;
            }

            // -- if no points, stop here

            if (N_PTS == 0)
            {
                //Identity Matrix
                this.Matrix = new Matrix4d(Vector4d.UnitX, Vector4d.UnitY, Vector4d.UnitZ, Vector4d.UnitW);
                return false;
            }
            double[] source_centroid = { 0, 0, 0 };
            double[] target_centroid = { 0, 0, 0 };
            
            FindCentroids(N_PTS, source_centroid, target_centroid);
           
            ///-------------------------------
            // -- if only one point, stop right here

            if (N_PTS == 1)
            {
                this.Matrix = new Matrix4d(Vector4d.UnitX, Vector4d.UnitY, Vector4d.UnitZ, Vector4d.UnitW);
                Matrix[0, 3] =  target_centroid[0] - source_centroid[0];
                this.Matrix[1, 3] =  target_centroid[1] - source_centroid[1];
                this.Matrix[2, 3] =  target_centroid[2] - source_centroid[2];
                return true;
            }

            // -- build the 3x3 matrix M --

            double[,] M = new double[3, 3];
            double[,] AAT = new double[3, 3];

            for (int i = 0; i < 3; i++)
            {
                AAT[i, 0] = M[i, 0] = 0.0F; // fill M with zeros
                AAT[i, 1] = M[i, 1] = 0.0F;
                AAT[i, 2] = M[i, 2] = 0.0F;
            }
            int pt;
          
            
            for (pt = 0; pt < N_PTS; pt++)
            {

                double scale = 0F;
                double[,] N = CreateMatrixForDiag(pt, source_centroid, target_centroid, M, ref scale);

                double[,] eigenvectorData = new double[4, 4];
                //double *eigenvectors[4],eigenvalues[4];
                double[,] eigenvectors = new double[4, 4];
                double[] eigenvalues = new double[4];



                //for (int i = 0; i < 4; i++)
                //{
                //    for (int j = 0; j < 4; j++)
                //    {
                //        eigenvectors[i, j] = eigenvectorData[i, j];
                //    }
                //}


                MathUtils.JacobiN(N, 4, eigenvalues, eigenvectors);

                //returns this.Matrix
                CreateMatrixOutOfDiagonalizationResult(eigenvectors, eigenvalues, N_PTS, source_centroid, target_centroid, scale);


                //this.Matrix.Modified();


            }
            return true;
        }
    

        //------------------------------------------------------------------------

        //----------------------------------------------------------------------------
        void Inverse()
        {
            List<Vector3d> tmp1 = this.SourceLandmarks;
            List<Vector3d> tmp2 = this.TargetLandmarks;
            this.TargetLandmarks = tmp1;
            this.SourceLandmarks = tmp2;

        }




    }
}