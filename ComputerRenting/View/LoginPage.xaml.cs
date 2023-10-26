using ComputerRenting.Model;
using Newtonsoft.Json;
using System.Text;

namespace ComputerRenting.View;

public partial class LoginPage : ContentPage
{
    static readonly HttpClient client = new HttpClient();
    public Bruger bruger { get; set; }
    public LoginPage()
	{
		InitializeComponent();
	}
    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (EmailEntry.Text == "" && PasswordEntry.Text == "")
        {
            bruger = new Bruger()
            {
                brugerID = 0,
                navn = "Person Navn Her",
                postNrID = 5700,
                postNrByNavn = "Svendborg",
                adresse = "Et eller andet sted",
                brugerGruppeNavn = "Bruger",
                adgangskode = "1234",
                email = "Noget@skole.dk",
                stamKlasseNavn = "H23e4adgy",
                cprNummer = "222222-2222",
                elevNummer = "ElevNr0"
            };
            App.Current.MainPage = new Udlån(bruger);
            EmailEntry.BackgroundColor = Color.FromRgb(255, 255, 255);
        }
        else if (EmailEntry.Text != "" && PasswordEntry.Text != "")
        {
            try
            {
                string json = "{\r\n    \"email\":\"" + EmailEntry.Text + "\",\r\n    \"adgangskode\":\"" + PasswordEntry.Text + "\"\r\n}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using HttpResponseMessage response = await client.PostAsync("https://gorilla-informed-asp.ngrok-free.app/bruger/login", content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                bruger = JsonConvert.DeserializeObject<Bruger>(responseBody);
                App.Current.MainPage = new Udlån(bruger);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
            }
        }
    }
}