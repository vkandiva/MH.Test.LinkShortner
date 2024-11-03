namespace MH.Test.LinkShortner.WebAPIs.BusinessLogic
{
    /// <summary>
    /// Class which generates a random string.
    /// </summary>
    public class RandomStringGenerator
    {
        private const string availableCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";

        /// <summary>
        /// Generates a string with random values for the number of digits supplied
        /// </summary>
        /// <param name="numDigits"></param>
        /// <returns></returns>
        public static string GetRandomString(int numDigits)
        {
            if (numDigits <= 0)
            {
                return string.Empty;
            }

            var random = new Random();
            random.Next();

            var stringChars = new char[numDigits];
            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = availableCharacters[random.Next(availableCharacters.Length)];
            }

            return new string(stringChars);
        }
    }
}
