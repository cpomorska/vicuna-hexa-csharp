using System.Net;
using System.Runtime.Serialization;
using vicuna_ddd.Shared.Response;

namespace vicuna_ddd.Domain.Users.Exceptions
{
    [Serializable]
    public class UserUnauthException : UserException
    {
        public UserUnauthException(HttpStatusCode status, ErrorCode errorCode, string msg) : base(status, errorCode, msg)
        {
        }
    }
}