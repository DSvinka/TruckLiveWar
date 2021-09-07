/*using UnityEditor;
using UnityEngine;

namespace Code.Utils.Testing.Editor
{
    [CustomEditor(typeof(TestBehaviour))]
    internal sealed class TestBehaviourEditor : UnityEditor.Editor
    {
        private bool m_isPressButtonOk;

        public override void OnInspectorGUI()
        {
            // DrawDefaultInspector();

            var testTarget = (TestBehaviour) target;

            testTarget.Count = EditorGUILayout.IntSlider(testTarget.Count, 10, 50);
            testTarget.Offset = EditorGUILayout.IntSlider(testTarget.Offset, 1, 5);

            testTarget.Selected = (TypeTest) EditorGUILayout.EnumPopup("Selected", testTarget.Selected);
            testTarget.Obj = EditorGUILayout.ObjectField("Объект который мы хотим вставить", testTarget.Obj, typeof(GameObject), false) as GameObject;

            var isPressButton = GUILayout.Button("Создание объектов по кнопке", EditorStyles.miniButton);

            m_isPressButtonOk = GUILayout.Toggle(m_isPressButtonOk, "Ok");

            if (isPressButton)
            {
                testTarget.CreateObj();
                m_isPressButtonOk = true;
            }

            if (m_isPressButtonOk)
            {
                testTarget.Test1 = EditorGUILayout.Slider(testTarget.Test1, 10, 50);
                EditorGUILayout.HelpBox("Вы нажали на кнопку", MessageType.Warning);
                
                var isPressAddButton = GUILayout.Button("Add Component", EditorStyles.miniButtonLeft);
                var isPressRemoveButton = GUILayout.Button("Remove Component", EditorStyles.miniButtonLeft);

                if (isPressAddButton)
                {
                    testTarget.AddComponent();
                }

                if (isPressRemoveButton)
                {
                    testTarget.RemoveComponent();
                }
            }
        }
    }
}*/