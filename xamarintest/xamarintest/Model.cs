using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace xamarintest
{
    public class Model
    {
        public ICommand TakePhotoCommand { get; set; }
        private ImageSource _mainImageSource;

        public ImageSource MainImageSource
        {
            get => _mainImageSource;
            set => ChangeValue(ref _mainImageSource, value);
        }
        private ImageSource _icon = ImageSource.FromFile("Icon.png");
        private ImageSource _effect;
        public Model()
        {
            MainImageSource = _icon;
            TakePhotoCommand = new Command(async () => await TakePhotoAsync());
        }

        private async Task TakePhotoAsync()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                DisplayShortAlert("No Camera");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            DisplayLongAlert($"File Location {file.Path}");

            MainThread.BeginInvokeOnMainThread(() => MainImageSource = ImageSource.FromFile(file.Path));
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
        private void DisplayShortAlert(string message) => DependencyService.Get<IMessage>().ShortAlert(message);
        private void DisplayLongAlert(string message) => DependencyService.Get<IMessage>().LongAlert(message);
    }
}
