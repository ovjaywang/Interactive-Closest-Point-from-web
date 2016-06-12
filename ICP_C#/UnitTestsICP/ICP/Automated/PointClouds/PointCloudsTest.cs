using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using OpenTKLib;
using KinectUtils;
using OpenTK;

namespace UnitTestsICP.Automated
{
    [TestFixture]
    [Category("UnitTest")]
    public class PointCloudsTest
    {
        private static string path;
        public PointCloudsTest()
        {
            path = AppDomain.CurrentDomain.BaseDirectory + "TestData";
            //string str = 

        }

   
        [Test]
        public void TransformPointCloud_SaveObjFile()
        {
            //open a file
            DepthMetaData DepthMetaData = new DepthMetaData();
            byte[] colorInfo = null;

           
            DepthMetaData.ReadDepthWithColor_OBJ(path, "KinectFace1.obj", ref DepthMetaData.FrameData, ref colorInfo);

            List<Vector3d> myVectors = Vertices.ConvertToVector3DList_FromArray(DepthMetaData.FrameData, DepthMetaData.XResDefault, DepthMetaData.YResDefault);
            List<float[]> myColorsFloats = PointCloudUtils.CreateColorInfo(colorInfo, DepthMetaData.FrameData, DepthMetaData.XResDefault, DepthMetaData.YResDefault);
            List<Vertex> myVertexList = Model3D.CreateVertexList(myVectors, myColorsFloats);
            
            VertexUtils.ScaleByFactor(myVertexList, 0.9);
            VertexUtils.RotateVertices30Degrees(myVertexList);
            VertexUtils.TranslateVertices(myVertexList, 10, 3, 5);

            Model3D.Save_ListVertices_Obj(myVertexList, path, "transformed.obj");

          
        }
         [Test]
        public void SaveObjFile()
        {
            //open a file
            DepthMetaData DepthMetaData = new DepthMetaData();
            byte[] colorInfo = null;

            //string fileName = "\\test.obj";
            DepthMetaData.ReadDepthWithColor_OBJ(path, "KinectFace1.obj", ref DepthMetaData.FrameData, ref colorInfo);

            PointCloudUtilsIO.Write_OBJ(colorInfo, DepthMetaData.FrameData, DepthMetaData.XResDefault, DepthMetaData.YResDefault, path, "test.obj");

            //PointCloudIO.Write_PLY(myColorPixels, this.DepthMetaData.FrameData, pathModels, FileNameColorInfoWithDepth);
            
            //List<Vertex> myVertexReference = this.OpenGLControl.GLrender.Models3D[0].Vertices;
            //List<Vertex> myVertexToBeMatched = this.OpenGLControl.GLrender.Models3D[1].Vertices;

        }
         
      
     
    }
}
