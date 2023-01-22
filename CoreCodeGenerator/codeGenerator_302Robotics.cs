using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Configuration;
using robotConfiguration;
using Robot;
using StateData;
using System.Collections;
using System.Reflection;

namespace CoreCodeGenerator
{
    public class codeGenerator_302Robotics : baseReportingClass
    {
        private robotConfig theRobotConfiguration = new robotConfig();

        public void generate(robotConfig theRobotConfig, toolConfiguration generatorConfig)
        {
            theRobotConfiguration = theRobotConfig;

            string rootFolder = generatorConfig.rootOutputFolder;

            addProgress("Output will be placed at " + rootFolder);

            if (!Directory.Exists(rootFolder))
            {
                Directory.CreateDirectory(rootFolder);
                addProgress("Created output directory " + rootFolder);
            }
            else
            {
                addProgress("Output directory " + rootFolder + " already exists");
            }

            addProgress("Writing mechanism files...");
            foreach (mechanism mech in theRobotConfiguration.theRobot.mechanism)
            {
                statedata sd = theRobotConfiguration.mechanismControlDefinition[mech.controlFile];

                writeMechanismFiles(rootFolder, generatorConfig, mech, sd);
            }
            
            writeUsagesFiles(rootFolder, generatorConfig);
            writeMechanismsFiles(rootFolder, generatorConfig);
        }

        private void writeMechanismFiles(string baseFolder, toolConfiguration generatorConfig, mechanism mech, statedata mechanismStateData)
        {
            string mechanismFolder = Path.Combine(baseFolder, "mechanisms", getMechanismName(mech.controlFile));

            if (!Directory.Exists(mechanismFolder))
            {
                addProgress("Creating folder " + mechanismFolder);
                Directory.CreateDirectory(mechanismFolder);
            }
            else
                addProgress("Output directory " + mechanismFolder + " already exists");

            writeStateMgrFiles(mechanismFolder, generatorConfig, mech, mechanismStateData);
            writeStateFiles(mechanismFolder, generatorConfig, mech, mechanismStateData);
            writeMainFiles(mechanismFolder, generatorConfig, mech, mechanismStateData);
            

        }

        private void writeMechanismsFiles(string baseFolder, toolConfiguration generatorConfig)
        {
            string mechanismFolder = Path.Combine(baseFolder, "mechanisms");

            writeMechanismTypeFiles(mechanismFolder, generatorConfig);

        }


        private void writeUsagesFiles(string baseFolder, toolConfiguration generatorConfig)
        {
            string OutFolder = Path.Combine(baseFolder, "hw", "usages");

            if (!Directory.Exists(OutFolder))
            {
                addProgress("Creating folder " + OutFolder);
                Directory.CreateDirectory(OutFolder);
            }
            else
                addProgress("Output directory " + OutFolder + " already exists");

            
            writeSolenoidUsageFiles(OutFolder, generatorConfig);
            writeMotorControllerUsageFiles(OutFolder, generatorConfig);
            writeDigitalInputUsageFiles(OutFolder, generatorConfig);
            writeServoUsageFiles(OutFolder, generatorConfig);
        }

        private string getMechanismName(string controlFileName)
        {
            return Path.GetFileNameWithoutExtension(controlFileName);
        }

        private void writeStateMgrFiles(string baseFolder, toolConfiguration generatorConfig, mechanism mech, statedata mechanismStateData)
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

            writeStateMgr_h_File(fullPathFilename_h, generatorConfig.stateManager_h, mech, mechanismStateData, states, stateText);
            writeStateMgr_cpp_File(fullPathFilename_cpp, generatorConfig.stateManager_cpp, mech, mechanismStateData, states, stateText);
        }

        private void writeStateFiles(string baseFolder, toolConfiguration generatorConfig, mechanism mech, statedata mechanismStateData)
        {
            string baseFileName = Path.Combine(baseFolder, getMechanismName(mech.controlFile) + "State");
            string fullPathFilename_h = baseFileName + ".h";
            string fullPathFilename_cpp = baseFileName + ".cpp";

            List<string> states = new List<string>();
            List<string> stateText = new List<string>();
            foreach (mechanismTarget mt in mechanismStateData.mechanismTarget)
            {
                states.Add(getStateNameFromText(getMechanismName(mech.controlFile), mt.stateIdentifier.ToString()));
                stateText.Add(mt.stateIdentifier.ToString());
            }
            writeState_h_File(fullPathFilename_h, generatorConfig.state_h, mech, mechanismStateData, states, stateText);
            writeState_cpp_File(fullPathFilename_cpp, generatorConfig.state_cpp, mech, mechanismStateData, states, stateText);
        }

