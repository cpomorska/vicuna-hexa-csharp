using System.Net;
using vicuna_ddd.Shared.Response;

namespace vicuna_ddd.Domain.Users.Exceptions
{
    [Serializable]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "<Ausstehend>")]
    public class UserNotFoundException : UserException
    {
        public UserNotFoundException(HttpStatusCode status, ErrorCode errorCode, string msg) : base(status, errorCode, msg)
        {
        }
    }
}