namespace Code.Interfaces.SaveData
{
    internal interface IData<T>
    {
        void Save(T data, string path = null);
        T Load(string path = null);
    }
}