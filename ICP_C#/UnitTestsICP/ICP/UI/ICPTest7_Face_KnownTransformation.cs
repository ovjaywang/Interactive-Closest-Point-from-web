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
    public class ICPTest7_Face_KnownTransformation : ICPTestBase
    {
        

        [Test]
        public void Horn()
        {
            Reset();
            SettingsRealData();

            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.MaximumNumberOfIterations = 100;
            
            meanDistance = ICPTestData.Test7_Face_KnownTransformation(ref verticesTarget, ref verticesSource, ref verticesResult);

            ShowResultsInWindow(false);
            CheckResult_MeanDistance(1e-3);
           
        }
    
        [Test]
        public void Du()
        {

            Reset();
            SettingsRealData();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Du;
           
            meanDistance = ICPTestData.Test7_Face_KnownTransformation(ref verticesTarget, ref verticesSource, ref verticesResult);
            
            ShowResultsInWindow(false);
            CheckResult_MeanDistance(1e-3);

        }
        [Test]
        public void Zinsser()
        {
            Reset();
            SettingsRealData();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Zinsser;
           
            meanDistance = ICPTestData.Test7_Face_KnownTransformation(ref verticesTarget, ref verticesSource, ref verticesResult);
            ShowResultsInWindow(false);
            CheckResult_MeanDistance(1e-10);

        }
        [Test]
        public void Umeyama()
        {
            Reset();
            SettingsRealData();

            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Umeyama;
            meanDistance = ICPTestData.Test7_Face_KnownTransformation(ref verticesTarget, ref verticesSource, ref verticesResult);

            ShowResultsInWindow(false);
            CheckResult_MeanDistance(1e-10);

        }
     
    }
}
