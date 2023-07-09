using System.Net;
using vicuna_ddd.Shared.Response;

namespace vicuna_ddd.Domain.Users.Exceptions
{
    [Serializable]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "<Ausstehend>")]
    public class UserException : Exception
    {
        public HttpStatusCode Status { get; private set; }
        public ErrorCode ErrorCode { get; private set; }

        public UserException(HttpStatusCode status, ErrorCode errorCode, string msg) : base(msg)
        {
            Status = status;
            ErrorCode = errorCode;
        }
    }
}
