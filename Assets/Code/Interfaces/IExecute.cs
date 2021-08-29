namespace Code.Interfaces
{
    internal interface IExecute : IController
    {
        void Execute(float deltaTime);
    }
}