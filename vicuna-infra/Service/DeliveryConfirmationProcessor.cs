using vicuna_ddd.Domain.Messages.Service;
using vicuna_ddd.Domain.Users.Dto;
using vicuna_ddd.Domain.Users.Messaging;

namespace vicuna_infra.Service
{

    using Microsoft.Extensions.Logging;

    public class DeliveryConfirmationProcessor
    {
        private readonly ILogger<DeliveryConfirmationProcessor> logger;
        private readonly IReadMessageConformationService? _readMessageConformationService;
        private readonly IWriteMessageConfirmationService? _writeMessageConfirmationService;

        public DeliveryConfirmationProcessor()
        {
            logger = new Logger<DeliveryConfirmationProcessor>(new LoggerFactory());
        }

        public DeliveryConfirmationProcessor(ILogger<DeliveryConfirmationProcessor> logger,
            IReadMessageConformationService? readMessageConformationService,
            IWriteMessageConfirmationService writeMessageConfirmationService)
        {
            this.logger = logger;
            this._readMessageConformationService = readMessageConformationService;
            this._writeMessageConfirmationService = _writeMessageConfirmationService;
        }

        public MessageResult ProcessMessageDeliveredDto(DeliveryConfirmationDto deliveryConfirmationDto)
        {
            if (deliveryConfirmationDto == null ||
                string.IsNullOrWhiteSpace(deliveryConfirmationDto.MessageKey.ToString()))
            {
                logger.LogError($"An error occurred while processing MessageDeliveredDto, Message is null or empty");
                return MessageResult.NoK;
            }

            try
            {
                var deliveredMessage =
                    _readMessageConformationService.FindMessageConformation(deliveryConfirmationDto.MessageKey);

                if (deliveredMessage == null)
                {
                    logger.LogWarning($"Could not find a message with ID '{deliveryConfirmationDto.MessageKey}'.");
                    return MessageResult.Unknown;
                }

                deliveredMessage.Result.MessageStatus = MessageStatus.Delivered;
                _writeMessageConfirmationService.StoreDeliveredMessage(deliveryConfirmationDto);

                return MessageResult.Ok;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    $"An error occurred while processing MessageDeliveredDto with ID '{deliveryConfirmationDto.MessageKey}': {ex}");
                return MessageResult.Unknown;
            }
        }
    }
}

