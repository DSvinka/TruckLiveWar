using UnityEngine;

namespace Code.Interfaces.Factory
{
    internal interface IHudFactory
    {
        Transform CreateHud();
        Transform CreateWinWindow();
        Transform CreateDeathWindow();
    }
}