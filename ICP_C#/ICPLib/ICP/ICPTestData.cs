using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Diagnostics;
using OpenTKLib;

namespace ICPLib
{
    public class ICPTestData
    {
        //private static List<Vertex> verticesTarget;
        //private static List<Vertex> verticesSource;
       // private static List<Vertex> vectorsResult;

        public static double Test1_Translation(ref List<Vertex> myVerticesTarget, ref List<Vertex> myVerticesSource, ref List<Vertex> myVerticesResult)
        {
            myVerticesTarget = Vertices.CreateSomePoints();
            myVerticesSource = VertexUtils.CloneListVertex(myVerticesTarget);
            VertexUtils.TranslateVertices(myVerticesSource, 10, 3, 8);

            
            myVerticesResult = IterativeClosestPointTransform.Instance.PerformICP(myVerticesTarget, myVerticesSource);
            return IterativeClosestPointTransform.Instance.MeanDistance;

        }
        public static double Test2_RotationX30Degrees(ref List<Vertex> myVerticesTarget, ref List<Vertex> myVerticesSource, ref List<Vertex> myVerticesResult)
        {
            
            
            //myVerticesTarget = Vertices.CreateSomePoints();
            myVerticesTarget = Vertices.CreateCube_Corners(50);
            myVerticesSource = VertexUtils.CloneListVertex(myVerticesTarget);

            Matrix3d R = Matrix3d.CreateRotationX(30);
            VertexUtils.RotateVertices(myVerticesSource, R);


            myVerticesResult = IterativeClosestPointTransform.Instance.PerformICP(myVerticesTarget, myVerticesSource);
            return IterativeClosestPointTransform.Instance.MeanDistance;

        }
  
