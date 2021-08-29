namespace Code.Interfaces
{
    internal interface ILateExecute : IController
    {
        void LateExecute(float deltaTime);
    }
}