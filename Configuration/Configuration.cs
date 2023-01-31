using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace Configuration
{
    [Serializable]
    public class toolConfiguration
    {
        public string rootOutputFolder = "";
        public string robotConfiguration = "";
        public List<string> robotConfigurations = new List<string>();
        public List<string> treeviewParentNameExtensions = new List<string>();

        public string AllMechanisms_h = "";
        public string AllMechanismsState_h = "";
        public string AllMechanismsStateMgr_h = "";

        public string stateManager_h = "";
        public string stateManager_cpp = "";

        public string state_h = "";
        public string state_cpp = "";

        public string main_h = "";
        public string main_cpp = "";

        public string SolenoidUsage_h = "";
        public string SolenoidUsage_cpp = "";

        public string MotorControllerUsage_h = "";
        public string MotorControllerUsage_cpp = "";

        public string DigitalInputUsage_h = "";
        public string DigitalInputUsage_cpp = "";

        public string ServoUsage_h = "";
        public string ServoUsage_cpp = "";

        public string MechanismTypes_h = "";
        public string MechanismTypes_cpp = "";

        public string StateStruc_h = "";

        public string RobotConfig_h = "";

        public string CopyrightNotice = "";

        public string GenerationNotice = ""; 

        public void loadDummyData()
        {
        }
        public override string ToString()
        {
            return "";
        }

        private void preSerialize()
        {
        }

        private void postSerialize()
        {
        }
        public void serialize(string rootPath)
        {
            var mySerializer = new XmlSerializer(typeof(toolConfiguration));
            using (var myFileStream = new FileStream(Path.Combine(rootPath, @"configuration.xml"), FileMode.Create))
            {
                mySerializer.Serialize(myFileStream, this);
            }
        }
        public toolConfiguration deserialize(string fullFilePathName)
        {
            var mySerializer = new XmlSerializer(typeof(toolConfiguration));

            using (var myFileStream = new FileStream(fullFilePathName, FileMode.Open))
            {
                return (toolConfiguration)mySerializer.Deserialize(myFileStream);
            }
        }
    }


}
