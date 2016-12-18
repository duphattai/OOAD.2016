using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarManager.Startup))]
namespace CarManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
