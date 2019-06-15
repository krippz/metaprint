namespace Extensions
{
    public static class StringExtensions
    {
        public static string GetValueOrNotAvalible(this string value)
        {
            return !string.IsNullOrEmpty(value) ? value : "N/A";
        }
    }
}