using System;
using System.Runtime.InteropServices;

namespace Crypto
{
    [StructLayout(LayoutKind.Explicit, Pack = 4, Size = 64)]
    public struct UInt512
    {
        private const int SIZE_IN_BYTES = checked(sizeof(uint) * 16);

        [FieldOffset((sizeof(uint) * 0))]
        private readonly uint m_i0;
        [FieldOffset((sizeof(uint) * 1))]
        private readonly uint m_i1;
        [FieldOffset((sizeof(uint) * 2))]
        private readonly uint m_i2;
        [FieldOffset((sizeof(uint) * 3))]
        private readonly uint m_i3;
        [FieldOffset((sizeof(uint) * 4))]
        private readonly uint m_i4;
        [FieldOffset((sizeof(uint) * 5))]
        private readonly uint m_i5;
        [FieldOffset((sizeof(uint) * 6))]
        private readonly uint m_i6;
        [FieldOffset((sizeof(uint) * 7))]
        private readonly uint m_i7;
        [FieldOffset((sizeof(uint) * 8))]
        private readonly uint m_i8;
        [FieldOffset((sizeof(uint) * 9))]
        private readonly uint m_i9;
        [FieldOffset((sizeof(uint) * 10))]
        private readonly uint m_iA;
        [FieldOffset((sizeof(uint) * 11))]
        private readonly uint m_iB;
        [FieldOffset((sizeof(uint) * 12))]
        private readonly uint m_iC;
        [FieldOffset((sizeof(uint) * 13))]
        private readonly uint m_iD;
        [FieldOffset((sizeof(uint) * 14))]
        private readonly uint m_iE;
        [FieldOffset((sizeof(uint) * 15))]
        private readonly uint m_iF;

        public unsafe uint this[ulong index]
        {
            get
            {
                if (checked(SIZE_IN_BYTES - 1) < index)
                {
                    throw new IndexOutOfRangeException();
                }

                fixed (UInt512* p = &this)
                {
                    return *(((uint*)p) + index);
                }
            }
        }

        public uint I0 => m_i0;
        public uint I1 => m_i1;
        public uint I2 => m_i2;
        public uint I3 => m_i3;
        public uint I4 => m_i4;
        public uint I5 => m_i5;
        public uint I6 => m_i6;
        public uint I7 => m_i7;
        public uint I8 => m_i8;
        public uint I9 => m_i9;
        public uint IA => m_iA;
        public uint IB => m_iB;
        public uint IC => m_iC;
        public uint ID => m_iD;
        public uint IE => m_iE;
        public uint IF => m_iF;

        public UInt512(
            uint i0,
            uint i1,
            uint i2,
            uint i3,
            uint i4,
            uint i5,
            uint i6,
            uint i7,
            uint i8,
            uint i9,
            uint iA,
            uint iB,
            uint iC,
            uint iD,
            uint iE,
            uint iF
        )
        {
            m_i0 = i0;
            m_i1 = i1;
            m_i2 = i2;
            m_i3 = i3;
            m_i4 = i4;
            m_i5 = i5;
            m_i6 = i6;
            m_i7 = i7;
            m_i8 = i8;
            m_i9 = i9;
            m_iA = iA;
            m_iB = iB;
            m_iC = iC;
            m_iD = iD;
            m_iE = iE;
            m_iF = iF;
        }
        public unsafe UInt512(byte[] state)
        {
            if (SIZE_IN_BYTES != state.Length)
            {
                throw new ArgumentException(message: ("array must be exactly " + SIZE_IN_BYTES.ToString() + " bytes long"), paramName: nameof(state));
            }

            fixed (byte* s = &state[0])
            {
                m_i0 = *(((uint*)s) + 0);
                m_i1 = *(((uint*)s) + 1);
                m_i2 = *(((uint*)s) + 2);
                m_i3 = *(((uint*)s) + 3);
                m_i4 = *(((uint*)s) + 4);
                m_i5 = *(((uint*)s) + 5);
                m_i6 = *(((uint*)s) + 6);
                m_i7 = *(((uint*)s) + 7);
                m_i8 = *(((uint*)s) + 8);
                m_i9 = *(((uint*)s) + 9);
                m_iA = *(((uint*)s) + 10);
                m_iB = *(((uint*)s) + 11);
                m_iC = *(((uint*)s) + 12);
                m_iD = *(((uint*)s) + 13);
                m_iE = *(((uint*)s) + 14);
                m_iF = *(((uint*)s) + 15);
            }
        }


        public unsafe void CopyTo(byte[] destination, ulong destinationOffset)
        {
            if (checked(SIZE_IN_BYTES + destinationOffset) > unchecked((ulong)destination.LongLength))
            {
                throw new IndexOutOfRangeException();
            }

            fixed (byte* d = &destination[destinationOffset])
            {
                *(((uint*)d) + 0) = m_i0;
                *(((uint*)d) + 1) = m_i1;
                *(((uint*)d) + 2) = m_i2;
                *(((uint*)d) + 3) = m_i3;
                *(((uint*)d) + 4) = m_i4;
                *(((uint*)d) + 5) = m_i5;
                *(((uint*)d) + 6) = m_i6;
                *(((uint*)d) + 7) = m_i7;
                *(((uint*)d) + 8) = m_i8;
                *(((uint*)d) + 9) = m_i9;
                *(((uint*)d) + 10) = m_iA;
                *(((uint*)d) + 11) = m_iB;
                *(((uint*)d) + 12) = m_iC;
                *(((uint*)d) + 13) = m_iD;
                *(((uint*)d) + 14) = m_iE;
                *(((uint*)d) + 15) = m_iF;
            }
        }
        public unsafe void CopyTo(byte[] destination)
        {
            CopyTo(destination, 0UL);
        }
    }
}