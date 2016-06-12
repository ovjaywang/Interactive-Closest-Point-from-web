using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKLib;
using ICPLib;

namespace UnitTestsICP.InWork
{
    [TestFixture]
    [Category("UnitTest")]
    public class ICPTest : ICPTestBase
    {
        
        [Test]
        public void ICP_Show_KnownTransformation()
        {
            
            ICPTestForm fOTK = new ICPTestForm();
            fOTK.OpenGLControl.RemoveAllModels();
            string fileNameLong = path + "\\KinectFace1.obj";
            fOTK.OpenGLControl.LoadModelFromFile(fileNameLong, true);

            fileNameLong = path + "\\transformed.obj";
            fOTK.OpenGLControl.LoadModelFromFile(fileNameLong, true);
            fOTK.ICP_OnCurrentModels();
            fOTK.ShowDialog();


        }
        [Test]
        public void Translation_Horn_Old()
        {

            Reset();
            IterativeClosestPointTransform.ICPVersion = ICP_VersionUsed.Horn;

            meanDistance = ICPTestData.Test1_Translation(ref verticesTarget, ref verticesSource, ref verticesResult);
            Assert.IsTrue(ICPTestData.CheckResult(verticesTarget, verticesResult, 1e-10));


            ICPTestForm fOTK = new ICPTestForm();
            fOTK.ShowICPResults(verticesTarget, verticesSource, verticesResult, true);
            fOTK.ShowDialog();

        }
        [Test]
        public void ICP_Face_Old()
        {

            ICPTestForm fOTK = new ICPTestForm();
            fOTK.IPCOnTwoPointClouds();
            fOTK.ShowDialog();

        }

     
    }
}
