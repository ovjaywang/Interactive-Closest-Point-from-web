using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKLib;
using OpenTK;

using ICPLib;

namespace UnitTestsICP
{
    [TestFixture]
    [Category("UnitTest")]
    public class KDTreeTest : ICPTestBase
    {
        
        public KDTreeTest()
        {
            path = AppDomain.CurrentDomain.BaseDirectory + "TestData";
           
        }


        [Test]
        public void Cube_RotateScaleTranslate_KDTree_Stark()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Du;
            IterativeClosestPointTransform.FixedTestPoints = true;
            KDTreeVertex.KDTreeMode = KDTreeMode.Stark;
            IterativeClosestPointTransform.ResetVertexToOrigin = true;
            meanDistance = ICPTestData.Test5_CubeRotateTranslate_ScaleUniform(ref verticesTarget, ref verticesSource, ref verticesResult);


            this.ShowResultsInWindow_CubeLines(false);
            
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));
        }
        [Test]
        public void Cube_RotateScaleTranslate_KDTreeBruteForce()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Du;
            KDTreeVertex.KDTreeMode = KDTreeMode.BruteForce;
            IterativeClosestPointTransform.ResetVertexToOrigin = true;
            meanDistance = ICPTestData.Test5_CubeRotateTranslate_ScaleUniform(ref verticesTarget, ref verticesSource, ref verticesResult);


            this.ShowResultsInWindow_CubeLines(false);
            //
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));
        }
   
   
    }
}
