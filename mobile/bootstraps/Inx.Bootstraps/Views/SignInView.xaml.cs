
using Xamarin.Forms;
using Inx.Bootstraps.ViewModels;
using Autofac;
using Inx.Bootstraps.Services;

namespace Inx.Bootstraps.Views
{
    public partial class SignInView : ContentPage
    {
        public SignInView()
        {
            InitializeComponent();

            var container = Application.Current.GetContainer();
            BindingContext = new SignInViewModel(Navigation, container.Resolve<IAuthService>());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
