using Code.Controller.Initialization;
using Code.Interfaces;

namespace Code.Controller
{
    internal sealed class HudController : IController, IInitialization, ICleanup
    {
        private readonly HudInitialization m_hudInitialization;

        public void Initialization()
        {
            throw new System.NotImplementedException();
        }

        public void Cleanup()
        {
            throw new System.NotImplementedException();
        }
    }
}