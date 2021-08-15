using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Interfaces.Hud.Components
{
    internal sealed class Message: MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _panel;
        [SerializeField] private float _defaultLiveTime = 5f;

        public void Init(string text, Color color, float liveTime)
        {
            _text.text = text;
            _panel.color = color;

            float time = liveTime;
            if (time == 0)
                time = _defaultLiveTime;

            StartCoroutine(Hide(time));
        }

        private IEnumerator Hide(float liveTime)
        {
            yield return new WaitForSecondsRealtime(liveTime);
            Destroy(gameObject);
        }
        
        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}