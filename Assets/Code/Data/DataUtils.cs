using System.Collections.Generic;
using System.IO;
using Code.Interfaces.Data;
using UnityEngine;

namespace Code.Data
{
    internal static class DataUtils
    {
        public static T GetData<T>(string path, ref T obj) where T : Object
        {
            if (obj == null)
                obj = Load<T>("Data/" + path);
            
            return obj;
        }

        public static Dictionary<string, IDictData> GetDatasDict<T>(string path, ref IDictData obj) where T : Object
        {
            var _objects = new Dictionary<string, IDictData>();

            if (obj != null) 
                return _objects;
            
            var data = LoadAll<T>(path);

            
            return _objects;
        }

        public static T Load<T>(string resourcesPath) where T : Object =>
            Resources.Load<T>(Path.ChangeExtension(resourcesPath, null));
        
        public static T[] LoadAll<T>(string resourcesPath) where T : Object =>
            Resources.LoadAll<T>(Path.ChangeExtension(resourcesPath, null));
    }
}