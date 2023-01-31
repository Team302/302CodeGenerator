
namespace FRCrobotCodeGen302
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.configurationBrowseButton = new System.Windows.Forms.Button();
            this.configurationFilePathNameTextBox = new System.Windows.Forms.TextBox();
            this.outputFolderLabel = new System.Windows.Forms.Label();
            this.configuredOutputFolderLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.robotConfigurationFileComboBox = new System.Windows.Forms.ComboBox();
            this.progressTextBox = new System.Windows.Forms.TextBox();
            this.theTabControl = new System.Windows.Forms.TabControl();
            this.tabMainPage = new System.Windows.Forms.TabPage();
            this.clearReportButton = new System.Windows.Forms.Button();
            this.tabConfigurationPage = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.robotTreeView = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.deleteTreeElementButton = new System.Windows.Forms.Button();
            this.addStateDataFileButton = new System.Windows.Forms.Button();
            this.addTreeElementButton = new System.Windows.Forms.Button();
            this.valueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.saveConfigBbutton = new System.Windows.Forms.Button();
            this.valueComboBox = new System.Windows.Forms.ComboBox();
            this.valueTextBox = new System.Windows.Forms.TextBox();
            this.configurationGroupBox.SuspendLayout();
            this.theTabControl.SuspendLayout();
            this.tabMainPage.SuspendLayout();
            this.tabConfigurationPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valueNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(767, 404);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(105, 49);
            this.button1.TabIndex = 0;
            this.button1.Text = "Generate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(580, 417);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // configurationBrowseButton
            // 
            this.configurationBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.configurationBrowseButton.Location = new System.Drawing.Point(787, 14);
            this.configurationBrowseButton.Margin = new System.Windows.Forms.Padding(8);
            this.configurationBrowseButton.Name = "configurationBrowseButton";
            this.configurationBrowseButton.Size = new System.Drawing.Size(85, 36);
            this.configurationBrowseButton.TabIndex = 3;
            this.configurationBrowseButton.Text = "Browse";
            this.configurationBrowseButton.UseVisualStyleBackColor = true;
            this.configurationBrowseButton.Click += new System.EventHandler(this.configurationBrowseButton_Click);
            // 
            // configurationFilePathNameTextBox
            // 
            this.configurationFilePathNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.configurationFilePathNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.configurationFilePathNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.configurationFilePathNameTextBox.Location = new System.Drawing.Point(179, 16);
            this.configurationFilePathNameTextBox.Multiline = true;
            this.configurationFilePathNameTextBox.Name = "configurationFilePathNameTextBox";
            this.configurationFilePathNameTextBox.Size = new System.Drawing.Size(594, 33);
            this.configurationFilePathNameTextBox.TabIndex = 4;
            // 
            // outputFolderLabel
            // 
            this.outputFolderLabel.AutoSize = true;
            this.outputFolderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputFolderLabel.Location = new System.Drawing.Point(26, 39);
            this.outputFolderLabel.Name = "outputFolderLabel";
            this.outputFolderLabel.Size = new System.Drawing.Size(124, 25);
            this.outputFolderLabel.TabIndex = 5;
            this.outputFolderLabel.Text = "Output folder";
            // 
            // configuredOutputFolderLabel
            // 
            this.configuredOutputFolderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.configuredOutputFolderLabel.BackColor = System.Drawing.SystemColors.Control;
            this.configuredOutputFolderLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.configuredOutputFolderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.configuredOutputFolderLabel.Location = new System.Drawing.Point(164, 39);
            this.configuredOutputFolderLabel.Name = "configuredOutputFolderLabel";
            this.configuredOutputFolderLabel.Size = new System.Drawing.Size(682, 29);
            this.configuredOutputFolderLabel.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Gen config file";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(30, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "Robot config";
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.configurationGroupBox.Controls.Add(this.robotConfigurationFileComboBox);
            this.configurationGroupBox.Controls.Add(this.configuredOutputFolderLabel);
            this.configurationGroupBox.Controls.Add(this.label2);
            this.configurationGroupBox.Controls.Add(this.outputFolderLabel);
            this.configurationGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.configurationGroupBox.Location = new System.Drawing.Point(11, 55);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(861, 132);
            this.configurationGroupBox.TabIndex = 8;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // robotConfigurationFileComboBox
            // 
            this.robotConfigurationFileComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.robotConfigurationFileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.robotConfigurationFileComboBox.FormattingEnabled = true;
            this.robotConfigurationFileComboBox.Location = new System.Drawing.Point(164, 81);
            this.robotConfigurationFileComboBox.Name = "robotConfigurationFileComboBox";
            this.robotConfigurationFileComboBox.Size = new System.Drawing.Size(682, 33);
            this.robotConfigurationFileComboBox.TabIndex = 8;
            this.robotConfigurationFileComboBox.TextChanged += new System.EventHandler(this.robotConfigurationFileComboBox_TextChanged);
            // 
            // progressTextBox
            // 
            this.progressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressTextBox.Location = new System.Drawing.Point(11, 193);
            this.progressTextBox.Multiline = true;
            this.progressTextBox.Name = "progressTextBox";
            this.progressTextBox.ReadOnly = true;
            this.progressTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.progressTextBox.Size = new System.Drawing.Size(861, 205);
            this.progressTextBox.TabIndex = 9;
            // 
            // theTabControl
            // 
            this.theTabControl.Controls.Add(this.tabMainPage);
            this.theTabControl.Controls.Add(this.tabConfigurationPage);
            this.theTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.theTabControl.Location = new System.Drawing.Point(0, 0);
            this.theTabControl.Name = "theTabControl";
            this.theTabControl.SelectedIndex = 0;
            this.theTabControl.Size = new System.Drawing.Size(886, 495);
            this.theTabControl.TabIndex = 10;
            // 
            // tabMainPage
            // 
            this.tabMainPage.Controls.Add(this.clearReportButton);
            this.tabMainPage.Controls.Add(this.button2);
            this.tabMainPage.Controls.Add(this.label1);
            this.tabMainPage.Controls.Add(this.button1);
            this.tabMainPage.Controls.Add(this.configurationFilePathNameTextBox);
            this.tabMainPage.Controls.Add(this.progressTextBox);
            this.tabMainPage.Controls.Add(this.configurationBrowseButton);
            this.tabMainPage.Controls.Add(this.configurationGroupBox);
            this.tabMainPage.Location = new System.Drawing.Point(4, 29);
            this.tabMainPage.Name = "tabMainPage";
            this.tabMainPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabMainPage.Size = new System.Drawing.Size(878, 462);
            this.tabMainPage.TabIndex = 0;
            this.tabMainPage.Text = "Main";
            this.tabMainPage.UseVisualStyleBackColor = true;
            // 
            // clearReportButton
            // 
            this.clearReportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clearReportButton.Location = new System.Drawing.Point(11, 404);
            this.clearReportButton.Name = "clearReportButton";
            this.clearReportButton.Size = new System.Drawing.Size(185, 49);
            this.clearReportButton.TabIndex = 10;
            this.clearReportButton.Text = "Clear report window";
            this.clearReportButton.UseVisualStyleBackColor = true;
            this.clearReportButton.Click += new System.EventHandler(this.clearReportButton_Click);
            // 
            // tabConfigurationPage
            // 
            this.tabConfigurationPage.Controls.Add(this.splitContainer1);
            this.tabConfigurationPage.Location = new System.Drawing.Point(4, 29);
            this.tabConfigurationPage.Name = "tabConfigurationPage";
            this.tabConfigurationPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfigurationPage.Size = new System.Drawing.Size(878, 462);
            this.tabConfigurationPage.TabIndex = 1;
            this.tabConfigurationPage.Text = "Configuration";
            this.tabConfigurationPage.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.robotTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(872, 456);
            this.splitContainer1.SplitterDistance = 305;
            this.splitContainer1.TabIndex = 1;
            // 
            // robotTreeView
            // 
            this.robotTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.robotTreeView.HideSelection = false;
            this.robotTreeView.Location = new System.Drawing.Point(0, 0);
            this.robotTreeView.Name = "robotTreeView";
            this.robotTreeView.Size = new System.Drawing.Size(305, 456);
            this.robotTreeView.TabIndex = 0;
            this.robotTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.robotTreeView_AfterSelect);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.deleteTreeElementButton);
            this.panel1.Controls.Add(this.addStateDataFileButton);
            this.panel1.Controls.Add(this.addTreeElementButton);
            this.panel1.Controls.Add(this.valueNumericUpDown);
            this.panel1.Controls.Add(this.saveConfigBbutton);
            this.panel1.Controls.Add(this.valueComboBox);
            this.panel1.Controls.Add(this.valueTextBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(563, 456);
            this.panel1.TabIndex = 2;
            // 
            // deleteTreeElementButton
            // 
            this.deleteTreeElementButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteTreeElementButton.Location = new System.Drawing.Point(26, 156);
            this.deleteTreeElementButton.Name = "deleteTreeElementButton";
            this.deleteTreeElementButton.Size = new System.Drawing.Size(230, 37);
            this.deleteTreeElementButton.TabIndex = 6;
            this.deleteTreeElementButton.Text = "Delete";
            this.deleteTreeElementButton.UseVisualStyleBackColor = true;
            this.deleteTreeElementButton.Click += new System.EventHandler(this.deleteTreeElementButton_Click);
            // 
            // addStateDataFileButton
            // 
            this.addStateDataFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addStateDataFileButton.Location = new System.Drawing.Point(26, 199);
            this.addStateDataFileButton.Name = "addStateDataFileButton";
            this.addStateDataFileButton.Size = new System.Drawing.Size(230, 37);
            this.addStateDataFileButton.TabIndex = 5;
            this.addStateDataFileButton.Text = "Add statedata file";
            this.addStateDataFileButton.UseVisualStyleBackColor = true;
            this.addStateDataFileButton.Click += new System.EventHandler(this.addStateDataButton_Click);
            // 
            // addTreeElementButton
            // 
            this.addTreeElementButton.Location = new System.Drawing.Point(26, 113);
            this.addTreeElementButton.Name = "addTreeElementButton";
            this.addTreeElementButton.Size = new System.Drawing.Size(230, 37);
            this.addTreeElementButton.TabIndex = 4;
            this.addTreeElementButton.Text = "button3";
            this.addTreeElementButton.UseVisualStyleBackColor = true;
            this.addTreeElementButton.Visible = false;
            this.addTreeElementButton.Click += new System.EventHandler(this.addTreeElementButton_Click);
            // 
            // valueNumericUpDown
            // 
            this.valueNumericUpDown.Location = new System.Drawing.Point(26, 81);
            this.valueNumericUpDown.Name = "valueNumericUpDown";
            this.valueNumericUpDown.Size = new System.Drawing.Size(230, 26);
            this.valueNumericUpDown.TabIndex = 3;
            this.valueNumericUpDown.Visible = false;
            this.valueNumericUpDown.ValueChanged += new System.EventHandler(this.valueNumericUpDown_ValueChanged);
            // 
            // saveConfigBbutton
            // 
            this.saveConfigBbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveConfigBbutton.Location = new System.Drawing.Point(153, 242);
            this.saveConfigBbutton.Name = "saveConfigBbutton";
            this.saveConfigBbutton.Size = new System.Drawing.Size(103, 37);
            this.saveConfigBbutton.TabIndex = 2;
            this.saveConfigBbutton.Text = "Save";
            this.saveConfigBbutton.UseVisualStyleBackColor = true;
            this.saveConfigBbutton.Click += new System.EventHandler(this.saveConfigBbutton_Click);
            // 
            // valueComboBox
            // 
            this.valueComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.valueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.valueComboBox.FormattingEnabled = true;
            this.valueComboBox.Location = new System.Drawing.Point(26, 47);
            this.valueComboBox.Name = "valueComboBox";
            this.valueComboBox.Size = new System.Drawing.Size(230, 28);
            this.valueComboBox.TabIndex = 1;
            this.valueComboBox.Visible = false;
            this.valueComboBox.SelectedValueChanged += new System.EventHandler(this.valueComboBox_SelectedValueChanged);
            // 
            // valueTextBox
            // 
            this.valueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.valueTextBox.Location = new System.Drawing.Point(26, 15);
            this.valueTextBox.Name = "valueTextBox";
            this.valueTextBox.Size = new System.Drawing.Size(230, 26);
            this.valueTextBox.TabIndex = 0;
            this.valueTextBox.Visible = false;
            this.valueTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 495);
            this.Controls.Add(this.theTabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Team 302 code generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            this.theTabControl.ResumeLayout(false);
            this.tabMainPage.ResumeLayout(false);
            this.tabMainPage.PerformLayout();
            this.tabConfigurationPage.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valueNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button configurationBrowseButton;
        private System.Windows.Forms.TextBox configurationFilePathNameTextBox;
        private System.Windows.Forms.Label outputFolderLabel;
        private System.Windows.Forms.Label configuredOutputFolderLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.TextBox progressTextBox;
        private System.Windows.Forms.TabControl theTabControl;
        private System.Windows.Forms.TabPage tabMainPage;
        private System.Windows.Forms.TabPage tabConfigurationPage;
        private System.Windows.Forms.TreeView robotTreeView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ComboBox valueComboBox;
        private System.Windows.Forms.TextBox valueTextBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button saveConfigBbutton;
        private System.Windows.Forms.NumericUpDown valueNumericUpDown;
        private System.Windows.Forms.Button addTreeElementButton;
        private System.Windows.Forms.Button addStateDataFileButton;
        private System.Windows.Forms.ComboBox robotConfigurationFileComboBox;
        private System.Windows.Forms.Button clearReportButton;
        private System.Windows.Forms.Button deleteTreeElementButton;
    }
}

