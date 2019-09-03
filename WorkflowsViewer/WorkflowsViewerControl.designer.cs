namespace WorkflowsViewer
{
    partial class WorkflowsViewerControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listEntities = new System.Windows.Forms.Panel();
            this.lblEntities = new System.Windows.Forms.Label();
            this.listWorkflows = new System.Windows.Forms.Panel();
            this.txtWorkflowsHeader = new System.Windows.Forms.Label();
            this.panelProcessInfo = new System.Windows.Forms.Panel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtSearchEntity = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.txtSearchWorkflow = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tssSeparator1,
            this.tsbRefresh,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.txtSearchEntity,
            this.toolStripSeparator2,
            this.toolStripLabel2,
            this.txtSearchWorkflow});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripMenu.Size = new System.Drawing.Size(1153, 27);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(49, 24);
            this.tsbClose.Text = "Close";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(62, 24);
            this.tsbRefresh.Text = "Refresh";
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 27);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelProcessInfo);
            this.splitContainer1.Size = new System.Drawing.Size(1153, 483);
            this.splitContainer1.SplitterDistance = 384;
            this.splitContainer1.TabIndex = 27;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listEntities);
            this.splitContainer2.Panel1.Controls.Add(this.lblEntities);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listWorkflows);
            this.splitContainer2.Panel2.Controls.Add(this.txtWorkflowsHeader);
            this.splitContainer2.Size = new System.Drawing.Size(384, 483);
            this.splitContainer2.SplitterDistance = 128;
            this.splitContainer2.TabIndex = 21;
            // 
            // listEntities
            // 
            this.listEntities.AutoScroll = true;
            this.listEntities.BackColor = System.Drawing.SystemColors.Window;
            this.listEntities.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.listEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listEntities.Location = new System.Drawing.Point(0, 41);
            this.listEntities.Margin = new System.Windows.Forms.Padding(5, 2, 3, 5);
            this.listEntities.Name = "listEntities";
            this.listEntities.Size = new System.Drawing.Size(128, 442);
            this.listEntities.TabIndex = 7;
            // 
            // lblEntities
            // 
            this.lblEntities.BackColor = System.Drawing.SystemColors.Window;
            this.lblEntities.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblEntities.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblEntities.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEntities.Location = new System.Drawing.Point(0, 0);
            this.lblEntities.Margin = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.lblEntities.Name = "lblEntities";
            this.lblEntities.Size = new System.Drawing.Size(128, 41);
            this.lblEntities.TabIndex = 4;
            this.lblEntities.Text = "Entities";
            this.lblEntities.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listWorkflows
            // 
            this.listWorkflows.AutoScroll = true;
            this.listWorkflows.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.listWorkflows.BackColor = System.Drawing.SystemColors.Window;
            this.listWorkflows.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.listWorkflows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listWorkflows.Location = new System.Drawing.Point(0, 41);
            this.listWorkflows.Margin = new System.Windows.Forms.Padding(3, 2, 3, 5);
            this.listWorkflows.Name = "listWorkflows";
            this.listWorkflows.Size = new System.Drawing.Size(252, 442);
            this.listWorkflows.TabIndex = 6;
            // 
            // txtWorkflowsHeader
            // 
            this.txtWorkflowsHeader.BackColor = System.Drawing.SystemColors.Window;
            this.txtWorkflowsHeader.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtWorkflowsHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtWorkflowsHeader.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWorkflowsHeader.Location = new System.Drawing.Point(0, 0);
            this.txtWorkflowsHeader.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.txtWorkflowsHeader.Name = "txtWorkflowsHeader";
            this.txtWorkflowsHeader.Size = new System.Drawing.Size(252, 41);
            this.txtWorkflowsHeader.TabIndex = 5;
            this.txtWorkflowsHeader.Text = "Workflows";
            this.txtWorkflowsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelProcessInfo
            // 
            this.panelProcessInfo.AutoScroll = true;
            this.panelProcessInfo.BackColor = System.Drawing.SystemColors.Window;
            this.panelProcessInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelProcessInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProcessInfo.Location = new System.Drawing.Point(0, 0);
            this.panelProcessInfo.Margin = new System.Windows.Forms.Padding(3, 2, 5, 5);
            this.panelProcessInfo.Name = "panelProcessInfo";
            this.panelProcessInfo.Size = new System.Drawing.Size(765, 483);
            this.panelProcessInfo.TabIndex = 7;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(94, 24);
            this.toolStripLabel1.Text = "Search Entity";
            // 
            // txtSearchEntity
            // 
            this.txtSearchEntity.Name = "txtSearchEntity";
            this.txtSearchEntity.Size = new System.Drawing.Size(300, 27);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(120, 24);
            this.toolStripLabel2.Text = "Search Workflow";
            // 
            // txtSearchWorkflow
            // 
            this.txtSearchWorkflow.Name = "txtSearchWorkflow";
            this.txtSearchWorkflow.Size = new System.Drawing.Size(300, 27);
            // 
            // WorkflowsViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStripMenu);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "WorkflowsViewerControl";
            this.Size = new System.Drawing.Size(1153, 510);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripButton tsbRefresh;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel listEntities;
        private System.Windows.Forms.Label lblEntities;
        private System.Windows.Forms.Panel listWorkflows;
        private System.Windows.Forms.Label txtWorkflowsHeader;
        private System.Windows.Forms.Panel panelProcessInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox txtSearchEntity;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox txtSearchWorkflow;
    }
}
