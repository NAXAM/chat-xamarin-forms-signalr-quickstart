using Inx.Bootstraps.Core;
using Xamarin.Forms;
using Inx.DTOs;
using System.Collections.ObjectModel;
using Inx.Bootstraps.Views;
using System.Windows.Input;
using Inx.Bootstraps.Services;
using Inx.Bootstraps.Models;

namespace Inx.Bootstraps.ViewModels
{
    public class InboxViewModel : ViewModelBase
    {
        ObservableCollection<FriendModel> _Friends;
        public ObservableCollection<FriendModel> Friends
        {
            get => _Friends;
            set => SetProperty(ref _Friends, value);
        }

        readonly INavigation navigation;
        readonly IFriendService friendService;

        public InboxViewModel(INavigation navigation, IFriendService friendService)
        {
            this.friendService = friendService;
            this.navigation = navigation;
        }

        public async void LoadData()
        {
            if (Friends != null)
            {
                return;
            }

            var friends = await friendService.GetMyFriendsAsync();

            Friends = new ObservableCollection<FriendModel>(friends);
        }

        ICommand _SelectFriendCommand;
        public ICommand SelectFriendCommand
        {
            get => (_SelectFriendCommand = _SelectFriendCommand ?? new Command<FriendModel>(ExecuteSelectFriendCommand));
        }
        async void ExecuteSelectFriendCommand(FriendModel user)
        {
            await navigation.PushAsync(new ConversationView(user));
        }
    }
}

