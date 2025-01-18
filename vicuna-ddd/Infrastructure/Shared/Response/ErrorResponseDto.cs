using System.ComponentModel.DataAnnotations;
using vicuna_ddd.Domain.Users.Exceptions;

namespace vicuna_ddd.Shared.Response
{
    public class ErrorResponseDto
    {
        public ErrorResponseDto(UserException ex)
        {
            Type = ex.GetType().Name;
            Message = ex.Message;
            ErrorCode = ex.ErrorCode;
        }

        [Required] public string Type { get; set; }

        [Required] public string Message { get; set; }

        public ErrorCode ErrorCode { get; set; }
    }
}