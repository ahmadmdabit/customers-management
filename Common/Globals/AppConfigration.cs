using Microsoft.Extensions.Configuration;
using System.IO;

namespace Common.Globals
{
    public class AppConfigration
    {
        public string SqlConnectionString { get; set; }
        public AppConfigration()
        {
            #region Init
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile(path, false);
            var root = configBuilder.Build();
            #endregion

            SqlConnectionString = root.GetSection("ConnectionStrings:DefaultConnection").Value;
        }
    }
}
