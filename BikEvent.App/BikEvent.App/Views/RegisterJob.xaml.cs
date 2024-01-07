using BikEvent.App.Models;
using BikEvent.App.Resources.Converters;
using BikEvent.App.Resources.Load;
using BikEvent.App.Services;
using BikEvent.Domain.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
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
	public partial class RegisterJob : ContentPage
	{
        private JobService _jobService;
		public RegisterJob ()
		{
			InitializeComponent ();
            _jobService = new JobService ();
		}

        private void GoBack(object sender, EventArgs e)
        {
			Navigation.PopAsync();
        }

        private async void Save(object sender, EventArgs e)
        {
            TxtMessages.Text = String.Empty;

            User user = JsonConvert.DeserializeObject<User>((string)App.Current.Properties["User"]);

            Job job = new Job()
            {
                Company = TxtCompany.Text,
                JobTitle = TxtJobTitle.Text,
                CityState = TxtCityState.Text,
                InitialSalary = TextToDoubleConverter.ToDouble(TxtInitialSalary.Text),
                FinalSalary = TextToDoubleConverter.ToDouble(TxtFinalSalary.Text),
                ContractType = (RBLCT.IsChecked) ? "CLT" : "PJ",
                TecnologyTools = TxtTecnologyTools.Text,
                CompanyDescription = TxtCompanyDescription.Text,
                JobDescription  = TxtJobDescription.Text,
                Benefits = TxtBenefits.Text,
                InterestedSendEmailTo = TxtInterestedSendEmailTo.Text,
                PublicationDate = DateTime.Now,
                UserId = user.Id
            };



            await Navigation.PushPopupAsync(new Loading());

            ResponseService<Job> responseService = await _jobService.AddJob(job);

            if (responseService.IsSuccess)
            {
                await Navigation.PopAllPopupAsync();
                await DisplayAlert("Cadastro de Vaga","Vaga cadastrada com sucesso!","OK");                
                await Navigation.PopAsync();
            }
            else
            {
                if (responseService.StatusCode == 400)
                {
                    StringBuilder stringBuilder = new StringBuilder();

                    foreach (var itemKey in responseService.Errors)
                    {
                        foreach (var message in itemKey.Value)
                        {
                            stringBuilder.AppendLine(message);
                        }
                    }
                    TxtMessages.Text = stringBuilder.ToString();
                }
                else
                {
                    await DisplayAlert("Erro", "Oops! Ocorreu um erro inesperado, tente novamente mais tarde.", "OK");
                }
                await Navigation.PopAllPopupAsync();
            }            
        }
    }
}