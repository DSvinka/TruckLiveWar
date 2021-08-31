using System.IO;
using System.Xml;
using Cinemachine;
using Code.Utils.Extensions;
using UnityEngine;

namespace Code.SaveData
{
    internal sealed class XMLData : IData<SavedData>
    {
        public void Save(SavedData player, string path = null)
        {
            var xmlDoc = new XmlDocument();

            var rootNode = xmlDoc.CreateElement("Player");
            xmlDoc.AppendChild(rootNode);

            var element = xmlDoc.CreateElement("Health");
            element.SetAttribute("value", player.Health.ToString());
            rootNode.AppendChild(element);

            var element2 = xmlDoc.CreateElement("Test");
            element2.SetAttribute("X", player.Position.X.ToString());
            element2.SetAttribute("Y", player.Position.Y.ToString());
            element2.SetAttribute("Z", player.Position.Z.ToString());
            element.AppendChild(element2);

            element = xmlDoc.CreateElement("PosX");
            element.SetAttribute("value", player.Position.X.ToString());
            rootNode.AppendChild(element);
            
            element = xmlDoc.CreateElement("PosY");
            element.SetAttribute("value", player.Position.Y.ToString());
            rootNode.AppendChild(element);
            
            element = xmlDoc.CreateElement("PosZ");
            element.SetAttribute("value", player.Position.Z.ToString());
            rootNode.AppendChild(element);

            element = xmlDoc.CreateElement(Crypto.CryptoXOR("IsEnable"));
            element.SetAttribute("value", Crypto.CryptoXOR(player.IsEnabled.ToString()));
            rootNode.AppendChild(element);

            var userNode = xmlDoc.CreateElement("Info");
            var attribute = xmlDoc.CreateAttribute("Unity");
            attribute.Value = Application.unityVersion;
            userNode.Attributes.Append(attribute);
            userNode.InnerText = "System Language: " + Application.systemLanguage;
            rootNode.AppendChild(userNode);
            xmlDoc.Save(path);
        }

        public SavedData Load(string path = null)
        {
            var result = new SavedData();
            if (!File.Exists(path))
                return result;
            using (var reader = new XmlTextReader(path))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement("Health"))
                         float.TryParse(reader.GetAttribute("value"), out result.Health);
                    
                    if (reader.IsStartElement("PosX"))
                        float.TryParse(reader.GetAttribute("value"), out result.Position.X);
                    
                    if (reader.IsStartElement("PosY"))
                        float.TryParse(reader.GetAttribute("value"), out result.Position.Y);
                    
                    if (reader.IsStartElement("PosZ"))
                        float.TryParse(reader.GetAttribute("value"), out result.Position.Z);
                    
                    if (reader.IsStartElement(Crypto.CryptoXOR("IsEnable")))
                        bool.TryParse(reader.GetAttribute(Crypto.CryptoXOR("value")), out result.IsEnabled);
                }
            }

            return result;
        }
    }
}