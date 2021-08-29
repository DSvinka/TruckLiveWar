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

    internal sealed class WeaponsController: IController, IExecute, IInitialization, ICleanup
    {
        private readonly PlayerInitialization m_playerInitialization;
        private readonly PlayerData m_playerData;

        private CarProvider m_carProvider;
        private Weapon[] m_weapons;

        private IUserKeyProxy m_mouseFireInputProxy;

        private bool m_mouseFireInput;

        public Weapon[] Weapons => m_weapons;

        public WeaponsController(
            IUserKeyProxy mouseInput,
            PlayerInitialization playerInitialization, PlayerData playerData)
        {
            m_playerInitialization = playerInitialization;
            m_playerData = playerData;
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
            if (m_weapons.All(x => x != null))
            {
                foreach (var weapon in m_weapons)
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

        private void Move(Weapon weapon)
        {
            var weaponProvider = weapon.WeaponProvider;
            var weaponData = weapon.WeaponData;

            var gunRotation = weaponProvider.Gun.rotation;
            gunRotation = Quaternion.Lerp(gunRotation, m_playerData.Camera.transform.rotation, weaponData.TurnSpeed);
            weaponProvider.Gun.rotation = gunRotation;

            var handleRotation = weaponProvider.Handle.rotation;
            handleRotation.Set(handleRotation.x, gunRotation.y, handleRotation.z, handleRotation.w);
        }

        private void Shot(Weapon weapon)
        {
            var weaponProvider = weapon.WeaponProvider;
            var weaponData = weapon.WeaponData;

            if (m_mouseFireInput && weapon.Cooldown <= 0) // TODO: ЧИТАЙТЕ TODO ВЫШЕ!
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
            m_mouseFireInputProxy.KeyOnChange -= MouseFireButtonChange;
            for (var index = 0; index < m_weapons.Length; index++)
            {
                m_weapons[index] = null;
            }
        }
    }
}