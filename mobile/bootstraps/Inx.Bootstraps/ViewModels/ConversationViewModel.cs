using System;
using Inx.Bootstraps.Core;
using System.Collections.ObjectModel;
using Inx.Bootstraps.Models;
using System.Windows.Input;
using Xamarin.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using Inx.DTOs;
using System.Threading.Tasks;
using System.Net.Http;
using Autofac;
using Inx.Bootstraps.Managers;
using System.Security.Cryptography.X509Certificates;
using Inx.Bootstraps.Services;
using Inx.Bootstraps.Extensions;

namespace Inx.Bootstraps.ViewModels
{
    public class ConversationViewModel : ViewModelBase
    {
        ObservableCollection<MessageModel> _Messages;
        public ObservableCollection<MessageModel> Messages
        {
            get => _Messages;
            set => SetProperty(ref _Messages, value);
        }

        string _NewText;
        public string NewText
        {
            get => _NewText;
            set => SetProperty(ref _NewText, value);
        }

        bool started;
        ConversationModel conversation;

        readonly HubConnection connection;
        readonly FriendModel recipient;
        readonly INavigation navigation;
        readonly ICurrentUserManager currentUserManager;
        readonly IConversationService conversationService;
        readonly IAuthManager authManager;

        public ConversationViewModel(
            INavigation navigation,
            FriendModel recipient)
        {
            this.navigation = navigation;
            this.recipient = recipient;
            this.currentUserManager = Application.Current.GetContainer().Resolve<ICurrentUserManager>();
            this.conversationService = Application.Current.GetContainer().Resolve<IConversationService>();
            this.authManager = Application.Current.GetContainer().Resolve<IAuthManager>();

            Messages = new ObservableCollection<MessageModel>(new MessageModel[0]);

            connection = new HubConnectionBuilder()
                .WithUrl(GetHubUrl())
                .WithConsoleLogger()
                .Build();

            connection.On<MessageDto>("message.sent", data =>
            {
                Messages.Add(data.ToModel());
            });

            connection.On<ConversationDto>("conversation.new", data =>
            {
                conversation = data.ToModel();
            });
        }

        public async void Connect()
        {
            if (false == started)
            {
                started = await connection.StartAsync()
                                .ContinueWith(x =>
                                {
                                    return x.IsCompleted && x.IsFaulted == false;
                                });
            }

            if (false == started) return;

            conversation = await conversationService.GetDirectConversationAsync(recipient.Id);

            if (conversation != null)
            {
                var msges = await conversationService.GetMessagesAsync(conversation.Id);
                Messages = new ObservableCollection<MessageModel>(msges);
            }
        }

        string GetHubUrl()
        {
            //return "http://nx-inx.azurewebsites.net/chat";
            var baseUrl = Device.RuntimePlatform == Device.iOS
                    ? "http://127.0.0.1:5000/chat"
                    : "http://10.0.2.2:5000/chat";

            return baseUrl + "?access_token=" + authManager.AuthorizeHeader.Value;
        }

        ICommand _SendCommand;
        public ICommand SendCommand
        {
            get => (_SendCommand = _SendCommand ?? new Command<object>(ExecuteSendCommand));
        }
        async void ExecuteSendCommand(object parameter)
        {
            if (string.IsNullOrWhiteSpace(NewText)) return;

            if (false == started) return;

            if (conversation == null)
            {
                await connection.SendAsync("StartDirectConverstation", new NewConversationDto
                {
                    Text = NewText,
                    RecipientIds = new[] {
                        recipient.Id
                    }
                });
            }
            else
            {
                await connection.SendAsync("Send", new NewMessageDto
                {
                    ConversationId = conversation.Id,
                    Text = NewText,
                });
            }
            NewText = string.Empty;
        }
    }
}
