using vicuna_ddd.Model.Users.Entity;
using vicuna_ddd.Shared.Util;

namespace vicuna_infra_test.Controller
{
    public static class RestControllerTestHelpers
    {
        public static User CreateTestUser(string username)
        {
            var randomSaltMann = HashUtil.GetRandomSalt(13);
            var guid = Guid.NewGuid();

            var userHash = new UserHash
            {
                saltField = randomSaltMann,
                hashField = HashUtil.CalculateCustomerHash(username, randomSaltMann)
            };

            var userRole = new UserRole
            {
                RoleName = "TestRole",
                RoleDescription = "TestDescription",
                RoleType = UserRoleTypes.Admin
            };

            return new User
            {
                UserName = username,
                UserEmail = "testemail@test.de",
                UserPass = "Testpass",
                UserNumber = guid,
                UserToken = "userToken",
                UserHash = userHash,
                UserRole = userRole,
                UserEnabled = true
                //CreatedAt = DateTime.UtcNow,
                //ModifiedAt = DateTime.UtcNow,
                //ModifiedFrom = "randomSaltmann!"
            };
        }
    }
}