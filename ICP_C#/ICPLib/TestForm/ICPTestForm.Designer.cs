using OpenTKLib;

namespace ICPLib
{
    partial class ICPTestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.experimentalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stitchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iCPOnStitchedPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iCPTestcaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelOpenKinect = new System.Windows.Forms.Panel();
            this.iCPTrialWorkflowOnLoadedPCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.experimentalToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1312, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(12, 20);
            // 
            // experimentalToolStripMenuItem
            // 
            this.experimentalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iCPTrialWorkflowOnLoadedPCToolStripMenuItem,
            this.stitchToolStripMenuItem,
            this.iCPOnStitchedPointsToolStripMenuItem,
            this.iCPTestcaseToolStripMenuItem});
            this.experimentalToolStripMenuItem.Name = "experimentalToolStripMenuItem";
            this.experimentalToolStripMenuItem.Size = new System.Drawing.Size(87, 20);
            this.experimentalToolStripMenuItem.Text = "Experimental";
            // 
            // stitchToolStripMenuItem
            // 
            this.stitchToolStripMenuItem.Name = "stitchToolStripMenuItem";
            this.stitchToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.stitchToolStripMenuItem.Text = "Stitch";
            
            // 
            // iCPOnStitchedPointsToolStripMenuItem
            // 
            this.iCPOnStitchedPointsToolStripMenuItem.Name = "iCPOnStitchedPointsToolStripMenuItem";
            this.iCPOnStitchedPointsToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.iCPOnStitchedPointsToolStripMenuItem.Text = "ICP on stitched points";
            this.iCPOnStitchedPointsToolStripMenuItem.Click += new System.EventHandler(this.iCPOnStitchedPointsToolStripMenuItem_Click);
            // 
            // iCPTestcaseToolStripMenuItem
            // 
            this.iCPTestcaseToolStripMenuItem.Name = "iCPTestcaseToolStripMenuItem";
            this.iCPTestcaseToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.iCPTestcaseToolStripMenuItem.Text = "ICP Testcase";
            this.iCPTestcaseToolStripMenuItem.Click += new System.EventHandler(this.iCPTestcaseToolStripMenuItem_Click);
            // 
            // panelOpenKinect
            // 
            this.panelOpenKinect.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelOpenKinect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOpenKinect.Location = new System.Drawing.Point(0, 24);
            this.panelOpenKinect.Name = "panelOpenKinect";
            this.panelOpenKinect.Size = new System.Drawing.Size(1312, 627);
            this.panelOpenKinect.TabIndex = 1;
            // 
            // iCPTrialWorkflowOnLoadedPCToolStripMenuItem
            // 
            this.iCPTrialWorkflowOnLoadedPCToolStripMenuItem.Name = "iCPTrialWorkflowOnLoadedPCToolStripMenuItem";
            this.iCPTrialWorkflowOnLoadedPCToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.iCPTrialWorkflowOnLoadedPCToolStripMenuItem.Text = "ICP Trial Workflow on loaded PC";
            this.iCPTrialWorkflowOnLoadedPCToolStripMenuItem.Click += new System.EventHandler(this.iCPTrialWorkflowOnLoadedPCToolStripMenuItem_Click);
            // 
            // OpenTKUtilsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 651);
            this.Controls.Add(this.panelOpenKinect);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "OpenTKUtilsForm";
            this.Text = "OpenTKTest";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem experimentalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stitchToolStripMenuItem;
        private System.Windows.Forms.Panel panelOpenKinect;
        private System.Windows.Forms.ToolStripMenuItem iCPOnStitchedPointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iCPTestcaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iCPTrialWorkflowOnLoadedPCToolStripMenuItem;

       
    }
}