using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using AndroidX.AppCompat.App;
using Xamarin.Forms;
using BikEvent.App.Views;

namespace BikEvent.App.Droid
{
    [Activity(Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;

            base.OnCreate(savedInstanceState);

            Xamarin.Forms.Forms.SetFlags(new string[] {"RadioButton_Experimental"});

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

        }
                
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            if (App.Current.MainPage is MasterDetailPage mainPage)
            {
                if (mainPage.Detail is NavigationPage navigationPage)
                {
                    if (navigationPage.Navigation.NavigationStack.Count > 1)
                    {
                        // Se houver mais de uma página no histórico, vá para a página anterior
                        navigationPage.Navigation.PopAsync();
                    }
                    else
                    {
                        navigationPage.Navigation.PushAsync(new Initial());
                        // Se houver apenas uma página no histórico, você pode manipular o fechamento do aplicativo ou realizar alguma ação personalizada.
                        // Exemplo: base.OnBackPressed();
                        // Ou: Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                    }
                }
            }
            else
            {
                base.OnBackPressed();
            }
        }
    }
}