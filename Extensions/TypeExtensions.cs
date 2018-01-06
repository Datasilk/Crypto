using System;
using System.Runtime.CompilerServices;

namespace Crypto
{
    /// <summary>
    /// A collection of extension methods that directly or indirectly augment the System.Type class.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns the underlying type of the input; this function only has an effect on <see cref="Nullable"/> types and is effectively a no-op in all other cases.
        /// </summary>
        /// <param name="value">The type that will have its <see cref="Nullable"/> wrapper removed.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type UnwrapIfNullable(this Type value)
        {
            return (Nullable.GetUnderlyingType(value) ?? value);
        }
    }
}