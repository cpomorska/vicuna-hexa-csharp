using System.Linq.Expressions;
using AutoMapper;
using Moq;
using vicuna_ddd.Domain.Meesage.Repository;
using vicuna_ddd.Domain.Messages.Entity;
using vicuna_ddd.Domain.Users.Dto;

namespace vicuna_infra.Service
{
    public class WriteMessageConfirmationServiceTests
    {
        private readonly Mock<IMapper> _mockedMapper;

        // Mock objects
        private readonly Mock<IDeliveryedMessageRepository<DeliveredMessage>> _mockedRepository;

        // System under test
        private readonly WriteMessageConfirmationService _sut;

        public WriteMessageConfirmationServiceTests()
        {
            _mockedRepository = new Mock<IDeliveryedMessageRepository<DeliveredMessage>>();
            _mockedMapper = new Mock<IMapper>();

            _sut = new WriteMessageConfirmationService(_mockedRepository.Object, _mockedMapper.Object);
        }

        [Fact]
        public async Task Given_DeliveryConfirmationDto_When_StoreDeliveredMessage_Then_ReturnsMessageKey()
        {
            // Given
            var testGuid = Guid.NewGuid();
            var deliveryConfirmationDto = new DeliveryConfirmationDto();
            var deliveredMessage = new DeliveredMessage { Messagekey = testGuid };

            _mockedMapper.Setup(m => m.Map<DeliveredMessage>(It.IsAny<DeliveryConfirmationDto>()))
                .Returns(deliveredMessage);

            // When
            var result = await _sut.StoreDeliveredMessage(deliveryConfirmationDto);

            // Then
            Assert.NotNull(result);
            Assert.IsType<Guid>(result);
            Assert.Equal(testGuid, result);
        }

        [Fact]
        public async Task Given_MessageKey_When_UpdateDeliveredMessage_Then_ReturnsMessageKey()
        {
            // Given
            var testGuid = Guid.NewGuid();
            var deliveredMessage = new DeliveredMessage { Messagekey = testGuid };

            _mockedRepository.Setup(s => s.GetSingle(It.IsAny<Expression<Func<DeliveredMessage, bool>>>()))
                .ReturnsAsync(deliveredMessage);

            // When
            var result = await _sut.UpdateDeliveredMessage(testGuid);

            // Then
            Assert.NotNull(result);
            Assert.IsType<Guid>(result);
            Assert.Equal(testGuid, result);
        }

        [Fact]
        public async Task Given_MessageKey_When_DeleteDeliveredMessage_Then_ReturnsMessageKey()
        {
            // Given
            var testGuid = Guid.NewGuid();
            var deliveredMessage = new DeliveredMessage { Messagekey = testGuid };

            _mockedRepository.Setup(s => s.GetSingle(It.IsAny<Expression<Func<DeliveredMessage, bool>>>()))
                .ReturnsAsync(deliveredMessage);

            // When
            var result = await _sut.DeleteDeliveredMessage(testGuid);

            // Then
            Assert.NotNull(result);
            Assert.IsType<Guid>(result);
            Assert.Equal(testGuid, result);
        }
    }
}