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
using robotConfiguration;
using System.Reflection;

namespace FRCrobotCodeGen302
{
    public partial class MainForm : Form
    {
        toolConfiguration generatorConfig = new toolConfiguration();
        robotConfig theRobotConfiguration = new robotConfig();
        codeGenerator_302Robotics codeGenerator = new codeGenerator_302Robotics();
       
        public MainForm()
        {
            InitializeComponent();
            codeGenerator.setProgressCallback(addProgress);
            theRobotConfiguration.setProgressCallback(addProgress);
        }

        private void addProgress(string info)
        {
            progressTextBox.AppendText(info + "\r\n");
        }

        private void AddNode(TreeNode parent, object obj, string nodeName)
        {
            // add this object to the tree
            Type objType = obj.GetType();
            PropertyInfo thePropertyInfo = objType.GetProperties().ToList().Find(p => p.Name == "usage");
            
            if (thePropertyInfo != null)
                nodeName += "_" + thePropertyInfo.GetValue(obj);

            TreeNode tn = parent.Nodes.Add(nodeName);
            tn.Tag = obj;

            // if it is an array, add an entry for each item
            if (objType.IsArray)
            {
                Array a = (Array)obj;
                if (a.Length > 0)
                {
                    int index = 0;
                    foreach (var v in a)
                    {
                        AddNode(tn, v, v.GetType().ToString() + index);

                        index++;
                    }
                }
            }
            else
            {
                if (objType.FullName != "System.String")
                {
                    // add its children
                    foreach (PropertyInfo pi in objType.GetProperties())
                    {
                        object theObj = pi.GetValue(obj);

                        if (theObj != null)
                        {
                            AddNode(tn, theObj, pi.Name);
                        }
                    }
                }
            }
        }

        private void populateTree(robotConfig myRobot)
        {
            robotTreeView.Nodes.Clear();

            TreeNode robotNode = robotTreeView.Nodes.Add("Root");

            
            AddNode(robotNode, myRobot.theRobot, "Robot");

            //Type robotType = myRobot.theRobot.GetType();
            //FieldInfo[] fields = robotType.GetFields();
            //MemberInfo[] mems = robotType.GetMembers();
            //TypeAttributes ta =  robotType.Attributes;

            //foreach(PropertyInfo pi in robotType.GetProperties())
            //{
            //    object obj = pi.GetValue(myRobot.theRobot);
            //    TreeNode tn = robotNode.Nodes.Add(pi.PropertyType.Name);
            //    tn.Tag = obj;

            //    Type t = pi.PropertyType;
            //    if (t.IsArray)
            //    {
            //        Array objArray = pi.GetValue(myRobot.theRobot) as Array;
            //        if (objArray != null)
            //        {
            //            if (objArray.Length > 0)
            //            {
            //                int index = 0;
            //                foreach (var v in objArray)
            //                {
            //                    TreeNode n = tn.Nodes.Add(v.GetType().ToString() + index);
            //                    n.Tag = v;
            //                    index++;

            //                    foreach (PropertyInfo pii in v.GetType().GetProperties())
            //                    {
            //                        n.Nodes.Add(pii.Name);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            //foreach (mechanism m in myRobot.theRobot.mechanism)
            //{
            //    TreeNode n = robotTreeView.Nodes.Add("hej");
            //    n.Nodes.Add("sdjs");
            //}
        }
        public void loadGeneratorConfig(string configurationFullPathName)
        {
            try
            {
                generatorConfig = (toolConfiguration)generatorConfig.deserialize(configurationFullPathName);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot load the generator configuration. " + ex.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                codeGenerator.generate(theRobotConfiguration,generatorConfig);
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

                    addProgress("Loading the generator configuration file " + configurationFilePathNameTextBox.Text);
                    loadGeneratorConfig(configurationFilePathNameTextBox.Text);
                    addProgress("Configuration file loaded.");

                    generatorConfig.rootOutputFolder = Path.Combine(Path.GetDirectoryName(configurationFilePathNameTextBox.Text), generatorConfig.rootOutputFolder);
                    generatorConfig.robotConfiguration = Path.Combine(Path.GetDirectoryName(configurationFilePathNameTextBox.Text), generatorConfig.robotConfiguration);

                    theRobotConfiguration.load(generatorConfig.robotConfiguration);


                    addProgress("Populating the robot configuration tree view.");
                    populateTree(theRobotConfiguration);
                    addProgress("... Tree view populated.");

                    configuredOutputFolderLabel.Text = generatorConfig.rootOutputFolder;
                    robotConfigurationFileLabel.Text = generatorConfig.robotConfiguration;
                }
            }
        }

        private void robotTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
                MessageBox.Show(e.Node.Tag.GetType().ToString());
        }
    }
}
