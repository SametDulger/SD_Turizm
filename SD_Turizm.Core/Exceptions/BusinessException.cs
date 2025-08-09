namespace SD_Turizm.Core.Exceptions
{
    public class BusinessException : Exception
    {
        public string ErrorCode { get; }
        public int StatusCode { get; }

        public BusinessException(string message, string errorCode = "BUSINESS_ERROR", int statusCode = 400)
            : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }

        public BusinessException(string message, Exception innerException, string errorCode = "BUSINESS_ERROR", int statusCode = 400)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }
    }
}
