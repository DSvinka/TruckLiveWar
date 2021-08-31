using System;
using System.IO;
using System.Xml.Serialization;

namespace Code.SaveData
{
    internal sealed class SerializableXMLData<T> : IData<T>
    {
        public static XmlSerializer m_formatter;

        public SerializableXMLData()
        {
            m_formatter = new XmlSerializer(typeof(T));
        }

        public void Save(T data, string path = null)
        {
            if (data == null && !String.IsNullOrEmpty(path))
                return;
            using (var fs = new FileStream(path, FileMode.Create))
                m_formatter.Serialize(fs, data);
        }

        public T Load(string path)
        {
            T result;
            if (!File.Exists(path))
                return default(T);
            using (var fs = new FileStream(path, FileMode.Open))
                result = (T) m_formatter.Deserialize(fs);
            return result;
        }
    }
}