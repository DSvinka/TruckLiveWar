using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Client.Interfaces.Hud.Components
{
    internal sealed class PlayerDeath : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                RestartLevel();
        }

        private void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}