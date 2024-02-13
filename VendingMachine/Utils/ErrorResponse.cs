namespace VendingMachineAPI.Utils
{

    public class ErrorResponse
    {
        public Error error { get; set; }
        public InnerError innerError { get; set; }
    }
    public class Error
    {
        public Error(string _code, Message _message)
        {
            code = _code;
            message = _message;
        }
        public string code { get; set; }
        public Message message { get; set; }
    }
    public class Message
    {
        public Message(string _value)
        {
            value = _value;
        }
        public string value { get; set; }
    }
    public class InnerError
    {
        public InnerError(string _trace)
        {
            trace = _trace;
        }
        public string trace { get; set; }
        public API_Context Context { get; set; }//for future use
    }
    public class API_Context
    {
        public API_Context(HttpRequest _req, HttpResponse _res, ConnectionInfo _info)
        {
            Request = _req;
            Response = _res;
            Connection = _info;
        }
        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }
        public ConnectionInfo Connection { get; set; }
    }
}
