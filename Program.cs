using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MessageEncoder;
using System.Timers;

class Program
{
    static void Main()
    {
        var aTimer = System.Diagnostics.Stopwatch.StartNew();

        // creating sample headers with value
        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "ExampleHeader1", "ExampleValue1" },
            { "ExampleHeader2", "ExampleValue2" },
            { "ExampleHeader3", "ExampleValue3" }
        };

        // creating a sample payload
        byte[] payload = Encoding.ASCII.GetBytes("Payload for: Simple binary message encoding scheme to be used in a signaling protocol.");

        // for encoding the message
        BinaryMessageEncoder encoder = new BinaryMessageEncoder();
        byte[] encodedMessage = encoder.Serialise(headers, payload);

        // for decoding the message
        (Dictionary<string, string> decodedHeaders, byte[] decodedPayload) = encoder.Deserialise(encodedMessage);

        Console.WriteLine($"------Binary message encoding using simple logic------");
        // to display decoded headers and payload
        foreach (var header in decodedHeaders)
        {
            Console.WriteLine($"Header: {header.Key}, Value: {header.Value}");
        }

        string decodedPayloadString = Encoding.ASCII.GetString(decodedPayload);
        Console.WriteLine($"Decoded Payload: {decodedPayloadString}");

        Console.WriteLine($"Time taken to complete: {aTimer.ElapsedMilliseconds}");
        Console.WriteLine($" ");
        Console.WriteLine($"*********************************************************");
        Console.WriteLine($" ");
        Console.WriteLine($"------Binary message encoding using Stream------");


        var sTimer = System.Diagnostics.Stopwatch.StartNew();
        //example usage of other method of encoding and decoding using stream

        MessageEncoders originalMessageStream = new MessageEncoders
        {
            Headers = { { "ExampleHeader1", "Value1" }, { "ExampleHeader2", "Value2" }, { "ExampleHeader3", "Value3" } },
            Payload = Encoding.ASCII.GetBytes("Payload for: Simple binary message stream encoding scheme to be used in a signaling protocol.")
        };

        byte[] encodedMessageStream = originalMessageStream.Encode();

        MessageEncoders decodedMessageStream = MessageEncoders.Decode(encodedMessageStream);

        Console.WriteLine("Decoded Message Stream:");
        foreach (var header in decodedMessageStream.Headers)
        {
            Console.WriteLine($"{header.Key}: {header.Value}");
        }

        Console.WriteLine("Decoded Payload: " + Encoding.ASCII.GetString(decodedMessageStream.Payload));
        Console.WriteLine($"Time taken to complete: {sTimer.ElapsedMilliseconds}");
        Console.WriteLine($" ");
        Console.WriteLine($"*********************************************************");
    }
}