using System.IO;

namespace Crypto
{
    /// <summary>
    /// A stream cipher class implemented using the ChaCha20 algorithm.
    /// </summary>
    public class ChaCha20
    {
        private readonly UInt512 m_state;

        public ulong IV => Math.CombineIntoUInt64(m_state.IC, m_state.ID);
        public ulong Nonce => Math.CombineIntoUInt64(m_state.IE, m_state.IF);

        public ChaCha20(UInt512 state)
        {
            m_state = state;
        }

        public void Transform(Stream source, Stream destination) => ChaCha.Transform(m_state, 20UL, source, destination);
        public void Transform(Stream stream) => ChaCha.Transform(m_state, 20UL, stream);
        public void Transform(byte[] data) => ChaCha.Transform(m_state, 20UL, data);
        public void Transform(string fileName) => ChaCha.Transform(m_state, 20UL, fileName);
    }
}
