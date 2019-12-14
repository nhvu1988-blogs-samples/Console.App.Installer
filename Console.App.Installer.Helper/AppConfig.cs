using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace Console.App.Installer.Helper
{
    public class AppConfig : AppSettingsReader
    {
        private XmlDocument cfgDoc;
        private readonly string docName = String.Empty;

        public AppConfig(string exePath)
        {
            docName = exePath;
            docName += ".config";
            if (cfgDoc == null)
                cfgDoc = new XmlDocument();
            cfgDoc.Load(docName);
        }

        public Dictionary<string, string> GetAppSettings()
        {
            var appSettings = new Dictionary<string, string>();

            var node = cfgDoc.SelectSingleNode("//appSettings");

            foreach (XmlNode elem in node.ChildNodes)
            {
                if (elem.NodeType != XmlNodeType.Comment)
                    appSettings.Add((string)elem.Attributes["key"].Value, (string)elem.Attributes["value"].Value);
            }
            return appSettings;
        }

        public bool SetConnectionString(string connectionName, string connectionString)
        {
            // retrieve the appSettings node 
            var node = cfgDoc.SelectSingleNode("//connectionStrings");

            if (node == null)
            {
                throw new InvalidOperationException("connectionStrings section not found");
            }

            try
            {
                // XPath select setting "add" element that contains this key 	  
                XmlElement addElem = (XmlElement)node.SelectSingleNode("//add[@name='" + connectionName + "']");
                if (addElem != null)
                {
                    addElem.SetAttribute("connectionString", connectionString);
                }
                // not found, so we need to add the element, key and value
                else
                {
                    XmlElement entry = cfgDoc.CreateElement("add");
                    entry.SetAttribute("name", connectionName);
                    entry.SetAttribute("connectionString", connectionString);
                    node.AppendChild(entry);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SetAppSettings(string key, string value)
        {
            // retrieve the appSettings node 
            var node = cfgDoc.SelectSingleNode("//appSettings");

            if (node == null)
            {
                throw new InvalidOperationException("appSettings section not found");
            }

            try
            {
                // XPath select setting "add" element that contains this key 	  
                XmlElement addElem = (XmlElement)node.SelectSingleNode("//add[@key='" + key + "']");
                if (addElem != null)
                {
                    addElem.SetAttribute("value", value);
                }
                // not found, so we need to add the element, key and value
                else
                {
                    XmlElement entry = cfgDoc.CreateElement("add");
                    entry.SetAttribute("key", key);
                    entry.SetAttribute("value", value);
                    node.AppendChild(entry);
                }
                //save it
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Save()
        {
            try
            {
                XmlTextWriter writer = new XmlTextWriter(docName, null)
                {
                    Formatting = Formatting.Indented
                };
                cfgDoc.WriteTo(writer);
                writer.Flush();
                writer.Close();
            }
            catch
            {
                throw;
            }
        }
        
    }
}
