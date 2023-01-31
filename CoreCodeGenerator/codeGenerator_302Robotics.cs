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
        public enum MECHANISM_FILE_TYPE { MAIN, STATE, STATE_MGR }

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
            List<string> mechMainFiles = new List<string>();
            List<string> mechStateFiles = new List<string>();
            List<string> mechStateMgrFiles = new List<string>();

            foreach (mechanism mech in theRobotConfiguration.theRobot.mechanism)
            {
                statedata sd = theRobotConfiguration.mechanismControlDefinition[mech.controlFile];

                List<string> genFiles = writeMechanismFiles(rootFolder, generatorConfig, mech, sd);
                if(genFiles.Count == 3)
                {
                    //the order needs to be the same as in writeMechanismFiles
                    mechMainFiles.Add(genFiles[0].Substring(rootFolder.Length).TrimStart('\\'));
                    mechStateFiles.Add(genFiles[1].Substring(rootFolder.Length).TrimStart('\\'));
                    mechStateMgrFiles.Add(genFiles[2].Substring(rootFolder.Length).TrimStart('\\'));
                }
            }

            writeMechAllHFiles(rootFolder, generatorConfig, mechMainFiles, mechStateFiles, mechStateMgrFiles);
            writeUsagesFiles(rootFolder, generatorConfig);
            writeMechanismsFiles(rootFolder, generatorConfig);
            writeRobotConfigFiles(rootFolder, generatorConfig);
        }

        #region Main generator functions
        private List<string> writeMechanismFiles(string baseFolder, toolConfiguration generatorConfig, mechanism mech, statedata mechanismStateData)
        {
            string mechanismFolder = Path.Combine(baseFolder, "mechanisms", getMechanismName(mech.controlFile));

            if (!Directory.Exists(mechanismFolder))
            {
                addProgress("Creating folder " + mechanismFolder);
                Directory.CreateDirectory(mechanismFolder);
            }
            else
                addProgress("Output directory " + mechanismFolder + " already exists");

            List<string> filePathnames = new List<string>();
            filePathnames.Add(writeMainFiles(mechanismFolder, generatorConfig, mech, mechanismStateData));
            filePathnames.Add(writeStateFiles(mechanismFolder, generatorConfig, mech, mechanismStateData));
            filePathnames.Add(writeStateMgrFiles(mechanismFolder, generatorConfig, mech, mechanismStateData));

            return filePathnames;
        }
        private void writeMechanismsFiles(string baseFolder, toolConfiguration generatorConfig)
        {
            string mechanismFolder = Path.Combine(baseFolder, "mechanisms");

            writeMechanismTypeFiles(mechanismFolder, generatorConfig);
            writeStateStrucFiles(mechanismFolder, generatorConfig);
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

            writeXXXUsageFiles(OutFolder, generatorConfig.SolenoidUsage_h, generatorConfig.SolenoidUsage_cpp, "SolenoidUsage", generatorConfig, typeof(solenoid), "SOLENOID");
            writeXXXUsageFiles(OutFolder, generatorConfig.MotorControllerUsage_h, generatorConfig.MotorControllerUsage_cpp, "MotorControllerUsage", generatorConfig, typeof(motor), "MOTOR_CONTROLLER");
            writeXXXUsageFiles(OutFolder, generatorConfig.DigitalInputUsage_h, generatorConfig.DigitalInputUsage_cpp, "DigitalInputUsage", generatorConfig, typeof(digitalInput), "DIGITAL_SENSOR");
            writeXXXUsageFiles(OutFolder, generatorConfig.ServoUsage_h, generatorConfig.ServoUsage_cpp, "ServoUsage", generatorConfig, typeof(servo), "SERVO");
        }
        private void writeMechAllHFiles(string baseFolder, toolConfiguration generatorConfig, List<string> mechMainFiles, List<string> mechStateFiles, List<string> mechStateMgrFiles)
        {
            string mechanismFolder = Path.Combine(baseFolder, "mechanisms");

            writeMechanisms_h_File(Path.Combine(mechanismFolder, "AllMechanismIncludes.h"), generatorConfig.AllMechanisms_h, generatorConfig, mechMainFiles);
            writeMechanisms_h_File(Path.Combine(mechanismFolder, "AllMechanismStateIncludes.h"), generatorConfig.AllMechanismsState_h, generatorConfig, mechStateFiles);
            writeMechanisms_h_File(Path.Combine(mechanismFolder, "AllMechanismStateMgrIncludes.h"), generatorConfig.AllMechanismsStateMgr_h, generatorConfig, mechStateMgrFiles);
        }

        #endregion

        #region Mechanism files
        private string writeStateMgrFiles(string baseFolder, toolConfiguration generatorConfig, mechanism mech, statedata mechanismStateData)
        {
            string baseFileName = Path.Combine(baseFolder, FirstCharSubstring(getMechanismName(mech.controlFile)) + "StateMgr");
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

            writeStateMgr_h_File(fullPathFilename_h, generatorConfig.stateManager_h, generatorConfig, mech, mechanismStateData, states, stateText);
            writeStateMgr_cpp_File(fullPathFilename_cpp, generatorConfig.stateManager_cpp, mech, generatorConfig, mechanismStateData, states, stateText);

            return baseFileName;
        }
        private string writeStateFiles(string baseFolder, toolConfiguration generatorConfig, mechanism mech, statedata mechanismStateData)
        {
            string baseFileName = Path.Combine(baseFolder, FirstCharSubstring(getMechanismName(mech.controlFile)) + "State");
            string fullPathFilename_h = baseFileName + ".h";
            string fullPathFilename_cpp = baseFileName + ".cpp";

            List<string> states = new List<string>();
            List<string> stateText = new List<string>();
            foreach (mechanismTarget mt in mechanismStateData.mechanismTarget)
            {
                states.Add(getStateNameFromText(getMechanismName(mech.controlFile), mt.stateIdentifier.ToString()));
                stateText.Add(mt.stateIdentifier.ToString());
            }
            writeState_h_File(fullPathFilename_h, generatorConfig.state_h, mech, generatorConfig, mechanismStateData, states, stateText);
            writeState_cpp_File(fullPathFilename_cpp, generatorConfig.state_cpp, mech, generatorConfig, mechanismStateData, states, stateText);

            return baseFileName;
        }
        private string writeMainFiles(string baseFolder, toolConfiguration generatorConfig, mechanism mech, statedata mechanismStateData)
        {
            string baseFileName = Path.Combine(baseFolder, FirstCharSubstring(getMechanismName(mech.controlFile)));
            string fullPathFilename_h = baseFileName + ".h";
            string fullPathFilename_cpp = baseFileName + ".cpp";

            List<string> states = new List<string>();
            List<string> stateText = new List<string>();
            foreach (mechanismTarget mt in mechanismStateData.mechanismTarget)
            {
                states.Add(getStateNameFromText(getMechanismName(mech.controlFile), mt.stateIdentifier.ToString()));
                stateText.Add(mt.stateIdentifier.ToString());
            }
            writeMain_h_File(fullPathFilename_h, generatorConfig.main_h, mech, generatorConfig, mechanismStateData, states, stateText);
            writeMain_cpp_File(fullPathFilename_cpp, generatorConfig.main_cpp, mech, generatorConfig, mechanismStateData, states, stateText);

            return baseFileName;
        }
        private void writeMechanismTypeFiles(string baseFolder, toolConfiguration generatorConfig)
        {
            string baseFileName = Path.Combine(baseFolder, "MechanismTypes");
            string fullPathFilename_h = baseFileName + ".h";
            string fullPathFilename_cpp = baseFileName + ".cpp";
           

            writeMechanismTypes_h_File(fullPathFilename_h, generatorConfig.MechanismTypes_h, generatorConfig);
            writeMechanismTypes_cpp_File(fullPathFilename_cpp, generatorConfig.MechanismTypes_cpp, generatorConfig);
        }
        private void writeStateStrucFiles(string baseFolder, toolConfiguration generatorConfig)
        {
            string baseFileName = Path.Combine(baseFolder, "StateStruc");
            string fullPathFilename_h = baseFileName + ".h";

            writeStateStruc_h_File(fullPathFilename_h, generatorConfig.StateStruc_h, generatorConfig);
        }

        private void writeRobotConfigFiles(string baseFolder, toolConfiguration generatorConfig)
        {
            string baseFileName = Path.Combine(baseFolder, "RobotConfig");
            string fullPathFilename_h = baseFileName + ".h";

            writeRobotConfig_h_File(fullPathFilename_h, generatorConfig.RobotConfig_h, generatorConfig);
        }


        private void writeStateMgr_h_File(string fullPathFilename, string template, toolConfiguration generatorConfig, mechanism mech, statedata mechanismStateData, List<string> states, List<string> stateText)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template, generatorConfig);
            
            StringBuilder enumContentsStr = new StringBuilder();
            StringBuilder XmlStringToStateEnumMapStr = new StringBuilder();
            StringBuilder stateStructStr = new StringBuilder();
            for (int i = 0; i < states.Count; i++)
            {
                enumContentsStr.AppendFormat("{0},\r\n", states[i]);
                XmlStringToStateEnumMapStr.AppendFormat("{{\"{0}\", {1}_STATE::{2}}},\r\n", stateText[i], getMechanismName(mech.controlFile).ToUpper(), states[i]);
                stateStructStr.AppendFormat("const StateStruc m_{2}State = {{ {0}_STATE::{3}, \"{1}\", StateType::{0}_STATE, true }};\r\n",
                    getMechanismName(mech.controlFile).ToUpper(),
                    stateText[i],
                    states[i].ToLower(),
                    states[i].ToUpper());
            }

            sb = sb.Replace("$STATE_STRUCT$", stateStructStr.ToString());
            sb = sb.Replace("$COMMA_SEPARATED_MECHANISM_STATES$", enumContentsStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            sb = sb.Replace("$XML_STRING_TO_STATE_ENUM_MAP$", XmlStringToStateEnumMapStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            sb = sb.Replace("$MECHANISM_NAME$", FirstCharSubstring(getMechanismName(mech.controlFile)));
            sb = sb.Replace("$MECHANISM_NAME_UPPERCASE$", getMechanismName(mech.controlFile).ToUpper());
            sb = sb.Replace("$MECHANISM_NAME_LOWERCASE$", getMechanismName(mech.controlFile).ToLower());
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeStateMgr_cpp_File(string fullPathFilename, string template, mechanism mech, toolConfiguration generatorConfig, statedata mechanismStateData, List<string> states, List<string> stateText)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template, generatorConfig);

            StringBuilder stateStructStr = new StringBuilder();
            for (int i = 0; i < states.Count; i++)
            {
                stateStructStr.AppendFormat("stateMap[\"{0}\"] = m_{1}State;\r\n",
                    stateText[i],
                    states[i].ToLower());
            }

            sb = sb.Replace("$STATE_MAP_INITIALIZATION$", stateStructStr.ToString());
            sb = sb.Replace("$MECHANISM_NAME$", FirstCharSubstring(getMechanismName(mech.controlFile)));
            sb = sb.Replace("$MECHANISM_NAME_UPPERCASE$", getMechanismName(mech.controlFile).ToUpper());
            sb = sb.Replace("$MECHANISM_NAME_LOWERCASE$", getMechanismName(mech.controlFile).ToLower());
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeState_h_File(string fullPathFilename, string template, mechanism mech, toolConfiguration generatorConfig, statedata mechanismStateData, List<string> states, List<string> stateText)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template, generatorConfig);

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

            string argumentList;
            string parameterList;
            string baseClassName = getMechanismBaseClassName(mech, MECHANISM_FILE_TYPE.STATE, out parameterList, out argumentList);
            sb = sb.Replace("$MECH_BASE_CLASS$", baseClassName);
            sb = sb.Replace("$MECHANISM_CONSTRUCTOR_PARAMETER_LIST$", parameterList);
            sb = sb.Replace("$MECHANISM_CONSTRUCTOR_ARGUMENT_LIST$", argumentList);

            sb = sb.Replace("$STATE_STRUCT$", stateStructStr.ToString());
            sb = sb.Replace("$COMMA_SEPARATED_MECHANISM_STATES$", enumContentsStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            sb = sb.Replace("$XML_STRING_TO_STATE_ENUM_MAP$", XmlStringToStateEnumMapStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            sb = sb.Replace("$MECHANISM_NAME$", FirstCharSubstring(getMechanismName(mech.controlFile)));
            sb = sb.Replace("$MECHANISM_NAME_UPPERCASE$", getMechanismName(mech.controlFile).ToUpper());
            sb = sb.Replace("$MECHANISM_NAME_LOWERCASE$", getMechanismName(mech.controlFile).ToLower());
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeState_cpp_File(string fullPathFilename, string template, mechanism mech, toolConfiguration generatorConfig, statedata mechanismStateData, List<string> states, List<string> stateText)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template, generatorConfig);

            StringBuilder stateStructStr = new StringBuilder();
            for (int i = 0; i < states.Count; i++)
            {
                stateStructStr.AppendFormat("stateMap[\"{0}\"] = m_{1}State;\r\n",
                    stateText[i],
                    states[i].ToLower());
            }

            string argumentList;
            string parameterList;
            string baseClassName = getMechanismBaseClassName(mech, MECHANISM_FILE_TYPE.STATE, out parameterList, out argumentList);
            sb = sb.Replace("$MECH_BASE_CLASS$", baseClassName);
            sb = sb.Replace("$MECHANISM_CONSTRUCTOR_PARAMETER_LIST$", parameterList);
            sb = sb.Replace("$MECHANISM_CONSTRUCTOR_ARGUMENT_LIST$", argumentList);

            sb = sb.Replace("$STATE_MAP_INITIALIZATION$", stateStructStr.ToString());
            sb = sb.Replace("$MECHANISM_NAME$", FirstCharSubstring(getMechanismName(mech.controlFile)));
            sb = sb.Replace("$MECHANISM_NAME_UPPERCASE$", getMechanismName(mech.controlFile).ToUpper());
            sb = sb.Replace("$MECHANISM_NAME_LOWERCASE$", getMechanismName(mech.controlFile).ToLower());
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeMain_cpp_File(string fullPathFilename, string template, mechanism mech, toolConfiguration generatorConfig, statedata mechanismStateData, List<string> states, List<string> stateText)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template, generatorConfig);

            StringBuilder stateStructStr = new StringBuilder();
            for (int i = 0; i < states.Count; i++)
            {
                stateStructStr.AppendFormat("stateMap[\"{0}\"] = m_{1}State;\r\n",
                    stateText[i],
                    states[i].ToLower());
            }

            string argumentList;
            string parameterList;
            string baseClassName = getMechanismBaseClassName(mech, MECHANISM_FILE_TYPE.MAIN, out parameterList, out argumentList);
            sb = sb.Replace("$MECH_BASE_CLASS$", baseClassName);
            sb = sb.Replace("$MECHANISM_CONSTRUCTOR_PARAMETER_LIST$", parameterList);
            sb = sb.Replace("$MECHANISM_CONSTRUCTOR_ARGUMENT_LIST$", argumentList);

            sb = sb.Replace("$STATE_MAP_INITIALIZATION$", stateStructStr.ToString());
            sb = sb.Replace("$MECHANISM_NAME$", FirstCharSubstring(getMechanismName(mech.controlFile)));
            sb = sb.Replace("$MECHANISM_NAME_UPPERCASE$", getMechanismName(mech.controlFile).ToUpper());
            sb = sb.Replace("$MECHANISM_NAME_LOWERCASE$", getMechanismName(mech.controlFile).ToLower());
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeMain_h_File(string fullPathFilename, string template, mechanism mech, toolConfiguration generatorConfig, statedata mechanismStateData, List<string> states, List<string> stateText)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template, generatorConfig);

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

            string argumentList;
            string parameterList;
            string baseClassName = getMechanismBaseClassName(mech, MECHANISM_FILE_TYPE.MAIN, out parameterList, out argumentList);
            sb = sb.Replace("$MECH_BASE_CLASS$", baseClassName);
            sb = sb.Replace("$MECHANISM_CONSTRUCTOR_PARAMETER_LIST$", parameterList);
            sb = sb.Replace("$MECHANISM_CONSTRUCTOR_ARGUMENT_LIST$", argumentList);

            sb = sb.Replace("$STATE_STRUCT$", stateStructStr.ToString());
            sb = sb.Replace("$COMMA_SEPARATED_MECHANISM_STATES$", enumContentsStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            sb = sb.Replace("$XML_STRING_TO_STATE_ENUM_MAP$", XmlStringToStateEnumMapStr.ToString().Trim(new char[] { ',', '\r', '\n' }));
            sb = sb.Replace("$MECHANISM_NAME$", FirstCharSubstring(getMechanismName(mech.controlFile)));
            sb = sb.Replace("$MECHANISM_NAME_UPPERCASE$", getMechanismName(mech.controlFile).ToUpper());
            sb = sb.Replace("$MECHANISM_NAME_LOWERCASE$", getMechanismName(mech.controlFile).ToLower());
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeMechanismTypes_h_File(string fullPathFilename, string template, toolConfiguration generatorConfig)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template, generatorConfig);

            StringBuilder enumContentsStr = new StringBuilder();


            foreach (KeyValuePair<string, statedata> kvp in theRobotConfiguration.mechanismControlDefinition)
            {
                string mechanmismName = getMechanismName(kvp.Key).ToUpper();
                enumContentsStr.AppendLine(mechanmismName + ",");
            }
            sb = sb.Replace("$COMMA_SEPARATED_MECHANISM_NAMES$", enumContentsStr.ToString());
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeMechanismTypes_cpp_File(string fullPathFilename, string template, toolConfiguration generatorConfig)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template, generatorConfig);

            StringBuilder XmlStringToEnumMapStr = new StringBuilder();

            foreach (KeyValuePair<string, statedata> kvp in theRobotConfiguration.mechanismControlDefinition)
            {
                string mechanmismName = getMechanismName(kvp.Key).ToUpper();
                XmlStringToEnumMapStr.AppendLine(string.Format("m_typeMap[\"{0}\"] = MECHANISM_TYPE::{0};", mechanmismName));
            }

            sb = sb.Replace("$MECHANISM_NAMES_MAPPED_TO_ENUMS$", XmlStringToEnumMapStr.ToString());

            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeStateStruc_h_File(string fullPathFilename, string template, toolConfiguration generatorConfig)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template, generatorConfig);

            StringBuilder stateStructStr = new StringBuilder();
            foreach (mechanism m in theRobotConfiguration.theRobot.mechanism)
            {
                stateStructStr.AppendLine(getMechanismName(m.controlFile) + "_STATE,");
            }
            
            sb = sb.Replace("$STATE_STRUCT$", stateStructStr.ToString().ToUpper());

            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeMechanisms_h_File(string fullPathFilename, string template, toolConfiguration generatorConfig, List<string> hFilesToInclude)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template, generatorConfig);

            StringBuilder contentsStr = new StringBuilder();


            foreach (string f in hFilesToInclude)
            {
                string mechanmismFile = f;
                contentsStr.AppendLine("#include <" + mechanmismFile + ".h>");
            }
            sb = sb.Replace("$INCLUDES_FOR_ALL_MECHANISMS$", contentsStr.ToString());
            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        #endregion

        #region Usage files
        private void writeXXXUsageFiles(string baseFolder, string template_h, string template_cpp, string filenameWithoutExtension, toolConfiguration generatorConfig,Type objectType, string usageName)
        {
            string baseFileName = Path.Combine(baseFolder, filenameWithoutExtension);
            string fullPathFilename_h = baseFileName + ".h";
            string fullPathFilename_cpp = baseFileName + ".cpp";

            writeXXXUsage_h_File(fullPathFilename_h, template_h, generatorConfig, objectType);
            writeXXXUsage_cpp_File(fullPathFilename_cpp, template_cpp, generatorConfig, objectType, usageName);
        }
        private void writeXXXUsage_h_File(string fullPathFilename, string template, toolConfiguration generatorConfig, Type objectType)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template, generatorConfig);

            List<string> usages = traverseRobotXML_getUsageList(theRobotConfiguration.theRobot, objectType);
            string enumContentsStr = toCommaSeparated(usages);

            sb = sb.Replace("$USAGE_ENUM_COMMA_SEPARATED$", enumContentsStr.TrimStart(new char[] { ',', '\r', '\n' }));

            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        private void writeXXXUsage_cpp_File(string fullPathFilename, string template, toolConfiguration generatorConfig, Type objectType, string usageName)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template, generatorConfig);

            List<string> usages = traverseRobotXML_getUsageList(theRobotConfiguration.theRobot, objectType);
            string contentsStr = toSeparateStatements("m_usageMap[\"{0}\"] = " + usageName + "_USAGE::{0};", usages);

            sb = sb.Replace("$USAGE_TEXT_TO_ENUM_MAP$", contentsStr);

            File.WriteAllText(fullPathFilename, sb.ToString());
        }


        private void writeRobotConfig_h_File(string fullPathFilename, string template, toolConfiguration generatorConfig)
        {
            addProgress("Generating " + fullPathFilename);

            StringBuilder sb = prepareFile(fullPathFilename, template, generatorConfig);

            string chassistype = " CHASSIS_TYPE_SWERVE_CHASSIS";

            sb = sb.Replace("$CHASSIS_TYPE_CONFIG$", chassistype);

            File.WriteAllText(fullPathFilename, sb.ToString());
        }

        #endregion

        #region Helper functions
        private string FirstCharSubstring(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            return $"{input[0].ToString().ToUpper()}{input.Substring(1)}";
        }
        private string getStateNameFromText(string mechanismName, string stateText)
        {
            string nameUpperCase = mechanismName.ToUpper();
            if (stateText.StartsWith(nameUpperCase))
            {
                stateText = stateText.Remove(0, nameUpperCase.Length).TrimStart('_');
            }

            return stateText;
        }
        private string toCommaSeparated(List<string> theList)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string theItem in theList)
            {
                sb.AppendLine(theItem + ",");
            }

            return sb.ToString();
        }
        private string toSeparateStatements(string formatString, List<string> theList)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string theItem in theList)
            {
                sb.AppendLine(string.Format(formatString, theItem));
            }

            return sb.ToString();
        }
        private int traverseRobotXML_countObjects(object obj, Type objectType)
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
        private List<string> traverseRobotXML_getUsageList(object obj, Type objectType)
        {
            List<string> usages = new List<string>();

            Type objType = obj.GetType();

            if (obj.GetType() == objectType)
            {
                PropertyInfo thePropertyInfo = objType.GetProperties().ToList().Find(p => p.Name == "usage");

                string usageName = "Usage name is missing in the robot xml configuration file";
                if (thePropertyInfo != null)
                {
                    usageName = thePropertyInfo.GetValue(obj).ToString();
                }
                
                usages.Add(usageName);
            }

            if (isACollection(obj))
            {
                ICollection ic = obj as ICollection;
                foreach (var v in ic)
                {
                    usages.AddRange(traverseRobotXML_getUsageList(v, objectType));
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
                                usages.AddRange(traverseRobotXML_getUsageList(theObj, objectType));
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

            return usages.Distinct().ToList();
        }
        private bool isACollection(object obj)
        {
            Type t = obj.GetType();
            return ((t.Name == "Collection`1") && (t.Namespace == "System.Collections.ObjectModel"));
        }
        private string getBaseClassName(int numberOfMotors, int numberOfSolenoids, int numberOfServos)
        {
            string returnValue = "Mech";

            returnValue += getNameSnippet(numberOfMotors, "IndMotor");
            returnValue += getNameSnippet(numberOfSolenoids, "Solenoid");
            returnValue += getNameSnippet(numberOfServos, "Servo");

            return returnValue;
        }

        private string getNameSnippet(int actuatorCount, string baseName)
        {
            string returnValue = "";

            if (actuatorCount > 0)
            {
                returnValue += actuatorCount + baseName;
                if (actuatorCount > 1)
                    returnValue += "s";
            }

            return returnValue;
        }
        private string getMechanismName(string controlFileName)
        {
            return Path.GetFileNameWithoutExtension(controlFileName);
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

        private StringBuilder prepareFile(string fullPathFilename, string template, toolConfiguration generatorConfig)
        {
            StringBuilder sb = new StringBuilder();
            String updatedFileContents = template;

            if (File.Exists(fullPathFilename))
            {
                string oldContent = File.ReadAllText(fullPathFilename);
                updatedFileContents = copyHandCodedSections(oldContent, template);

                File.Delete(fullPathFilename);
            }

            sb.AppendLine(generatorConfig.CopyrightNotice.Trim());
            sb.AppendLine(generatorConfig.GenerationNotice.Trim());
            sb.AppendLine(updatedFileContents);
           

            return sb;
        }
        private string getMechanismBaseClassName(object mech, MECHANISM_FILE_TYPE fileType, out string mainConstructorParameterList, out string mainConstructorArgumentList)
        {
            int numberOfMotors = traverseRobotXML_countObjects(mech, typeof(motor));
            int numberOfSolenoids = traverseRobotXML_countObjects(mech, typeof(solenoid));
            int numberOfServos = traverseRobotXML_countObjects(mech, typeof(servo));

            mainConstructorArgumentList = "";
            mainConstructorParameterList = ",\r\n";

            for (int i = 0; i < numberOfMotors; i++)
            {
                if (fileType == MECHANISM_FILE_TYPE.MAIN)
                {
                    mainConstructorParameterList += "std::shared_ptr<IDragonMotorController>     motorController" + i + ",\r\n";
                    mainConstructorArgumentList += ", motorController" + i;
                }
                else if (fileType == MECHANISM_FILE_TYPE.STATE)
                {
                    mainConstructorParameterList += "ControlData* control" + i + ",\r\n";
                    mainConstructorParameterList += "double target" + i + ",\r\n";
                    mainConstructorArgumentList += ", control" + i + ", target" + i;
                }

            }
            for (int i = 0; i < numberOfSolenoids; i++)
            {
                if (fileType == MECHANISM_FILE_TYPE.MAIN)
                {
                    mainConstructorParameterList += "std::shared_ptr<DragonSolenoid>     solenoid" + i + ",\r\n";
                    mainConstructorArgumentList += ", solenoid" + i;
                }
                else if (fileType == MECHANISM_FILE_TYPE.STATE)
                {
                    mainConstructorParameterList += "MechanismTargetData::SOLENOID solState" + i + ",\r\n";
                    mainConstructorArgumentList += ", solState" + i;
                }
            }
            for (int i = 0; i < numberOfServos; i++)
            {
                if (fileType == MECHANISM_FILE_TYPE.MAIN)
                    mainConstructorParameterList += "std::shared_ptr<DragonServo>     servo" + i + ",\r\n";
                else if (fileType == MECHANISM_FILE_TYPE.STATE)
                {
                    mainConstructorParameterList += "double target" + i + ",\r\n";
                }

                mainConstructorArgumentList += ", servo" + i;
            }

            mainConstructorParameterList = mainConstructorParameterList.TrimEnd(new char[] { ',', '\r', '\n' });

            return getBaseClassName(numberOfMotors, numberOfSolenoids, numberOfServos);
        }

        #endregion
    }
}
