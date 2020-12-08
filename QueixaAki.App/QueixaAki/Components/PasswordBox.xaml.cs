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
        }

        public string Text { get; set; }

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

        public string Placeholder { get; set; }

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
    }
}