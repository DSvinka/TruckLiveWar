using UnityEngine;

namespace Client.Interfaces.Hud.Components
{
    internal sealed class Bonuses : MonoBehaviour
    {
        [SerializeField] private GameObject _messagePrefab;
        [SerializeField] private GameObject _layoutGroup;

        public void CreateMessage(string text, Color color, float liveTime)
        {
            var messageObject = Instantiate(_messagePrefab, _layoutGroup.transform, false);
            var message = messageObject.GetComponent<Message>();
            message.Init(text, color, liveTime);
        }
    }
}
