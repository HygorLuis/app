using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using QueixaAki.Components;
using QueixaAki.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(BaseEntry), typeof(BaseEntryRenderer))]
namespace QueixaAki.Droid.CustomRenderers
{
    public class BaseEntryRenderer : EntryRenderer
    {
        public BaseEntryRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null) return;

            var parsedColor = Color.Transparent;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                Control.BackgroundTintList = ColorStateList.ValueOf(parsedColor);
            else
                Control.Background.SetColorFilter(parsedColor, PorterDuff.Mode.SrcAtop);
        }
    }
}