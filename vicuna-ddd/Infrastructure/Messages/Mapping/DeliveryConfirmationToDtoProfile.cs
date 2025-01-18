using AutoMapper;
using vicuna_ddd.Domain.Messages.Entity;
using vicuna_ddd.Domain.Users.Dto;

namespace vicuna_ddd.Domain.Shared.Mapping
{
    /// <summary>
    ///     Represents a mapping profile for converting DeliveryConfirmationDto objects to Message objects.
    /// </summary>
    public class DeliveryConfirmationToDtoProfile : Profile
    {
        public DeliveryConfirmationToDtoProfile()
        {
            CreateMap<DeliveredMessage, DeliveryConfirmationDto>()
                .ForMember(dest => dest.MessageKey, opt => opt.MapFrom(src => $"{src.Messagekey}"))
                .ForMember(dest => dest.MessageType, opt => opt.MapFrom(src => $"{src.MessageType}"))
                .ForMember(dest => dest.MessageStatus, opt => opt.MapFrom(src => $"{src.MessageStatus}"));
        }
    }
}