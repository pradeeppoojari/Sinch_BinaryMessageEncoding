# Sinch_BinaryMessageEncoding

Binary Message Encoding Scheme

This project aims to implement a simple binary message encoding scheme using C#. The assumptions made in designing this solution are as follows:

Input Validity:

It is assumed that the input messages provided to the encoding and decoding functions are valid and within the supported strings set.
Binary Representation:

Each message in the input message is assumed to be represented by a unique binary message code.

ASCII Characters:

The encoding scheme focuses on encoding ASCII characters. Extended ASCII or Unicode characters are not considered in this implementation.

Encoding Consistency:

The encoding and decoding functions are assumed to be consistent, meaning that encoding a message and then decoding the encoded message should result in the original input message.

Error Handling:

Error handling for invalid inputs or unexpected scenarios is assumed to be handled gracefully. This may include providing meaningful error messages or logging.

Performance:

Here i have implemented two different cs file that performs encoding of binary string with different methods or techniques to perform unique performance.
The implementation assumes a reasonable trade-off between simplicity and performance. It may not be optimized for extremely large messages or real-time encoding/decoding requirements.

Solution:

As mentioned in performance, implemented two different solutions to encode the binary message as mentioned in the problem task given.

In order to deploy this to production we always keep in mind that in a production environment, additional considerations for error handling, performance optimization, and extensibility may be required.
Addition to this i have added one simple simple CI/CD pipeline created for both build and release for this particular project appliaction.
