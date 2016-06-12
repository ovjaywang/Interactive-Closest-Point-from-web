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
    public class ICPTest2_Rotation : ICPTestBase
    {
         
        
        [Test]
        public void RotationX_Horn()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.FixedTestPoints = true;
            meanDistance = ICPTestData.Test2_RotationX30Degrees(ref verticesTarget, ref verticesSource, ref verticesResult);

            
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));
        }
        
        [Test]
        public void RotationX_Umeyama()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Umeyama;
            IterativeClosestPointTransform.FixedTestPoints = true;
            meanDistance = ICPTestData.Test2_RotationX30Degrees(ref verticesTarget, ref verticesSource, ref verticesResult);

            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-5));
        }
        [Test]
        public void RotationX_Zinsser()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Zinsser;
            IterativeClosestPointTransform.FixedTestPoints = true;
            meanDistance = ICPTestData.Test2_RotationX30Degrees(ref verticesTarget, ref verticesSource, ref verticesResult);

            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-5));
        }
        [Test]
        public void RotationX_Du()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Zinsser;
            IterativeClosestPointTransform.FixedTestPoints = true;
            meanDistance = ICPTestData.Test2_RotationX30Degrees(ref verticesTarget, ref verticesSource, ref verticesResult);

            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-5));
        }
        [Test]
        public void RotationXYZ_Horn()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.FixedTestPoints = true;
            meanDistance = ICPTestData.Test2_RotationXYZ(ref verticesTarget, ref verticesSource, ref verticesResult);

            //this.ShowResultsInWindowIncludingLines(false);
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));
        }
        [Test]
        public void RotationXYZ_Umeyama()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Umeyama;
            IterativeClosestPointTransform.FixedTestPoints = true;
            meanDistance = ICPTestData.Test2_RotationXYZ(ref verticesTarget, ref verticesSource, ref verticesResult);

            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-5));
        }
        [Test]
        public void RotationXYZ_Zinsser()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Zinsser;
            IterativeClosestPointTransform.FixedTestPoints = true;
            meanDistance = ICPTestData.Test2_RotationXYZ(ref verticesTarget, ref verticesSource, ref verticesResult);

            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));
        }
        [Test]
        public void RotationXYZ_Du()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Du;
            IterativeClosestPointTransform.FixedTestPoints = true;
            meanDistance = ICPTestData.Test2_RotationXYZ(ref verticesTarget, ref verticesSource, ref verticesResult);

            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));
        }
     
    }
}
