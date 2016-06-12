using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKLib;
using UnitTestsICP;
using ICPLib;


namespace UnitTestsICP.InWork
{
    [TestFixture]
    [Category("UnitTest")]
    public class ICPTest8_Outliers : ICPTestBase
    {

        [Test]
        public void Outliers_CubeTranslate_FixedPoints()
        {
            Reset();
            
            
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.FixedTestPoints = true;
            //KDTreeVertex.KDTreeMode = KDTreeMode.Rednaxala;
            IterativeClosestPointTransform.MaximumNumberOfIterations = 10;


            meanDistance = ICPTestData.Test8_CubeOutliers_Translate(ref verticesTarget, ref verticesSource, ref verticesResult);
            this.ShowResultsInWindow_CubeLines(false);

            
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-3));

        }
        [Test]
        public void Outliers_CubeTranslate_NotGood()
        {
            Reset();
           

            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.MaximumNumberOfIterations = 10;

            meanDistance = ICPTestData.Test8_CubeOutliers_Translate(ref verticesTarget, ref verticesSource, ref verticesResult);

            this.ShowResultsInWindow_CubeLines(false);
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-3));

        }
        [Test]
        public void Outliers_CubeTranslate_DistanceOptimization()
        {
            Reset();


            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.MaximumNumberOfIterations = 10;
            IterativeClosestPointTransform.DistanceOptimization = true;

            meanDistance = ICPTestData.Test8_CubeOutliers_Translate(ref verticesTarget, ref verticesSource, ref verticesResult);

            this.ShowResultsInWindow_CubeLines(false);
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-3));

        }
        [Test]
        public void Outliers_CubeTranslate_NormalsCheck()
        {
            Reset();
            

            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.MaximumNumberOfIterations = 10;
            IterativeClosestPointTransform.NormalsCheck = true;

            meanDistance = ICPTestData.Test8_CubeOutliers_Translate(ref verticesTarget, ref verticesSource, ref verticesResult);

            this.ShowResultsInWindow_CubeLines(false);
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-3));
            
        }
        [Test]
        public void Face_NormalsCheck()
        {
            Reset();

            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Horn;
            KDTreeVertex.KDTreeMode = KDTreeMode.Rednaxala;
            IterativeClosestPointTransform.MaximumNumberOfIterations = 10;
            IterativeClosestPointTransform.NormalsCheck = true;

            meanDistance = ICPTestData.Test9_Face_Stitch(ref verticesTarget, ref verticesSource, ref verticesResult);

            ShowResultsInWindow(false);
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-3));

        }
        [Test]
        public void Outliers_CubeRotate()
        {
            Reset();
      
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.MaximumNumberOfIterations = 10;
            IterativeClosestPointTransform.SimulatedAnnealing = true;



            meanDistance = ICPTestData.Test8_CubeOutliers_Rotate(ref verticesTarget, ref verticesSource, ref verticesResult);

            ShowResultsInWindow(true);
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 2e-1));
           
        }
       
     
     
    }
}
