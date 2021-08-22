using System.IO;
using UnityEngine;

namespace Code.Data
{
    internal static class DataUtils
    {
        public static T GetData<T>(string path, ref T obj) where T : Object
        {
            if (obj == null)
            {
                obj = Load<T>("Data/" + path);
            }
            return obj;
        }
        
        public static T Load<T>(string resourcesPath) where T : Object =>
            Resources.Load<T>(Path.ChangeExtension(resourcesPath, null));
    }
}