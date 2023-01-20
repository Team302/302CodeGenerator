using Configuration;
using CoreCodeGenerator;
using robotConfiguration;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;

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
            {
                nodeName += "_" + thePropertyInfo.GetValue(obj);
            }
            else
            {
                thePropertyInfo = objType.GetProperties().ToList().Find(p => p.Name == "type");
                if (thePropertyInfo != null)
                {
                    nodeName += "_" + thePropertyInfo.GetValue(obj);
                }
            }

            if (objType.IsArray)
                nodeName += "s";

             TreeNode tn;

            if(parent == null)
                tn = robotTreeView.Nodes.Add(nodeName);
            else
                tn = parent.Nodes.Add(nodeName);

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
                PropertyInfo[] propertyInfos= objType.GetProperties();  

                if( (objType.FullName != "System.String") && (propertyInfos.Length > 0) )
                {
                    // add its children
                    foreach (PropertyInfo pi in propertyInfos)
                    {
                        object theObj = pi.GetValue(obj);

                        if (theObj != null)
                        {
                            AddNode(tn, theObj, pi.Name);
                        }
                    }
                }
                else
                {
                    // this means that this is a leaf node
                    leafNodeTag lnt = new leafNodeTag(obj.GetType(), nodeName);
                    tn.Tag = lnt;
                }
            }
        }

        private void populateTree(robotConfig myRobot)
        {
            robotTreeView.Nodes.Clear();
            AddNode(null, myRobot.theRobot, "Robot");
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

        TreeNode lastSelectedValueNode = null;
        bool enableCallback = false;
        private void robotTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            valueTextBox.Visible = false;
            valueComboBox.Visible = false;
            valueNumericUpDown.Visible = false;

            if (e.Node.Tag != null)
            {
                if (e.Node.GetNodeCount(false) == 0) // means that the node is a leaf
                {
                    lastSelectedValueNode = e.Node;

                    leafNodeTag lnt = (leafNodeTag)(e.Node.Tag);

                    PropertyInfo prop = lastSelectedValueNode.Parent.Tag.GetType().GetProperty(lnt.name, BindingFlags.Public | BindingFlags.Instance);
                    object value = null;
                    if (null != prop)
                    {
                        value = prop.GetValue(lastSelectedValueNode.Parent.Tag);
                    }

                    enableCallback = false;
                    if (lnt.type.IsEnum)
                    {
                        valueComboBox.Visible = true;
                        valueComboBox.Items.Clear();

                        string[] enumList = Enum.GetNames(lnt.type);
                        foreach (string en in enumList)
                            valueComboBox.Items.Add(en);

                        valueComboBox.SelectedIndex = valueComboBox.FindStringExact(value.ToString());
                    }
                    else if(value is uint)
                    {
                        valueNumericUpDown.Visible = true;
                        valueNumericUpDown.Value = (uint)value;
                    }
                    else
                    {
                        valueTextBox.Visible = true;
                        valueTextBox.Text = value.ToString();
                    }
                    enableCallback = true;
                }
                else
                    lastSelectedValueNode = null;
            }
            else
                lastSelectedValueNode = null;
        }

        private void valueComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (enableCallback)
            {
                if (lastSelectedValueNode != null)
                {
                    try
                    {
                        leafNodeTag lnt = (leafNodeTag)(lastSelectedValueNode.Tag);

                        PropertyInfo prop = lastSelectedValueNode.Parent.Tag.GetType().GetProperty(lnt.name, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                        {
                            prop.SetValue(lastSelectedValueNode.Parent.Tag, Enum.Parse(lnt.type, valueComboBox.Text));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to set " + lastSelectedValueNode.Text + " to " + valueComboBox.Text);
                    }
                }
            }
        }

        private void valueTextBox_TextChanged(object sender, EventArgs e)
        {
            if (enableCallback)
            {
                if (lastSelectedValueNode != null)
                {
                    try
                    {
                        leafNodeTag lnt = (leafNodeTag)(lastSelectedValueNode.Tag);

                        Type t = lastSelectedValueNode.Tag.GetType();
                        PropertyInfo prop = lastSelectedValueNode.Parent.Tag.GetType().GetProperty(lnt.name, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                        {
                            prop.SetValue(lastSelectedValueNode.Parent.Tag, valueTextBox.Text);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to set " + lastSelectedValueNode.Text + " to " + valueTextBox.Text);
                    }
                }
            }
        }
        private void valueNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (enableCallback)
            {
                if (lastSelectedValueNode != null)
                {
                    try
                    {
                        leafNodeTag lnt = (leafNodeTag)(lastSelectedValueNode.Tag);

                        Type t = lastSelectedValueNode.Tag.GetType();
                        PropertyInfo prop = lastSelectedValueNode.Parent.Tag.GetType().GetProperty(lnt.name, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                        {
                            prop.SetValue(lastSelectedValueNode.Parent.Tag, (uint)valueNumericUpDown.Value);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to set " + lastSelectedValueNode.Text + " to " + valueNumericUpDown.Text);
                    }
                }
            }
        }

        private void saveConfigBbutton_Click(object sender, EventArgs e)
        {
            theRobotConfiguration.save(generatorConfig.robotConfiguration);
        }


    }

    class leafNodeTag
    {
        public Type type { get; private set; }
        public string name { get; private set; }

        public leafNodeTag(Type type, string name)
        {
            this.type = type;
            this.name = name;
        }
    }
}
