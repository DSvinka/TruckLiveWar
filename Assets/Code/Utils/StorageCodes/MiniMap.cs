using UnityEngine;

namespace Code.MiniMap
{
    /// <summary>
    /// СДЕЛАТЬ MVC
    /// </summary>
    internal sealed class MiniMap : MonoBehaviour
    {
        private Transform _player;

        private void Start()
        {
            var main = Camera.main;
            _player = main.transform;

            transform.parent = null;
            transform.rotation = Quaternion.Euler(90.0f, 0, 0);
            transform.position = _player.position + new Vector3(0, 5.0f, 0);

            var rt = Resources.Load<RenderTexture>("MiniMap/MiniMapTexture");

            var component = GetComponent<Camera>();
            component.targetTexture = rt;
            component.depth = --main.depth;
        }

        private void LateUpdate()
        {
            var newPosition = _player.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;
            transform.rotation = Quaternion.Euler(90, _player.eulerAngles.y, 0);
        }
    }
}