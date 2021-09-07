using System;
using System.Collections.Generic;
using System.IO;
using Code.Interfaces.Data;
using Code.Utils.Extensions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Data
{
    internal static class DataUtils
    {
        public static T GetData<T>(string path, ref T obj) where T : Object
        {
            if (obj == null)
            {
                obj = Load<T>(path);
            }
            
            if (obj is IData item)
                item.Path = path;

            return obj;
        }

        public static Dictionary<string, T> GetDatasDict<T>(string path, ref Dictionary<string, T> obj) where T : Object
        {
            if (obj != null)
                return obj;
            
            obj = new Dictionary<string, T>();

            var datas = LoadAll<T>(path);

            foreach (var data in datas)
            {
                var dictData = data as IDictData;
                if (dictData == null)
                    throw new Exception("Локация не имеет интерфейса IDictData.");
                obj.Add(dictData.IDName, data);
            }

            return obj;
        }

        private static T Load<T>(string path) where T : Object =>
            AssetDatabase.LoadAssetAtPath<T>(path);
        
        private static T[] LoadAll<T>(string path) where T : Object =>
            Resources.LoadAll<T>(path);
    }
}