        private void writeMainFiles(string baseFolder, toolConfiguration generatorConfig, mechanism mech, statedata mechanismStateData)
        {
            string baseFileName = Path.Combine(baseFolder, getMechanismName(mech.controlFile));
            string fullPathFilename_h = baseFileName + ".h";
            string fullPathFilename_cpp = baseFileName + ".cpp";

            List<string> states = new List<string>();
            List<string> stateText = new List<string>();
            foreach (mechanismTarget mt in mechanismStateData.mechanismTarget)
            {
                states.Add(getStateNameFromText(getMechanismName(mech.controlFile), mt.stateIdentifier.ToString()));
                stateText.Add(mt.stateIdentifier.ToString());
            }
            writeMain_h_File(fullPathFilename_h, generatorConfig.main_h, mech, mechanismStateData, states, stateText);
            writeMain_cpp_File(fullPathFilename_cpp, generatorConfig.main_cpp, mech, mechanismStateData, states, stateText);
        }

        private void writeSolenoidUsageFiles(string baseFolder, toolConfiguration generatorConfig)
        {
            string baseFileName = Path.Combine(baseFolder, "SolenoidUsage");
            string fullPathFilename_h = baseFileName + ".h";
            string fullPathFilename_cpp = baseFileName + ".cpp";

            List<string> states = new List<string>();
            List<string> stateText = new List<string>();
        
            writeSolenoidUsage_h_File(fullPathFilename_h, generatorConfig.SolenoidUsage_h);
            writeSolenoidUsage_cpp_File(fullPathFilename_cpp, generatorConfig.SolenoidUsage_cpp);
        }

        private void writeMotorControllerUsageFiles(string baseFolder, toolConfiguration generatorConfig)
        {
            string baseFileName = Path.Combine(baseFolder, "MotorControllerUsage");
            string fullPathFilename_h = baseFileName + ".h";
            string fullPathFilename_cpp = baseFileName + ".cpp";

            List<string> states = new List<string>();
            List<string> stateText = new List<string>();
            
            writeMotorControllerUsage_h_File(fullPathFilename_h, generatorConfig.MotorControllerUsage_h);
            writeMotorControllerUsage_cpp_File(fullPathFilename_cpp, generatorConfig.MotorControllerUsage_cpp);
        }

        private void writeDigitalInputUsageFiles(string baseFolder, toolConfiguration generatorConfig)
        {
            string baseFileName = Path.Combine(baseFolder, "DigitalInputUsage");
            string fullPathFilename_h = baseFileName + ".h";
            string fullPathFilename_cpp = baseFileName + ".cpp";

            List<string> states = new List<string>();
            List<string> stateText = new List<string>();
         
            writeDigitalInputUsage_h_File(fullPathFilename_h, generatorConfig.DigitalInputUsage_h);
            writeDigitalInputUsage_cpp_File(fullPathFilename_cpp, generatorConfig.DigitalInputUsage_cpp);
        }

        private void writeServoUsageFiles(string baseFolder, toolConfiguration generatorConfig)
        {
            string baseFileName = Path.Combine(baseFolder, "ServoUsage");
            string fullPathFilename_h = baseFileName + ".h";
            string fullPathFilename_cpp = baseFileName + ".cpp";

            List<string> states = new List<string>();
            List<string> stateText = new List<string>();
            writeServoUsage_h_File(fullPathFilename_h, generatorConfig.ServoUsage_h);
            writeServoUsage_cpp_File(fullPathFilename_cpp, generatorConfig.ServoUsage_cpp);
        }

