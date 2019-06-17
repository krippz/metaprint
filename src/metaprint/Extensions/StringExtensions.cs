namespace Extensions
{
    public static class StringExtensions
    {
        public static string GetValueOrNotAvalible(this string value)
        {
            return !string.IsNullOrEmpty(value) ? value : "N/A";
        }

        public static string Truncate(this string value, int maxChars)
        {
            if (value != null)
            {
                return value.Length <= maxChars ? value : value.Substring(0, maxChars);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}