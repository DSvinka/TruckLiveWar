/*using UnityEngine;

namespace Code.Utils.Testing
{
    public enum TypeTest
    {
        Test, Name
    }
    
    public sealed class TestBehaviour: MonoBehaviour
    {
        public int Count { get; set; } = 10;
        public int Offset { get; set; } = 1;
        public TypeTest Selected { get; set; }
        public GameObject Obj { get; set; }
        
        public float Test1 { get; set; }
        private Transform m_root;

        private void Start()
        {
            CreateObj();
        }

        public void CreateObj()
        {
            m_root = new GameObject("root").transform;
            for (var i = 1; i <= Count; i++)
            {
                Instantiate(Obj, new Vector3(0, Offset * i, 0), Quaternion.identity, m_root);
            }
        }

        public void AddComponent()
        {
            gameObject.AddComponent<Rigidbody>();
            gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<BoxCollider>();
        }

        public void RemoveComponent()
        {
            DestroyImmediate(GetComponent<Rigidbody>());
            DestroyImmediate(GetComponent<MeshRenderer>());
            DestroyImmediate(GetComponent<BoxCollider>());
        }
    }
}*/