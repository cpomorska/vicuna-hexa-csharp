using System.Net;

namespace vicuna_ddd.Domain.Users.Exceptions
{
    [Serializable]
    public class UserCreationException : UserException
    {
        public UserCreationException(HttpStatusCode status, string msg) : base(status, msg)
        {
        }
    }
}