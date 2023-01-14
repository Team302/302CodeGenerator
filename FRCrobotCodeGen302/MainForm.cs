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

namespace FRCrobotCodeGen302
{
    public partial class MainForm : Form
    {
        static toolConfiguration generatorConfig = new toolConfiguration();

        public MainForm()
        {
            InitializeComponent();
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
                loadGeneratorConfig(configurationFilePathNameTextBox.Text);
                addProgress("Configuration file loaded.");

                string rootFolder = Path.Combine(generatorConfig.rootOutputFolder, "output");
                string rootRobotConfigFolder = Path.GetDirectoryName(generatorConfig.robotConfiguration);

                addProgress("Output will be placed at " + rootFolder);
                addProgress("Robot configuration is " + generatorConfig.robotConfiguration);

                if (!Directory.Exists(rootFolder))
                {
                    Directory.CreateDirectory(rootFolder);
                    addProgress("Created output directory " + rootFolder);
                }
                else
                {
                    addProgress("Output directory " + rootFolder + " already exists");
                }

                addProgress("Loading robot configuration " + generatorConfig.robotConfiguration);
                robot myRobot = loadRobotConfiguration(generatorConfig.robotConfiguration);

                addProgress("Writing mechanism files...");
                foreach (mechanism mech in myRobot.mechanism)
                {
                    string mechanismConfig = Path.Combine(rootRobotConfigFolder, "states", mech.controlFile);

                    addProgress("======== Loading mechanism configuration " + mechanismConfig);
                    statedata sd = loadStateDataConfiguration(mechanismConfig);

                    writeMechanismFiles(rootFolder, mech, sd);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Something went wrong. See below. \r\n\r\n" + ex.Message, "Code generator error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        robot loadRobotConfiguration(string fullPathName)
        {
            var mySerializer = new XmlSerializer(typeof(robot));
            using (var myFileStream = new FileStream(fullPathName, FileMode.Open))
            {
                return (robot)mySerializer.Deserialize(myFileStream);
            }
        }
        statedata loadStateDataConfiguration(string fullPathName)
        {
            var mySerializer = new XmlSerializer(typeof(statedata));
            using (var myFileStream = new FileStream(fullPathName, FileMode.Open))
            {
                return (statedata)mySerializer.Deserialize(myFileStream);
            }
        }

        private void loadGeneratorConfig(string configurationFullPathName)
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

        private void writeMechanismFiles(string baseFolder, mechanism mech, statedata mechanismStateData)
        {
            string mechanismFolder = Path.Combine(baseFolder, getMechanismName(mech.controlFile));

            if (!Directory.Exists(mechanismFolder))
            {
                addProgress("Creating folder " + mechanismFolder);
                Directory.CreateDirectory(mechanismFolder);
            }
            else
                addProgress("Output directory " + mechanismFolder + " already exists");

            writeStateMgrFiles(mechanismFolder, mech, mechanismStateData);
        }

        private string getMechanismName(string controlFileName)
        {
            return Path.GetFileNameWithoutExtension(controlFileName); 
        }

        private void writeStateMgrFiles(string baseFolder, mechanism mech, statedata mechanismStateData)
        {
            string baseFileName = Path.Combine(baseFolder, getMechanismName(mech.controlFile) + "StateMgr");
            string fullPathFilename_h = baseFileName + ".h";
            string fullPathFilename_cpp = baseFileName + ".cpp";
            string fullPathFilename_User_cpp = baseFileName + "_user.cpp";

            List<string> states = new List<string>();
            List<string> stateText = new List<string>();
            foreach (mechanismTarget mt in mechanismStateData.mechanismTarget)
            {
                states.Add(getStateNameFromText(getMechanismName(mech.controlFile), mt.stateIdentifier.ToString()));
                stateText.Add(mt.stateIdentifier.ToString());
            }

            writeStateMgr_h_File(fullPathFilename_h, mech, mechanismStateData, states, stateText);
            writeStateMgr_cpp_File(fullPathFilename_cpp, mech, mechanismStateData, states, stateText);
            //writeStateMgr_user_cpp_File(fullPathFilename_cpp, mech, mechanismStateData, states, stateText);
        }

        /// <summary>
        /// Keep in mind that the number of hand coded sections in the template might be different
        /// than in the previous file
        /// </summary>
        /// <param name="previousContent"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        private string copyHandCodedSections(string previousContent, string template)
        {
            string handCodedStart = @"//========= Hand modified code start section (\d+) ========";
            string handCodedEnd   = @"//========= Hand modified code end section (\d+) ========";

            string regex = handCodedStart + "([\\s\\S]*?)" + handCodedEnd;
            Regex matchesInPreviousFile = new Regex(regex);
            Regex matchesInTemplateFile = new Regex(regex);

            MatchCollection previousFileMatches = matchesInPreviousFile.Matches(previousContent);
            MatchCollection templateFileMatches = matchesInPreviousFile.Matches(template);

            if (previousFileMatches.Count > 0)
            {
                int allGroupID = 0;
                int sectionGroupID = 1;
                //todo add code here to validate that the templates and the regex captures are correct

                List<(int, string)> handCodedSections = new List<(int, string)>();
                foreach (Match m in previousFileMatches)
                {
                    handCodedSections.Add((System.Convert.ToInt32(m.Groups[sectionGroupID].Value), m.Value));
                }

                //section ID, text start, text length, text
                List<(int, int, int, string)> templateHandCodedSections = new List<(int, int, int, string)>();
                foreach (Match m in templateFileMatches)
                {
                    templateHandCodedSections.Add(  (Convert.ToInt32(m.Groups[sectionGroupID].Value), 
                                                    m.Groups[allGroupID].Index, m.Groups[allGroupID].Length,
                                                    m.Value));
                }

                foreach((int, int, int, string) t in templateHandCodedSections.OrderByDescending(t => t.Item2))
                {
                    int sectionID = t.Item1;
                    int startIndex = t.Item2;
                    int length = t.Item3;

                    (int, string) handCode = handCodedSections.Find(c => c.Item1 == sectionID);
                    if (handCode != (0,null))
                        template = template.Remove(startIndex, length).Insert(startIndex, handCode.Item2);
                }
            }

            return template;
        }

        StringBuilder prepareFile(string fullPathFilename, string template)
        {
            StringBuilder sb = new StringBuilder();
            String updatedFileContents = template;

            if (File.Exists(fullPathFilename))
            {
                string oldContent = File.ReadAllText(fullPathFilename);
                updatedFileContents = copyHandCodedSections(oldContent, template);

                File.Delete(fullPathFilename);
            }

            sb.AppendLine(updatedFileContents);

            return sb;
        }

        private void writeStateMgr_h_File(string fullPathFilename, mechanism mech, statedata mechanismStateData, List<string> states, List<string> stateText)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile( fullPathFilename, generatorConfig.stateManager_h);

            StringBuilder enumContentsStr = new StringBuilder();
            StringBuilder XmlStringToStateEnumMapStr = new StringBuilder();
            StringBuilder stateStructStr = new StringBuilder();
            for (int i=0; i<states.Count; i++)
            {
                enumContentsStr.AppendFormat("{0},\r\n", states[i]);
                XmlStringToStateEnumMapStr.AppendFormat("\"{0}\", {1}_STATE::{2},\r\n", stateText[i], getMechanismName(mech.controlFile).ToUpper(), states[i]);
                stateStructStr.AppendFormat("const StateStruc m_{2}State = {{ {0}_STATE::{3}, \"{1}\", StateType::{0}_STATE, true }};\r\n",
                    getMechanismName(mech.controlFile).ToUpper(),
                    stateText[i],
                    states[i].ToLower(),
                    states[i].ToUpper());
            }

            sb = sb.Replace("$STATE_STRUCT$", stateStructStr.ToString());
            sb = sb.Replace("$COMMA_SEPARATED_MECHANISM_STATES$", enumContentsStr.ToString().Trim(new char[] { ',','\r','\n' }));
            sb = sb.Replace("$XML_STRING_TO_STATE_ENUM_MAP$", XmlStringToStateEnumMapStr.ToString().Trim(new char[] { ',','\r','\n' }));
            sb = sb.Replace("$MECHANISM_NAME$", getMechanismName(mech.controlFile));
            sb = sb.Replace("$MECHANISM_NAME_UPPERCASE$", getMechanismName(mech.controlFile).ToUpper());
            sb = sb.Replace("$MECHANISM_NAME_LOWERCASE$", getMechanismName(mech.controlFile).ToLower());
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeStateMgr_cpp_File(string fullPathFilename, mechanism mech, statedata mechanismStateData, List<string> states, List<string> stateText)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, generatorConfig.stateManager_cpp);

            StringBuilder stateStructStr = new StringBuilder();
            for (int i = 0; i < states.Count; i++)
            {
                stateStructStr.AppendFormat("stateMap[\"{0}\"] = m_{1}State;\r\n",
                    stateText[i],
                    states[i].ToLower());
            }

            sb = sb.Replace("$STATE_MAP_INITIALIZATION$", stateStructStr.ToString());
            sb = sb.Replace("$MECHANISM_NAME$", getMechanismName(mech.controlFile));
            sb = sb.Replace("$MECHANISM_NAME_UPPERCASE$", getMechanismName(mech.controlFile).ToUpper());
            sb = sb.Replace("$MECHANISM_NAME_LOWERCASE$", getMechanismName(mech.controlFile).ToLower());
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        string getStateNameFromText(string mechanismName, string stateText)
        {
            string nameUpperCase = mechanismName.ToUpper();
            if(stateText.StartsWith(nameUpperCase))
            {
                stateText = stateText.Remove(0, nameUpperCase.Length).TrimStart('_');
            }

            return stateText;
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

                    loadGeneratorConfig(configurationFilePathNameTextBox.Text);
                    configuredOutputFolderLabel.Text = generatorConfig.rootOutputFolder;
                    robotConfigurationFileLabel.Text = generatorConfig.robotConfiguration;
                }
            }
        }
    }
}
