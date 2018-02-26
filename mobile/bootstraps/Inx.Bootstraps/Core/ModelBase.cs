using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Inx.Bootstraps.Core
{
    public abstract class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected ModelBase()
        {
        }

        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T property, T newValue, [CallerMemberName]string propertyName = null)
        {
            if (Equals(property, newValue)) return;

            property = newValue;

            RaisePropertyChanged(propertyName);
        }
    }
}
