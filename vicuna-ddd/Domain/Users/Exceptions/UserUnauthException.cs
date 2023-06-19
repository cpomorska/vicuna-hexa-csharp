using System.Net;
using System.Runtime.Serialization;

namespace vicuna_ddd.Domain.Users.Exceptions
{
    [Serializable]
    public class UserUnauthException : UserException
    {
        public UserUnauthException(HttpStatusCode status, string msg) : base(status, msg)
        {
        }
    }
}