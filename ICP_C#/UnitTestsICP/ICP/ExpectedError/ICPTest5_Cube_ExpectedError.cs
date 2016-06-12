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
    public class ICPTest5_Cube_ExpectedError : ICPTestBase
    {
             
   
      
        [Test]
        public void Cube_ScaleInhomogenous_Horn()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.FixedTestPoints = true;
            meanDistance = ICPTestData.Test5_CubeScale_Inhomogenous(ref verticesTarget, ref verticesSource, ref verticesResult);
           

            this.ShowResultsInWindow_CubeLines(false);
            
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));
        }

    
     
     
    }
}
