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
    public class ICPTest1_Translation : ICPTestBase
    {
         
        
        [Test]
        public void Translation_Horn()
        {

            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Horn;
            meanDistance = ICPTestData.Test1_Translation(ref verticesTarget, ref verticesSource, ref verticesResult);
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));


        }
        [Test]
        public void Translation_Umeyama()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Umeyama;
            meanDistance = ICPTestData.Test1_Translation(ref verticesTarget, ref verticesSource, ref verticesResult);
            //have to check why Umeyama is not exact to e-10 - perhaps because of diagonalization lib (for the scale factor) 
            if (!ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-5))
            {
                System.Diagnostics.Debug.WriteLine("Translation Umeyama failed");
                Assert.Fail("Translation Umeyama failed");
            }
            
        }
        [Test]
        public void Translation_Du()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Du;
            meanDistance = ICPTestData.Test1_Translation(ref verticesTarget, ref verticesSource, ref verticesResult);
            if (!ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10))
            {
                System.Diagnostics.Debug.WriteLine("Translation Du failed");
                Assert.Fail("Translation Du failed");
            }

        }
        [Test]
        public void Translation_Zinsser()
        {
            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Scaling_Zinsser;
            meanDistance = ICPTestData.Test1_Translation(ref verticesTarget, ref verticesSource, ref verticesResult);
            if (!ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10))
            {
                System.Diagnostics.Debug.WriteLine("Translation Zinsser failed");
                Assert.Fail("Translation Zinsser failed");
            }

        }
   
     
    }
}
