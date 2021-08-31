using System;
using System.Collections.Generic;
using System.IO;
using Code.Interfaces.Data;
using Code.Utils.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

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

        public static Dictionary<string, T> GetDatasDict<T>(string path, ref Dictionary<string, T> obj) where T : Object
        {
            if (obj != null)
                return obj;
            
            obj = new Dictionary<string, T>();

            var datas = LoadAll<T>("Data/" + path);

            foreach (var data in datas)
            {
                var dictData = data as IDictData;
                if (dictData == null)
                    throw new Exception("Локация не имеет интерфейса IDictData 0_o");
                obj.Add(dictData.IDName, data);
            }

            return obj;
        }

        private static T Load<T>(string resourcesPath) where T : Object =>
            Resources.Load<T>(Path.ChangeExtension(resourcesPath, null));
        
        private static T[] LoadAll<T>(string resourcesPath) where T : Object =>
            Resources.LoadAll<T>(Path.ChangeExtension(resourcesPath, null));
    }
}