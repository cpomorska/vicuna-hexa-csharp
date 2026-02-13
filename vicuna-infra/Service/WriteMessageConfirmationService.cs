using AutoMapper;
using vicuna_ddd.Domain.Messages.Entity;
using vicuna_ddd.Domain.Messages.Repository;
using vicuna_ddd.Domain.Messages.Service;
using vicuna_ddd.Domain.Users.Dto;
using vicuna_ddd.Domain.Users.Messaging;

namespace vicuna_infra.Service
{
    public class WriteMessageConfirmationService(
        IDeliveryedMessageRepository<DeliveredMessage> deliveredMessageRepository,
        IMapper mapper)
        : IWriteMessageConfirmationService
    {
        public async Task<Guid?> StoreDeliveredMessage(DeliveryConfirmationDto? deliveryConfirmationDto)
        {
            var deliveryConfirmation = mapper.Map<DeliveredMessage>(deliveryConfirmationDto);
            await deliveredMessageRepository.Add(deliveryConfirmation);
            return deliveryConfirmation.Messagekey;
        }

        public async Task<Guid?> UpdateDeliveredMessage(Guid? id)
        {
            var deliveryConfirmation = await GetDeliveredMessageById(id);

            if (deliveryConfirmation == null)
            {
                return Guid.Empty;
            }

            deliveryConfirmation.MessageStatus = MessageStatus.Finalized;
            await deliveredMessageRepository.Update(deliveryConfirmation);
            return deliveryConfirmation.Messagekey;
        }

        public async Task<Guid?> DeleteDeliveredMessage(Guid id)
        {
            var deliveryConfirmation = await GetDeliveredMessageById(id);

            if (deliveryConfirmation == null)
            {
                return Guid.Empty;
            }

            await deliveredMessageRepository.Remove(deliveryConfirmation);
            return deliveryConfirmation.Messagekey;
        }

        private async Task<DeliveredMessage> GetDeliveredMessageById(Guid? id)
        {
            return await deliveredMessageRepository.GetSingle(x => x.Messagekey == id);
        }
    }
}