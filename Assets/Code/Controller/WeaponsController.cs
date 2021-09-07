using System.Linq;
using Code.Controller.Initialization;
using Code.Data;
using Code.Interfaces;
using Code.Interfaces.Providers;
using Code.Interfaces.UserInput;
using Code.Providers;
using Code.Utils.Extensions;
using UnityEngine;

namespace Code.Controller
{
    public sealed class Weapon
    {
        private readonly WeaponProvider m_weaponProviderPrefab;
        private readonly WeaponData m_weaponData;

        public float Cooldown { get; set; }
        public WeaponProvider WeaponProvider { get; set; }
        public int Index { get; set; }

        public WeaponProvider WeaponProviderPrefab => m_weaponProviderPrefab;
        public WeaponData WeaponData => m_weaponData;

        public Weapon(WeaponData weaponData)
        {
            m_weaponProviderPrefab = weaponData.Prefab;
            m_weaponData = weaponData;
        }
    }

    public sealed class WeaponsController: IController, IExecute, IInitialization, ICleanup
    {
        private readonly PlayerInitialization m_playerInitialization;

        private CarProvider m_carProvider;
        private Weapon[] m_weapons;

        private IUserKeyProxy m_mouseFireInputProxy;

        private bool m_mouseFireInput;

        public Weapon[] Weapons => m_weapons;
        public Weapon[] SetWeaponOnSpawn { get; set; }

        public WeaponsController(
            IUserKeyProxy mouseInput,
            PlayerInitialization playerInitialization)
        {
            m_playerInitialization = playerInitialization;
            m_mouseFireInputProxy = mouseInput;
        }

        public void Initialization()
        {
            m_carProvider = m_playerInitialization.GetPlayerTransport();

            m_weapons = new Weapon[m_carProvider.WeaponSlots.Length];
            m_mouseFireInputProxy.KeyOnChange += MouseFireButtonChange;
        }

        public void MouseFireButtonChange(bool value)
        {
            m_mouseFireInput = value;
        }

        public void Execute(float deltatime)
        {
            if (m_carProvider == null)
            {
                m_carProvider = m_playerInitialization.GetPlayerTransport();
                InitWeapon();
                return;
            }
            
            if (m_weapons.Any(x => x == null)) 
                return;
            
            foreach (var weapon in m_weapons)
            {
                Move(weapon);
                Shot(weapon);

                if (weapon.Cooldown >= 0f)
                    weapon.Cooldown -= deltatime;
            }
        }
        
#pragma warning disable 618
        public bool SetWeapon(int index, Weapon weapon)
        {
            if (weapon.WeaponData.SlotType != m_carProvider.WeaponSlots[index].SlotType)
                return false;

            if (m_weapons[index] != null)
                RemoveWeapon(index);

            m_weapons[index] = weapon;

            weapon.WeaponProvider = m_carProvider.PlaceWeapon(index, weapon);

            weapon.Index = index;
            return true;
        }

        public bool RemoveWeapon(int index)
        {
            if (m_weapons[index] == null)
                return false;

            m_weapons[index] = null;
            m_carProvider.RemoveWeapon(index);
            return true;
        }

        public void InitWeapon()
        {
            if (SetWeaponOnSpawn != null && SetWeaponOnSpawn.Length != 0)
            {
                for (var index = 0; index < SetWeaponOnSpawn.Length; index++)
                {
                    var weapon = SetWeaponOnSpawn[index];
                    SetWeapon(index, weapon);
                }
            }
        }
#pragma warning restore 618
        
        private void Move(Weapon weapon)
        {
            var weaponProvider = weapon.WeaponProvider;
            var weaponData = weapon.WeaponData;

            var gunRotation = weaponProvider.Gun.rotation;
            gunRotation = Quaternion.Lerp(gunRotation, m_playerInitialization.Camera.transform.rotation, weaponData.TurnSpeed);
            weaponProvider.Gun.rotation = gunRotation;

            var handleRotation = weaponProvider.Handle.rotation;
            handleRotation.Set(handleRotation.x, gunRotation.y, handleRotation.z, handleRotation.w);
        }

        private void Shot(Weapon weapon)
        {
            var weaponProvider = weapon.WeaponProvider;
            var weaponData = weapon.WeaponData;
            
            if (m_mouseFireInput && weapon.Cooldown <= 0)
            {
                var barrelPosition = weaponProvider.FirePoint.position;
                var barrelForward = weaponProvider.FirePoint.forward;

                if (Physics.Raycast(barrelPosition, barrelForward, out var raycastHit, weaponData.MaxDistance))
                {
                    if (m_carProvider.gameObject.GetInstanceID() != raycastHit.collider.gameObject.GetInstanceID())
                    {
                        var unit = raycastHit.collider.gameObject.GetComponent<IUnit>();
                        unit?.AddDamage(weaponProvider.gameObject, weaponData.Damage);
                    }
                }
                weapon.Cooldown = weaponData.FireRate;
            }
        }

        public void Cleanup()
        {
            m_mouseFireInputProxy.KeyOnChange -= MouseFireButtonChange;
        }
    }
}