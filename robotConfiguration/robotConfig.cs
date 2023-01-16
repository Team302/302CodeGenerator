using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace robotConfiguration
{
    public class robotConfig : baseReportingClass
    {
        public robot theRobot;
        public Dictionary<string, statedata> mechanismControlDefinition;

        public void load(string theRobotConfigFullPathFileName)
        {
            string rootRobotConfigFolder = Path.GetDirectoryName(theRobotConfigFullPathFileName);

            addProgress("Loading robot configuration " + theRobotConfigFullPathFileName);
            theRobot = loadRobotConfiguration(theRobotConfigFullPathFileName);

            addProgress("Loading mechanism files...");
            mechanismControlDefinition = new Dictionary<string, statedata>();
            foreach (mechanism mech in theRobot.mechanism)
            {
                string mechanismConfig = Path.Combine(rootRobotConfigFolder, "states", mech.controlFile);

                addProgress("======== Loading mechanism configuration " + mechanismConfig);
                statedata sd = loadStateDataConfiguration(mechanismConfig);
                mechanismControlDefinition.Add(mech.controlFile, sd);
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
    }

    public class baseReportingClass
    {
        public delegate void showMessage(string message);

        protected showMessage progressCallback;
        protected void addProgress(string info)
        {
            if( progressCallback != null)
                progressCallback(info);
        }
        public void setProgressCallback(showMessage callback)
        {
            progressCallback = callback;
        }
    }
}
