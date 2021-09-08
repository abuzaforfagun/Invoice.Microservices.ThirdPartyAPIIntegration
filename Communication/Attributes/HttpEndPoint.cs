using System;

namespace Communication.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HttpEndPoint : Attribute
    {
        public string Path { get; init; }

        public HttpEndPoint(string path)
        {
            Path = path;
        }
    }
}
