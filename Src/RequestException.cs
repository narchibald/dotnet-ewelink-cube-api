using System;
using System.Net.Http;

namespace EWeLink.Cube.Api
{
    public class RequestException : HttpRequestException
    {
        public RequestException(int error)
            : base($"Response failed with status code: {error}")
        {
            Error = error;
        }

        public RequestException(int error, string message)
            : base(message)
        {
            Error = error;
        }

        public RequestException(int error, string message, Exception inner)
            : base(message, inner)
        {
            Error = error;
        }

        public int Error { get; }
    }
}