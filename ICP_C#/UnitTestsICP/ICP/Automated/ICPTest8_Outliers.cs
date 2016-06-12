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
    public class ICPTest8_Outliers : ICPTestBase
    {

        [Test]
        public void Outliers_CubeTranslate_FixedPoints()
        {
            Reset();

            
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.FixedTestPoints = true;
            //KDTreeVertex.KDTreeMode = KDTreeMode.Rednaxala;
            IterativeClosestPointTransform.MaximumNumberOfIterations = 10;
          //  IterativeClosestPointTransform.SimulatedAnnealing = true;

            meanDistance = ICPTestData.Test8_CubeOutliers_Translate(ref verticesTarget, ref verticesSource, ref verticesResult);
           // this.ShowResultsInWindowIncludingLines(false);

            
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-3));

        }
    
     
     
    }
}
