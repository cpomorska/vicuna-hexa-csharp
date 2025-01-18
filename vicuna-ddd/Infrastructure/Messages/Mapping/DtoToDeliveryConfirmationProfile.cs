using AutoMapper;
using vicuna_ddd.Domain.Messages.Entity;
using vicuna_ddd.Domain.Users.Dto;

namespace vicuna_ddd.Domain.Shared.Mapping
{
    /// <summary>
    ///     Represents a mapping profile for converting DeliveryConfirmationDto objects to Message objects.
    /// </summary>
    public class DtoToDeliveryConfirmationProfile : Profile
    {
        public DtoToDeliveryConfirmationProfile()
        {
            CreateMap<DeliveryConfirmationDto, DeliveredMessage>()
                .ForMember(dest => dest.Messagekey, opt => opt.MapFrom(src => $"{src.MessageKey}"))
                .ForMember(dest => dest.MessageType, opt => opt.MapFrom(src => $"{src.MessageType}"))
                .ForMember(dest => dest.MessageStatus, opt => opt.MapFrom(src => $"{src.MessageStatus}"));
        }
    }
}