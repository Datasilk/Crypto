using System.Runtime.CompilerServices;

namespace Crypto
{
    /// <summary>
    /// A collection of extension methods that directly or indirectly augment the System.String class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Indicates whether the input string is null, empty, or consists entirely of white-space characters.
        /// </summary>
        /// <param name="value">The string that will be tested.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}