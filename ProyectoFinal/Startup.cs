using Microsoft.Owin;
using Owin;

namespace ProyectoFinal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}