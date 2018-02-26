using System;
using Inx.Bootstraps.Core;
using System.Net.Http;
using Xamarin.Forms;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Text;
using Inx.DTOs;
using System.Net.Http.Headers;
using Inx.Bootstraps.Views;
using Inx.Bootstraps.Services;

namespace Inx.Bootstraps.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        string _Email;
        public string Email
        {
            get => _Email;
            set => SetProperty(ref _Email, value);
        }

        string _Password;
        public string Password
        {
            get => _Password;
            set => SetProperty(ref _Password, value);
        }

        string _PasswordConfirmation;
        public string PasswordConfirmation
        {
            get => _PasswordConfirmation;
            set => SetProperty(ref _PasswordConfirmation, value);
        }

        readonly INavigation navigation;
        readonly IAuthService authService;

        public SignInViewModel(INavigation navigation, IAuthService authService)
        {
            this.authService = authService;
            this.navigation = navigation;

#if DEBUG
            Email = Device.RuntimePlatform == Device.Android
                          ? "tuyen@naxam.net"
                          : "kimanh@naxam.net";
            Password = "Shar#Nx2018";
#endif
        }

        ICommand _SignInCommand;
        public ICommand SignInCommand
        {
            get => (_SignInCommand = _SignInCommand ?? new Command<object>(ExecuteSignInCommand));
        }
        async void ExecuteSignInCommand(object parameter)
        {
            var ok = await authService.SignInAsync(Email, Password);

            if (false == ok) return;

            await navigation.PushAsync(new InboxView());
        }

        ICommand _SignUpCommand;
        public ICommand SignUpCommand
        {
            get => (_SignUpCommand = _SignUpCommand ?? new Command<object>(ExecuteSignUpCommand));
        }
        async void ExecuteSignUpCommand(object parameter)
        {
            var ok = await authService.SignUpAsync(Email, Password);

            if (false == ok) return;

            await navigation.PushAsync(new InboxView());
        }

        string GetBaseUrl()
        {
            //return "http://nx-inx.azurewebsites.net/chat";
            return Device.RuntimePlatform == Device.iOS
                    ? "http://127.0.0.1:5000/api/account/"
                    : "http://10.0.2.2:5000/api/account/";
        }

    }
}
