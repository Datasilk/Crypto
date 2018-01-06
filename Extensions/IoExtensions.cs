using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Crypto
{
    /// <summary>
    /// A collection of extension methods that directly or indirectly augment the System.IO class.
    /// </summary>
    public static class IoExtensions
    {
        private static IDictionary<Type, Func<string, object>> m_stringToClrTypeMap = new Dictionary<Type, Func<string, object>> {
            { typeof(int), (value) => { return int.TryParse(value, out int intValue) ? intValue : throw new InvalidOperationException(); } },
            { typeof(long), (value) => { return long.TryParse(value, out long longValue) ? longValue : throw new InvalidOperationException(); } },
        };

        public static IEnumerable<string> ReadLines(this Stream stream, Encoding encoding, char escapeChar)
        {
            if (stream.IsNull())
            {
                throw new ArgumentNullException(paramName: nameof(stream));
            }

            if (encoding.IsNull())
            {
                throw new ArgumentNullException(paramName: nameof(encoding));
            }

            var byteBuffer = new byte[1024];
            var charBuffer = new char[encoding.GetMaxCharCount(byteBuffer.Length)];
            var decoder = encoding.GetDecoder();
            var escaping = false;
            var numBytesRead = 0;
            var stringBuffer = new StringBuilder(137);

            while (0 < (numBytesRead = stream.Read(byteBuffer, 0, byteBuffer.Length)))
            {
                var byteOffset = 0;
                var completed = false;

                while (completed == false)
                {
                    var head = 0;
                    var tail = 0;

                    decoder.Convert(
                        bytes: byteBuffer,
                        byteIndex: byteOffset,
                        byteCount: (numBytesRead - byteOffset),
                        chars: charBuffer,
                        charIndex: 0,
                        charCount: byteBuffer.Length,
                        flush: (numBytesRead == 0),
                        bytesUsed: out int numBytesUsed,
                        charsUsed: out int numCharsUsed,
                        completed: out completed
                    );

                    while (tail < numCharsUsed)
                    {
                        var currentChar = charBuffer[tail++];

                        if (currentChar == escapeChar)
                        {
                            escaping = !escaping;
                        }

                        if ((escaping == false) && ((currentChar == '\r') || (currentChar == '\n')))
                        {
                            if ((currentChar == '\n') && (tail > 1) && (charBuffer[tail - 2] == '\r'))
                            {
                                head = tail;

                                continue;
                            }

                            if (0 == stringBuffer.Length)
                            {
                                yield return new string(charBuffer, head, (tail - head) - 1);
                            }
                            else
                            {
                                yield return stringBuffer.Append(charBuffer, head, (tail - head) - 1).ToString();

                                stringBuffer.Clear();
                            }

                            head = tail;
                        }
                    }

                    byteOffset += numBytesUsed;

                    stringBuffer.Append(charBuffer, head, (tail - head));
                }
            }

            if (0 < stringBuffer.Length)
            {
                yield return stringBuffer.ToString();
            }
        }
    }
}