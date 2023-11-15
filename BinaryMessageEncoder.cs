using System;
using System.Collections.Generic;
using System.Text;

namespace MessageEncoder
{
    public class BinaryMessageEncoder
    {
        private const int MaxHeaders = 63;
        private const int MaxHeaderSize = 1023;
        private const int MaxPayloadSize = 256 * 1024; // to set maximum payload size 256 KB

        public byte[] Serialise(Dictionary<string, string> headers, byte[] payload)
        {
            if (headers.Count > MaxHeaders)
            {
                throw new ArgumentException("Exceeded maximum number of headers");
            }

            foreach (var header in headers)
            {
                if (header.Key.Length > MaxHeaderSize || header.Value.Length > MaxHeaderSize)
                {
                    throw new ArgumentException("Header name or value exceeds the size limit");
                }
            }

            if (payload.Length > MaxPayloadSize)
            {
                throw new ArgumentException("Payload size exceeds the limit");
            }

            List<byte> encodedMessage = new List<byte>();

            // Encode the number of headers
            encodedMessage.Add((byte)headers.Count);

            // Encode headers
            foreach (var header in headers)
            {
                EncodeMessage(header.Key, encodedMessage);
                EncodeMessage(header.Value, encodedMessage);
            }

            // Encode payload length
            byte[] payloadLength = BitConverter.GetBytes(payload.Length);
            encodedMessage.AddRange(payloadLength);

            // Encode payload
            encodedMessage.AddRange(payload);

            return encodedMessage.ToArray();
        }

        private void EncodeMessage(string input, List<byte> output)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(input);

            // Add the length of the string
            byte[] length = BitConverter.GetBytes((ushort)bytes.Length);
            output.AddRange(length);

            // Add the string bytes
            output.AddRange(bytes);
        }

        public (Dictionary<string, string>, byte[]) Deserialise(byte[] encodedMessage)
        {
            List<byte> messageData = new List<byte>(encodedMessage);
            Dictionary<string, string> headers = new Dictionary<string, string>();

            // Decode number of headers
            int numHeaders = messageData[0];
            messageData.RemoveAt(0);

            // Decode headers
            for (int i = 0; i < numHeaders; i++)
            {
                string name = DecodeMessage(messageData);
                string value = DecodeMessage(messageData);
                headers[name] = value;
            }

            // Decode payload length
            byte[] payloadLengthBytes = messageData.GetRange(0, sizeof(int)).ToArray();
            int payloadLength = BitConverter.ToInt32(payloadLengthBytes, 0);
            messageData.RemoveRange(0, sizeof(int));

            // Get the payload
            byte[] payload = messageData.GetRange(0, payloadLength).ToArray();

            return (headers, payload);
        }

        private string DecodeMessage(List<byte> data)
        {
            byte[] lengthBytes = data.GetRange(0, sizeof(ushort)).ToArray();
            data.RemoveRange(0, sizeof(ushort));

            ushort length = BitConverter.ToUInt16(lengthBytes, 0);

            byte[] stringBytes = data.GetRange(0, length).ToArray();
            data.RemoveRange(0, length);

            return Encoding.ASCII.GetString(stringBytes);
        }
    }
}