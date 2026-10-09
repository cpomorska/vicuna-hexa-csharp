using Microsoft.EntityFrameworkCore;
using vicuna_ddd.Model.Users.Entity;
using vicuna_ddd.Shared.Provider;
using Assert = Xunit.Assert;

namespace vicuna_infra_test.Provider;

public class UserDbContextTests
{
    [Fact]
    public void UserDbContext_InheritsFromGenericDbContext()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new UserDbContext(options);

        Assert.IsAssignableFrom<GenericDbContext>(context);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Constructor_WithUseInMemoryDb_SetsProperty(bool useInMemoryDb)
    {
        using var context = new UserDbContext(useInMemoryDb);

        Assert.Equal(useInMemoryDb, context.UseInMemoryDb);
    }

    [Fact]
    public void Constructor_WithOptions_InitializesSuccessfully()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new UserDbContext(options);

        Assert.NotNull(context);
        Assert.NotNull(context.Database);
    }

    [Fact]
    public async Task Users_CanAddAndRetrieveUser()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var user = new User
        {
            UserNumber = Guid.NewGuid(),
            UserName = "testuser",
            UserPass = "hashed_pass",
            UserEmail = "test@example.com",
            UserToken = "test_token",
            UserEnabled = true
        };

        using (var context = new UserDbContext(options))
        {
            await context.Users!.AddAsync(user);
            await context.SaveChangesAsync();
        }

        using (var context = new UserDbContext(options))
        {
            var retrievedUser = await context.Users!.FirstOrDefaultAsync(u => u.UserNumber == user.UserNumber);

            Assert.NotNull(retrievedUser);
            Assert.Equal("testuser", retrievedUser.UserName);
            Assert.Equal("test@example.com", retrievedUser.UserEmail);
        }
    }

    [Fact]
    public async Task UserHash_CanAddAndRetrieveUserHash()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var userHash = new UserHash
        {
            hashField = "sample_hash",
            saltField = "sample_salt"
        };

        using (var context = new UserDbContext(options))
        {
            await context.UserHash!.AddAsync(userHash);
            await context.SaveChangesAsync();
        }

        using (var context = new UserDbContext(options))
        {
            var retrievedHash = await context.UserHash!.FirstOrDefaultAsync(h => h.hashField == "sample_hash");

            Assert.NotNull(retrievedHash);
            Assert.Equal("sample_salt", retrievedHash.saltField);
        }
    }

    [Fact]
    public async Task UserRoles_CanAddAndRetrieveUserRole()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var userRole = new UserRole
        {
            RoleName = "Admin",
            RoleType = UserRoleTypes.Admin,
            RoleDescription = "Administrator role"
        };

        using (var context = new UserDbContext(options))
        {
            await context.UserRoles!.AddAsync(userRole);
            await context.SaveChangesAsync();
        }

        using (var context = new UserDbContext(options))
        {
            var retrievedRole = await context.UserRoles!.FirstOrDefaultAsync(r => r.RoleName == "Admin");

            Assert.NotNull(retrievedRole);
            Assert.Equal(UserRoleTypes.Admin, retrievedRole.RoleType);
            Assert.Equal("Administrator role", retrievedRole.RoleDescription);
        }
    }

    [Fact]
    public void Constructor_WithInMemoryDbFalse_ConfiguresDatabaseFromSettings()
    {
        using var context = new UserDbContext(false);
        Assert.NotNull(context.Database);
    }

    [Fact]
    public void DbSets_CanBeSetDirectly()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new UserDbContext(options);

        context.Users = context.Set<User>();
        context.UserHash = context.Set<UserHash>();
        context.UserRoles = context.Set<UserRole>();

        Assert.NotNull(context.Users);
        Assert.NotNull(context.UserHash);
        Assert.NotNull(context.UserRoles);
    }
}
