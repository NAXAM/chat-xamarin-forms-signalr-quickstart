using Inx.Bootstraps.Core;
namespace Inx.Bootstraps.Models
{
    public class ConversationModel : ModelBase
    {
        public int Id { get; set; }

        string _Title;
        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }

        public string LastMessage { get; set; }
    }
}
