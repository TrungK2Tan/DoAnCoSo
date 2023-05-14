using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Booking_Dental_Clinic.Startup))]
namespace Booking_Dental_Clinic
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
