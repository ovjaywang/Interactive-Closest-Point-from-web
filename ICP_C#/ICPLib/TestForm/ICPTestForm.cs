using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using OpenTKLib;
using System.Windows.Media.Media3D;
using OpenTK;

namespace ICPLib
{
    public partial class ICPTestForm : Form
    {
       
        
        public OpenGLControl OpenGLControl;

        public ICPTestForm()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(CultureInfo.CurrentCulture.LCID);
            InitializeComponent();
            AddOpenGLControl();

        }
        private void AddOpenGLControl()
        {
            this.OpenGLControl = new OpenGLControl();
            this.SuspendLayout();
            // 
            // openGLControl1
            // 
            this.OpenGLControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OpenGLControl.Location = new System.Drawing.Point(0, 0);
            this.OpenGLControl.Name = "openGLControl1";
            this.OpenGLControl.Size = new System.Drawing.Size(854, 453);
            this.OpenGLControl.TabIndex = 0;

            panelOpenKinect.Controls.Add(this.OpenGLControl);
            //this.Controls.Add(this.openGLControl1);
           
            this.ResumeLayout(false);
        }
        public void ShowPointCloud(ushort[] depthInfo, int width, int height)
        {
            List<Vector3d> myVectors = Vertices.ConvertToVector3DList_FromArray(depthInfo, width, height);
            this.OpenGLControl.ShowPointCloud("Depth Point Cloud", myVectors, null);
           

        }
   
        //private void stitchToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Stitch();
        //}
       
        private void iCPTestcaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Vertex> myVerticesTarget = null;
            List<Vertex> myVerticesSource = null;
            List<Vertex> myVerticesResult = null;


            ICPTestData.Test1_Translation(ref myVerticesTarget, ref myVerticesSource, ref myVerticesResult);

            ShowICPResults(myVerticesTarget, myVerticesSource, myVerticesResult, true);
        }
    
        private void iCPOnStitchedPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICP_OnCurrentModels();
            

        }

        private void iCPTrialWorkflowOnLoadedPCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IPCOnTwoPointClouds();

        }
     
    }
}
