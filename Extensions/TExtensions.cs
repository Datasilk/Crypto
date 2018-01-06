using System;
using System.Runtime.CompilerServices;

namespace Crypto
{
    /// <summary>
    /// A collection of extension methods that directly or indirectly augment types generically.
    /// </summary>
    public static class TExtensions
    {
        /// <summary>
        /// Attempt to dispose of the input using the specified action.
        /// </summary>
        /// <typeparam name="T">The type of the instance that will be disposed of.</typeparam>
        /// <param name="instance">The instance that wlil be disposed of.</param>
        /// <param name="disposeAction">The dispose action that will be executed.</param>
        /// <param name="isDisposed">The indicator that will be set to 'true' once disposal is complete.</param>
        public static void Dispose<T>(this T instance, Action disposeAction, ref bool isDisposed) where T : IDisposable
        {
            if (!isDisposed)
            {
                disposeAction();

                isDisposed = true;
            }
        }

        /// <summary>
        /// Indicates whether the input is not null.
        /// </summary>
        /// <param name="value">The value that will be tested.</param>
        /// <typeparam name="T">The type of the instance that will be tested.</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotNull<T>(this T value)
        {
            return (value != null);
        }

        /// <summary>
        /// Indicates whether the input is null.
        /// </summary>
        /// <param name="value">The value that will be tested.</param>
        /// <typeparam name="T">The type of the instance that will be tested.</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNull<T>(this T value)
        {
            return (value == null);
        }
    }
}