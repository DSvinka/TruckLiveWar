using System;
using System.Collections.Generic;
using System.Linq;
using Code.Controller.Initialization;
using Code.Data;
using Code.Interfaces;
using Code.Interfaces.Providers;
using Code.Providers;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Controller.UI
{
    internal struct BonusMessage
    {
        private readonly GameObject m_gameObject;
        private readonly Modificator m_modificator;
        private readonly TMP_Text m_text;

        public GameObject GameObject() => m_gameObject;
        public Modificator Modificator() => m_modificator;
        public TMP_Text Text() => m_text;

        public BonusMessage(GameObject gameObject, Modificator modificator, TMP_Text mText)
        {
            m_gameObject = gameObject;
            m_modificator = modificator;
            m_text = mText;
        }
    }
    
    internal sealed class HudController : IController, IExecute, IInitialization, ICleanup
    {
        private readonly HudInitialization m_hudInitialization;
        private readonly ModificatorsController m_modificatorsController;
        private readonly PlayerInitialization m_playerInitialization;
        private readonly PlayerData m_playerData;
        private readonly CarController m_carController;

        private HudProvider m_hudProvider;
        private List<BonusMessage> m_bonusMessages;

        public HudController(HudInitialization hudInitialization, ModificatorsController modificatorsController, CarController carController, PlayerInitialization playerInitialization, PlayerData mPlayerData)
        {
            m_modificatorsController = modificatorsController;
            m_hudInitialization = hudInitialization;
            m_playerInitialization = playerInitialization;
            m_playerData = mPlayerData;
            m_carController = carController;
        }

        public void Initialization()
        {
            m_bonusMessages = new List<BonusMessage>();
            
            var hudTransform = m_hudInitialization.GetPlayerHud();
            m_hudProvider = hudTransform.GetComponent<HudProvider>();
            if (m_hudProvider == null)
                throw new Exception("Компонент HudProvider отсуствует!");
            
            m_hudProvider.EnemyInformation.GameObject.SetActive(false);
            m_hudProvider.Message.GameObject.SetActive(false);

            m_modificatorsController.ModificatorCreate += ModificatorCreated;
            m_modificatorsController.ModificatorRemove += ModificatorRemoved;

            m_carController.CarExplosion += EndGame;
        }

        public void Execute(float deltaTime)
        {
            if (m_carController.CarProvider == null)
                return;
            
            EnemyInformation();
            Modificators();
        }

        private void EnemyInformation()
        {
            var enemyInfo = m_hudProvider.EnemyInformation;
            var transformCamera = m_playerInitialization.Camera.transform;
            
            if (Physics.Raycast(transformCamera.position, transformCamera.forward, out var hit, m_playerData.RayDistance))
            {
                if (hit.collider.gameObject.GetInstanceID() == m_carController.CarProvider.gameObject.GetInstanceID())
                    return;
                
                var unit = hit.collider.gameObject.GetComponent<IUnit>();
                
                if (unit != null)
                {
                    enemyInfo.GameObject.SetActive(true);
                    enemyInfo.NameText.text = unit.UnitData.Name;
                    enemyInfo.HealthText.text = $"HP: {unit.Health}";
                }
                else if (enemyInfo.GameObject.activeSelf)
                {
                    enemyInfo.GameObject.SetActive(false);
                }
            }
        }

        private void Modificators()
        {
            if (m_bonusMessages.Count == 0)
                return;
            
            foreach (var message in m_bonusMessages)
            {
                var modificator = message.Modificator();
                var data = modificator.Data;
                
                message.Text().text = $"[{(int) modificator.Cooldown} секунд] {data.ModificatorName}";
            }
        }

        private void ModificatorCreated(Modificator modificator)
        {
            var data = modificator.Data;
            var bonusesList = m_hudProvider.BonusesList;
            
            var messageObject = Object.Instantiate(bonusesList.MessagePrefab, bonusesList.Content.transform);
            var text = messageObject.GetComponentInChildren<TMP_Text>();
            text.text = $"[{(int) modificator.Cooldown} секунд] {data.ModificatorName}";

            var message = new BonusMessage(messageObject, modificator, text);
            m_bonusMessages.Add(message);
        }
        private void ModificatorRemoved(Modificator modificator)
        {
            var message = m_bonusMessages.FirstOrDefault(x => Equals(x.Modificator(), modificator));
            Object.Destroy(message.GameObject());
            m_bonusMessages.Remove(message);
        }

        private void EndGame(CarController carController)
        {
            m_hudInitialization.DisableAllHud();
            m_hudInitialization.GetDeathHud().gameObject.SetActive(true);
        }

        public void SetMessage(string text)
        {
            var message = m_hudProvider.Message;
            if (message.GameObject.activeSelf) 
                return;
            
            message.GameObject.SetActive(true);
            message.Text.text = text;

        }

        public void RemoveMessage()
        {
            var message = m_hudProvider.Message;
            if (message.GameObject.activeSelf) 
                message.GameObject.SetActive(false);
        }

        public void Cleanup()
        {
            m_modificatorsController.ModificatorCreate -= ModificatorCreated;
            m_modificatorsController.ModificatorRemove -= ModificatorRemoved;
            m_carController.CarExplosion -= EndGame;
        }
    }
}