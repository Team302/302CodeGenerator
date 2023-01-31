using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Robot;
using StateData;

namespace robotConfiguration
{
    public class robotConfig : baseReportingClass
    {
        public robot theRobot;
        public Dictionary<string, statedata> mechanismControlDefinition;

        public void load(string theRobotConfigFullPathFileName)
        {
            try
            {
                string rootRobotConfigFolder = Path.GetDirectoryName(theRobotConfigFullPathFileName);

                addProgress("Loading robot configuration " + theRobotConfigFullPathFileName);
                theRobot = loadRobotConfiguration(theRobotConfigFullPathFileName);

                if (theRobot.pdp == null)
                    theRobot.pdp = new pdp(); 
                
                if (theRobot.chassis == null)
                    theRobot.chassis = new chassis();

                mechanismControlDefinition = new Dictionary<string, statedata>();
                if (theRobot.mechanism != null)
                {
                    addProgress("Loading mechanism files...");
                    foreach (mechanism mech in theRobot.mechanism)
                    {
                        if (string.IsNullOrEmpty(mech.controlFile))
                        {
                            progressCallback("controlFile for " + mech.type + " cannot be empty. Skipping");
                        }
                        else
                        {
                            string mechanismConfig = Path.Combine(rootRobotConfigFolder, "states", mech.controlFile);

                            addProgress("======== Loading mechanism configuration " + mechanismConfig);
                            statedata sd = loadStateDataConfiguration(mechanismConfig);
                            mechanismControlDefinition.Add(mech.controlFile, sd);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                progressCallback(ex.Message);
            }
        }

        public void save(string theRobotConfigFullPathFileName)
        {
            try
            {
                string rootRobotConfigFolder = Path.GetDirectoryName(theRobotConfigFullPathFileName);

                addProgress("Saving robot configuration " + theRobotConfigFullPathFileName);
                saveRobotConfiguration(theRobotConfigFullPathFileName);

                if (theRobot.mechanism != null)
                {
                    addProgress("Saving state data files...");
                    foreach (KeyValuePair<string, statedata> kvp in mechanismControlDefinition)
                    {
                        string path = Path.GetDirectoryName(theRobotConfigFullPathFileName);
                        path = Path.Combine(path, "states", kvp.Key);
                        saveStateDataConfiguration(path, kvp.Value);
                    }

                    //mechanismControlDefinition = new Dictionary<string, statedata>();
                    //foreach (mechanism mech in theRobot.mechanism)
                    //{
                    //    if (string.IsNullOrEmpty(mech.controlFile))
                    //        throw new Exception("controlFile for " + mech.type + " cannot be empty");

                    //    string mechanismConfig = Path.Combine(rootRobotConfigFolder, "states", mech.controlFile);

                    //    addProgress("======== Loading mechanism configuration " + mechanismConfig);
                    //    statedata sd = loadStateDataConfiguration(mechanismConfig);
                    //    mechanismControlDefinition.Add(mech.controlFile, sd);
                    //}
                }
            }
            catch(Exception ex)
            {
                progressCallback(ex.Message);
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

        void saveRobotConfiguration(string fullPathName)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.NewLineOnAttributes = true;
            xmlWriterSettings.Indent = true;

            var mySerializer = new XmlSerializer(typeof(robot));
            XmlWriter tw = XmlWriter.Create(fullPathName, xmlWriterSettings);
            mySerializer.Serialize(tw, theRobot);
            tw.Close();
        }

        statedata loadStateDataConfiguration(string fullPathName)
        {
            var mySerializer = new XmlSerializer(typeof(statedata));
            using (var myFileStream = new FileStream(fullPathName, FileMode.Open))
            {
                return (statedata)mySerializer.Deserialize(myFileStream);
            }
        }
        void saveStateDataConfiguration(string fullPathName, statedata obj)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.NewLineOnAttributes = true;
            xmlWriterSettings.Indent = true;

            var mySerializer = new XmlSerializer(typeof(statedata));
            XmlWriter tw = XmlWriter.Create(fullPathName, xmlWriterSettings);
            mySerializer.Serialize(tw, obj);
            tw.Close();
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
