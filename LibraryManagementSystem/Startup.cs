using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LibraryManagementSystem.Startup))]
namespace LibraryManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
