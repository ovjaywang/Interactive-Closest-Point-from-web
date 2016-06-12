using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKLib;
using ICPLib;


namespace UnitTestsICP.ExpectedError
{
    [TestFixture]
    [Category("UnitTest")]
    public class ICPTest6_Bunny_ExpectedError : ICPTestBase
    {
        
        
        [Test]
        public void Horn()
        {
            Reset();
            SettingsRealData();

            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Horn;

            meanDistance = ICPTestData.Test6_Bunny_ExpectedError(ref verticesTarget, ref verticesSource, ref verticesResult);
            ShowResultsInWindow(true);

            CheckResult_MeanDistance(1e-3);
           
        }
        [Test]
        public void Umeyama()
        {

            Reset();
            SettingsRealData();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Umeyama;

            meanDistance = ICPTestData.Test6_Bunny_ExpectedError(ref verticesTarget, ref verticesSource, ref verticesResult);
            ShowResultsInWindow(true);

            CheckResult_MeanDistance(1e-3);


        }
        [Test]
        public void Du()
        {

            Reset();
            SettingsRealData();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Du;

            meanDistance = ICPTestData.Test6_Bunny_ExpectedError(ref verticesTarget, ref verticesSource, ref verticesResult);

            ShowResultsInWindow(true);

            CheckResult_MeanDistance(1e-3);

        }
        [Test]
        public void Zinsser()
        {

            Reset();
            SettingsRealData();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Zinsser;


            meanDistance = ICPTestData.Test6_Bunny_ExpectedError(ref verticesTarget, ref verticesSource, ref verticesResult);

            ShowResultsInWindow(true);
            CheckResult_MeanDistance(1e-3);

        }
     
     
    }
}
