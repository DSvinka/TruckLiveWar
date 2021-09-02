namespace Code.Utils.Extensions
{
    internal static class Crypto
    {
        public static string CryptoXOR(string text, int key = 42)
        {
            var result = string.Empty;
            foreach (var symbol in text)
            {
                result += (char) (symbol ^ key);
            }
            return result;
        }
    }
}