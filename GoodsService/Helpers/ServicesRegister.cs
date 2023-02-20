using GoodsService.Models.Interfaceimpl;
using GoodsService.Models.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GoodsService.Helpers
{
    public static class ServicesRegister
    {
        public static void AddServices(this IServiceCollection services)
        {
            var configurebuilder = new ConfigurationBuilder();
            configurebuilder.AddIniFile("conf.ini");
            var AppConfig = configurebuilder.Build();
            
            services.AddTransient<IDataBase>(x=>new MySqlDataBaseWork(AppConfig["server"], AppConfig["user"], AppConfig["password"], AppConfig["database"]));
            services.AddTransient<IBaseService,OrderGoodsService>();

        }
    }
}
