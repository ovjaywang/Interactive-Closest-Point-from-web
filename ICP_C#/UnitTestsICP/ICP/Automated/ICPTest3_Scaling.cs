using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKLib;
using UnitTestsICP;
using ICPLib;


namespace UnitTestsICP.Automated
{
    [TestFixture]
    [Category("UnitTest")]
    public class ICPTest3_Scaling : ICPTestBase
    {
         
       
        [Test]
        public void Scale_Horn()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.FixedTestPoints = true;
            meanDistance = ICPTestData.Test3_Scale(ref verticesTarget, ref verticesSource, ref verticesResult);

            //this.ShowResultsInWindowIncludingLines(false);
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));
        }
        [Test]
        public void Scale_Umeyama()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Umeyama;
            IterativeClosestPointTransform.FixedTestPoints = true;
            meanDistance = ICPTestData.Test3_Scale(ref verticesTarget, ref verticesSource, ref verticesResult);

            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));
        }
        [Test]
        public void Scale_Zinsser()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Zinsser;
            IterativeClosestPointTransform.FixedTestPoints = true;
            meanDistance = ICPTestData.Test3_Scale(ref verticesTarget, ref verticesSource, ref verticesResult);

            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));
        }
        [Test]
        public void Scale_Du()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Du;
            IterativeClosestPointTransform.FixedTestPoints = true;
            meanDistance = ICPTestData.Test3_Scale(ref verticesTarget, ref verticesSource, ref verticesResult);

            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));
        }
        [Test]
        public void Scale_AllAxes_Du()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Du;
            IterativeClosestPointTransform.FixedTestPoints = true;
            meanDistance = ICPTestData.Test3_Scale(ref verticesTarget, ref verticesSource, ref verticesResult);

            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));
        }
        
      
    }
}
