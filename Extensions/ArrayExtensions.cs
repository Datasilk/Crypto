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
        public static void VectorCopy(this byte[] source, byte[] destination, ulong count, ulong sourceOffset, ulong destinationOffset)
        {
            if (count > 511)
            {
                Array.Copy(source, checked((int)sourceOffset), destination, checked((int)destinationOffset), checked((int)count));
            }
            else
            {
                if (ReferenceEquals(source, destination) && (sourceOffset == destinationOffset)) { return; }
                if (checked(sourceOffset + count) > unchecked((ulong)source.LongLength)) { throw new IndexOutOfRangeException(); }
                if (checked(destinationOffset + count) > unchecked((ulong)destination.LongLength)) { throw new IndexOutOfRangeException(); }

                UnsafeVectorCopy(source, destination, count, sourceOffset, destinationOffset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void VectorCopy(this byte[] source, byte[] destination, ulong count)
        {
            source.VectorCopy(destination, count, 0UL, 0UL);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void VectorCopy(this byte[] source, byte[] destination)
        {
            source.VectorCopy(destination, unchecked((ulong)source.LongLength));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool VectorEquals(this byte[] source, byte[] other, ulong count, ulong sourceOffset, ulong otherOffset)
        {
            if ((source.LongLength == 0L) || (other.LongLength == 0L)) { return (source.LongLength == other.LongLength); }
            if (checked(sourceOffset + count) > unchecked((ulong)source.LongLength)) { throw new IndexOutOfRangeException(); }
            if (checked(otherOffset + count) > unchecked((ulong)other.LongLength)) { throw new IndexOutOfRangeException(); }

            if (!ReferenceEquals(source, other))
            { // contents are in different arrays, compare contents
                return UnsafeVectorEquals(source, other, count, sourceOffset, otherOffset);
            }
            else
            { // contents are in the same array, compare offsets
                return (sourceOffset == otherOffset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool VectorEquals(this byte[] source, byte[] other, ulong count)
        {
            return source.VectorEquals(other, count, 0UL, 0UL);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool VectorEquals(this byte[] source, byte[] other)
        {
            return source.VectorEquals(other, unchecked((ulong)source.LongLength));
        }

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
        public static void VectorXor(this byte[] source, byte[] other)
        {
            source.VectorXor(other, unchecked((ulong)source.LongLength));
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
        private unsafe static void UnsafeVectorCopy(void* source, void* destination, ulong count)
        {
            var stride = 1;

            if (VectorsOverlap(source, destination, count))
            {
                var loopBackwards = (source < destination);
                var offset = (loopBackwards ? unchecked(count - 1UL) : 0UL);

                destination = (((byte*)destination) + offset);
                source = (((byte*)source) + offset);
                stride = unchecked((-Convert.ToInt32(loopBackwards)) | 1);
            }
            else
            {
                // align destination with size of long
                while ((0UL < count) && ((((long)(destination)) & unchecked(sizeof(long) - 1)) != 0L))
                {
                    UnsafeCopy((byte*)source, (byte*)destination);

                    count -= sizeof(byte);
                    destination = (((byte*)destination) + stride);
                    source = (((byte*)source) + stride);
                }

                // process 256-bits per iteration
                stride *= 4;

                while (((sizeof(long) * 4) - 1) < count)
                {
                    UnsafeCopy(((long*)source) + 0, ((long*)destination) + 0);
                    UnsafeCopy(((long*)source) + 1, ((long*)destination) + 1);
                    UnsafeCopy(((long*)source) + 2, ((long*)destination) + 2);
                    UnsafeCopy(((long*)source) + 3, ((long*)destination) + 3);

                    count -= (sizeof(long) * 4);
                    destination = (((long*)destination) + stride);
                    source = (((long*)source) + stride);
                }

                // process 64-bits per iteration
                stride /= 4;

                while (((sizeof(long) * 1) - 1) < count)
                {
                    UnsafeCopy((long*)source, (long*)destination);

                    count -= (sizeof(long) * 1);
                    destination = (((long*)destination) + stride);
                    source = (((long*)source) + stride);
                }
            }

            // process 8-bits per iteration
            while (0UL < count)
            {
                UnsafeCopy((byte*)source, (byte*)destination);

                count -= sizeof(byte);
                destination = (((byte*)destination) + stride);
                source = (((byte*)source) + stride);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe static void UnsafeVectorCopy(byte[] source, byte[] destination, ulong count, ulong sourceOffset, ulong destinationOffset)
        {
            fixed (byte* left = &source[sourceOffset])
            fixed (byte* right = &destination[destinationOffset])
            {
                UnsafeVectorCopy(left, right, count);
            }
        }
        private unsafe static bool UnsafeVectorEquals(void* source, void* other, ulong count)
        {
            var stride = 1;

            // align other with size of long
            while ((0UL < count) && ((((long)(other)) & unchecked(sizeof(long) - 1)) != 0L))
            {
                if (UnsafeNotEqual((byte*)source, (byte*)other))
                {
                    return false;
                }

                count -= sizeof(byte);
                other = (((byte*)other) + stride);
                source = (((byte*)source) + stride);
            }

            // process 256-bits per iteration
            stride *= 4;

            while (((sizeof(long) * 4) - 1) < count)
            {
                if (UnsafeNotEqual(((long*)source) + 0, ((long*)other) + 0)
                 || UnsafeNotEqual(((long*)source) + 1, ((long*)other) + 1)
                 || UnsafeNotEqual(((long*)source) + 2, ((long*)other) + 2)
                 || UnsafeNotEqual(((long*)source) + 3, ((long*)other) + 3)
                )
                {
                    return false;
                }

                count -= (sizeof(long) * 4);
                other = (((long*)other) + stride);
                source = (((long*)source) + stride);
            }

            // process 64-bits per iteration
            stride /= 4;

            while (((sizeof(long) * 1) - 1) < count)
            {
                if (UnsafeNotEqual((long*)source, (long*)other))
                {
                    return false;
                }

                count -= (sizeof(long) * 1);
                other = (((long*)other) + stride);
                source = (((long*)source) + stride);
            }

            // process 8-bits per iteration
            while (0UL < count)
            {
                if (UnsafeNotEqual((byte*)source, (byte*)other))
                {
                    return false;
                }

                count -= sizeof(byte);
                other = (((byte*)other) + stride);
                source = (((byte*)source) + stride);
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe static bool UnsafeVectorEquals(byte[] source, byte[] other, ulong count, ulong sourceOffset, ulong otherOffset)
        {
            fixed (byte* left = &source[sourceOffset])
            fixed (byte* right = &other[otherOffset])
            {
                return UnsafeVectorEquals(left, right, count);
            }
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