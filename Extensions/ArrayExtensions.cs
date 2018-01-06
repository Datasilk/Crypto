using System;
using System.Runtime.CompilerServices;

namespace Crypto
{
    /// <summary>
    /// A collection of extension methods that directly or indirectly augment the System.Array class.
    /// </summary>
    public static class ArrayExtensions
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void VectorXor(this byte[] source, byte[] other, ulong count, ulong sourceOffset, ulong otherOffset)
        {
            if (ReferenceEquals(source, other) && (sourceOffset == otherOffset)) { return; }
            if (checked(sourceOffset + count) > unchecked((ulong)source.LongLength)) { throw new IndexOutOfRangeException(); }
            if (checked(otherOffset + count) > unchecked((ulong)other.LongLength)) { throw new IndexOutOfRangeException(); }

            UnsafeVectorXor(source, other, count, sourceOffset, otherOffset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void VectorXor(this byte[] source, byte[] other, ulong count)
        {
            source.VectorXor(other, count, 0UL, 0UL);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe static void UnsafeCopy(byte* source, byte* destination)
        {
            *destination = *source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe static void UnsafeCopy(long* source, long* destination)
        {
            *destination = *source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe static bool UnsafeNotEqual(byte* source, byte* destination)
        {
            return *source != *destination;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe static bool UnsafeNotEqual(long* source, long* destination)
        {
            return *source != *destination;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe static void UnsafeXor(byte* source, byte* other)
        {
            *source ^= *other;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe static void UnsafeXor(long* source, long* other)
        {
            *source ^= *other;
        }
        
        private unsafe static void UnsafeVectorXor(void* source, void* other, ulong count)
        {
            var stride = 1;

            if (VectorsOverlap(source, other, count))
            {
                var loopBackwards = (source < other);
                var offset = (loopBackwards ? unchecked(count - 1UL) : 0UL);

                other = (((byte*)other) + offset);
                source = (((byte*)source) + offset);
                stride = unchecked((-Convert.ToInt32(loopBackwards)) | 1);
            }
            else
            {
                // align other with size of long
                while ((0UL < count) && ((((long)(other)) & unchecked(sizeof(long) - 1)) != 0L))
                {
                    UnsafeXor((byte*)source, (byte*)other);

                    count -= sizeof(byte);
                    other = (((byte*)other) + stride);
                    source = (((byte*)source) + stride);
                }

                // process 256-bits per iteration
                stride *= 4;

                while (((sizeof(long) * 4) - 1) < count)
                {
                    UnsafeXor(((long*)source) + 0, ((long*)other) + 0);
                    UnsafeXor(((long*)source) + 1, ((long*)other) + 1);
                    UnsafeXor(((long*)source) + 2, ((long*)other) + 2);
                    UnsafeXor(((long*)source) + 3, ((long*)other) + 3);

                    count -= (sizeof(long) * 4);
                    other = (((long*)other) + stride);
                    source = (((long*)source) + stride);
                }

                // process 64-bits per iteration
                stride /= 4;

                while (((sizeof(long) * 1) - 1) < count)
                {
                    UnsafeXor((long*)source, (long*)other);

                    count -= (sizeof(long) * 1);
                    other = (((long*)other) + stride);
                    source = (((long*)source) + stride);
                }
            }

            // process 8-bits per iteration
            while (0UL < count)
            {
                UnsafeXor((byte*)source, (byte*)other);

                count -= sizeof(byte);
                other = (((byte*)other) + stride);
                source = (((byte*)source) + stride);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe static void UnsafeVectorXor(byte[] source, byte[] other, ulong count, ulong sourceOffset, ulong otherOffset)
        {
            fixed (byte* left = &source[sourceOffset])
            fixed (byte* right = &other[otherOffset])
            {
                UnsafeVectorXor(left, right, count);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe static bool VectorsOverlap(void* source, void* other, ulong count)
        {
            var x = ((byte*)source);
            var y = ((byte*)other);

            return (((x <= y) && ((x + count) > y)) || ((y <= x) && ((y + count) > x)));
        }
    }
}