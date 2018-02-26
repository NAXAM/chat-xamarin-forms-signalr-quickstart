using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Inx.Bootstraps.ViewModels;
using Autofac;
using Inx.Bootstraps.Services;

namespace Inx.Bootstraps.Views
{
    public partial class InboxView : ContentPage
    {
        readonly InboxViewModel vm;

        public InboxView()
        {
            InitializeComponent();

            var container = Application.Current.GetContainer();

            vm = new InboxViewModel(Navigation, container.Resolve<IFriendService>());
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            vm.LoadData();
        }
    }
}
