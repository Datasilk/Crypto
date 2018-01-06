using System;
using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace Crypto
{
    /// <summary>
    /// A stream cipher class implemented using the XChaCha20 algorithm.
    /// </summary>
    public class XChaCha20 : ChaCha20
    {
        public new ImmutableArray<byte> Nonce { get; }

        public unsafe XChaCha20(UInt512 state, ulong iv) : base(Initialize(state, checked((uint)iv.CleaveHigh()), checked((uint)iv.CleaveLow()), SecureRandom.GetUInt32(), SecureRandom.GetUInt32()))
        {
            var nonce = new byte[24];

            fixed (byte* d = &nonce[0])
            {
                *(((uint*)d) + 0) = state.IC;
                *(((uint*)d) + 1) = state.ID;
                *(((uint*)d) + 2) = state.IE;
                *(((uint*)d) + 3) = state.IF;
                *(((ulong*)d) + 2) = base.Nonce;
            }

            Nonce = ImmutableArray.Create(nonce);
        }

        public unsafe static XChaCha20 Initialize(byte[] key, ulong iv)
        {
            const int KEY_SIZE_IN_BYTES = 32;

            if (key.Length != KEY_SIZE_IN_BYTES)
            {
                throw new ArgumentException(message: ("array must be exactly " + KEY_SIZE_IN_BYTES.ToString() + " bytes long"), paramName: nameof(key));
            }

            var state = new byte[Marshal.SizeOf(typeof(UInt512))];

            fixed (byte* statePointer = &state[0])
            fixed (byte* keyPointer = &key[0])
            {
                *(((uint*)statePointer) + 0) = ChaCha.SIGMA0;
                *(((uint*)statePointer) + 1) = ChaCha.SIGMA1;
                *(((uint*)statePointer) + 2) = ChaCha.SIGMA2;
                *(((uint*)statePointer) + 3) = ChaCha.SIGMA3;
                *(((ulong*)statePointer) + 2) = *(((ulong*)keyPointer) + 0);
                *(((ulong*)statePointer) + 3) = *(((ulong*)keyPointer) + 1);
                *(((ulong*)statePointer) + 4) = *(((ulong*)keyPointer) + 2);
                *(((ulong*)statePointer) + 5) = *(((ulong*)keyPointer) + 3);

                SecureRandom.GetBytes(state, 48, 16);

                return new XChaCha20(*((UInt512*)statePointer), iv);
            }
        }

        private static UInt512 Initialize(UInt512 bState, uint xState0, uint xState1, uint xState2, uint xState3)
        {
            var t0 = bState.I0;
            var t1 = bState.I1;
            var t2 = bState.I2;
            var t3 = bState.I3;
            var t4 = bState.I4;
            var t5 = bState.I5;
            var t6 = bState.I6;
            var t7 = bState.I7;
            var t8 = bState.I8;
            var t9 = bState.I9;
            var tA = bState.IA;
            var tB = bState.IB;
            var tC = bState.IC;
            var tD = bState.ID;
            var tE = bState.IE;
            var tF = bState.IF;

            for (var i = 0UL; i < 10UL; i++)
            {
                ChaCha.DoubleRound(
                    ref t0,
                    ref t1,
                    ref t2,
                    ref t3,
                    ref t4,
                    ref t5,
                    ref t6,
                    ref t7,
                    ref t8,
                    ref t9,
                    ref tA,
                    ref tB,
                    ref tC,
                    ref tD,
                    ref tE,
                    ref tF
                );
            }

            return new UInt512(
                ChaCha.SIGMA0,
                ChaCha.SIGMA1,
                ChaCha.SIGMA2,
                ChaCha.SIGMA3,
                t0,
                t1,
                t2,
                t3,
                tC,
                tD,
                tE,
                tF,
                xState0,
                xState1,
                xState2,
                xState3
            );
        }
    }
}