using vicuna_ddd.Domain.Users.Dto;

namespace vicuna_ddd.Shared.Response
{
    public class RestErrorResponse : ErrorResponseDto
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public RestErrorResponse(Exception ex)
        {
            Type = ex.GetType().Name;
            Message = ex.Message;
        }
    }
}