        private void writeMechanismTypeFiles(string baseFolder, toolConfiguration generatorConfig)
        {
            string baseFileName = Path.Combine(baseFolder, "MechanismTypes");
            string fullPathFilename_h = baseFileName + ".h";
            string fullPathFilename_cpp = baseFileName + ".cpp";
           

            writeMechanismTypes_h_File(fullPathFilename_h, generatorConfig.MechanismTypes_h);
            writeMechanismTypes_cpp_File(fullPathFilename_cpp, generatorConfig.MechanismTypes_cpp);
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
            string handCodedEnd = @"//========= Hand modified code end section (\d+) ========";

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
                    templateHandCodedSections.Add((Convert.ToInt32(m.Groups[sectionGroupID].Value),
                                                    m.Groups[allGroupID].Index, m.Groups[allGroupID].Length,
                                                    m.Value));
                }

                foreach ((int, int, int, string) t in templateHandCodedSections.OrderByDescending(t => t.Item2))
                {
                    int sectionID = t.Item1;
                    int startIndex = t.Item2;
                    int length = t.Item3;

                    (int, string) handCode = handCodedSections.Find(c => c.Item1 == sectionID);
                    if (handCode != (0, null))
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

        private void writeStateMgr_h_File(string fullPathFilename, string template, mechanism mech, statedata mechanismStateData, List<string> states, List<string> stateText)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template);

            StringBuilder enumContentsStr = new StringBuilder();
            StringBuilder XmlStringToStateEnumMapStr = new StringBuilder();
            StringBuilder stateStructStr = new StringBuilder();
            for (int i = 0; i < states.Count; i++)
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
            sb = sb.Replace("$COMMA_SEPARATED_MECHANISM_STATES$", enumContentsStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            sb = sb.Replace("$XML_STRING_TO_STATE_ENUM_MAP$", XmlStringToStateEnumMapStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            sb = sb.Replace("$MECHANISM_NAME$", getMechanismName(mech.controlFile));
            sb = sb.Replace("$MECHANISM_NAME_UPPERCASE$", getMechanismName(mech.controlFile).ToUpper());
            sb = sb.Replace("$MECHANISM_NAME_LOWERCASE$", getMechanismName(mech.controlFile).ToLower());
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeStateMgr_cpp_File(string fullPathFilename, string template, mechanism mech, statedata mechanismStateData, List<string> states, List<string> stateText)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template);

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

        private void writeState_h_File(string fullPathFilename, string template, mechanism mech, statedata mechanismStateData, List<string> states, List<string> stateText)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template);

            StringBuilder enumContentsStr = new StringBuilder();
            StringBuilder XmlStringToStateEnumMapStr = new StringBuilder();
            StringBuilder stateStructStr = new StringBuilder();
            for (int i = 0; i < states.Count; i++)
            {
                enumContentsStr.AppendFormat("{0},\r\n", states[i]);
                XmlStringToStateEnumMapStr.AppendFormat("\"{0}\", {1}_STATE::{2},\r\n", stateText[i], getMechanismName(mech.controlFile).ToUpper(), states[i]);
                stateStructStr.AppendFormat("const StateStruc m_{2}State = {{ {0}_STATE::{3}, \"{1}\", StateType::{0}_STATE, true }};\r\n",
                    getMechanismName(mech.controlFile).ToUpper(),
                    stateText[i],
                    states[i].ToLower(),
                    states[i].ToUpper());
            }

            
            string baseClassName = getMechanismBaseClassName(mech, true);
            sb = sb.Replace("$MECH_BASE_CLASS$", baseClassName);

