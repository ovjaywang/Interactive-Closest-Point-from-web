using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKLib;
using ICPLib;


namespace UnitTestsICP
{
    //[TestFixture]
    //[Category("UnitTest")]
    public class ICPTestBase
    {
         protected static string path;
         protected List<Vertex> verticesTarget = null;
         protected List<Vertex> verticesSource = null;
         protected List<Vertex> verticesResult = null;

         protected List<Vertex> linesFrom = null;
         protected List<Vertex> linesTo = null;
         protected double meanDistance;

    


        protected void ShowResultsInWindow(bool changeColor)
        {
            ICPTestForm fOTK = new ICPTestForm();
            fOTK.ShowICPResults(verticesTarget, verticesSource, verticesResult, changeColor);
            fOTK.ShowDialog();

            
        }
    
         public ICPTestBase()
        {
            path = AppDomain.CurrentDomain.BaseDirectory + "TestData";
            //string str = 

        }
         protected void Reset()
         {
             verticesTarget = null;
             verticesSource = null;
             verticesResult = null;

             linesFrom = new List<Vertex>();
             linesTo = new List<Vertex>();
             IterativeClosestPointTransform.Reset();
         }
        /// <summary>
        /// for better showing results on OpenGL control
        /// </summary>
        /// <param name="myVertex"></param>
         private void CreateLinesForCube(List<Vertex> myVertex)
         {

             linesFrom.Add(myVertex[0]);
             linesTo.Add(myVertex[1]);

             linesFrom.Add(myVertex[1]);
             linesTo.Add(myVertex[5]);

             linesFrom.Add(myVertex[5]);
             linesTo.Add(myVertex[4]);

             linesFrom.Add(myVertex[4]);
             linesTo.Add(myVertex[0]);

             linesFrom.Add(myVertex[1]);
             linesTo.Add(myVertex[2]);

             linesFrom.Add(myVertex[2]);
             linesTo.Add(myVertex[3]);

             linesFrom.Add(myVertex[3]);
             linesTo.Add(myVertex[0]);


             linesFrom.Add(myVertex[3]);
             linesTo.Add(myVertex[7]);

             linesFrom.Add(myVertex[7]);
             linesTo.Add(myVertex[6]);

             linesFrom.Add(myVertex[2]);
             linesTo.Add(myVertex[6]);

             linesFrom.Add(myVertex[4]);
             linesTo.Add(myVertex[7]);

             linesFrom.Add(myVertex[5]);
             linesTo.Add(myVertex[6]);


         }

         protected void ShowResultsInWindow_CubeLines(bool changeColor)
         {

             //color code: 
             //Target is green
             //source : white
             //result : red

             //so - if there is nothing red on the OpenTK control, the result overlaps the target
             Vertices.SetColorOfListTo(verticesTarget, 0.0f, 1f, 0f, 1f);
             Vertices.SetColorOfListTo(verticesSource, 1f, 1f, 1f, 1f);
             if (verticesResult != null)
             {
                 Vertices.SetColorOfListTo(verticesResult, 1f, 0f, 0f, 1f);
                 CreateLinesForCube(verticesResult);
             }
             CreateLinesForCube(verticesTarget);
             CreateLinesForCube(verticesSource);
             

             ICPTestForm fOTK = new ICPTestForm();
             fOTK.ShowICPResults_WithLines(verticesTarget, verticesSource, verticesResult, this.linesFrom, this.linesTo, changeColor);
             fOTK.ShowDialog();


         }
        protected void CheckResult_MeanDistance(double threshold)
         {
             Assert.IsTrue(meanDistance - threshold < 0);


         }
     
       
        protected void CheckResult_Vectors()
        {
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-3));
        }
        protected void CheckResult_VectorsUltra()
        {
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));
        }
        protected void SettingsRealData()
        {
            IterativeClosestPointTransform.MaximumNumberOfIterations = 50;
            IterativeClosestPointTransform.FixedTestPoints = false;
            IterativeClosestPointTransform.ResetVertexToOrigin = true;
            KDTreeVertex.KDTreeMode = KDTreeMode.Rednaxala;
        }

    }
}
