using BikEvent.App.ViewModels;
using BikEvent.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BikEvent.App.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Visualizer : ContentPage
	{
        private VisualizerViewModel _viewModel;

        public Visualizer(Event eventToShow)
        {
            InitializeComponent();
            _viewModel = new VisualizerViewModel(eventToShow);
            BindingContext = _viewModel;
        }

        private void GoBack(object sender, EventArgs e)
        {
			Navigation.PopAsync();
        }

        /*protected override void OnAppearing()
        {
            base.OnAppearing();            

            Event _event = ((Event)BindingContext);

            if (string.IsNullOrEmpty(_event.SocialMedia))
            {
                HeaderSocialMedia.IsVisible = false;
                TextSocialMedia.IsVisible = false;
            }

            if (string.IsNullOrEmpty(_event.Description))
            {
                HeaderDescription.IsVisible = false;
                TextDescription.IsVisible = false;
            }

            if (string.IsNullOrEmpty(_event.Benefits))
            {
                HeaderBenefits.IsVisible = false;
                TextBenefits.IsVisible = false;
            }
        }*/

        private async void EditEvent(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditEvent((Event)BindingContext));
        }
    }
}