using Code.Client;
using Code.Guns.Base;
using UnityEngine;

namespace Code.Transports.Base
{
    internal sealed class GunSlot: MonoBehaviour
    {
        [SerializeField] private Transform _placePoint;
        private Gun _placedGun;
        
        public Gun PlacedGun
        {
            get => _placedGun;
            set => _placedGun = value;
        }
        
        public void PlaceGun(Player player, Gun gun)
        {
            if (!_placedGun)
            {
                var placePointTransform = _placePoint.transform;
                var gunObject = Instantiate(gun.gameObject, placePointTransform.parent, true);
                gunObject.transform.position = placePointTransform.position;

                _placedGun = gunObject.GetComponent<Gun>();
                _placedGun.Init(player);
            }
        }

        public void RemoveGun()
        {
            Destroy(gameObject);
            _placedGun = null;
        }
    }
}