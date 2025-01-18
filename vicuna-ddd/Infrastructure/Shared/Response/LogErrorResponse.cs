using vicuna_ddd.Domain.Users.Exceptions;

namespace vicuna_ddd.Shared.Response
{
    internal class LogErrorResponse : ErrorResponseDto
    {
        public LogErrorResponse(UserException ex) : base(ex)
        {
            StackTrace = ex.StackTrace;
        }

        public string? StackTrace { get; private set; }
    }
}