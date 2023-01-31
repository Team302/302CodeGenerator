using Configuration;
using CoreCodeGenerator;
using robotConfiguration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using Robot;
using StateData;
using System.Collections;
using System.Collections.ObjectModel;
using System.Web;
using System.Drawing;
using System.Deployment.Application;

namespace FRCrobotCodeGen302
{
    public partial class MainForm : Form
    {
        toolConfiguration generatorConfig = new toolConfiguration();
        robotConfig theRobotConfiguration = new robotConfig();
        codeGenerator_302Robotics codeGenerator = new codeGenerator_302Robotics();
        bool needsSaving = false;
        bool loadRobotConfig = false;

        public MainForm()
        {
            InitializeComponent();
            codeGenerator.setProgressCallback(addProgress);
            theRobotConfiguration.setProgressCallback(addProgress);
            clearNeedsSaving();

            this.Text += " Version " + ProductVersion;
        }

        private void addProgress(string info)
        {
            progressTextBox.AppendText(info + "\r\n");
        }

        private string getTreeNodeDisplayName(object parentObject, object obj, string nodeName)
        {
            if (isACollection(obj))
                nodeName += "s";
            else
            {
                Type objType = obj.GetType();
                PropertyInfo[] properties = objType.GetProperties();

                string nodeValueString = "";
                if ((properties.Length == 0) || (objType.FullName == "System.String"))
                {
                    if(parentObject != null)
                    {
                        PropertyInfo prop = parentObject.GetType().GetProperty(nodeName, BindingFlags.Public | BindingFlags.Instance);
                        object value = null;
                        if (null != prop)
                        {
                            value = prop.GetValue(parentObject);
                            nodeValueString = value.ToString();
                        }
                    }
                }
                else
                {
                    foreach (string s in generatorConfig.treeviewParentNameExtensions)
                    {
                        PropertyInfo propertyInfo = properties.ToList().Find(p => p.Name == s);
                        if (propertyInfo != null)
                            nodeValueString += propertyInfo.GetValue(obj) + ", ";
                    }
                }

                nodeName = getTreeNodeDisplayName(nodeValueString, nodeName);
            }

            return nodeName;
        }
        private string getTreeNodeDisplayName(string nodeValueString, string nodeName)
        {
            nodeValueString = nodeValueString.Trim();
            nodeValueString = nodeValueString.Trim(',');
            nodeValueString = nodeValueString.Trim();

            if (!string.IsNullOrEmpty(nodeValueString))
                nodeName += " (" + nodeValueString + ")";

            return nodeName;
        }


        private void AddNode(TreeNode parent, object obj, string nodeName)
        {
            // add this object to the tree
            string extendedNodeName = getTreeNodeDisplayName(parent==null?null:parent.Tag, obj, nodeName);

            TreeNode tn;
            if (parent == null)
                tn = robotTreeView.Nodes.Add(extendedNodeName);
            else
                tn = parent.Nodes.Add(extendedNodeName);

            tn.Tag = obj;

            // if it is a collection, add an entry for each item
            if (isACollection(obj))
            {
                ICollection ic = obj as ICollection;
                if (ic.Count > 0)
                {
                    int index = 0;
                    foreach (var v in ic)
                    {
                        AddNode(tn, v, v.GetType().Name + index);

                        index++;
                    }
                }
            }
            else
            {
                Type objType = obj.GetType();

                PropertyInfo[] propertyInfos= objType.GetProperties();  

                if( (objType.FullName != "System.String") && (propertyInfos.Length > 0) )
                {
                    // add its children
                    string previousName = "";
                    foreach (PropertyInfo pi in propertyInfos)
                    {
                        object theObj = pi.GetValue(obj);

                        //strings have to have some extra handling
                        if(pi.PropertyType.FullName == "System.String")
                        {
                            if (theObj == null)
                            {
                                theObj = "";
                                pi.SetValue(obj, "");
                            }
                        }

                        if (theObj != null)
                        {
                            if (pi.Name != previousName + "Specified")
                            {
                                AddNode(tn, theObj, pi.Name);
                                previousName = pi.Name;
                            }
                        }
                    }
                }
                else
                {
                    // this means that this is a leaf node
                    leafNodeTag lnt = new leafNodeTag(obj.GetType(), nodeName, obj);
                    tn.Tag = lnt;
                }
            }
        }

