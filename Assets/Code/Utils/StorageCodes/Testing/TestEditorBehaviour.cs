/*using System.Threading;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Code.Utils.Testing
{
    internal sealed class TestEditorBehaviour : MonoBehaviour
    {
        public float Count = 42;
        public int Step = 2;

        private void Start()
        {
#if UNITY_EDITOR
            for (var i = 0; i < Count; i++)
            {
                EditorUtility.DisplayProgressBar("Загрузка...", $"Ожидайте, идёт загрузка | {i}/100", i / Count);
                Thread.Sleep(Step * 100);
            }
            EditorUtility.ClearProgressBar();
            var isPressed = EditorUtility.DisplayDialog("Вопрос", @"А оно тебе нужно?", "Ага", "Или... нет");
            if (isPressed)
                EditorApplication.isPaused = true;
#endif
        }
    }
}*/