namespace TruSaber.DebugTool
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cameraView1 = new TruSaber.DebugTool.CameraView();
            this.cameraView2 = new TruSaber.DebugTool.CameraView();
            this.cameraView3 = new TruSaber.DebugTool.CameraView();
            this.cameraView4 = new TruSaber.DebugTool.CameraView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(348, 835);
            this.propertyGrid1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.propertyGrid1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(1363, 835);
            this.splitContainer1.SplitterDistance = 348;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.Controls.Add(this.cameraView1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cameraView2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cameraView3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cameraView4, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1010, 835);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // cameraView1
            // 
            this.cameraView1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.cameraView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cameraView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraView1.Location = new System.Drawing.Point(4, 3);
            this.cameraView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cameraView1.Name = "cameraView1";
            this.cameraView1.Size = new System.Drawing.Size(497, 411);
            this.cameraView1.TabIndex = 0;
            this.cameraView1.Load += new System.EventHandler(this.cameraView1_Load);
            // 
            // cameraView2
            // 
            this.cameraView2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.cameraView2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cameraView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraView2.HeaderText = "Front";
            this.cameraView2.Location = new System.Drawing.Point(509, 3);
            this.cameraView2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cameraView2.Name = "cameraView2";
            this.cameraView2.Size = new System.Drawing.Size(497, 411);
            this.cameraView2.TabIndex = 1;
            this.cameraView2.ViewType = TruSaber.DebugTool.CameraViewType.Front;
            this.cameraView2.Load += new System.EventHandler(this.cameraView2_Load);
            // 
            // cameraView3
            // 
            this.cameraView3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.cameraView3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cameraView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraView3.HeaderText = "Right";
            this.cameraView3.Location = new System.Drawing.Point(509, 420);
            this.cameraView3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cameraView3.Name = "cameraView3";
            this.cameraView3.Size = new System.Drawing.Size(497, 412);
            this.cameraView3.TabIndex = 2;
            this.cameraView3.ViewType = TruSaber.DebugTool.CameraViewType.Right;
            // 
            // cameraView4
            // 
            this.cameraView4.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.cameraView4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cameraView4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraView4.HeaderText = "FirstPerson";
            this.cameraView4.Location = new System.Drawing.Point(4, 420);
            this.cameraView4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cameraView4.Name = "cameraView4";
            this.cameraView4.Size = new System.Drawing.Size(497, 412);
            this.cameraView4.TabIndex = 3;
            this.cameraView4.ViewType = TruSaber.DebugTool.CameraViewType.FirstPerson;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1363, 835);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "TruSaber.DebugTool";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

        private System.Windows.Forms.SplitContainer splitContainer1;

        private System.Windows.Forms.PropertyGrid propertyGrid1;

        #endregion

        private CameraView cameraView1;
        private CameraView cameraView2;
        private CameraView cameraView3;
        private CameraView cameraView4;
    }
}