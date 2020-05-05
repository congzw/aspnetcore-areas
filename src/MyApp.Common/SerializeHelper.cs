using Newtonsoft.Json;
using System;
using System.Xml;

namespace MyApp.Common
{
    public interface ISerializeHelper
    {
        string Serialize(object instance);
        object Deserialize(string content);
    }

    public interface IJsonHelper : ISerializeHelper
    {
    }

    public interface IXmlHelper : ISerializeHelper
    {
    }

    public class JsonHelper : IJsonHelper
    {
        public object Deserialize(string content)
        {
            return JsonConvert.DeserializeObject(content);
        }

        public string Serialize(object instance)
        {
            return JsonConvert.SerializeObject(instance);
        }

        private static readonly Lazy<JsonHelper> _lazy = new Lazy<JsonHelper>(() => new JsonHelper());
        public static Func<IJsonHelper> Instance = () => _lazy.Value;
    }

    public class XmlHelper : IXmlHelper
    {
        public object Deserialize(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var json = JsonConvert.SerializeXmlNode(doc);
            return JsonConvert.DeserializeObject(json);
        }

        public string Serialize(object instance)
        {
            //Elements remain unchanged.
            //Attributes are prefixed with an @ and should be at the start of the object.
            //Single child text nodes are a value directly against an element, otherwise they are accessed via #text.
            //The XML declaration and processing instructions are prefixed with ?.
            //Character data, comments, whitespace and significant whitespace nodes are accessed via #cdata-section, #comment, #whitespace and #significant-whitespace respectively.
            //Multiple nodes with the same name at the same level are grouped together into an array.
            //Empty elements are null.

            var json = JsonConvert.SerializeObject(instance);
            var typeName = instance.GetType().Name;
            var xmlDoc = JsonConvert.DeserializeXmlNode(json, typeName);
            return xmlDoc.InnerXml;
        }
        private static readonly Lazy<XmlHelper> _lazy = new Lazy<XmlHelper>(() => new XmlHelper());
        public static Func<IXmlHelper> Instance = () => _lazy.Value;
    }
}
