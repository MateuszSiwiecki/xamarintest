using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace xamarintest
{
    public class Model
    {
        private ImageSource _mainImageSource;

        public ImageSource MainImageSource
        {
            get => _mainImageSource;
            set => ChangeValue(ref _mainImageSource, value);
        }
        private ImageSource _icon = ImageSource.FromFile("Icon.png");
        public Model()
        {
            MainImageSource = _icon;
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
    }
}
