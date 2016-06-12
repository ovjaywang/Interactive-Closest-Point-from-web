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
    public class ICPTest7_Face_KnownTransformation : ICPTestBase
    {
        

      
        [Test]
        public void Zinsser()
        {

            Reset();

            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Zinsser;
            KDTreeVertex.KDTreeMode = KDTreeMode.Rednaxala;
            IterativeClosestPointTransform.MaximumNumberOfIterations = 50;


            meanDistance = ICPTestData.Test7_Face_KnownTransformation(ref verticesTarget, ref verticesSource, ref verticesResult);

            //ShowResultsInWindow(false);
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-3));

        }
        [Test]
        public void Umeyama()
        {

            Reset();

            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Umeyama;
            KDTreeVertex.KDTreeMode = KDTreeMode.Rednaxala;
            IterativeClosestPointTransform.MaximumNumberOfIterations = 50;


            meanDistance = ICPTestData.Test7_Face_KnownTransformation(ref verticesTarget, ref verticesSource, ref verticesResult);

            //ShowResultsInWindow(false);
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-3));

        }
     
    }
}
