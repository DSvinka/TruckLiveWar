using System;
using UnityEngine;

namespace Code.SaveData.Data
{
    [Serializable]
    internal struct Vector3Serializable
    {
        public float X;
        public float Y;
        public float Z;

        private Vector3Serializable(float valueX, float valueY, float valueZ)
        {
            X = valueX;
            Y = valueY;
            Z = valueZ;
        }

        public static implicit operator Vector3(Vector3Serializable value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }

        public static implicit operator Vector3Serializable(Vector3 value)
        {
            return new Vector3Serializable(value.x, value.y, value.z);
        }

        public override string ToString() => $"[ X = {X}, Y = {Y}, Z = {Z} ]";
    }

    [Serializable]
    internal struct Car
    {
        public string PathToData;
        
        public float Health { get; set; }
        public float Fuel { get; set; }
    }

    [Serializable]
    internal struct Weapon
    {
        public string PathToData;
        
        public int Ammo { get; set; }
        public int AmmoInClip { get; set; }
    }
    
    [Serializable]
    internal sealed class PlayerSaveData
    {
        public Car Car;
        public Weapon[] Weapons;

        public Vector3Serializable Position;
        public Vector3Serializable Rotation;

        public override string ToString() =>
            $"<color=red>Health</color> {Car.Health} \n<color=red>Position</color> {Position} \n<color=red>Rotation</color> {Rotation}";
    }
}