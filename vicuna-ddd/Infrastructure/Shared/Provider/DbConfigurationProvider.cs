using Microsoft.Extensions.Configuration;

namespace vicuna_ddd.Shared.Provider
{
    public static class DbConfigurationProvider
    {
        private static IConfigurationRoot _userConfigurationRoot;

        public static IConfigurationRoot GetApplicationConfigurationRoot()
        {
            return BuildConfiguration();
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            {
                if (_userConfigurationRoot == null)
                {
                    var basePath = AppContext.BaseDirectory;
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(basePath)
                        .AddJsonFile("appsettings.json");

                    _userConfigurationRoot = builder.Build();
                }
                return _userConfigurationRoot;
            }

        }
    }
}
