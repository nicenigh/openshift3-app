namespace Microservices.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNotNull(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }
    }
}
