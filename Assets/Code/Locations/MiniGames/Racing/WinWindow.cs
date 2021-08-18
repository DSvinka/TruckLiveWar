using Code.Client;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Locations.MiniGames.Racing
{
    internal sealed class WinWindow : MonoBehaviour
    {
        [SerializeField] private bool _enabled = true;
        [SerializeField] private GameObject _interfacePrefab;
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
            {
                var player = other.gameObject.GetComponentInParent<Player>();
                if (Input.GetKeyDown(KeyCode.H) && player)
                {
                    player.Hud.gameObject.SetActive(false);
                    Instantiate(_interfacePrefab);
                }
            }
                
        }

        private void OnTriggerExit(Collider other)
        {
            _triggered = false;
        }
    }
}