        private void populateTree(robotConfig myRobot)
        {
            robotTreeView.Nodes.Clear();
            AddNode(null, myRobot.theRobot, "Robot");
            foreach (KeyValuePair<string, statedata> kvp in myRobot.mechanismControlDefinition)
            {
                AddNode(null, kvp.Value, kvp.Key);
            }
        }


        public void loadGeneratorConfig(string configurationFullPathName)
        {
            try
            {
                generatorConfig = (toolConfiguration)generatorConfig.deserialize(configurationFullPathName);
                if(generatorConfig.robotConfigurations.Count == 0)
                {
                    generatorConfig.robotConfigurations = new List<string>();
                    if(!string.IsNullOrEmpty(generatorConfig.robotConfiguration.Trim()))
                        generatorConfig.robotConfigurations.Add(generatorConfig.robotConfiguration.Trim());
                }
                if(generatorConfig.treeviewParentNameExtensions.Count == 0)
                {
                    generatorConfig.treeviewParentNameExtensions.Add("usage");
                    generatorConfig.treeviewParentNameExtensions.Add("type");
                    generatorConfig.treeviewParentNameExtensions.Add("stateIdentifier");
                    generatorConfig.treeviewParentNameExtensions.Add("identifier");
                }
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
            try
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        configurationFilePathNameTextBox.Text = dlg.FileName;

                        addProgress("Loading the generator configuration file " + configurationFilePathNameTextBox.Text);
                        loadGeneratorConfig(configurationFilePathNameTextBox.Text);
                        addProgress("Configuration file loaded.");

                        loadRobotConfig = false;
                        #region Load the Combobox with the robot configuration file list and select the first one
                        robotConfigurationFileComboBox.Items.Clear();
                        foreach (string f in generatorConfig.robotConfigurations)
                        {
                            string fullfilePath = Path.Combine(Path.GetDirectoryName(configurationFilePathNameTextBox.Text), f);
                            fullfilePath = Path.GetFullPath(fullfilePath);
                            robotConfigurationFileComboBox.Items.Add(fullfilePath);
                        }
                        if (robotConfigurationFileComboBox.Items.Count > 0)
                            robotConfigurationFileComboBox.SelectedIndex = 0;
                        #endregion

                        generatorConfig.rootOutputFolder = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(configurationFilePathNameTextBox.Text), generatorConfig.rootOutputFolder));
                        generatorConfig.robotConfiguration = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(configurationFilePathNameTextBox.Text), robotConfigurationFileComboBox.Text));
                        loadRobotConfig = true;
                    }
                }
            }
            catch(Exception ex)
            {
                addProgress("Issue encountered while loading the generator configuration file\r\n" + ex.ToString());   
            }

            robotConfigurationFileComboBox_TextChanged(null, null);
        }

        private void robotConfigurationFileComboBox_TextChanged(object sender, EventArgs e)
        {
            if (loadRobotConfig)
            {
                try
                {
                    generatorConfig.robotConfiguration = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(configurationFilePathNameTextBox.Text), robotConfigurationFileComboBox.Text));

                    try
                    {
                        theRobotConfiguration.load(generatorConfig.robotConfiguration);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Issue encountered while loading the robot configuration file\r\n" + ex.ToString());
                    }

                    try
                    {
                        addProgress("Populating the robot configuration tree view.");
                        populateTree(theRobotConfiguration);
                        addProgress("... Tree view populated.");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Issue encountered while populating the robot configuration tree view\r\n" + ex.ToString());
                    }

                    configuredOutputFolderLabel.Text = generatorConfig.rootOutputFolder;

                    deleteTreeElementButton.Enabled = false;
                    clearNeedsSaving();
                }
                catch (Exception ex)
                {
                    addProgress(ex.ToString());
                }
            }
        }

        void setNeedsSaving()
        {
            needsSaving = true;
            saveConfigBbutton.Enabled = needsSaving;
        }

        void clearNeedsSaving()
        {
            needsSaving = false;
            saveConfigBbutton.Enabled = needsSaving;
        }

        TreeNode lastSelectedValueNode = null;
        TreeNode lastSelectedArrayNode = null;
        bool enableCallback = false;
        private void robotTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            valueTextBox.Visible = false;
            valueComboBox.Visible = false;
            valueNumericUpDown.Visible = false;
            addTreeElementButton.Visible = false;

            lastSelectedArrayNode = null;
            lastSelectedValueNode= null;

            deleteTreeElementButton.Enabled = isDeletable(e.Node);

            if (e.Node.Tag != null)
            {   
                if (isACollection( e.Node.Tag))
                {
                    lastSelectedArrayNode = e.Node;
                    addTreeElementButton.Text = "Add " + e.Node.Tag.GetType().GetElementType();
                    addTreeElementButton.Visible = true;
                }
                else if ((e.Node.Parent!=null) && (e.Node.Parent.Tag is robot))
                {
                    // do nothing
                }                
                else if (e.Node.GetNodeCount(false) == 0)
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
                    else if (value is uint)
                    {
                        valueNumericUpDown.DecimalPlaces = 0;
                        valueNumericUpDown.Value = (uint)value;
                        valueNumericUpDown.Visible = true;
                    }
                    else if (value is double)
                    {
                        valueNumericUpDown.DecimalPlaces = 5;
                        valueNumericUpDown.Value = Convert.ToDecimal(value);
                        valueNumericUpDown.Visible = true;
                    }
                    else if (lastSelectedValueNode.Text == "controlFile")
                    {
                        valueComboBox.Visible = true;
                        valueComboBox.Items.Clear();

                        string stateDataFilesPath = Path.Combine(Path.GetDirectoryName(generatorConfig.robotConfiguration), "states");

                        string[] files = Directory.GetFiles(stateDataFilesPath, "*.xml");
                        foreach(string f in files)
                            valueComboBox.Items.Add(Path.GetFileName(f));

                        valueComboBox.SelectedIndex = valueComboBox.FindStringExact(value.ToString());
                    }
                    else
                    {
                        valueTextBox.Visible = true;
                        valueTextBox.Text = value.ToString();
                    }
                    enableCallback = true;
                }
            }
        }

        #region handle the events when values are changed
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
                            if (lastSelectedValueNode.Text == "controlFile")
                                prop.SetValue(lastSelectedValueNode.Parent.Tag, valueComboBox.Text);
                            else
                            {
                                prop.SetValue(lastSelectedValueNode.Parent.Tag, Enum.Parse(lnt.type, valueComboBox.Text));
                                lastSelectedValueNode.Parent.Text = getTreeNodeDisplayName(null, lastSelectedValueNode.Parent.Tag, lastSelectedValueNode.Parent.Tag.GetType().Name);
                            }

                            lastSelectedValueNode.Text = getTreeNodeDisplayName(valueComboBox.Text, lnt.name);
                            setNeedsSaving();
                        }
                    }
                    catch (Exception)
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

                        lastSelectedValueNode.Text = getTreeNodeDisplayName(valueTextBox.Text, lnt.name);
                        setNeedsSaving();
                    }
                    catch (Exception)
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
                            if (lnt.obj is uint)
                                prop.SetValue(lastSelectedValueNode.Parent.Tag, (uint)valueNumericUpDown.Value);
                            else if (lnt.obj is double)
                                prop.SetValue(lastSelectedValueNode.Parent.Tag, (double)valueNumericUpDown.Value);
                        }

                        lastSelectedValueNode.Text = getTreeNodeDisplayName(valueNumericUpDown.Value.ToString(), lnt.name);
                        setNeedsSaving() ;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Failed to set " + lastSelectedValueNode.Text + " to " + valueNumericUpDown.Text);
                    }
                }
            }
        }
        #endregion
        private void saveConfigBbutton_Click(object sender, EventArgs e)
        {
            try
            {
                theRobotConfiguration.save(generatorConfig.robotConfiguration);
                //MessageBox.Show("File saved");
                addProgress("File saved");
                clearNeedsSaving();
            }
            catch (Exception ex)
            {
                addProgress(ex.Message);
            }
        }

        private void addTreeElementButton_Click(object sender, EventArgs e)
        {
            if (lastSelectedArrayNode != null)
            {
                // first create a new instance
                Type elementType = lastSelectedArrayNode.Tag.GetType().GetGenericArguments().Single(); ;
                object obj = Activator.CreateInstance(elementType);

                // then add it to the collection
                lastSelectedArrayNode.Tag.GetType().GetMethod("Add").Invoke(lastSelectedArrayNode.Tag, new object[] {obj});
                int count = (int)lastSelectedArrayNode.Tag.GetType().GetProperty("Count").GetValue(lastSelectedArrayNode.Tag);

                AddNode(lastSelectedArrayNode, obj, elementType.Name + (count-1));
            }
        }

        private bool isACollection(object obj)
        {
            Type t = obj.GetType();
            return ((t.Name == "Collection`1") && (t.Namespace == "System.Collections.ObjectModel"));
        }

        private void addStateDataButton_Click(object sender, EventArgs e)
        {
            string mechanismName = "";
            if (InputBox("Enter mechanism name", "Please enter a name for the mechanism file. A .xml extension will be added.", ref mechanismName) == DialogResult.OK)
            {
                statedata sd = new statedata();
                string filename = Path.GetFileNameWithoutExtension(mechanismName) + ".xml";

                theRobotConfiguration.mechanismControlDefinition.Add(filename, sd);
                AddNode(null, sd, filename);
            }
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(needsSaving)
            {
                DialogResult dlgRes = MessageBox.Show("Do you want to save changes?", "302 Code Generator", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if(dlgRes == DialogResult.Yes)
                {
                    saveConfigBbutton_Click(null, null);
                    clearNeedsSaving();
                }
                else if(dlgRes == DialogResult.Cancel) 
                {
                    e.Cancel = true;
                }
            }
        }

        private void clearReportButton_Click(object sender, EventArgs e)
        {
            progressTextBox.Clear();
        }
        private bool isDeletable(TreeNode tn)
        {
            TreeNode parent = tn.Parent;
            if (parent != null)
            {
                if(parent.Tag != null)
                    return isACollection(parent.Tag);
            }
            return false;
        }
        private void deleteTreeElementButton_Click(object sender, EventArgs e)
        {
            // The delete button will be disabled if the highlighted tree item cannot be deleted
            // Only a member of a collection can be deleted
            TreeNode tn = robotTreeView.SelectedNode;
            if(isDeletable(tn))
            {
                TreeNode parent = tn.Parent;
                if (parent != null)
                {
                    if( (parent.Tag != null) && (tn.Tag != null) )
                    {

                        object theObjectToDelete = tn.Tag;
                        if (tn.Tag is leafNodeTag)
                            theObjectToDelete = ((leafNodeTag)tn.Tag).obj;

                        if (theObjectToDelete != null)
                        {
                            parent.Tag.GetType().GetMethod("Remove").Invoke(parent.Tag, new object[] { theObjectToDelete });
                            tn.Remove();
                            setNeedsSaving();
                        }
                    }
                }
            }
        }
    }

    class leafNodeTag
    {
        public Type type { get; private set; }
        public string name { get; private set; }
        public object obj { get; private set; }

        public leafNodeTag(Type type, string name, object obj)
        {
            this.type = type;
            this.name = name;
            this.obj = obj;
        }
    }
}
