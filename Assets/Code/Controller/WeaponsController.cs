using System.Linq;
using Code.Controller.Initialization;
using Code.Data;
using Code.Interfaces;
using Code.Interfaces.Providers;
using Code.Interfaces.UserInput;
using Code.Providers;
using UnityEngine;

namespace Code.Controller
{
    internal sealed class Weapon
    {
        private readonly WeaponProvider _weaponProviderPrefab;
        private readonly WeaponData _weaponData;

        public float Cooldown { get; set; }
        public WeaponProvider WeaponProvider { get; set; }
        public int Index { get; set; }

        public WeaponProvider WeaponProviderPrefab => _weaponProviderPrefab;
        public WeaponData WeaponData => _weaponData;
        
        public Weapon(WeaponData weaponData)
        {
            _weaponProviderPrefab = weaponData.Prefab;
            _weaponData = weaponData;
        }
    }
    
    internal sealed class WeaponsController: IController, IExecute, IInitialization, ICleanup
    {
        private readonly PlayerInitialization _playerInitialization;
        private readonly PlayerData _playerData;
        
        private CarProvider _carProvider;
        private Weapon[] _weapons;
        
        private IUserKeyProxy _mouseFireInputProxy;

        private bool _mouseFireInput;

        public Weapon[] Weapons => _weapons;

        public WeaponsController(
            IUserKeyProxy mouseInput,
            PlayerInitialization playerInitialization, PlayerData playerData)
        {
            _playerInitialization = playerInitialization;
            _playerData = playerData;
            _mouseFireInputProxy = mouseInput;
        }
        
        public void Initialization()
        {
            _carProvider = _playerInitialization.GetPlayerTransport();
            
            _weapons = new Weapon[_carProvider.WeaponSlots.Length];
            _mouseFireInputProxy.KeyOnChange += MouseFireButtonChange;
        }

        public void MouseFireButtonChange(bool value)
        {
            _mouseFireInput = value;
        }

        public void Execute(float deltatime)
        {
            if (_weapons.All(x => x != null))
            {
                foreach (var weapon in _weapons)
                {
                    Move(weapon);
                    Shot(weapon);

                    if (weapon.Cooldown >= 0f)
                        weapon.Cooldown -= deltatime;
                }
            }
        }

        public bool AddWeapon(int index, Weapon weapon)
        {
            if (weapon.WeaponData.SlotType != _carProvider.WeaponSlots[index].SlotType)
                return false;

            if (_weapons[index] != null)
                RemoveWeapon(index);

            _weapons[index] = weapon;
            weapon.WeaponProvider = _carProvider.PlaceWeapon(index, weapon);
            weapon.Index = index;
            return true;
        }

        public bool RemoveWeapon(int index)
        {
            if (_weapons[index] == null)
                return false;
            
            _weapons[index] = null;
            _carProvider.RemoveWeapon(index);
            return true;
        }
        
        private void Move(Weapon weapon)
        {
            var weaponProvider = weapon.WeaponProvider;
            var weaponData = weapon.WeaponData;
                        
            var gunRotation = weaponProvider.Gun.rotation;
            gunRotation = Quaternion.Lerp(gunRotation, _playerData.Camera.transform.rotation, weaponData.TurnSpeed);
            weaponProvider.Gun.rotation = gunRotation;

            var handleRotation = weaponProvider.Handle.rotation;
            handleRotation.Set(handleRotation.x, gunRotation.y, handleRotation.z, handleRotation.w); 
        }
        
        private void Shot(Weapon weapon)
        {
            var weaponProvider = weapon.WeaponProvider;
            var weaponData = weapon.WeaponData;

            if (_mouseFireInput && weapon.Cooldown <= 0) // TODO: ЧИТАЙТЕ TODO ВЫШЕ!
            {
                RaycastHit raycastHit;
                Vector3 barrelPosition = weaponProvider.FirePoint.position;
                Vector3 barrelForward = weaponProvider.FirePoint.forward;
                
                if (Physics.Raycast(barrelPosition, barrelForward, out raycastHit, weaponData.MaxDistance))
                {
                    var unit = raycastHit.collider.gameObject.GetComponent<IUnit>();
                    if (unit != null)
                    {
                        unit.AddDamage(weaponProvider.gameObject, weaponData.Damage);
                    }
                }
                weapon.Cooldown = weaponData.FireRate;
            }
        }

        public void Cleanup()
        {
            _mouseFireInputProxy.KeyOnChange -= MouseFireButtonChange;
            for (var index = 0; index < _weapons.Length; index++)
            {
                _weapons[index] = null;
            }
        }
    }   
}