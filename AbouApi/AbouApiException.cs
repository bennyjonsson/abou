using System;

namespace AbouApi
{
    public class AbouApiException : Exception
    {
        public AbouApiException(System.Net.HttpStatusCode httpStatusCode, string content) : base()
        {
            HttpStatusCode = httpStatusCode;
            Content = content;
        }

        public System.Net.HttpStatusCode HttpStatusCode { get; set; }
        public string Content { get; set; }
    }

}
