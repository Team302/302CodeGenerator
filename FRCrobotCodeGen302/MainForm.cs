using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using Configuration;
using CoreCodeGenerator;

namespace FRCrobotCodeGen302
{
    public partial class MainForm : Form
    {
        codeGenerator_302Robotics codeGenerator = new codeGenerator_302Robotics();

        public MainForm()
        {
            InitializeComponent();
            codeGenerator.setProgressCallback(addProgress);
        }

        private void addProgress(string info)
        {
            progressTextBox.AppendText(info + "\r\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                addProgress("Loading the generator configuration file " + configurationFilePathNameTextBox.Text);
                codeGenerator.loadGeneratorConfig(configurationFilePathNameTextBox.Text);
                addProgress("Configuration file loaded.");

                codeGenerator.generate();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Something went wrong. See below. \r\n\r\n" + ex.Message, "Code generator error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            // Construct an instance of the XmlSerializer with the type
            // of object that is being deserialized.
            var mySerializer = new XmlSerializer(typeof(robot));
            // To read the file, create a FileStream.
            var myFileStream = new FileStream(@"C:\FRC\robot\deploy\robot_dm_.xml", FileMode.Create);
            robot myRobot = new robot();
            // Call the Deserialize method and cast to the object type.
            mySerializer.Serialize(myFileStream, myRobot);

            mySerializer = new XmlSerializer(typeof(statedata));
            // To read the file, create a FileStream.
            myFileStream = new FileStream(@"C:\FRC\robot\deploy\states\example_dm_.xml", FileMode.Create);
            statedata myStates = new statedata();
            // Call the Deserialize method and cast to the object type.
            mySerializer.Serialize(myFileStream, myStates);
        }

        private void configurationBrowseButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    configurationFilePathNameTextBox.Text = dlg.FileName;

                    codeGenerator.loadGeneratorConfig(configurationFilePathNameTextBox.Text);
                    configuredOutputFolderLabel.Text = codeGenerator.config.rootOutputFolder;
                    robotConfigurationFileLabel.Text = codeGenerator.config.robotConfiguration;
                }
            }
        }
    }
}
