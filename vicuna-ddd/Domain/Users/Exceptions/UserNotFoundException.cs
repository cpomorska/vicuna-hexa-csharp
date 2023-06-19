using System.Net;
using vicuna_ddd.Shared.Response;

namespace vicuna_ddd.Domain.Users.Exceptions
{
    [Serializable]
    public class UserNotFoundException : UserException
    {
        public UserNotFoundException(HttpStatusCode status, ErrorCode errorCode, string msg) : base(status, errorCode, msg)
        {
        }
    }
}