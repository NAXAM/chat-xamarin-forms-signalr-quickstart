using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Inx.Bootstraps.ViewModels;
using Inx.DTOs;
using Inx.Bootstraps.Models;

namespace Inx.Bootstraps.Views
{
    public partial class ConversationView : ContentPage
    {
        readonly ConversationViewModel vm;

        public ConversationView(FriendModel user)
        {
            InitializeComponent();

            vm = new ConversationViewModel(Navigation, user);
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            vm.Connect();
        }
    }
}
