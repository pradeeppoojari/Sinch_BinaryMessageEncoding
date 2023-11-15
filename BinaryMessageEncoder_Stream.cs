using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageEncoder
{
    public class MessageEncoders
    {
        private const int MaxHeaderCount = 63;
        private const int MaxHeaderSize = 1023;
        private const int MaxPayloadSize = 256 * 1024; // to set maximum payload size 256 KB

        public Dictionary<string, string> Headers { get; set; }
        public byte[] Payload { get; set; }

        public MessageEncoders()
        {
            Headers = new Dictionary<string, string>();
        }

        //method will encodes message using memory stream this will be much faster than other implementation.
        public byte[] Encode()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // Write header count
                stream.WriteByte((byte)Headers.Count);

                // Write headers key and value
                foreach (var header in Headers)
                {
                    WriteMessage(stream, header.Key);
                    WriteMessage(stream, header.Value);
                }

                // Write payload size
                byte[] payloadSizeBytes = BitConverter.GetBytes(Payload.Length);
                stream.Write(payloadSizeBytes, 0, payloadSizeBytes.Length);

                // Write payload
                stream.Write(Payload, 0, Payload.Length);

                return stream.ToArray();
            }
        }

        //method will decodes message stream
        public static MessageEncoders Decode(byte[] data)
        {
            MessageEncoders message = new MessageEncoders();
            using (MemoryStream stream = new MemoryStream(data))
            {
                int headerCount = stream.ReadByte();

                for (int i = 0; i < headerCount; i++)
                {
                    string key = ReadMessage(stream);
                    string value = ReadMessage(stream);
                    message.Headers[key] = value;
                }

                byte[] payloadSizeBytes = new byte[4];
                stream.Read(payloadSizeBytes, 0, 4);
                int payloadSize = BitConverter.ToInt32(payloadSizeBytes, 0);

                message.Payload = new byte[payloadSize];
                stream.Read(message.Payload, 0, payloadSize);
            }

            return message;
        }

        //method will be used to write the message stream
        private static void WriteMessage(Stream stream, string text)
        {
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(text);
            if (bytes.Length > MaxHeaderSize)
            {
                throw new InvalidOperationException("Header size exceeds the maximum allowed size.");
            }

            if (bytes.Length > 255)
            {
                stream.WriteByte(0xFF);
                stream.Write(BitConverter.GetBytes((ushort)bytes.Length), 0, 2);
            }
            else
            {
                stream.WriteByte((byte)bytes.Length);
            }
            stream.Write(bytes, 0, bytes.Length);
        }

        //method will be used to read the message stream
        private static string ReadMessage(Stream stream)
        {
            int length = stream.ReadByte();
            if (length == 0xFF)
            {
                byte[] lengthBytes = new byte[2];
                stream.Read(lengthBytes, 0, 2);
                length = BitConverter.ToUInt16(lengthBytes, 0);
            }

            if (length > MaxHeaderSize)
            {
                throw new InvalidOperationException("Header size exceeds the maximum allowed size.");
            }

            byte[] bytes = new byte[length];
            stream.Read(bytes, 0, length);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}