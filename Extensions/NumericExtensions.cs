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
        public static uint RotateRight(this uint value, int count)
        {
            return Math.RotateRight(value, count);
        }
    }
}