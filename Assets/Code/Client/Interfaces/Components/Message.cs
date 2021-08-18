using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Client.Interfaces.Hud.Components
{
    internal sealed class Message: MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _panel;
        [SerializeField] private int _defaultLiveTime = 5;

        public void Init(string text, int liveTime, bool viewCounter, Color color)
        {
            _panel.color = color;
            _text.text = $"{liveTime} секунд - {text}";

            int time = liveTime;
            if (time == 0)
            {
                time = _defaultLiveTime;
            }

            StartCoroutine(Hide(time, text, viewCounter));
        }

        private IEnumerator Hide(int liveTime, string text, bool viewCounter)
        {
            float counter = liveTime;
            while (counter >= 0) {
                yield return new WaitForSeconds (1);
                if (viewCounter)
                    _text.text = $"{counter} секунд - {text}";
                counter--;
            }
            Destroy(gameObject);
        }
        
        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}