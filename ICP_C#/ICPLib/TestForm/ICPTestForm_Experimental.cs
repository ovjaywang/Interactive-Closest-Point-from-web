using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.Windows.Media.Media3D;
using OpenTK;
using OpenTKLib;


namespace ICPLib
{
    public partial class ICPTestForm 
    {
        List<Point> pointsLeft;
        List<Point> pointsRight;

        List<Vector3d> lastPointCloud;
        List<float[]> lastColors;

        public void ShowPointCloud(byte[] mycolorInfo, ushort[] depthInfo, int width, int height)
        {

            List<Vector3d> myVectors = Vertices.ConvertToVector3DList_FromArray(depthInfo, width, height);
            List<float[]> myColors = PointCloudUtils.CreateColorInfo(mycolorInfo, depthInfo, width, height);
            this.lastPointCloud = myVectors;
            this.lastColors = myColors;

            this.OpenGLControl.ShowPointCloud("Color Point Cloud", myVectors, myColors);

          
        }
      

        //private void Stitch()
        //{
        //    AccordStitchForm accordStitch = new AccordStitchForm();
        //    accordStitch.ShowDialog();
        //    pointsLeft = accordStitch.PointsLeft;
        //    pointsRight = accordStitch.PointsRight;
        //}

      
        public void ShowICPResults(List<Vertex> myVerticesTarget, List<Vertex> myVerticesSource, List<Vertex> myTransformPoints, bool changeColor)
        {
            
            this.OpenGLControl.RemoveAllModels();

            //target in green
            List<float[]> myColors = PointCloudUtils.CreateColorList(myVerticesTarget.Count, 0, 255, 0, 255);
            if(myVerticesTarget != null)
            {
                
                if (changeColor)
                    Vertices.SetColorToList(myVerticesTarget, myColors);
                this.OpenGLControl.ShowPointCloud("ICP Target", myVerticesTarget);

            }

            if(myVerticesSource != null)
            {
                //source in white
                myColors = PointCloudUtils.CreateColorList(myVerticesSource.Count, 255, 255, 255, 255);
                if (changeColor)
                    Vertices.SetColorToList(myVerticesSource, myColors);
                this.OpenGLControl.ShowPointCloud("ICP To be matched", myVerticesSource);

            }

            if (myTransformPoints != null)
            {

                //transformed in red
                myColors = PointCloudUtils.CreateColorList(myTransformPoints.Count, 255, 0, 0, 255);
                Vertices.SetColorToList(myTransformPoints, myColors);
                this.OpenGLControl.ShowPointCloud("ICP Solution", myTransformPoints);

            }

        }
        public void ShowICPResults_WithLines(List<Vertex> myVerticesTarget, List<Vertex> myVerticesSource, List<Vertex> myTransformPoints, List<Vertex> myLinesFrom, List<Vertex> myLinesTo, bool changeColor)
        {
            this.OpenGLControl.GLrender.LinesFrom = myLinesFrom;
            this.OpenGLControl.GLrender.LinesTo = myLinesTo;

            ShowICPResults(myVerticesTarget, myVerticesSource, myTransformPoints, changeColor);
           


        }
        private void ResetModelsToOrigin()
        {
            
            List<List<Vertex>> myTempList = new List<List<Vertex>>();
            List<string> myNames = new List<string>();
            for (int i = 0; i < OpenGLControl.GLrender.Models3D.Count; i++)
            {
                List<Vertex> myVertices = OpenGLControl.GLrender.Models3D[i].VertexList;
                myTempList.Add(myVertices);
                myNames.Add(OpenGLControl.GLrender.Models3D[i].Name);
            }
            OpenGLControl.RemoveAllModels();


            for (int i = 0; i < myTempList.Count; i++)
            {
                
                List<Vertex> myVertices = myTempList[i];
                Vertices.GetVertexMax(myVertices);
                this.OpenGLControl.ShowPointCloud(myNames[i], myVertices);
                
            }
            
        }
        public void ICP_OnCurrentModels()
        {
            
            //convert Points
            if (this.OpenGLControl.GLrender.Models3D.Count > 1)
            {
                List<Vertex> myVertexReference = this.OpenGLControl.GLrender.Models3D[0].VertexList;
                List<Vertex> myVertexToBeMatched = this.OpenGLControl.GLrender.Models3D[1].VertexList;

               
                ResetModelsToOrigin();

                IterativeClosestPointTransform icpSharp = new IterativeClosestPointTransform();
                List<Vertex> myVertexTransformed = icpSharp.PerformICP(myVertexReference, myVertexToBeMatched);

                if (myVertexTransformed != null)
                {
                    //show result
                    Vertices.SetColorOfListTo(myVertexTransformed, 1, 0, 0, 1);
                    this.OpenGLControl.ShowPointCloud("IPC Solution", myVertexTransformed);
                }

            }
           

        }
        public void IPCOnTwoPointClouds()
        {

            this.OpenGLControl.RemoveAllModels();
            this.OpenGLControl.OpenTwoTrialPointClouds();
            ICP_OnCurrentModels();

        }
        //private void IPCTestAutomated_Stitch()
        //{
        //    AccordStitchForm accordStitch = new AccordStitchForm();
        //    accordStitch.Show();
        //    accordStitch.TrialPoints();
        //    pointsLeft = accordStitch.PointsLeft;
        //    pointsRight = accordStitch.PointsRight;

        //    ICP_OnCurrentModels();

        //    accordStitch.Dispose();
        //    this.OpenGLControl.RemoveAllModels();
        //    this.OpenGLControl.OpenTwoTrialPointClouds();
        //    ICP_OnCurrentModels();

        //}
    }
}
