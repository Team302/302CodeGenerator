
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.configurationBrowseButton = new System.Windows.Forms.Button();
            this.configurationFilePathNameTextBox = new System.Windows.Forms.TextBox();
            this.outputFolderLabel = new System.Windows.Forms.Label();
            this.configuredOutputFolderLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.robotConfigurationFileLabel = new System.Windows.Forms.Label();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.progressTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(769, 434);
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
            this.button2.Location = new System.Drawing.Point(579, 447);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // configurationBrowseButton
            // 
            this.configurationBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.configurationBrowseButton.Location = new System.Drawing.Point(794, 12);
            this.configurationBrowseButton.Margin = new System.Windows.Forms.Padding(8);
            this.configurationBrowseButton.Name = "configurationBrowseButton";
            this.configurationBrowseButton.Size = new System.Drawing.Size(80, 36);
            this.configurationBrowseButton.TabIndex = 3;
            this.configurationBrowseButton.Text = "Browse";
            this.configurationBrowseButton.UseVisualStyleBackColor = true;
            this.configurationBrowseButton.Click += new System.EventHandler(this.configurationBrowseButton_Click);
            // 
            // configurationFilePathNameTextBox
            // 
            this.configurationFilePathNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.configurationFilePathNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.configurationFilePathNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.configurationFilePathNameTextBox.Location = new System.Drawing.Point(185, 12);
            this.configurationFilePathNameTextBox.Multiline = true;
            this.configurationFilePathNameTextBox.Name = "configurationFilePathNameTextBox";
            this.configurationFilePathNameTextBox.Size = new System.Drawing.Size(598, 36);
            this.configurationFilePathNameTextBox.TabIndex = 4;
            // 
            // outputFolderLabel
            // 
            this.outputFolderLabel.AutoSize = true;
            this.outputFolderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputFolderLabel.Location = new System.Drawing.Point(47, 96);
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
            this.configuredOutputFolderLabel.Location = new System.Drawing.Point(185, 96);
            this.configuredOutputFolderLabel.Name = "configuredOutputFolderLabel";
            this.configuredOutputFolderLabel.Size = new System.Drawing.Size(683, 29);
            this.configuredOutputFolderLabel.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 29);
            this.label1.TabIndex = 6;
            this.label1.Text = "Gen config file";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(47, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "Robot config";
            // 
            // robotConfigurationFileLabel
            // 
            this.robotConfigurationFileLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.robotConfigurationFileLabel.BackColor = System.Drawing.SystemColors.Control;
            this.robotConfigurationFileLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.robotConfigurationFileLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.robotConfigurationFileLabel.Location = new System.Drawing.Point(185, 142);
            this.robotConfigurationFileLabel.Name = "robotConfigurationFileLabel";
            this.robotConfigurationFileLabel.Size = new System.Drawing.Size(683, 29);
            this.robotConfigurationFileLabel.TabIndex = 7;
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.configurationGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.configurationGroupBox.Location = new System.Drawing.Point(12, 67);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(862, 124);
            this.configurationGroupBox.TabIndex = 8;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // progressTextBox
            // 
            this.progressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressTextBox.Location = new System.Drawing.Point(12, 197);
            this.progressTextBox.Multiline = true;
            this.progressTextBox.Name = "progressTextBox";
            this.progressTextBox.ReadOnly = true;
            this.progressTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.progressTextBox.Size = new System.Drawing.Size(862, 231);
            this.progressTextBox.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 495);
            this.Controls.Add(this.progressTextBox);
            this.Controls.Add(this.robotConfigurationFileLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.configuredOutputFolderLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.outputFolderLabel);
            this.Controls.Add(this.configurationFilePathNameTextBox);
            this.Controls.Add(this.configurationBrowseButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.configurationGroupBox);
            this.Name = "Form1";
            this.Text = "Team 302 code generator";
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.Label robotConfigurationFileLabel;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.TextBox progressTextBox;
    }
}

