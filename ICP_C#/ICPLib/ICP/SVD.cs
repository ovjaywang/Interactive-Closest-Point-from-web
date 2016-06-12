using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTKLib;

namespace ICPLib
{
    public class SVD
    {
        //other methods for SVD, for possible later usage
        //MathUtils.SingularValueDecomposition3x3(Harray, Uarray, warray, VTarray);
        //MathNet.Numerics.Providers.LinearAlgebra.Mkl.MklLinearAlgebraProvider svd = new MathNet.Numerics.Providers.LinearAlgebra.Mkl.MklLinearAlgebraProvider();
        //double[] a = MatrixUtilsNumerics.doubleFromArrayDouble(Harray);
        //double[] u = MatrixUtilsNumerics.doubleFromArrayDouble(Uarray);
        //double[] vt = MatrixUtilsNumerics.doubleFromArrayDouble(Uarray);
        //double[] s = new double[3];
        //svd.SingularValueDecomposition(true, a, 3, 3, s, u, vt);
        private static Matrix3d CalculateRotationBySingularValueDecomposition(Matrix3d H, List<Vector3d> pointsSourceShift, ICP_VersionUsed icpVersionUsed)
        {
            double[,] Harray = TransformPointsUtils.DoubleArrayFromMatrix(H);
            double[,] Uarray = new double[3, 3];
            double[,] VTarray = new double[3, 3];
            double[] eigenvalues = new double[3];


            //trial 3:
            alglib.svd.rmatrixsvd(Harray, 3, 3, 2, 2, 2, ref eigenvalues, ref Uarray, ref VTarray);


            Matrix3d U = MatrixUtilsOpenTK.DoubleArrayToMatrix3d(Uarray);
            Matrix3d VT = MatrixUtilsOpenTK.DoubleArrayToMatrix3d(VTarray);
            Matrix3d R = Matrix3d.Mult(U, VT);

            Matrix3d UT = Matrix3d.Transpose(U);
            Matrix3d V = Matrix3d.Transpose(VT);
            Matrix3d Rtest = Matrix3d.Mult(UT, V);
            //R = Rtest;

            Matrix3d checkShouldGiveI = Matrix3d.Mult(UT, U);
            checkShouldGiveI = Matrix3d.Mult(VT, V);
            Matrix3d RT = Matrix3d.Transpose(R);
            checkShouldGiveI = Matrix3d.Mult(RT, R);
            //see article by Umeyama

            //if (H.Determinant < 0)
            //{
            //    R[2, 2] = -R[2, 2];
            //    //S[2, 2] = -1;
            //}

            //calculate the sign matrix for using the scale factor
            Matrix3d K2 = Matrix3d.Identity;
            double check = U.Determinant * VT.Determinant;
            //if (check < 0 && Math.Abs(check) > 1E-3)
            //{
            //    K2[2, 2] = -1;
            //}
            
            RT = Matrix3d.Transpose(R);
            checkShouldGiveI = Matrix3d.Mult(RT, R);

            double scale = CalculateScale_Umeyama(pointsSourceShift, eigenvalues, K2);
            R = Matrix3d.Mult(R, K2);
            if (icpVersionUsed == ICP_VersionUsed.Scaling_Umeyama)
            {
                //R = Matrix3d.Mult(R, K2);
                R = MatrixUtilsOpenTK.MultiplyScalar3D(R, scale);
            }
            ////check eigenvectors
            //Matrix3d Snew = Matrix3d.Mult(U, MatrixUtilsOpenTK.Matrix3FromMatrix3d(H));
            //Snew = Matrix3d.Mult(Snew, VT);

            //Matrix3d si = S.Inverted();
            //Matrix3d Rnew = Matrix3d.Mult(VT, si);
            //Rnew = Matrix3d.Mult(Rnew, U);

            //Rnew = Matrix3d.Mult(VT, S);
            //Rnew = Matrix3d.Mult(Rnew, U);

            return R;
        }

