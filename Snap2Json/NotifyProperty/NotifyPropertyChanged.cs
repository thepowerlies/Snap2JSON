using System.ComponentModel;
using System.Runtime.CompilerServices;
using Snap2Json.Annotations;

namespace Snap2Json.NotifyProperty
{
    public class NotifyPropertyChanged: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}