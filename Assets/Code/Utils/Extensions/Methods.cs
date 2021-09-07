using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Code.Providers;
using UnityEngine;

namespace Code.Utils.Extensions
{
    public static class Methods
    {
        public static T AddTo<T>(this T self, ICollection<T> coll)
        {
            coll.Add(self);
            return self;
        }

        public static T[] Increase<T>(this T[] values, int increment)
        {
            T[] array = new T[values.Length + increment];
            values.CopyTo(array, 0);
            return array;
        }

        public static void DebugLog<T>(this T self)
        {
            Debug.Log(self);
        }

        public static Dictionary<TKey, TValue> DictDebugLog<TKey, TValue>(this Dictionary<TKey, TValue> self)
        {
            var array = self.ToArray();

            Debug.Log("==== DictDebugLog ====");
            for (var index = 0; index < array.Length; index++)
            {
                var value = array[index];
                Debug.Log($"{value.Key} - {value.Value}");
            }

            Debug.Log("==== =========== ====");
            return self;
        }
        
        public static void ArrayDebugLog<T>(this IList<T> self)
        {
            Debug.Log("==== ArrayDebugLog ====");
            for (var index = 0; index < self.Count; index++)
            {
                var value = self[index];
                Debug.Log($"{index} - {value}");
            }
            Debug.Log("==== =========== ====");
        }
        
        public static void ListDebugLog<T>(this IList<T> self)
        {
            Debug.Log("==== ListDebugLog ====");
            for (var index = 0; index < self.Count; index++)
            {
                var value = self[index];
                Debug.Log($"{index} - {value}");
            }

            Debug.Log("==== =========== ====");
        }

        public static bool IsOneOf<T>(this T self, params T[] elem) where T : struct
        {
            return elem.Contains(self);
        }

        public static T GetOrAddComponent<T>(this GameObject child) where T : Component
        {
            var result = child.GetComponent<T>();
            if (result == null)
            {
                result = child.AddComponent<T>();
            }

            return result;
        }

        public static int ReturnNearestIndex(this Vector3[] nodes, Vector3 destination)
        {
            var nearestDistance = Mathf.Infinity;
            var index = 0;
            var length = nodes.Length;

            for (int i = 0; i < length; i++)
            {
                var distanceToNode = (destination + nodes[i]).sqrMagnitude;
                if (!(nearestDistance > distanceToNode)) continue;
                nearestDistance = distanceToNode;
                index = i;
            }

            return index;
        }

        public static T ReturnRandom<T>(this List<T> list, T[] itemsToExclude)
        {
            var val = list[UnityEngine.Random.Range(0, list.Count)];

            while (itemsToExclude.Contains(val))
                val = list[UnityEngine.Random.Range(0, list.Count)];

            return val;
        }

        public static T ReturnRandom<T>(this List<T> list)
        {
            var val = list[UnityEngine.Random.Range(0, list.Count)];
            return val;
        }

        public static Vector3 UpdateX(this Vector3 vector, float value)
        {
            vector = new Vector3(value, vector.y, vector.z);
            return vector;
        }

        public static Vector3 UpdateY(this Vector3 vector, float value)
        {
            vector = new Vector3(vector.x, value, vector.z);
            return vector;
        }

        public static Vector3 UpdateZ(this Vector3 vector, float value)
        {
            vector = new Vector3(vector.x, vector.y, value);
            return vector;
        }
        
        public static Vector3 UpdateAll(this Vector3 vector, float value)
        {
            vector = new Vector3(vector.x * value, vector.y * value, vector.z * value);
            return vector;
        }

        public static Color SetColorAlpha(this Color c, float alpha)
        {
            return new Color(c.r, c.g, c.b, alpha);
        }

        public static T[] Concat<T>(this T[] x, T[] y)
        {
            if (x == null) throw new ArgumentNullException("x");
            if (x == null) throw new ArgumentNullException("y");
            var oldLen = x.Length;
            Array.Resize<T>(ref x, x.Length + y.Length);
            Array.Copy(y, 0, x, oldLen, x.Length);
            return x;
        }

        public static T DeepCopy<T>(this T self)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("Аргумент должен быть Serializable");
            }

            if (ReferenceEquals(self, null))
            {
                return default;
            }

            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, self);
                stream.Seek(0, SeekOrigin.Begin);
                return (T) formatter.Deserialize(stream);
            }
        }

        public static Transform FindDeep(this Transform obj, string name)
        {
            if (obj.name == name)
            {
                return obj;
            }

            var count = obj.childCount;
            for (var i = 0; i < count; ++i)
            {
                var posObj = obj.GetChild(i).FindDeep(name);
                if (posObj != null)
                {
                    return posObj;
                }
            }

            return null;
        }

        public static Transform FindDeep(this Transform obj, int id)
        {
            if (obj.GetInstanceID() == id)
            {
                return obj;
            }

            var count = obj.childCount;
            for (var i = 0; i < count; ++i)
            {
                var posObj = obj.GetChild(i).FindDeep(id);
                if (posObj != null)
                {
                    return posObj;
                }
            }

            return null;
        }

        public static List<T> GetAll<T>(this Transform obj)
        {
            var results = new List<T>();
            obj.GetComponentsInChildren(results);
            return results;
        }
    }
}
