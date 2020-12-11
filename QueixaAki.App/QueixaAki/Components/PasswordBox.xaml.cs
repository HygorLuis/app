using System;
using System.Runtime.CompilerServices;
using QueixaAki.ViewModels.Components;
using Xamarin.Forms;

namespace QueixaAki.Components
{
    public partial class PasswordBox
    {
        private PasswordBoxViewModel _viewModel;

        public PasswordBox()
        {
            InitializeComponent();

            _viewModel = new PasswordBoxViewModel();
            BindingContext = _viewModel;

            BaseEntryBox.TextChanged += BaseEntryBox_TextChanged;
        }

        public string Text 
        { 
            get => (string)GetValue(TextProperty); 
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(PasswordBox),
            "",
            BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (newValue != null && bindable is PasswordBox control)
                {
                    var actualNewValue = (string)newValue;
                    control.BaseEntryBox.Text = actualNewValue;
                }
            });

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
            nameof(Placeholder),
            typeof(string),
            typeof(PasswordBox),
            "",
            BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (newValue != null && bindable is PasswordBox control)
                {
                    var actualNewValue = (string)newValue;
                    control.BaseEntryBox.Placeholder = actualNewValue;
                }
            });

        public event EventHandler OnTextChanged;

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }

        private void BaseEntryBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnTextChanged?.Invoke(sender, e);
        }
    }
}