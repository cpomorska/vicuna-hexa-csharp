using vicuna_ddd.Domain.Users.Exceptions;

namespace vicuna_ddd.Shared.Response
{
    public class RestErrorResponse : ErrorResponseDto
    {

        public RestErrorResponse(UserException ex) : base(ex)
        {
        }
    }
}
