/*using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Code.Utils.Testing.Editor
{
    [CustomEditor(typeof(CreateWayPoint))]
    internal sealed class CreateWayPointEditor: UnityEditor.Editor
    {
        private CreateWayPoint m_testTarget;

        private void OnEnable()
        {
            m_testTarget = (CreateWayPoint) target;
        }

        private void OnSceneGUI()
        {
            if (Event.current.button == 0 && Event.current.type == EventType.MouseDown)
            {
                Ray ray = Camera.current.ScreenPointToRay(new Vector3(Event.current.mousePosition.x,
                    SceneView.currentDrawingSceneView.camera.pixelHeight - Event.current.mousePosition.y));

                if (Physics.Raycast(ray, out var hit))
                {
                    m_testTarget.InstantiateObj(hit.point);
                    SetObjectDirty(m_testTarget.gameObject);
                }
            }
            
            // AssetDatabase
            Selection.activeGameObject = m_testTarget.gameObject;
        }

        private void SetObjectDirty(GameObject obj)
        {
            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(obj);
                EditorSceneManager.MarkSceneDirty(obj.scene);
            }
        }
    }
}*/