            sb = sb.Replace("$STATE_STRUCT$", stateStructStr.ToString());
            sb = sb.Replace("$COMMA_SEPARATED_MECHANISM_STATES$", enumContentsStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            sb = sb.Replace("$XML_STRING_TO_STATE_ENUM_MAP$", XmlStringToStateEnumMapStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            sb = sb.Replace("$MECHANISM_NAME$", getMechanismName(mech.controlFile));
            sb = sb.Replace("$MECHANISM_NAME_UPPERCASE$", getMechanismName(mech.controlFile).ToUpper());
            sb = sb.Replace("$MECHANISM_NAME_LOWERCASE$", getMechanismName(mech.controlFile).ToLower());
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private string getMechanismBaseClassName(object mech, bool state)
        {
            int numberOfMotors = traverseRobotXML_countObjects(mech, typeof(motor));
            int numberOfSolenoids = traverseRobotXML_countObjects(mech, typeof(solenoid));
            int numberOfServos = traverseRobotXML_countObjects(mech, typeof(servo));

            return getBaseClassName(numberOfMotors, numberOfSolenoids, numberOfServos, state);
        }
        private void writeState_cpp_File(string fullPathFilename, string template, mechanism mech, statedata mechanismStateData, List<string> states, List<string> stateText)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template);

            StringBuilder stateStructStr = new StringBuilder();
            for (int i = 0; i < states.Count; i++)
            {
                stateStructStr.AppendFormat("stateMap[\"{0}\"] = m_{1}State;\r\n",
                    stateText[i],
                    states[i].ToLower());
            }

            string baseClassName = getMechanismBaseClassName(mech, true);
            sb = sb.Replace("$MECH_BASE_CLASS$", baseClassName);

            sb = sb.Replace("$STATE_MAP_INITIALIZATION$", stateStructStr.ToString());
            sb = sb.Replace("$MECHANISM_NAME$", getMechanismName(mech.controlFile));
            sb = sb.Replace("$MECHANISM_NAME_UPPERCASE$", getMechanismName(mech.controlFile).ToUpper());
            sb = sb.Replace("$MECHANISM_NAME_LOWERCASE$", getMechanismName(mech.controlFile).ToLower());
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeMain_cpp_File(string fullPathFilename, string template, mechanism mech, statedata mechanismStateData, List<string> states, List<string> stateText)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template);

            StringBuilder stateStructStr = new StringBuilder();
            for (int i = 0; i < states.Count; i++)
            {
                stateStructStr.AppendFormat("stateMap[\"{0}\"] = m_{1}State;\r\n",
                    stateText[i],
                    states[i].ToLower());
            }

            string baseClassName = getMechanismBaseClassName(mech, false);
            sb = sb.Replace("$MECH_BASE_CLASS$", baseClassName);

