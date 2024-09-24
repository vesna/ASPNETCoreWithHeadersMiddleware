namespace ASPNETCoreWithHeadersMiddleware.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException(int statusCode, string message) : base(message)
        {
            Status = statusCode;
        }

        public int Status { get; }
    }
}
