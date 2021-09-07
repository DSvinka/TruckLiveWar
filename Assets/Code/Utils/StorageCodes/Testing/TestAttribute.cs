/*using System;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Code.Utils.Testing
{
    [RequireComponent(typeof(Renderer)), ExecuteInEditMode]
    internal sealed class TestAttribute: MonoBehaviour
    {
        [HideInInspector] public float TestPublic;

        [Serializable]
        private struct MyStruct
        {
            public int T;
        }

        [SerializeField] private MyStruct Struct;
        private const int k_min = 0;
        private const int k_max = 100;
        [Header("TestLayerMask variables")] 
        [ContextMenuItem("Randomize Number", nameof(Randomize))] 
        [SerializeField] private float m_testPrivate = 7;

        [Range(k_min, k_max)] 
        public int SecondTest;
        private int m_privateTest;

        [Space(60)] 
        [SerializeField, Multiline(5)] private string m_testMultiline;
        [Space(60)] 
        [SerializeField, TextArea(5, 5), Tooltip("Tooltip text")] private string _testTextArea;

        private void OnGUI()
        {
            GUI.Button(new Rect(50, 50, 50, 50), "Roman");
        }

        private void Update()
        {
            GetComponent<Renderer>().sharedMaterial.color = UnityEngine.Random.ColorHSV();
        }

        private void Randomize()
        {
            m_testPrivate = UnityEngine.Random.Range(k_min, k_max);
            //TestObsolete();
        }

        [Obsolete("Устарело. Используй что-то другое")]
        private void TestObsolete()
        {
            
        }
    }
}*/