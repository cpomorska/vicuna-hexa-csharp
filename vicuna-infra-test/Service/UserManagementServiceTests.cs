using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Moq;
using vicuna_ddd.Domain.Users.Events;
using vicuna_ddd.Domain.Users.Repository;
using vicuna_ddd.Infrastructure.Events;
using vicuna_ddd.Model.Users.Entity;
using vicuna_infra.Controllers;
using vicuna_infra.Service;
using Assert = Xunit.Assert;

namespace vicuna_infra.Tests.Service;

public sealed class UserManagementServiceTests
{
    private readonly Mock<ILoggerFactory> _loggerFactory = new();
    private readonly Mock<ILogger<RestUserController>> _logger = new();
    private readonly Mock<IDomainEventDispatcher> _dispatcher = new();
    private readonly Mock<IGenericUserRepository<User>> _userRepository = new();

    public UserManagementServiceTests()
    {
        _loggerFactory
            .Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(_logger.Object);

        _dispatcher
            .Setup(x => x.DispatchAsync(It.IsAny<object>()))
            .Returns(Task.CompletedTask);
    }

    [Fact]
    public async Task AddUser_WhenRepositorySucceeds_ReturnsUserNumber()
    {
        var user = CreateUser();

        _userRepository
            .Setup(x => x.Add(user))
            .Returns(Task.CompletedTask);

        var service = CreateService();

        var result = await service.AddUser(user);

        Assert.Equal(user.UserNumber, result);
    }

    [Fact]
    public async Task AddUser_WhenRepositorySucceeds_AddsUser()
    {
        var user = CreateUser();

        _userRepository
            .Setup(x => x.Add(user))
            .Returns(Task.CompletedTask);

        var service = CreateService();

        await service.AddUser(user);

        _userRepository.Verify(x => x.Add(user), Times.Once);
    }

    [Fact]
    public async Task AddUser_WhenRepositorySucceeds_DispatchesUserCreatedEvent()
    {
        var user = CreateUser();

        _userRepository
            .Setup(x => x.Add(user))
            .Returns(Task.CompletedTask);

        var service = CreateService();

        await service.AddUser(user);

        _dispatcher.Verify(
            x => x.DispatchAsync(It.Is<UserCreatedEvent>(evt =>
                evt.UserId == user.UserNumber &&
                evt.UserName == user.UserName)),
            Times.Once);
    }

