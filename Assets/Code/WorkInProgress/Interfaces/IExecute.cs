namespace Code.Interfaces
{
    public interface IExecute : IController
    {
        void Execute(float deltaTime);
    }
}