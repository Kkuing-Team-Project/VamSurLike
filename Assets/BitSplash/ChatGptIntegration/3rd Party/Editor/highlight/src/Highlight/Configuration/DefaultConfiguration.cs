using System.Xml.Linq;
using UnityEngine;

namespace Highlight.Configuration
{
    public class DefaultConfiguration : XmlConfiguration
    {
        public DefaultConfiguration()
        {
            var textAsset = Resources.Load<TextAsset>("DefaultDefinitions");   
            XmlDocument = XDocument.Parse(textAsset.text);
        }
    }
}
