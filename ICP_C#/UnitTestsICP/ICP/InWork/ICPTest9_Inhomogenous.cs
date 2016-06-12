using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKLib;
using ICPLib;

namespace UnitTestsICP.InWork
{
    [TestFixture]
    [Category("UnitTest")]
    public class ICPTest5_CubeInhomogenous : ICPTestBase
    {
         
      
        [Test]
        public void Prespective_Horn()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.FixedTestPoints = true;
            meanDistance = ICPTestData.Test9_Inhomogenous(ref verticesTarget, ref verticesSource, ref verticesResult);

            this.ShowResultsInWindow_CubeLines(false);
            CheckResult_MeanDistance(1e-3);

            
        }
        [Test]
        public void Prespective_Du()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Du;
            IterativeClosestPointTransform.FixedTestPoints = true;
            meanDistance = ICPTestData.Test9_Inhomogenous(ref verticesTarget, ref verticesSource, ref verticesResult);

            this.ShowResultsInWindow_CubeLines(false);
            CheckResult_MeanDistance(1e-3);
        }
        [Test]
        public void Prespective_Umeyama()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Umeyama;
            IterativeClosestPointTransform.FixedTestPoints = true;
            meanDistance = ICPTestData.Test9_Inhomogenous(ref verticesTarget, ref verticesSource, ref verticesResult);

            this.ShowResultsInWindow_CubeLines(false);
            CheckResult_MeanDistance(1e-7);
            
        }
        [Test]
        public void Prespective_Zinsser()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Zinsser;
            IterativeClosestPointTransform.FixedTestPoints = true;
            meanDistance = ICPTestData.Test9_Inhomogenous(ref verticesTarget, ref verticesSource, ref verticesResult);

            this.ShowResultsInWindow_CubeLines(false);
            CheckResult_MeanDistance(1e-3);
        }
       
    }
}
