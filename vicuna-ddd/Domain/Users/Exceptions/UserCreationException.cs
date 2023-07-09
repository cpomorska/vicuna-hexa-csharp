using System.Net;
using System.Runtime.Serialization;
using vicuna_ddd.Shared.Response;

namespace vicuna_ddd.Domain.Users.Exceptions
{
    public class UserCreationException : UserException
    {
        public UserCreationException(HttpStatusCode status, ErrorCode errorCode, string msg) : base(status, errorCode, msg)
        {
        }
    }
}