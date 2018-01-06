using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Crypto
{
    public class Blake2s
    {
        private static readonly ImmutableArray<int> m_sigma = ImmutableArray.Create(new int[160] {
            00, 01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12, 13, 14, 15,
            14, 10, 04, 08, 09, 15, 13, 06, 01, 12, 00, 02, 11, 07, 05, 03,
            11, 08, 12, 00, 05, 02, 15, 13, 10, 14, 03, 06, 07, 01, 09, 04,
            07, 09, 03, 01, 13, 12, 11, 14, 02, 06, 05, 10, 04, 00, 15, 08,
            09, 00, 05, 07, 02, 04, 10, 15, 14, 01, 11, 12, 06, 08, 03, 13,
            02, 12, 06, 10, 00, 11, 08, 03, 04, 13, 07, 05, 15, 14, 01, 09,
            12, 05, 01, 15, 14, 13, 04, 10, 00, 07, 06, 03, 09, 02, 08, 11,
            13, 11, 07, 14, 12, 01, 03, 09, 05, 00, 15, 04, 08, 06, 02, 10,
            06, 15, 14, 09, 11, 03, 00, 08, 12, 02, 13, 07, 01, 04, 10, 05,
            10, 02, 08, 04, 07, 06, 01, 05, 15, 11, 09, 14, 03, 12, 13, 00,
        });

        public const int BLOCK_SIZE_IN_BYTES = (WORD_SIZE_IN_BYTES * 16);
        public const uint IV0 = 0X6A09E667U;
        public const uint IV1 = 0XBB67AE85U;
        public const uint IV2 = 0X3C6EF372U;
        public const uint IV3 = 0XA54FF53AU;
        public const uint IV4 = 0X510E527FU;
        public const uint IV5 = 0X9B05688CU;
        public const uint IV6 = 0X1F83D9ABU;
        public const uint IV7 = 0X5BE0CD19U;
        public const int WORD_SIZE_IN_BYTES = sizeof(uint);

        public static void BlockRound(uint[] state, uint[] data, ulong numRounds, ulong counter, ulong finalizer)
        {
            var i = 0UL;
            var j = 0;
            var t0 = state[0];
            var t1 = state[1];
            var t2 = state[2];
            var t3 = state[3];
            var t4 = state[4];
            var t5 = state[5];
            var t6 = state[6];
            var t7 = state[7];
            var t8 = IV0;
            var t9 = IV1;
            var tA = IV2;
            var tB = IV3;
            var tC = (IV4 ^ ((uint)counter.CleaveHigh()));
            var tD = (IV5 ^ ((uint)counter.CleaveLow()));
            var tE = (IV6 ^ ((uint)finalizer.CleaveHigh()));
            var tF = (IV7 ^ ((uint)finalizer.CleaveLow()));

            for (; i < numRounds; i++)
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
                    ref tF,
                    data[m_sigma[j + 0]],
                    data[m_sigma[j + 1]],
                    data[m_sigma[j + 2]],
                    data[m_sigma[j + 3]],
                    data[m_sigma[j + 4]],
                    data[m_sigma[j + 5]],
                    data[m_sigma[j + 6]],
                    data[m_sigma[j + 7]],
                    data[m_sigma[j + 8]],
                    data[m_sigma[j + 9]],
                    data[m_sigma[j + 10]],
                    data[m_sigma[j + 11]],
                    data[m_sigma[j + 12]],
                    data[m_sigma[j + 13]],
                    data[m_sigma[j + 14]],
                    data[m_sigma[j + 15]]
                );

                j += 16;
            }

            state[0] ^= (t0 ^ t8);
            state[1] ^= (t1 ^ t9);
            state[2] ^= (t2 ^ tA);
            state[3] ^= (t3 ^ tB);
            state[4] ^= (t4 ^ tC);
            state[5] ^= (t5 ^ tD);
            state[6] ^= (t6 ^ tE);
            state[7] ^= (t7 ^ tF);
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
            ref uint stateF,
            uint data0,
            uint data1,
            uint data2,
            uint data3,
            uint data4,
            uint data5,
            uint data6,
            uint data7,
            uint data8,
            uint data9,
            uint dataA,
            uint dataB,
            uint dataC,
            uint dataD,
            uint dataE,
            uint dataF
        )
        {
            QuarterRound(ref state0, ref state4, ref state8, ref stateC, data0, data1);
            QuarterRound(ref state1, ref state5, ref state9, ref stateD, data2, data3);
            QuarterRound(ref state2, ref state6, ref stateA, ref stateE, data4, data5);
            QuarterRound(ref state3, ref state7, ref stateB, ref stateF, data6, data7);
            QuarterRound(ref state0, ref state5, ref stateA, ref stateF, data8, data9);
            QuarterRound(ref state1, ref state6, ref stateB, ref stateC, dataA, dataB);
            QuarterRound(ref state2, ref state7, ref state8, ref stateD, dataC, dataD);
            QuarterRound(ref state3, ref state4, ref state9, ref stateE, dataE, dataF);
        }

        /// <summary>
        /// Executes four SingleRound operations.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void QuarterRound(ref uint w, ref uint x, ref uint y, ref uint z, uint v0, uint v1)
        {
            SingleRound(ref w, ref x, ref z, v0, 16);
            SingleRound(ref y, ref z, ref x, 0U, 12);
            SingleRound(ref w, ref x, ref z, v1, 8);
            SingleRound(ref y, ref z, ref x, 0U, 7);
        }

        /// <summary>
        /// Executes the basic operation of the Blake2 algorithm which "mixes" three state variables with one value per invocation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SingleRound(ref uint x, ref uint y, ref uint z, uint v, int r) => z = (z ^ unchecked(x += (y + v))).RotateRight(r);
    }
}