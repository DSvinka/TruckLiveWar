using Code.Controller.Initialization;
using UnityEngine;

namespace Code.Controller.Starter
{
    public sealed class GameStarter : MonoBehaviour
    {
        [SerializeField] private Data.Data m_data;
        private Controllers m_controllers;

        private void Start()
        {
            m_controllers = new Controllers();
            var game = new GameInitialization(m_controllers, m_data);
            m_controllers.Initialization();
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            m_controllers.Execute(deltaTime);
        }

        private void LateUpdate()
        {
            var deltaTime = Time.deltaTime;
            m_controllers.LateExecute(deltaTime);
        }

        private void OnDestroy()
        {
            m_controllers.Cleanup();
        }
    }
}
