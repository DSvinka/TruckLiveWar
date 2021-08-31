using System.IO;

namespace Code.SaveData
{
    internal sealed class StreamData : IData<SavedData>
    {
        public void Save(SavedData data, string path = null)
        {
            if (path == null)
                return;
            using (var sw = new StreamWriter(path))
            {
                sw.WriteLine(data.Health);
                sw.WriteLine(data.Position.X);
                sw.WriteLine(data.Position.Y);
                sw.WriteLine(data.Position.Z);
                sw.WriteLine(data.IsEnabled);
            }
        }

        public SavedData Load(string path = null)
        {
            var result = new SavedData();

            using (var sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                { 
                    float.TryParse(sr.ReadLine(), out result.Health);
                    float.TryParse(sr.ReadLine(), out result.Position.X);
                    float.TryParse(sr.ReadLine(), out result.Position.Y);
                    float.TryParse(sr.ReadLine(), out result.Position.Z);
                    bool.TryParse(sr.ReadLine(), out result.IsEnabled);
                }
            }
            return result;
        }
    }
}