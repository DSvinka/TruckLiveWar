using System;
using Code.Controller.Initialization;
using Code.Interfaces;
using Code.Interfaces.Input;
using Code.Providers;
using Code.SaveData;
using Code.UserInput.Inputs;
using UnityEngine;

namespace Code.Controller.UI
{
    internal sealed class EscapeMenuController : IController, IInitialization, ICleanup
    {
        private readonly PlayerInitialization m_playerInitialization;
        private readonly LocationInitialization m_locationInitialization;
        private readonly EscapeMenuInitilization m_escapeMenuInitilization;
        private readonly CarController m_carController;
        
        private EscapeMenuProvider m_escapeMenuProvider;
        private SaveDataRepository m_saveDataRepository;
        
        private bool m_escapeInput;
        private IUserKeyDownProxy m_escapeInputProxy;

        public EscapeMenuController(EscapeMenuInitilization escapeMenuInitilization,
            PlayerInitialization playerInitialization, CarController carController,
            LocationInitialization locationInitialization, SaveDataRepository saveDataRepository)
        {
            m_saveDataRepository = saveDataRepository;
            m_locationInitialization = locationInitialization;
            m_playerInitialization = playerInitialization;
            m_escapeMenuInitilization = escapeMenuInitilization;
            m_carController = carController;
            m_escapeInputProxy = KeysInput.Escape;
        }

        public void Initialization()
        {
            var transform = m_escapeMenuInitilization.GetEscapeMenu();
            m_escapeMenuProvider = transform.GetComponent<EscapeMenuProvider>();
            if (m_escapeMenuProvider == null)
                throw new Exception("Компонент EscapeMenuProvider отсуствует!");
            
            m_escapeMenuProvider.RestartButton.onClick.AddListener(RestartGame);
            m_escapeMenuProvider.SaveButton.onClick.AddListener(SaveGame);
            m_escapeMenuProvider.LoadButton.onClick.AddListener(LoadGame);
            m_escapeMenuProvider.ExitButton.onClick.AddListener(ExitGame);

            m_escapeInputProxy.KeyOnDown += OnChangeEscapeKey;
        }

        private void OnChangeEscapeKey(bool value)
        {
            if (value)
            {
                CloseOpenMenu(!m_escapeMenuProvider.gameObject.activeSelf);
            }
                
        }

        private void CloseOpenMenu(bool value)
        {
            m_escapeMenuProvider.gameObject.SetActive(value);
            if (value)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
                
        }
        
        private void RestartGame()
        {
            m_locationInitialization.ChangeLocation();
            CloseOpenMenu(false);
        }

        private void SaveGame()
        {
            m_saveDataRepository.Save(m_carController);
            CloseOpenMenu(false);
        }

        private void LoadGame()
        {
            m_saveDataRepository.Load(m_playerInitialization);
            CloseOpenMenu(false);
        }

        private void ExitGame()
        {
            Application.Quit(-1);
            CloseOpenMenu(false);
        }

        public void Cleanup()
        {
            m_escapeMenuProvider.RestartButton.onClick.RemoveListener(RestartGame);
            m_escapeMenuProvider.SaveButton.onClick.RemoveListener(SaveGame);
            m_escapeMenuProvider.LoadButton.onClick.RemoveListener(LoadGame);
            m_escapeMenuProvider.ExitButton.onClick.RemoveListener(ExitGame);
            
            m_escapeInputProxy.KeyOnDown -= OnChangeEscapeKey;
        }
    }
}