    [Fact]
    public async Task AddUser_WhenRepositoryThrows_ReturnsNull()
    {
        var user = CreateUser();

        _userRepository
            .Setup(x => x.Add(user))
            .ThrowsAsync(new InvalidOperationException("Repository error"));

        var service = CreateService();

        var result = await service.AddUser(user);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddUser_WhenRepositoryThrows_DoesNotDispatchEvent()
    {
        var user = CreateUser();

        _userRepository
            .Setup(x => x.Add(user))
            .ThrowsAsync(new InvalidOperationException("Repository error"));

        var service = CreateService();

        await service.AddUser(user);

        _dispatcher.Verify(
            x => x.DispatchAsync(It.IsAny<object>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateUser_WhenRepositorySucceeds_ReturnsUserNumber()
    {
        var user = CreateUser();

        _userRepository
            .Setup(x => x.Update(user))
            .Returns(Task.CompletedTask);

        var service = CreateService();

        var result = await service.UpdateUser(user);

        Assert.Equal(user.UserNumber, result);
    }

    [Fact]
    public async Task UpdateUser_WhenRepositorySucceeds_UpdatesUser()
    {
        var user = CreateUser();

        _userRepository
            .Setup(x => x.Update(user))
            .Returns(Task.CompletedTask);

        var service = CreateService();

        await service.UpdateUser(user);

        _userRepository.Verify(x => x.Update(user), Times.Once);
    }

    [Fact]
    public async Task UpdateUser_WhenRepositorySucceeds_DispatchesUserUpdatedEvent()
    {
        var user = CreateUser();

        _userRepository
            .Setup(x => x.Update(user))
            .Returns(Task.CompletedTask);

        var service = CreateService();

        await service.UpdateUser(user);

        _dispatcher.Verify(
            x => x.DispatchAsync(It.Is<UserUpdatedEvent>(evt =>
                evt.UserId == user.UserNumber &&
                evt.UserName == user.UserName)),
            Times.Once);
    }

    [Fact]
    public async Task UpdateUser_WhenRepositoryThrows_ReturnsNull()
    {
        var user = CreateUser();

        _userRepository
            .Setup(x => x.Update(user))
            .Throws(new InvalidOperationException("Repository error"));

        var service = CreateService();

        var result = await service.UpdateUser(user);

        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveUser_WhenUserIsPassedAndRepositorySucceeds_ReturnsUserNumber()
    {
        var user = CreateUser();

        _userRepository
            .Setup(x => x.Remove(user))
            .Returns(Task.CompletedTask);

        var service = CreateService();

        var result = await service.RemoveUser(user);

        Assert.Equal(user.UserNumber, result);
    }

    [Fact]
    public async Task RemoveUser_WhenUserIsPassedAndRepositorySucceeds_RemovesUser()
    {
        var user = CreateUser();

        _userRepository
            .Setup(x => x.Remove(user))
            .Returns(Task.CompletedTask);

        var service = CreateService();

        await service.RemoveUser(user);

        _userRepository.Verify(x => x.Remove(user), Times.Once);
    }

    [Fact]
    public async Task RemoveUser_WhenUserIsPassedAndRepositorySucceeds_DispatchesUserRemovedEvent()
    {
        var user = CreateUser();

        _userRepository
            .Setup(x => x.Remove(user))
            .Returns(Task.CompletedTask);

        var service = CreateService();

        await service.RemoveUser(user);

        _dispatcher.Verify(
            x => x.DispatchAsync(It.Is<UserRemovedEvent>(evt =>
                evt.UserId == user.UserNumber &&
                evt.UserName == user.UserName)),
            Times.Once);
    }

    [Fact]
    public async Task RemoveUser_WhenUserIsPassedAndRepositoryThrows_ReturnsNull()
    {
        var user = CreateUser();

        _userRepository
            .Setup(x => x.Remove(user))
            .Throws(new InvalidOperationException("Repository error"));

        var service = CreateService();

        var result = await service.RemoveUser(user);

        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveUser_WhenUserIdExists_ReturnsUserNumber()
    {
        var user = CreateUser();

        _userRepository
            .Setup(x => x.GetList(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync([user]);

        _userRepository
            .Setup(x => x.Remove(user))
            .Returns(Task.CompletedTask);

        var service = CreateService();

        var result = await service.RemoveUser(user.UserNumber);

        Assert.Equal(user.UserNumber, result);
    }

    [Fact]
    public async Task RemoveUser_WhenUserIdExists_RemovesUser()
    {
        var user = CreateUser();

        _userRepository
            .Setup(x => x.GetList(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync([user]);

        _userRepository
            .Setup(x => x.Remove(user))
            .Returns(Task.CompletedTask);

        var service = CreateService();

        await service.RemoveUser(user.UserNumber);

        _userRepository.Verify(x => x.Remove(user), Times.Once);
    }

    [Fact]
    public async Task RemoveUser_WhenUserIdExists_DispatchesUserRemovedEvent()
    {
        var user = CreateUser();

        _userRepository
            .Setup(x => x.GetList(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync([user]);

        _userRepository
            .Setup(x => x.Remove(user))
            .Returns(Task.CompletedTask);

        var service = CreateService();

        await service.RemoveUser(user.UserNumber);

        _dispatcher.Verify(
            x => x.DispatchAsync(It.Is<UserRemovedEvent>(evt =>
                evt.UserId == user.UserNumber &&
                evt.UserName == user.UserName)),
            Times.Once);
    }

    [Fact]
    public async Task RemoveUser_WhenUserIdDoesNotExist_ReturnsNull()
    {
        var userId = Guid.NewGuid();

        _userRepository
            .Setup(x => x.GetList(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync([]);

        var service = CreateService();

        var result = await service.RemoveUser(userId);

        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveUser_WhenUserIdDoesNotExist_DoesNotRemoveUser()
    {
        var userId = Guid.NewGuid();

        _userRepository
            .Setup(x => x.GetList(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync([]);

        var service = CreateService();

        await service.RemoveUser(userId);

        _userRepository.Verify(x => x.Remove(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task RemoveUser_WhenUserIdDoesNotExist_DoesNotDispatchEvent()
    {
        var userId = Guid.NewGuid();

        _userRepository
            .Setup(x => x.GetList(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync([]);

        var service = CreateService();

        await service.RemoveUser(userId);

        _dispatcher.Verify(
            x => x.DispatchAsync(It.IsAny<object>()),
            Times.Never);
    }

    [Fact]
    public async Task RemoveUser_WhenGetListThrows_ReturnsNull()
    {
        var userId = Guid.NewGuid();

        _userRepository
            .Setup(x => x.GetList(It.IsAny<Expression<Func<User, bool>>>()))
            .ThrowsAsync(new InvalidOperationException("Repository error"));

        var service = CreateService();

        var result = await service.RemoveUser(userId);

        Assert.Null(result);
    }

    private UserManagementService CreateService()
    {
        return new UserManagementService(
            _loggerFactory.Object,
            _dispatcher.Object,
            _userRepository.Object);
    }

    private static User CreateUser()
    {
        return new User
        {
            UserNumber = Guid.NewGuid(),
            UserName = "test-user"
        };
    }
}