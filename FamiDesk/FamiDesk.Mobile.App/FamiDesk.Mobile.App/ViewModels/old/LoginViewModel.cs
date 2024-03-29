﻿using System.Threading.Tasks;
using System.Windows.Input;

using FamiDesk.Mobile.App.Models;
using FamiDesk.Mobile.App.Helpers;
using FamiDesk.Mobile.App.Services;

using Xamarin.Forms;

namespace FamiDesk.Mobile.App.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            SignInCommand = new Command(async () => await SignIn());
            NotNowCommand = new Command(App.GoToMainPage);
        }

        string message = string.Empty;
        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(); }
        }

        public ICommand NotNowCommand { get; }
        public ICommand SignInCommand { get; }

        async Task SignIn()
        {
            try
            {
                IsBusy = true;
                Message = "Signing In...";

                // Log the user in
                await TryLoginAsync();
            }
            finally
            {
                Message = string.Empty;
                IsBusy = false;

                if (Settings.IsLoggedIn)
                    App.GoToMainPage();
            }
        }

        public static async Task<bool> TryLoginAsync()
        {
            var authentication = DependencyService.Get<IAuthenticator>();
            authentication.ClearCookies();

            var dataStore = DependencyService.Get<IDataStore<Person>>() as PersonAzureDataStore;
            await dataStore.InitializeAsync();

            if (dataStore.UseAuthentication)
            {
                var user = await authentication.LoginAsync(dataStore.MobileService, dataStore.AuthProvider, App.LoginParameters);
                if (user == null)
                {
                    MessagingCenter.Send(new MessagingCenterAlert
                    {
                        Title = "Sign In Error",
                        Message = "Unable to sign in, please check your credentials and try again.",
                        Cancel = "OK"
                    }, "message");
                    return false;
                }

                Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                Settings.UserId = user?.UserId ?? string.Empty;
            }

            return true;
        }
    }
}