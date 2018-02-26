
using Xamarin.Forms;
using Inx.Bootstraps.Views;
using Inx.DTOs;
using Autofac;
using Inx.Networking.Core;
using Inx.Bootstraps.Managers;
using Inx.Networking;
using Inx.Bootstraps.Services;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace Inx.Bootstraps
{
    public partial class App : Application
    {
        public IContainer Container { get; private set; }

        public App()
        {
            InitializeComponent();

            var builder = new ContainerBuilder();


            builder.Register(cg => new DefaultNetworkingClient(
                        () => new Newtonsoft.Json.JsonSerializer(),
                        () => Container.Resolve<IAuthManager>().AuthorizeHeader,
                        GetBaseUrl()
                    ))
                   .As<INetworkingClient>()
                   .SingleInstance();

            builder.RegisterType<AuthClient>();
            builder.RegisterType<UserClient>();
            builder.RegisterType<ConversationClient>();
            builder.RegisterType<MessageClient>();

            builder.RegisterType<AuthManager>()
                   .As<IAuthManager>()
                   .SingleInstance();
            builder.RegisterType<CurrentUserManager>()
                   .As<ICurrentUserManager>()
                   .SingleInstance();

            builder.RegisterType<AuthService>().As<IAuthService>();
            builder.RegisterType<FriendService>().As<IFriendService>();
            builder.RegisterType<ConversationService>().As<IConversationService>();

            Container = builder.Build();

            MainPage = new NavigationPage(new SignInView());
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        string GetBaseUrl()
        {
            return Device.RuntimePlatform == Device.iOS
                    ? "http://127.0.0.1:5000/api/"
                    : "http://10.0.2.2:5000/api/";
        }
    }

    public static class AppExtensions
    {
        public static IContainer GetContainer(this Application app)
        {
            return ((App)app).Container;
        }
    }
}
