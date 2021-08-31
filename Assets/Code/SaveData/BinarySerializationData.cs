using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Code.Utils.Extensions;

namespace Code.SaveData
{
    internal sealed class BinarySerializationData<T> : IData<T>
    {
        private static BinaryFormatter m_formatter;

        public BinarySerializationData()
        {
            m_formatter = new BinaryFormatter();
        }

        public void Save(T data, string path = null)
        {
            if (data == null && !string.IsNullOrEmpty(path))
                throw new ArgumentException("Data не может быть равен null если имеется Path!");
            if (!typeof(T).IsSerializable)
                throw new InvalidOperationException("T не может быть сериализированым.");
            
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