        public static double Test2_RotationXYZ(ref List<Vertex> myVerticesTarget, ref List<Vertex> myVerticesSource, ref List<Vertex> myVerticesResult)
        {
            myVerticesTarget = Vertices.CreateCube_Corners(50);
            myVerticesSource = VertexUtils.CloneListVertex(myVerticesTarget);


            Matrix3d R = new Matrix3d();
            R = R.RotateSome();

            VertexUtils.RotateVertices(myVerticesSource, R);


            myVerticesResult = IterativeClosestPointTransform.Instance.PerformICP(myVerticesTarget, myVerticesSource);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static double Test3_Scale(ref List<Vertex> myVerticesTarget, ref List<Vertex> myVerticesSource, ref List<Vertex> myVerticesResult)
        {
          
            //myVerticesTarget = CreateSomePoints();
            myVerticesTarget = Vertices.CreateCube_Corners(50);

            myVerticesSource = VertexUtils.CloneListVertex(myVerticesTarget);

            VertexUtils.ScaleByFactor(myVerticesSource, 0.2);
            
            myVerticesResult = IterativeClosestPointTransform.Instance.PerformICP(myVerticesTarget, myVerticesSource);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
       
        public static double Test5_CubeTranslation(ref List<Vertex> myVerticesTarget, ref List<Vertex> myVerticesSource, ref List<Vertex> myVerticesResult)
        {

            myVerticesTarget = Vertices.CreateCube_Corners(50);
            
            myVerticesSource = VertexUtils.CloneListVertex(myVerticesTarget);

            VertexUtils.TranslateVertices(myVerticesSource, 0, -300, 0);
            
            myVerticesResult = IterativeClosestPointTransform.Instance.PerformICP(myVerticesTarget, myVerticesSource);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static double Test5_CubeRotate(ref List<Vertex> myVerticesTarget, ref List<Vertex> myVerticesSource, ref List<Vertex> myVerticesResult)
        {

            myVerticesTarget = Vertices.CreateCube_Corners(50);

            myVerticesSource = VertexUtils.CloneListVertex(myVerticesTarget);

            Matrix3d R = new Matrix3d();
            R = R.RotateSome();
            VertexUtils.RotateVertices(myVerticesSource, R);

            myVerticesResult = IterativeClosestPointTransform.Instance.PerformICP(myVerticesTarget, myVerticesSource);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static double Test5_CubeInhomogenous(ref List<Vertex> myVerticesTarget, ref List<Vertex> myVerticesSource, ref List<Vertex> myVerticesResult)
        {

            myVerticesTarget = Vertices.CreateCube_Corners(50);
            myVerticesSource = VertexUtils.CloneListVertex(myVerticesTarget);

            VertexUtils.ScaleByVector(myVerticesSource, new Vertex(1,2,3));
            
            myVerticesResult = IterativeClosestPointTransform.Instance.PerformICP(myVerticesTarget, myVerticesSource);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static double Test5_CubeScale_Uniform(ref List<Vertex> myVerticesTarget, ref List<Vertex> myVerticesSource, ref List<Vertex> myVerticesResult)
        {

            myVerticesTarget = Vertices.CreateCube_Corners(50);

            myVerticesSource = VertexUtils.CloneListVertex(myVerticesTarget);
            VertexUtils.ScaleByFactor(myVerticesSource, 0.2);

            myVerticesResult = IterativeClosestPointTransform.Instance.PerformICP(myVerticesTarget, myVerticesSource);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static double Test5_CubeScale_Inhomogenous(ref List<Vertex> myVerticesTarget, ref List<Vertex> myVerticesSource, ref List<Vertex> myVerticesResult)
        {

            myVerticesTarget = Vertices.CreateCube_Corners(50);
            
            myVerticesSource = VertexUtils.CloneListVertex(myVerticesTarget);
            VertexUtils.ScaleByVector(myVerticesSource, new Vertex(1, 2, 3));
            
            myVerticesResult = IterativeClosestPointTransform.Instance.PerformICP(myVerticesTarget, myVerticesSource);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static double Test5_CubeRotateTranslate_ScaleUniform(ref List<Vertex> myVerticesTarget, ref List<Vertex> myVerticesSource, ref List<Vertex> myVerticesResult)
        {

            myVerticesTarget = Vertices.CreateCube_Corners(50);

            myVerticesSource = VertexUtils.CloneListVertex(myVerticesTarget);

            VertexUtils.TranslateVertices(myVerticesSource, 220, -300, 127);
            VertexUtils.ScaleByFactor(myVerticesSource, 0.2);
            
            Matrix3d R = new Matrix3d();
            R = R.RotateSome();
            VertexUtils.RotateVertices(myVerticesSource, R);

            myVerticesResult = IterativeClosestPointTransform.Instance.PerformICP(myVerticesTarget, myVerticesSource);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static double Test5_CubeRotateTranslate_ScaleInhomogenous(ref List<Vertex> myVerticesTarget, ref List<Vertex> myVerticesSource, ref List<Vertex> myVerticesResult)
        {

            myVerticesTarget = Vertices.CreateCube_Corners(50);

            myVerticesSource = VertexUtils.CloneListVertex(myVerticesTarget);

            VertexUtils.TranslateVertices(myVerticesSource, 0, 0, 149);
            VertexUtils.ScaleByVector(myVerticesSource, new Vertex(1, 2, 3));

            Matrix3d R = new Matrix3d();
            R = R.RotateSome();
            VertexUtils.RotateVertices(myVerticesSource, R);

            myVerticesResult = IterativeClosestPointTransform.Instance.PerformICP(myVerticesTarget, myVerticesSource);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
     
        public static double Test6_Bunny(ref List<Vertex> myVerticesTarget, ref List<Vertex> myVerticesSource, ref List<Vertex> myVerticesResult)
        {


            string path = AppDomain.CurrentDomain.BaseDirectory + "TestData";
            
            Model3D model3DTarget = new Model3D(path + "\\bunny.obj");
            myVerticesTarget = model3DTarget.VertexList;
            Vertices.GetVertexMax(myVerticesTarget);

            myVerticesSource = VertexUtils.CloneListVertex(myVerticesTarget);

            VertexUtils.TranslateVertices(myVerticesSource, -0.15, 0.05, 0.02);
            Matrix3d R = new Matrix3d();
            R = R.Rotation30Degrees();
            VertexUtils.RotateVertices(myVerticesSource, R);
            VertexUtils.ScaleByFactor(myVerticesSource, 0.8);


            myVerticesResult = IterativeClosestPointTransform.Instance.PerformICP(myVerticesTarget, myVerticesSource);

            return IterativeClosestPointTransform.Instance.MeanDistance;


        }
        public static double Test6_Bunny_ExpectedError(ref List<Vertex> myVerticesTarget, ref List<Vertex> myVerticesSource, ref List<Vertex> myVerticesResult)
        {


            string path = AppDomain.CurrentDomain.BaseDirectory + "TestData";

            Model3D model3DTarget = new Model3D(path + "\\bunny.obj");
            myVerticesTarget = model3DTarget.VertexList;
            Vertices.GetVertexMax(myVerticesTarget);

            myVerticesSource = VertexUtils.CloneListVertex(myVerticesTarget);


            
            Matrix3d R = new Matrix3d();
            //ICP converges with a rotation of
            //R = R.RotationXYZ(60, 60, 60);
            R = R.RotationXYZ(65, 65, 65);
            

            VertexUtils.RotateVertices(myVerticesSource, R);
            

            myVerticesResult = IterativeClosestPointTransform.Instance.PerformICP(myVerticesTarget, myVerticesSource);

            return IterativeClosestPointTransform.Instance.MeanDistance;


        }
        public static double Test7_Face_KnownTransformation(ref List<Vertex> myVerticesTarget, ref List<Vertex> myVerticesSource, ref List<Vertex> myVerticesResult)
        {


            string path = AppDomain.CurrentDomain.BaseDirectory + "TestData";
            Model3D model3DTarget = new Model3D(path + "\\KinectFace1.obj");
            myVerticesTarget = model3DTarget.VertexList;
            Vertices.GetVertexMax(myVerticesTarget);


            myVerticesSource = VertexUtils.CloneListVertex(myVerticesTarget);


            VertexUtils.ScaleByFactor(myVerticesSource, 0.9);
            Matrix3d R = new Matrix3d();
            R = R.Rotation60Degrees();
            VertexUtils.RotateVertices(myVerticesSource, R);
            VertexUtils.TranslateVertices(myVerticesSource, 0.3, 0.5, -0.4);


            myVerticesResult = IterativeClosestPointTransform.Instance.PerformICP(myVerticesTarget, myVerticesSource);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
    
        public static double Test8_CubeOutliers_Translate(ref List<Vertex> myVerticesTarget, ref List<Vertex> myVerticesSource, ref List<Vertex> myVerticesResult)
        {

            myVerticesTarget = Vertices.CreateCube_Corners(20);
            myVerticesSource = VertexUtils.CloneListVertex(myVerticesTarget);
            VertexUtils.TranslateVertices(myVerticesSource, 0, -300, 0);
            VertexUtils.CreateOutliers(myVerticesSource, 5);
            
            
            myVerticesResult = IterativeClosestPointTransform.Instance.PerformICP(myVerticesTarget, myVerticesSource);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static double Test8_CubeOutliers_Rotate(ref List<Vertex> myVerticesTarget, ref List<Vertex> myVerticesSource, ref List<Vertex> myVerticesResult)
        {

            //myVerticesTarget = Vertices.CreateCube_Corners(50);
            Model3D myModel = Example3DModels.Cuboid("Cuboid", 20f, 40f, 100, new Vector3d(1, 1, 1), null);
            myVerticesTarget = myModel.VertexList;

            myVerticesSource = VertexUtils.CloneListVertex(myVerticesTarget);
            Matrix3d R = new Matrix3d();
            R = R.Rotation30Degrees();
            VertexUtils.RotateVertices(myVerticesSource, R);

            VertexUtils.CreateOutliers(myVerticesSource, 5);
            

            myVerticesResult = IterativeClosestPointTransform.Instance.PerformICP(myVerticesTarget, myVerticesSource);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static double Test9_Inhomogenous(ref List<Vertex> myVerticesTarget, ref List<Vertex> myVerticesSource, ref List<Vertex> myVerticesResult)
        {
            myVerticesTarget = Vertices.CreateCube_Corners(50);
            //myVerticesTarget = Vertices.CreateSomePoints();
            myVerticesSource = VertexUtils.CloneListVertex(myVerticesTarget);

            VertexUtils.InhomogenousTransform(myVerticesSource, 2);

            myVerticesResult = IterativeClosestPointTransform.Instance.PerformICP(myVerticesTarget, myVerticesSource);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static double Test9_Face_Stitch(ref List<Vertex> myVerticesTarget, ref List<Vertex> myVerticesSource, ref List<Vertex> myVerticesResult)
        {


            string path = AppDomain.CurrentDomain.BaseDirectory + "TestData";
            Model3D model3DTarget = new Model3D(path + "\\KinectFace1.obj");
            myVerticesTarget = model3DTarget.VertexList;
            Vertices.GetVertexMax(myVerticesTarget);

            Model3D model3DSource = new Model3D(path + "\\KinectFace2.obj");
            myVerticesSource = model3DSource.VertexList;
            Vertices.GetVertexMax(myVerticesSource);


            myVerticesResult = IterativeClosestPointTransform.Instance.PerformICP(myVerticesTarget, myVerticesSource);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static bool CheckResult(List<Vertex> myVerticesTarget, List<Vertex> myVerticesResult, double threshold)
        {
            if (myVerticesResult == null || myVerticesTarget == null)
                return false;


            for (int i = 0; i < myVerticesTarget.Count; i++)
            {
                double dx = Math.Abs(myVerticesTarget[i].Vector.X - myVerticesResult[i].Vector.X);
                double dy = Math.Abs(myVerticesTarget[i].Vector.Y - myVerticesResult[i].Vector.Y);
                double dz = Math.Abs(myVerticesTarget[i].Vector.Z - myVerticesResult[i].Vector.Z);
                if (double.IsNaN(dx) || double.IsNaN(dy) || double.IsNaN(dz))
                    return false;
                if (dx > threshold || dy > threshold || dz > threshold)
                    return false;
                //needs a lot of exection time - only for error cases  
                //Vector3d v = new Vector3d(dx, dy, dz);
                //Debug.WriteLine(i.ToString() + "Vector is OK, distance difference is: " + v.Length.ToString());

            }
            return true;
        }
     
  
   
    
    }
}
