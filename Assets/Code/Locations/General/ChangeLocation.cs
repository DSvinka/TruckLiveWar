using Code.Client;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Locations.General
{
    internal sealed class ChangeLocation : MonoBehaviour
    {
        [SerializeField] private bool _enabled = true;
        [SerializeField] private SceneAsset _scene;
        private bool _triggered;

        private void OnTriggerEnter(Collider other)
        {
            if (_enabled && !_triggered && other.gameObject.GetComponentInParent<Player>())
            {
                Debug.Log("Нажмите кнопку H чтобы перейти на другую локацию");
                _triggered = true;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (_enabled && _triggered)
                if (Input.GetKeyDown(KeyCode.H) && other.gameObject.GetComponentInParent<Player>())
                    LoadLocation();
        }

        private void OnTriggerExit(Collider other)
        {
            _triggered = false;
        }

        private void LoadLocation()
        {
            _triggered = false;
            if (_scene == null)
                Debug.LogError("Локации нету :(");
            else 
                SceneManager.LoadScene(_scene.name);
        }
    }
}
