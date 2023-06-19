using System.ComponentModel.DataAnnotations;
using vicuna_ddd.Domain.Users.Exceptions;

namespace vicuna_ddd.Shared.Response
{
    public class ErrorResponseDto
    {
        [Required]
        public string Type { get; set; }
        [Required]
        public string Message { get; set; }
        public ErrorCode ErrorCode { get; set; }
        public Exception Ex { get; }

        public ErrorResponseDto(UserException ex)
        {
            Type = ex.GetType().Name;
            Message = ex.Message;
            ErrorCode = ex.ErrorCode;
        }

        public ErrorResponseDto(Exception ex)
        {
            Ex = ex;
        }
    }
}
