using System;
using Code.Data;
using Code.Providers;
using UnityEngine;

namespace Code.SaveData
{
    [Serializable]
    internal sealed class PlayerSaveData
    {
        public float Health;
        public CarData Car;
        public WeaponSlot[] Weapons;

        public Vector3Serializable Position;
        public QuaternionSerializable Rotation;

        public override string ToString() =>
            $"<color=red>Health</color> {Health} <color=red>Position</color> {Position}";

        [Serializable]
        public struct QuaternionSerializable
        {
            public float X;
            public float Y;
            public float Z;

            private QuaternionSerializable(float valueX, float valueY, float valueZ)
            {
                X = valueX;
                Y = valueY;
                Z = valueZ;
            }

            public static implicit operator Quaternion(QuaternionSerializable value)
            {
                return new Quaternion(value.X, value.Y, value.Z, 0f);
            }

            public static implicit operator QuaternionSerializable(Quaternion value)
            {
                return new QuaternionSerializable(value.x, value.y, value.z);
            }

            public override string ToString() => $"[ X = {X}, Y = {Y}, Z = {Z} ]";
        }
        
        [Serializable]
        public struct Vector3Serializable
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
    }
}