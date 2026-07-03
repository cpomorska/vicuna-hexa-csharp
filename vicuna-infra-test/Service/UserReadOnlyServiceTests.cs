using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Moq;
using vicuna_ddd.Domain.Users.Dto;
using vicuna_ddd.Domain.Users.Repository;
using vicuna_ddd.Model.Users.Entity;
using vicuna_infra.Service;
using Assert = Xunit.Assert;

namespace vicuna_infra.Tests.Service
{
    public class UserReadOnlyServiceTests
    {
        private readonly Mock<ILoggerFactory> _loggerFactoryMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<IGenericUserRepository<User>> _userRepositoryMock;
        private readonly UserReadOnlyService _service;

        public UserReadOnlyServiceTests()
        {
            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _loggerMock = new Mock<ILogger>();
            _userRepositoryMock = new Mock<IGenericUserRepository<User>>();

            _loggerFactoryMock
                .Setup(x => x.CreateLogger(It.IsAny<string>()))
                .Returns(_loggerMock.Object);

            _service = new UserReadOnlyService(
                _loggerFactoryMock.Object,
                _userRepositoryMock.Object);
        }

        [Fact]
        public async Task FindUser_ReturnsFirstMatchingUser()
        {
            var guid = Guid.NewGuid();
            var userDto = new UserDto
            {
                UserName = "test-user",
                UserNumber = guid,
                UserPass = "password",
                UserEnabled = true
            };

            var expectedUser = new User
            {
                UserName = "test-user",
                UserNumber = guid,
                UserPass = "password",
                UserEnabled = true
            };

            _userRepositoryMock
                .Setup(x => x.GetList(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { expectedUser });

            var result = await _service.FindUser(userDto);

            Assert.Same(expectedUser, result);

            _userRepositoryMock.Verify(
                x => x.GetList(It.IsAny<Expression<Func<User, bool>>>()),
                Times.Once);
        }

        [Fact]
        public async Task FindUser_WhenRepositoryThrows_ReturnsNull()
        {
            var guid = Guid.NewGuid();
            var userDto = new UserDto
            {
                UserName = "test-user",
                UserNumber = guid,
                UserPass = "password",
                UserEnabled = true
            };

            _userRepositoryMock
                .Setup(x => x.GetList(It.IsAny<Expression<Func<User, bool>>>()))
                .ThrowsAsync(new InvalidOperationException("Repository error"));

            var result = await _service.FindUser(userDto);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByEmail_ReturnsFirstMatchingUser()
        {
            var expectedUser = new User
            {
                UserEmail = "user@example.com"
            };

            _userRepositoryMock
                .Setup(x => x.GetList(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { expectedUser });

            var result = await _service.GetUserByEmail("user@example.com");

            Assert.Same(expectedUser, result);

            _userRepositoryMock.Verify(
                x => x.GetList(It.IsAny<Expression<Func<User, bool>>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetUserByEmail_WhenRepositoryThrows_ReturnsNull()
        {
            _userRepositoryMock
                .Setup(x => x.GetList(It.IsAny<Expression<Func<User, bool>>>()))
                .ThrowsAsync(new InvalidOperationException("Repository error"));

            var result = await _service.GetUserByEmail("user@example.com");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByUsernnameAndPassword_ReturnsFirstMatchingUser()
        {
            var expectedUser = new User
            {
                UserName = "test-user",
                UserPass = "password"
            };

            _userRepositoryMock
                .Setup(x => x.GetList(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { expectedUser });

            var result = await _service.GetUserByUsernnameAndPassword(
                "test-user",
                "password");

            Assert.Same(expectedUser, result);

            _userRepositoryMock.Verify(
                x => x.GetList(It.IsAny<Expression<Func<User, bool>>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetUserByUsernnameAndPassword_WhenRepositoryThrows_ReturnsNull()
        {
            _userRepositoryMock
                .Setup(x => x.GetList(It.IsAny<Expression<Func<User, bool>>>()))
                .ThrowsAsync(new InvalidOperationException("Repository error"));

            var result = await _service.GetUserByUsernnameAndPassword(
                "test-user",
                "password");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByUsername_ReturnsFirstMatchingUser()
        {
            var expectedUser = new User
            {
                UserName = "test-user"
            };

            _userRepositoryMock
                .Setup(x => x.GetList(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { expectedUser });

            var result = await _service.GetUserByUsername("test-user");

            Assert.Same(expectedUser, result);

            _userRepositoryMock.Verify(
                x => x.GetList(It.IsAny<Expression<Func<User, bool>>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetUserByUsername_WhenRepositoryThrows_ReturnsNull()
        {
            _userRepositoryMock
                .Setup(x => x.GetList(It.IsAny<Expression<Func<User, bool>>>()))
                .ThrowsAsync(new InvalidOperationException("Repository error"));

            var result = await _service.GetUserByUsername("test-user");

            Assert.Null(result);
        }
    }
}