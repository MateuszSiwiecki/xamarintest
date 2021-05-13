using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace xamarintest.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public ICommand BackCommand { get; set; }

        protected static volatile bool CommandExecuting = false;
        public NavigationPage NavigationPage => Application.Current.MainPage as NavigationPage;
        public BaseViewModel()
        {
            BackCommand = new Command(async () => await PerformCommand(NavigationPage.PopAsync));
        }


        protected void ChangeValue<T>(ref T changingProp, T newValue, [CallerMemberName] string propertyName = null)
        {
            changingProp = newValue;
            OnPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool CanExecuteCommand() => !CommandExecuting;
        protected async Task PerformCommand(Func<Task> action)
        {
            if (CommandExecuting) return;
            CommandExecuting = true;
            await action.Invoke();
            await Task.Delay(100);
            CommandExecuting = false;
        }
        protected async Task PerformCommand(Func<object, Task> action, object objectToPass)
        {
            if (CommandExecuting) return;
            CommandExecuting = true;
            await action.Invoke(objectToPass);
            await Task.Delay(100);
            CommandExecuting = false;
        }
        protected async Task PerformCommand(Func<object, object, Task> action, object objectToPass1, object objectToPass2)
        {
            if (CommandExecuting) return;
            CommandExecuting = true;
            await action.Invoke(objectToPass1, objectToPass2);
            await Task.Delay(100);
            CommandExecuting = false;
        }
    }
}
