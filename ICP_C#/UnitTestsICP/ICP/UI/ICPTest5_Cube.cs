using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKLib;
using UnitTestsICP;
using ICPLib;


namespace UnitTestsICP.UI
{
    [TestFixture]
    [Category("UnitTest")]
    public class ICPTest5_Cube : ICPTestBase
    {
             
        [Test]
        public void Cube_Translate()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.FixedTestPoints = true;
            IterativeClosestPointTransform.ResetVertexToOrigin = false;

            meanDistance = ICPTestData.Test5_CubeTranslation(ref verticesTarget, ref verticesSource, ref verticesResult);
          
            this.ShowResultsInWindow_CubeLines(false);

            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));
        }
        [Test]
        public void Cube_Rotate()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.FixedTestPoints = true;
            IterativeClosestPointTransform.ResetVertexToOrigin = false;

            meanDistance = ICPTestData.Test5_CubeRotate(ref verticesTarget, ref verticesSource, ref verticesResult);

            this.ShowResultsInWindow_CubeLines(false);

            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));
        }
        [Test]
        public void Cube_Scale_Uniform()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.FixedTestPoints = true;
            IterativeClosestPointTransform.ResetVertexToOrigin = false;

            meanDistance = ICPTestData.Test5_CubeScale_Uniform(ref verticesTarget, ref verticesSource, ref verticesResult);


            this.ShowResultsInWindow_CubeLines(false);

            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));
        }
        [Test]
        public void Cube_ScaleInhomogenous_Du()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Du;
            IterativeClosestPointTransform.FixedTestPoints = true;
            meanDistance = ICPTestData.Test5_CubeScale_Inhomogenous(ref verticesTarget, ref verticesSource, ref verticesResult);
           

            this.ShowResultsInWindow_CubeLines(false);
            //
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));
        }

        [Test]
        public void Cube_RotateTranslate_ScaleUniform_Umeyama()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Umeyama;
            IterativeClosestPointTransform.FixedTestPoints = true;
            KDTreeVertex.KDTreeMode = KDTreeMode.Rednaxala;
            meanDistance = ICPTestData.Test5_CubeRotateTranslate_ScaleUniform(ref verticesTarget, ref verticesSource, ref verticesResult);


            this.ShowResultsInWindow_CubeLines(false);
            //
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));
        }
        [Test]
        public void Cube_RotateTranslate_ScaleUniform_Du()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Du;
            IterativeClosestPointTransform.FixedTestPoints = true;
            KDTreeVertex.KDTreeMode = KDTreeMode.Rednaxala;
            meanDistance = ICPTestData.Test5_CubeRotateTranslate_ScaleUniform(ref verticesTarget, ref verticesSource, ref verticesResult);


            this.ShowResultsInWindow_CubeLines(false);
            //
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));
        }
        [Test]
        public void Cube_RotateTranslate_ScaleInhomegenous_Du()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Du;
            IterativeClosestPointTransform.FixedTestPoints = true;
            KDTreeVertex.KDTreeMode = KDTreeMode.Rednaxala;
            meanDistance = ICPTestData.Test5_CubeRotateTranslate_ScaleInhomogenous(ref verticesTarget, ref verticesSource, ref verticesResult);


            this.ShowResultsInWindow_CubeLines(false);
            //
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));
        }
 
     
    }
}
