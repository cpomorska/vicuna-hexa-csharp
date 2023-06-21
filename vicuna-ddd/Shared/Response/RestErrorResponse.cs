using vicuna_ddd.Domain.Users.Exceptions;

namespace vicuna_ddd.Shared.Response
{
    public class RestErrorResponse : ErrorResponseDto
    {
        private Exception? exception;

        public RestErrorResponse(UserException ex) : base(ex)
        {
        }
    }
}
