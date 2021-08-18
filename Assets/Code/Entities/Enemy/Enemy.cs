using Code.Entities.Base;
using UnityEngine;

namespace Code.Entities.Enemy
{
    public class Enemy: Entity
    {
        protected override void Death()
        {
            base.Death();
            Debug.Log("Не-е-ет!"); // TODO: Заменить это на HUD
        }
    }
}