using UnityEngine;

namespace Code.Client.Interfaces.Hud.Components
{
    internal sealed class Bonuses : MonoBehaviour
    {
        [SerializeField] private GameObject _messagePrefab;
        [SerializeField] private GameObject _layoutGroup;

        public void CreateMessage(string text, int liveTime, bool viewCounter, Color color)
        {
            var messageObject = Instantiate(_messagePrefab, _layoutGroup.transform, false);
            var message = messageObject.GetComponent<Message>();
            message.Init(text, liveTime, viewCounter, color);
        }
    }
}
