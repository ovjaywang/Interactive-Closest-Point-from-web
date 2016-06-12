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
    public class ICPTest9_Face : ICPTestBase
    {
        

       
        [Test]
        public void Du()
        {
            Reset();
            SettingsRealData();
            IterativeClosestPointTransform.MaximumNumberOfIterations = 100;
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Du;
           

            meanDistance = ICPTestData.Test9_Face_Stitch(ref verticesTarget, ref verticesSource, ref verticesResult);

            ShowResultsInWindow(true);
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-3));

        }
          [Test]
        public void Umeyama_SA()
        {
            Reset();
            SettingsRealData();

            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Umeyama;
            
            IterativeClosestPointTransform.SimulatedAnnealing = true;


            meanDistance = ICPTestData.Test9_Face_Stitch(ref verticesTarget, ref verticesSource, ref verticesResult);

            ShowResultsInWindow(true);
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-3));

        }
          
     
    }
}
