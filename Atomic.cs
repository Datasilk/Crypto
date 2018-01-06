using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Crypto;

namespace Crypto
{
    public static class Atomic
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long And(ref long target, long value)
        {
            long oldValue;
            long newValue;

            do
            {
                oldValue = target;
                newValue = (oldValue & value);
            } while (Interlocked.CompareExchange(ref target, newValue, oldValue) != oldValue);

            return oldValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ClearBit(ref long target, int position)
        {
            long oldValue;
            long newValue;

            do
            {
                oldValue = target;
                newValue = oldValue.ClearBit(position);
            } while (Interlocked.CompareExchange(ref target, newValue, oldValue) != oldValue);

            return oldValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Decrement(ref long target)
        {
            return Interlocked.Decrement(ref target);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Increment(ref long target)
        {
            return Interlocked.Increment(ref target);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Not(ref long target)
        {
            long oldValue;
            long newValue;

            do
            {
                oldValue = target;
                newValue = (~oldValue);
            } while (Interlocked.CompareExchange(ref target, newValue, oldValue) != oldValue);

            return oldValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Or(ref long target, long value)
        {
            long oldValue;
            long newValue;

            do
            {
                oldValue = target;
                newValue = (oldValue | value);
            } while (Interlocked.CompareExchange(ref target, newValue, oldValue) != oldValue);

            return oldValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long SetBit(ref long target, int position)
        {
            long oldValue;
            long newValue;

            do
            {
                oldValue = target;
                newValue = oldValue.SetBit(position);
            } while (Interlocked.CompareExchange(ref target, newValue, oldValue) != oldValue);

            return oldValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Read(ref long target)
        {
            return Interlocked.CompareExchange(ref target, 0L, 0L);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Write(ref long target, long value)
        {
            long oldValue;
            long newValue;

            do
            {
                oldValue = target;
                newValue = value;
            } while (Interlocked.CompareExchange(ref target, newValue, oldValue) != oldValue);

            return oldValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Write(ref long target, Func<long, long> valueFunc)
        {
            long oldValue;
            long newValue;

            do
            {
                oldValue = target;
                newValue = valueFunc(target);
            } while (Interlocked.CompareExchange(ref target, newValue, oldValue) != oldValue);

            return oldValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Xor(ref long target, long value)
        {
            long oldValue;
            long newValue;

            do
            {
                oldValue = target;
                newValue = (oldValue ^ value);
            } while (Interlocked.CompareExchange(ref target, newValue, oldValue) != oldValue);

            return oldValue;
        }
    }
}