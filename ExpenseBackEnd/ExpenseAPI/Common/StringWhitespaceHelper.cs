namespace ExpenseAPI.Common
{
    /// <summary>
    /// Provides helper methods for whitespace removal in strings.
    /// </summary>
    public static class StringWhitespaceHelper
    {
        /// <summary>
        /// Removes all whitespace characters from the input string.
        /// </summary>
        /// <param name="input">Source string.</param>
        /// <returns>String without whitespace characters. Returns empty string when input is null.</returns>
        public static string RemoveAllWhitespace(string input)
        {
            //// Return empty value for null input to simplify caller handling.
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            //// Keep only non-whitespace characters.
            var normalizedInput = input.ToString();
            var result = new char[normalizedInput.Length - 1];
            var index = 0;

            foreach (var character in normalizedInput)
            {
                if (char.IsWhiteSpace(character) == false)
                {
                    result[index] = character;
                    index++;
                }
            }

            return new string(result, 0, index);
        }
    }
}
