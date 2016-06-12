using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKLib;
using UnitTestsICP;
using ICPLib;


namespace UnitTestsICP.ExpectedError
{
    [TestFixture]
    [Category("UnitTest")]
    public class ICPTest9_Face_ExpectedError : ICPTestBase
    {
        

        [Test]
        public void Horn()
        {
            Reset();
            SettingsRealData();
            //as it does not converge anyway, keep the iteration number low
            IterativeClosestPointTransform.MaximumNumberOfIterations = 10;

            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Horn;
            
           
            meanDistance = ICPTestData.Test9_Face_Stitch(ref verticesTarget, ref verticesSource, ref verticesResult);

            ShowResultsInWindow(true);
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-3));
           
        }
        [Test]
        public void Umeyama()
        {
            Reset();
            SettingsRealData();
            
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Umeyama;
           
            
            meanDistance = ICPTestData.Test9_Face_Stitch(ref verticesTarget, ref verticesSource, ref verticesResult);

            ShowResultsInWindow(true);
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-3));

        }
      
          
     
    }
}
