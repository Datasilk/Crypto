using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Crypto
{
    /// <remarks>
    /// https://cr.yp.to/chacha/chacha-20080128.pdf
    /// https://cr.yp.to/snuffle/spec.pdf
    /// https://eprint.iacr.org/2013/759.pdf
    /// https://tools.ietf.org/html/rfc7539
    /// http://loup-vaillant.fr/tutorials/chacha20-design
    /// </remarks>
    public static class ChaCha
    {
        public const int BLOCK_SIZE_IN_BYTES = (WORD_SIZE_IN_BYTES * 16);
        public const uint SIGMA0 = 0x61707865U;
        public const uint SIGMA1 = 0x3320646EU;
        public const uint SIGMA2 = 0x79622D32U;
        public const uint SIGMA3 = 0x6B206574U;
        public const int WORD_SIZE_IN_BYTES = sizeof(uint);

        /// <summary>
        /// Fills an array of bytes with the key stream calculated from the specified state, number of rounds, and iv.
        /// </summary>
        public unsafe static void BlockRound(UInt512 state, ulong numRounds, ulong iv, byte[] destination, ulong destinationOffset)
        {
            if (checked(BLOCK_SIZE_IN_BYTES + destinationOffset) > unchecked((ulong)destination.LongLength))
            {
                throw new ArgumentOutOfRangeException(paramName: nameof(destinationOffset));
            }

            var counterLow = checked((uint)iv.CleaveHigh());
            var counterHigh = checked((uint)iv.CleaveLow());

            var t0 = state.I0;
            var t1 = state.I1;
            var t2 = state.I2;
            var t3 = state.I3;
            var t4 = state.I4;
            var t5 = state.I5;
            var t6 = state.I6;
            var t7 = state.I7;
            var t8 = state.I8;
            var t9 = state.I9;
            var tA = state.IA;
            var tB = state.IB;
            var tC = counterLow;
            var tD = counterHigh;
            var tE = state.IE;
            var tF = state.IF;

            for (var i = 0UL; i < (numRounds >> 1); i++)
            {
                DoubleRound(
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

            unchecked
            {
                t0 += state.I0;
                t1 += state.I1;
                t2 += state.I2;
                t3 += state.I3;
                t4 += state.I4;
                t5 += state.I5;
                t6 += state.I6;
                t7 += state.I7;
                t8 += state.I8;
                t9 += state.I9;
                tA += state.IA;
                tB += state.IB;
                tC += counterLow;
                tD += counterHigh;
                tE += state.IE;
                tF += state.IF;
            }

            if (BitConverter.IsLittleEndian)
            {
                fixed (byte* d = &destination[destinationOffset])
                {
                    *(((uint*)d) + 0) = t0;
                    *(((uint*)d) + 1) = t1;
                    *(((uint*)d) + 2) = t2;
                    *(((uint*)d) + 3) = t3;
                    *(((uint*)d) + 4) = t4;
                    *(((uint*)d) + 5) = t5;
                    *(((uint*)d) + 6) = t6;
                    *(((uint*)d) + 7) = t7;
                    *(((uint*)d) + 8) = t8;
                    *(((uint*)d) + 9) = t9;
                    *(((uint*)d) + 10) = tA;
                    *(((uint*)d) + 11) = tB;
                    *(((uint*)d) + 12) = tC;
                    *(((uint*)d) + 13) = tD;
                    *(((uint*)d) + 14) = tE;
                    *(((uint*)d) + 15) = tF;
                }
            }
            else
            {
                fixed (byte* d = &destination[destinationOffset])
                {
                    *(((uint*)d) + 0) = t0.ReverseBytes();
                    *(((uint*)d) + 1) = t1.ReverseBytes();
                    *(((uint*)d) + 2) = t2.ReverseBytes();
                    *(((uint*)d) + 3) = t3.ReverseBytes();
                    *(((uint*)d) + 4) = t4.ReverseBytes();
                    *(((uint*)d) + 5) = t5.ReverseBytes();
                    *(((uint*)d) + 6) = t6.ReverseBytes();
                    *(((uint*)d) + 7) = t7.ReverseBytes();
                    *(((uint*)d) + 8) = t8.ReverseBytes();
                    *(((uint*)d) + 9) = t9.ReverseBytes();
                    *(((uint*)d) + 10) = tA.ReverseBytes();
                    *(((uint*)d) + 11) = tB.ReverseBytes();
                    *(((uint*)d) + 12) = tC.ReverseBytes();
                    *(((uint*)d) + 13) = tD.ReverseBytes();
                    *(((uint*)d) + 14) = tE.ReverseBytes();
                    *(((uint*)d) + 15) = tF.ReverseBytes();
                }
            }
        }
        /// <summary>
        /// Executes eight QuarterRound operations (four "column-rounds" and four "row-rounds").
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DoubleRound(
            ref uint state0,
            ref uint state1,
            ref uint state2,
            ref uint state3,
            ref uint state4,
            ref uint state5,
            ref uint state6,
            ref uint state7,
            ref uint state8,
            ref uint state9,
            ref uint stateA,
            ref uint stateB,
            ref uint stateC,
            ref uint stateD,
            ref uint stateE,
            ref uint stateF
        )
        {
            QuarterRound(ref state0, ref state4, ref state8, ref stateC);
            QuarterRound(ref state1, ref state5, ref state9, ref stateD);
            QuarterRound(ref state2, ref state6, ref stateA, ref stateE);
            QuarterRound(ref state3, ref state7, ref stateB, ref stateF);
            QuarterRound(ref state0, ref state5, ref stateA, ref stateF);
            QuarterRound(ref state1, ref state6, ref stateB, ref stateC);
            QuarterRound(ref state2, ref state7, ref state8, ref stateD);
            QuarterRound(ref state3, ref state4, ref state9, ref stateE);
        }
        /// <summary>
        /// Executes four SingleRound operations.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void QuarterRound(ref uint w, ref uint x, ref uint y, ref uint z)
        {
            SingleRound(ref w, ref x, ref z, 16);
            SingleRound(ref y, ref z, ref x, 12);
            SingleRound(ref w, ref x, ref z, 8);
            SingleRound(ref y, ref z, ref x, 7);
        }
        /// <summary>
        /// Executes the basic operation of the ChaCha algorithm which "mixes" three state variables per invocation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SingleRound(ref uint x, ref uint y, ref uint z, int r) => z = (z ^ unchecked(x += y)).RotateLeft(r);

        public static void Transform(UInt512 state, ulong numRounds, Stream source, Stream destination)
        {
            var dataBuffer = new byte[BLOCK_SIZE_IN_BYTES];
            var keyStreamBuffer = new byte[BLOCK_SIZE_IN_BYTES];
            var keyStreamPosition = Math.CombineIntoUInt64(state.IC, state.ID);
            var numBytesRead = 0;

            while (unchecked(BLOCK_SIZE_IN_BYTES - 1) < (numBytesRead = source.Read(dataBuffer, 0, BLOCK_SIZE_IN_BYTES)))
            {
                if (source == destination)
                {
                    destination.Position -= BLOCK_SIZE_IN_BYTES;
                }

                BlockRound(state, numRounds, checked(keyStreamPosition++), keyStreamBuffer, 0UL); // get next key stream chunk
                dataBuffer.VectorXor(keyStreamBuffer, BLOCK_SIZE_IN_BYTES); // xor data chunk with key stream
                destination.Write(dataBuffer, 0, BLOCK_SIZE_IN_BYTES); // write transformed data chunk to destination stream
            }

            if (0 < numBytesRead)
            {
                if (source == destination)
                {
                    destination.Position -= numBytesRead;
                }

                BlockRound(state, numRounds, checked(keyStreamPosition++), keyStreamBuffer, 0UL); // get next key stream chunk
                dataBuffer.VectorXor(keyStreamBuffer, unchecked((ulong)numBytesRead)); // xor data chunk with key stream
                destination.Write(dataBuffer, 0, numBytesRead); // write transformed data chunk to destination stream
            }
        }

        public static void Transform(UInt512 state, ulong numRounds, Stream stream) => Transform(state, numRounds, stream, stream);

        public static void Transform(UInt512 state, ulong numRounds, byte[] data)
        {
            using (var memoryStream = new MemoryStream(data, true))
            {
                Transform(state, numRounds, memoryStream);
            }
        }

        public static void Transform(UInt512 state, ulong numRounds, string fileName)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                Transform(state, numRounds, fileStream);
            }
        }
    }
}