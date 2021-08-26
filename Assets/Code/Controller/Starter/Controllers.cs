using System.Collections.Generic;
using Code.Interfaces;

namespace Code.Controller.Starter
{
    internal sealed class Controllers : IInitialization, IExecute, ILateExecute, ICleanup
    {
        private readonly List<IInitialization> m_initializeControllers;
        private readonly List<IExecute> m_executeControllers;
        private readonly List<ILateExecute> m_lateControllers;
        private readonly List<ICleanup> m_cleanupControllers;

        internal Controllers()
        {
            m_initializeControllers = new List<IInitialization>(8);
            m_executeControllers = new List<IExecute>(8);
            m_lateControllers = new List<ILateExecute>(8);
            m_cleanupControllers = new List<ICleanup>(8);
        }

        internal Controllers Add(IController controller)
        {
            if (controller is IInitialization initializeController)
            {
                m_initializeControllers.Add(initializeController);
            }

            if (controller is IExecute executeController)
            {
                m_executeControllers.Add(executeController);
            }

            if (controller is ILateExecute lateExecuteController)
            {
                m_lateControllers.Add(lateExecuteController);
            }
            
            if (controller is ICleanup cleanupController)
            {
                m_cleanupControllers.Add(cleanupController);
            }

            return this;
        }

        public void Initialization()
        {
            for (var index = 0; index < m_initializeControllers.Count; ++index)
            {
                m_initializeControllers[index].Initialization();
            }
        }

        public void Execute(float deltaTime)
        {
            for (var index = 0; index < m_executeControllers.Count; ++index)
            {
                m_executeControllers[index].Execute(deltaTime);
            }
        }

        public void LateExecute(float deltaTime)
        {
            for (var index = 0; index < m_lateControllers.Count; ++index)
            {
                m_lateControllers[index].LateExecute(deltaTime);
            }
        }

        public void Cleanup()
        {
            for (var index = 0; index < m_cleanupControllers.Count; ++index)
            {
                m_cleanupControllers[index].Cleanup();
            }
        }
    }
}