            sb = sb.Replace("$STATE_MAP_INITIALIZATION$", stateStructStr.ToString());
            sb = sb.Replace("$MECHANISM_NAME$", getMechanismName(mech.controlFile));
            sb = sb.Replace("$MECHANISM_NAME_UPPERCASE$", getMechanismName(mech.controlFile).ToUpper());
            sb = sb.Replace("$MECHANISM_NAME_LOWERCASE$", getMechanismName(mech.controlFile).ToLower());
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeMain_h_File(string fullPathFilename, string template, mechanism mech, statedata mechanismStateData, List<string> states, List<string> stateText)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template);

            StringBuilder enumContentsStr = new StringBuilder();
            StringBuilder XmlStringToStateEnumMapStr = new StringBuilder();
            StringBuilder stateStructStr = new StringBuilder();
            for (int i = 0; i < states.Count; i++)
            {
                enumContentsStr.AppendFormat("{0},\r\n", states[i]);
                XmlStringToStateEnumMapStr.AppendFormat("\"{0}\", {1}_STATE::{2},\r\n", stateText[i], getMechanismName(mech.controlFile).ToUpper(), states[i]);
                stateStructStr.AppendFormat("const StateStruc m_{2}State = {{ {0}_STATE::{3}, \"{1}\", StateType::{0}_STATE, true }};\r\n",
                    getMechanismName(mech.controlFile).ToUpper(),
                    stateText[i],
                    states[i].ToLower(),
                    states[i].ToUpper());
            }

            string baseClassName = getMechanismBaseClassName(mech, false);
            sb = sb.Replace("$MECH_BASE_CLASS$", baseClassName);
            
            sb = sb.Replace("$STATE_STRUCT$", stateStructStr.ToString());
            sb = sb.Replace("$COMMA_SEPARATED_MECHANISM_STATES$", enumContentsStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            sb = sb.Replace("$XML_STRING_TO_STATE_ENUM_MAP$", XmlStringToStateEnumMapStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            sb = sb.Replace("$MECHANISM_NAME$", getMechanismName(mech.controlFile));
            sb = sb.Replace("$MECHANISM_NAME_UPPERCASE$", getMechanismName(mech.controlFile).ToUpper());
            sb = sb.Replace("$MECHANISM_NAME_LOWERCASE$", getMechanismName(mech.controlFile).ToLower());
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeSolenoidUsage_h_File(string fullPathFilename, string template)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template);

            StringBuilder enumContentsStr = new StringBuilder();
            StringBuilder XmlStringToStateEnumMapStr = new StringBuilder();
            StringBuilder stateStructStr = new StringBuilder();
           
            sb = sb.Replace("$STATE_STRUCT$", stateStructStr.ToString());
            sb = sb.Replace("$COMMA_SEPARATED_MECHANISM_STATES$", enumContentsStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            sb = sb.Replace("$XML_STRING_TO_STATE_ENUM_MAP$", XmlStringToStateEnumMapStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeSolenoidUsage_cpp_File(string fullPathFilename, string template)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template);

            StringBuilder enumContentsStr = new StringBuilder();
            StringBuilder XmlStringToStateEnumMapStr = new StringBuilder();
            StringBuilder stateStructStr = new StringBuilder();
           
            sb = sb.Replace("$STATE_STRUCT$", stateStructStr.ToString());
            sb = sb.Replace("$COMMA_SEPARATED_MECHANISM_STATES$", enumContentsStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            sb = sb.Replace("$XML_STRING_TO_STATE_ENUM_MAP$", XmlStringToStateEnumMapStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeMotorControllerUsage_h_File(string fullPathFilename, string template)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template);

            StringBuilder enumContentsStr = new StringBuilder();
            StringBuilder XmlStringToStateEnumMapStr = new StringBuilder();
            StringBuilder stateStructStr = new StringBuilder();
           
            sb = sb.Replace("$STATE_STRUCT$", stateStructStr.ToString());
            sb = sb.Replace("$COMMA_SEPARATED_MECHANISM_STATES$", enumContentsStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            sb = sb.Replace("$XML_STRING_TO_STATE_ENUM_MAP$", XmlStringToStateEnumMapStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeMotorControllerUsage_cpp_File(string fullPathFilename, string template)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template);

            StringBuilder enumContentsStr = new StringBuilder();
            StringBuilder XmlStringToStateEnumMapStr = new StringBuilder();
            StringBuilder stateStructStr = new StringBuilder();
           
            sb = sb.Replace("$STATE_STRUCT$", stateStructStr.ToString());
            sb = sb.Replace("$COMMA_SEPARATED_MECHANISM_STATES$", enumContentsStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            sb = sb.Replace("$XML_STRING_TO_STATE_ENUM_MAP$", XmlStringToStateEnumMapStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeDigitalInputUsage_h_File(string fullPathFilename, string template)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template);

            StringBuilder enumContentsStr = new StringBuilder();
            StringBuilder XmlStringToStateEnumMapStr = new StringBuilder();
            StringBuilder stateStructStr = new StringBuilder();
            
            sb = sb.Replace("$STATE_STRUCT$", stateStructStr.ToString());
            sb = sb.Replace("$COMMA_SEPARATED_MECHANISM_STATES$", enumContentsStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            sb = sb.Replace("$XML_STRING_TO_STATE_ENUM_MAP$", XmlStringToStateEnumMapStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeDigitalInputUsage_cpp_File(string fullPathFilename, string template)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template);

            StringBuilder enumContentsStr = new StringBuilder();
            StringBuilder XmlStringToStateEnumMapStr = new StringBuilder();
            

            
            sb = sb.Replace("$DIGITAL_SENSOR_USAGE_ENUM_CONTENTS_COMMA_SEPARATED$", enumContentsStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            sb = sb.Replace("$DIGITAL_SENSOR_USAGE_XML_TEXT2ENUM_MAP$", XmlStringToStateEnumMapStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeServoUsage_h_File(string fullPathFilename, string template)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template);

            StringBuilder enumContentsStr = new StringBuilder();
            StringBuilder XmlStringToStateEnumMapStr = new StringBuilder();
            StringBuilder stateStructStr = new StringBuilder();
           
            sb = sb.Replace("$STATE_STRUCT$", stateStructStr.ToString());
            sb = sb.Replace("$COMMA_SEPARATED_MECHANISM_STATES$", enumContentsStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            sb = sb.Replace("$XML_STRING_TO_STATE_ENUM_MAP$", XmlStringToStateEnumMapStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeServoUsage_cpp_File(string fullPathFilename, string template)
         {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template);

            StringBuilder enumContentsStr = new StringBuilder();
            StringBuilder XmlStringToStateEnumMapStr = new StringBuilder();
            StringBuilder stateStructStr = new StringBuilder();
           
            sb = sb.Replace("$STATE_STRUCT$", stateStructStr.ToString());
            sb = sb.Replace("$COMMA_SEPARATED_MECHANISM_STATES$", enumContentsStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            sb = sb.Replace("$XML_STRING_TO_STATE_ENUM_MAP$", XmlStringToStateEnumMapStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            File.WriteAllText(fullPathFilename, sb.ToString());
        }
        private void writeMechanismTypes_h_File(string fullPathFilename, string template)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template);

            StringBuilder enumContentsStr = new StringBuilder();


            foreach (KeyValuePair < string,statedata > kvp in theRobotConfiguration.mechanismControlDefinition)
            {
               string mechanmismName = getMechanismName (kvp.Key).ToUpper();
                enumContentsStr.AppendLine (mechanmismName + ",");
            }
            sb = sb.Replace("$COMMA_SEPARATED_MECHANISM_NAMES$", enumContentsStr.ToString());
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeMechanismTypes_cpp_File(string fullPathFilename, string template)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template);

            StringBuilder XmlStringToEnumMapStr = new StringBuilder();

            foreach (KeyValuePair<string, statedata> kvp in theRobotConfiguration.mechanismControlDefinition)
            {
                string mechanmismName = getMechanismName(kvp.Key).ToUpper();
                XmlStringToEnumMapStr.AppendLine(string.Format("m_typeMap[\"{0}\"] = MECHANISM_TYPE::{0};", mechanmismName));
            }

            sb = sb.Replace("$MECHANISM_NAMES_MAPPED_TO_ENUMS$", XmlStringToEnumMapStr.ToString());

            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        string getStateNameFromText(string mechanismName, string stateText)
        {
            string nameUpperCase = mechanismName.ToUpper();
            if (stateText.StartsWith(nameUpperCase))
            {
                stateText = stateText.Remove(0, nameUpperCase.Length).TrimStart('_');
            }

            return stateText;
        }

        public int traverseRobotXML_countObjects(object obj, Type objectType)
        {
            int count = 0;

            Type objType = obj.GetType();

            if(obj.GetType() == objectType)
            {
                count++;
            }

            if (isACollection(obj))
            {
                ICollection ic = obj as ICollection;
                foreach (var v in ic)
                {
                    count += traverseRobotXML_countObjects(v, objectType);
                }
            }
            else
            {
                PropertyInfo[] propertyInfos = objType.GetProperties();

                if ((objType.FullName != "System.String") && (propertyInfos.Length > 0))
                {
                    string previousName = "";
                    foreach (PropertyInfo pi in propertyInfos)
                    {
                        object theObj = pi.GetValue(obj);

                        if (theObj != null)
                        {
                            // in the generated code each property is followed by a 2nd property with the same name + "Specified"
                            // we want to skip these
                            if (pi.Name != previousName + "Specified")
                            {
                                count += traverseRobotXML_countObjects(theObj, objectType);
                                previousName = pi.Name;
                            }
                        }
                    }
                }
                else
                {
                    // this means that this is a leaf node
                }
            }

            return count;
        }

        private bool isACollection(object obj)
        {
            Type t = obj.GetType();
            return ((t.Name == "Collection`1") && (t.Namespace == "System.Collections.ObjectModel"));
        }

        string getBaseClassName(int numberOfMotors, int numberOfSolenoids, int numberOfServos, bool state)
        {
            string returnValue = "Mech";
            
            if(numberOfMotors > 0)
                returnValue += numberOfMotors + "IndMotor";

            if (numberOfSolenoids > 0)
                returnValue += numberOfSolenoids + "Solenoid";

            if (numberOfServos > 0)
                returnValue += numberOfServos + "Servo";

            if (state) 
                returnValue += "State";

            return returnValue;
        }
    }
}
