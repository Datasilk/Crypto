using System;
using System.Runtime.CompilerServices;

namespace Crypto
{
    public static class NumericExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ClearBit(this long value, int position)
        {
            return Math.ClearBit(value, position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ClearBit(this ulong value, int position)
        {
            return Math.ClearBit(value, position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong CleaveHigh(this ulong value)
        {
            return Math.CleaveHigh(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong CleaveLow(this ulong value)
        {
            return Math.CleaveLow(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long FlipBit(this long value, int position)
        {
            return Math.FlipBit(value, position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong FlipBit(this ulong value, int position)
        {
            return Math.FlipBit(value, position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetBit(this long value, int position)
        {
            return Math.GetBit(value, position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetBit(this ulong value, int position)
        {
            return Math.GetBit(value, position);
        }

        public unsafe static byte[] GetBytes(this uint value, byte[] destination, ulong destinationOffset)
        {
            if (destination.IsNull())
            {
                throw new ArgumentNullException(paramName: nameof(destination));
            }

            if (checked((sizeof(uint) + destinationOffset)) > unchecked((ulong)destination.LongLength))
            {
                throw new ArgumentOutOfRangeException(paramName: nameof(destinationOffset));
            }

            fixed (void* v = &destination[destinationOffset])
            {
                (*(uint*)v) = value;
            }

            return destination;
        }

        public static byte[] GetBytes(this uint value, byte[] destination)
        {
            return value.GetBytes(destination, 0U);
        }

        public static byte[] GetBytes(this uint value)
        {
            return value.GetBytes(new byte[sizeof(uint)]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOdd(this int value)
        {
            return Math.IsOdd(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOdd(this long value)
        {
            return Math.IsOdd(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOdd(this uint value)
        {
            return Math.IsOdd(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOdd(this ulong value)
        {
            return Math.IsOdd(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositive(this int value)
        {
            return Math.IsPositive(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositive(this long value)
        {
            return Math.IsPositive(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositive(this uint value)
        {
            return Math.IsPositive(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositive(this ulong value)
        {
            return Math.IsPositive(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPowerOfTwo(this long value)
        {
            return Math.IsPowerOfTwo(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPowerOfTwo(this ulong value)
        {
            return Math.IsPowerOfTwo(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(this long value)
        {
            return Math.IsZero(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(this ulong value)
        {
            return Math.IsZero(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long MaskBit(this long value, int position)
        {
            return Math.MaskBit(value, position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong MaskBit(this ulong value, int position)
        {
            return Math.MaskBit(value, position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Mod64(this long value)
        {
            return Math.Mod64(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Mod64(this ulong value)
        {
            return Math.Mod64(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReverseBits(this uint value)
        {
            Math.ReverseBits(ref value);

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReverseBytes(this uint value)
        {
            Math.ReverseBytes(ref value);

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateLeft(this uint value, int count)
        {
            return Math.RotateLeft(value, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateLeft(this ulong value, int count)
        {
            return Math.RotateLeft(value, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateRight(this uint value, int count)
        {
            return Math.RotateRight(value, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateRight(this ulong value, int count)
        {
            return Math.RotateRight(value, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long SetBit(this long value, int position)
        {
            return Math.SetBit(value, position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong SetBit(this ulong value, int position)
        {
            return Math.SetBit(value, position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Sign(this long value)
        {
            return Math.Sign(value);
        }
    }
}