        public static Matrix4d FindTransformationMatrix(List<Vector3d> pointsTarget, List<Vector3d> pointsSource, ICP_VersionUsed icpVersionUsed)
        {


            //shift points to the center of mass (centroid) 
            Vector3d centroidReference = TransformPointsUtils.CalculateCentroid(pointsTarget);
            List<Vector3d> pointsTargetShift = TransformPointsUtils.CalculatePointsShiftedByCentroid(pointsTarget, centroidReference);

            Vector3d centroidToBeMatched = TransformPointsUtils.CalculateCentroid(pointsSource);
            List<Vector3d> pointsSourceShift = TransformPointsUtils.CalculatePointsShiftedByCentroid(pointsSource, centroidToBeMatched);


            //calculate correlation matrix
            Matrix3d H = TransformPointsUtils.CalculateCorrelationMatrix(pointsTargetShift, pointsSourceShift);

            //Matrix3d S;
            double[] eigenvalues = new double[3];
            
            Matrix3d R = CalculateRotationBySingularValueDecomposition(H, pointsSourceShift, icpVersionUsed);
            
            double c;
                        
            if (icpVersionUsed == ICP_VersionUsed.Scaling_Zinsser)
            {
                c = CalculateScale_Zinsser(pointsSourceShift, pointsTargetShift, ref R);
            }
            if (icpVersionUsed == ICP_VersionUsed.Scaling_Du)
            {
                Matrix3d C = CalculateScale_Du(pointsSourceShift, pointsTargetShift, R);
                R = Matrix3d.Mult(R, C);
            }
            
            Vector3d T = SVD.CalculateTranslation(centroidReference, centroidToBeMatched, R);

           
           

            Matrix4d myMatrix = SVD.PutTheMatrix4together(T, R);
            //double d = myMatrix.Determinant;

            return myMatrix;

        }
        private static Vector3d CalculateTranslation(Vector3d centroidReference, Vector3d centroidToBeMatched, Matrix3d Rotation)
        {


            //T = centroidReference - centroidToBeMatched * RotationMatrix
            //Vector3d T = MatrixUtilsOpenTK.Multiply3x3(tempMat, centroidToBeMatched);
            //Vector3d T = MatrixUtilsOpenTK.Multiply3x3(Rotation, centroidToBeMatched);
            Vector3d T = Rotation.TransformVector(centroidToBeMatched);
            T = Vector3d.Subtract(centroidReference, T);


            return T;

        }
        private static Matrix4d PutTheMatrix4together(Vector3d T, Matrix3d Rotation)
        {

            //put the 4d matrix together
            Matrix3d r3D = MatrixUtilsOpenTK.Matrix3dfromMatrix3d(Rotation);
            Matrix4d myMatrix = new Matrix4d(r3D);
            myMatrix[0, 3] = T.X;
            myMatrix[1, 3] = T.Y;
            myMatrix[2, 3] = T.Z;
            myMatrix[3, 3] = 1D;
            //myMatrix[0, 3] = T.X;
            //myMatrix[3, 1] = T.Y;
            //myMatrix[3, 2] = T.Z;
            //myMatrix[3, 3] = 1D;
            return myMatrix;

        }
       
    

        private static double CalculateScale_Umeyama(List<Vector3d> pointsSourceShift, double[] eigenvalues, Matrix3d K2)
        {
            double sigmaSquared = 0f;
            for (int i = 0; i < pointsSourceShift.Count; i++)
            {
                sigmaSquared += MatrixUtilsOpenTK.Norm(pointsSourceShift[i]);
            }

            sigmaSquared /= pointsSourceShift.Count;

            double c = 0.0F;
            for (int i = 0; i < 3; i++)
            {
                c += eigenvalues[i];
            }
            if (K2[2, 2] < 0)
            {
                c -= 2 * eigenvalues[2];
            }
            c = c / sigmaSquared;
            return c;
        }
        private static double CalculateScale_Zinsser(List<Vector3d> pointsSourceShift, List<Vector3d> pointsTargetShift, ref Matrix3d R)
        {
            double sum1 = 0;
            double sum2 = 0;
            Matrix3d RT = Matrix3d.Transpose(R);
            Matrix3d checkT = Matrix3d.Mult(RT, R);

            Matrix4d R4D = PutTheMatrix4together(Vector3d.Zero, R);
            //Matrix4d R4DT = Matrix4d.Transpose(R4D);


            //double check = 0;

            //Matrix4d checkIdentity = Matrix4d.Mult(R4DT, R4D);
            
            //checkIdentity = Matrix4d.Mult(R4DT, R4D);
            for(int i = 0; i < pointsSourceShift.Count; i++)
            {
                //Vector3d v = Vector3d.TransformNormalInverse(pointsSourceShift[i], R4D);

                               
                Vector3d v = Vector3d.TransformNormalInverse(pointsSourceShift[i], R4D);
                //v = Matrix3d.Tr Vector3d.TransformNormal(pointsSourceShift[i], R4D);
                v = R.TransformVector(pointsSourceShift[i]);

                //v = Vector3d.TransformVector(pointsSourceShift[i], R4D);
                //v = Vector3d.TransformNormalInverse(pointsSourceShift[i], R4D);




                sum1 += Vector3d.Dot(v , pointsTargetShift[i]);
                sum2 += Vector3d.Dot(pointsSourceShift[i], pointsSourceShift[i]);
                
            }

            double c = sum1 / sum2;
            R = MatrixUtilsOpenTK.MultiplyScalar3D(R, c);
            return c;
        }
        private static Matrix3d CalculateScale_Du(List<Vector3d> pointsSourceShift, List<Vector3d> pointsTargetShift, Matrix3d R)
        {
           
            //Matrix4d R4D = PutTheMatrix4together(Vector3d.Zero, R);
            Matrix3d S = Matrix3d.Identity;
            Matrix3d K = Matrix3d.Identity;
            for (int i = 0; i < 3; i++)
            {
                K[i, i] = 0;
            }
            for (int i = 0; i < 3; i++)
            {
                K[i, i] = 1;
                double sum1 = 0;
                double sum2 = 0;
                for (int j = 0; j < pointsSourceShift.Count; j++)
                {
                    Vector3d vKumultiplied = K.TransformVector(pointsSourceShift[j]);
                    Vector3d v = R.TransformVector(vKumultiplied);
                    sum1 += Vector3d.Dot(pointsTargetShift[j], v);

                    sum2 += Vector3d.Dot(pointsSourceShift[j], vKumultiplied);
                }
                K[i, i] = 0;
                S[i, i] = sum1 / sum2;
            }

           
            return S;
        }
    }
}
