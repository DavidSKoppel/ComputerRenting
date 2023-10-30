using CommunityToolkit.Maui.Views;
using ComputerRenting.Model;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace ComputerRenting.View;

public partial class Udlån : ContentPage
{
    static readonly HttpClient client = new HttpClient();
    public Bruger bruger { get; set; }
    public ObservableCollection<RentedComputer> RentedComputers { get; set; }
    public ObservableCollection<FreeComputer> FreeComputers { get; set; }
    public ICommand ReserveComputer => new Command<FreeComputer>(ReserveFreeComputer);
    public ICommand CancelComputer => new Command<RentedComputer>(CancelRent);

    public Udlån(Bruger user)
    {
        InitializeComponent();
        bruger = user;
        /*RentedComputers = new ObservableCollection<RentedComputer>
        {
            new RentedComputer()
            {
                computer = "Dell",
                model = "Latitude E5440",
                udlåningsdato = "2023-12-21",
                udløbsdato = "2023-12-24"
            }
        };*/
        /*FreeComputers = new ObservableCollection<FreeComputer>
        {
            new FreeComputer()
            {
                id = 0,
                computer = "Dell",
                model = "Latitude E5440",
                mus = "Optisk",
                register = "aduhgsayggduya"
            }
        };*/
        FreeComputers = Task.Run( async () => await HttpGetAllNonRentedComputer()).Result;
        RentedComputers = Task.Run( async () => await HttpGetAllUserRentedComputer()).Result;
        BindingContext = this;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new ProfilPage(bruger));
    }

    private async Task<ObservableCollection<FreeComputer>> HttpGetAllNonRentedComputer()
    {
        using HttpResponseMessage response = await client.GetAsync("https://gorilla-informed-asp.ngrok-free.app/computer/GetAllNonRentedComputers");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        var computers = JsonConvert.DeserializeObject<ObservableCollection<FreeComputer>>(responseBody);
        return computers;
    }

    private async Task<ObservableCollection<RentedComputer>> HttpGetAllUserRentedComputer()
    {
        using HttpResponseMessage response = await client.GetAsync("https://gorilla-informed-asp.ngrok-free.app/Udlån/GetRentedComputersByUserID/" + bruger.brugerID);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        var computers = JsonConvert.DeserializeObject<ObservableCollection<RentedComputer>>(responseBody);
        foreach (var item in computers)
        {
            var convert = DateTime.Parse(item.udløbsdato);
            item.udløbsdato = convert.ToString("yyyy'-'MM'-'dd");
            item.color = (item.status == "Reserveret") ? Color.FromRgb(255,255,0) : Color.FromRgb(255,0,0);
        }
        return computers;
    }
    private async void CancelRent(RentedComputer com)
    {
        if(com.status == "Reserveret")
        {
            bool answer = await DisplayAlert("Fortryd reservation", "Vil du fortryde din reservation?", "Ja", "Nej");
            if (answer)
            {
                var freeComputer = new FreeComputer
                {
                    id = com.id,
                    register = com.register,
                    computer = com.computer,
                    mus = com.mus,
                    model = com.model
                };
                var httpResult = await HttpDeleteReserveComputer(com.rentId);
                if (httpResult)
                {
                    RentedComputers.Remove(com);
                    FreeComputers.Add(freeComputer);
                }
            }
        }
    }

    private async void ReserveFreeComputer(FreeComputer com)
    {
        var popup = new RentComputerPopUp(com);
        var popUpResult = await this.ShowPopupAsync(popup);
        if (popUpResult is DateTime date)
        {
            string udlåningsdato = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            string udløbsdato = date.ToString("yyyy'-'MM'-'dd");
            var httpResult = await HttpPostReserveComputer(com, udlåningsdato, udløbsdato);
            var rentedComputer = new RentedComputer
            {
                id = com.id,
                rentId = httpResult.id,
                register = com.register,
                computer = com.computer,
                mus = com.mus,
                udlåningsdato = udlåningsdato,
                udløbsdato = udløbsdato,
                model = com.model,
                status = "Reserveret"
            };
            rentedComputer.color = (rentedComputer.status == "Reserveret") ? Color.FromRgb(255, 255, 0) : Color.FromRgb(255, 0, 0);
            if (httpResult.success)
            {
                FreeComputers.Remove(com);
                RentedComputers.Add(rentedComputer);
            }
        }
    }
    private async Task<bool> HttpDeleteReserveComputer(int id)
    {
        try
        {
            using HttpResponseMessage response = await client.DeleteAsync("https://gorilla-informed-asp.ngrok-free.app/Udlån/DeleteUdlaan/" + id);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            await DisplayAlert("Succes", "Computeren er ikke længere reserveret", "OK");
            return true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Alert", ex.Message, "OK");
            return false;
        }
    }
    private async Task<HttpPostMessage> HttpPostReserveComputer(FreeComputer computer, string udlåningsdato, string udløbsdato)
    {
        try
        {
            string json = "{\"elevNummer\": \"" + bruger.elevNummer + "\", \"registreringsNummer\": \"" + computer.register + "\", \"udlånsdato\": \"" + udlåningsdato + "\",\"udløbsdato\": \"" + udløbsdato + "\"}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PostAsync("https://gorilla-informed-asp.ngrok-free.app/Udlån/RentComputer", content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            HttpPostMessage message = JsonConvert.DeserializeObject<HttpPostMessage>(responseBody);
            message.success = true;
            await DisplayAlert("Succes", "Computeren er nu reserveret", "OK");
            return message;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Alert", ex.Message, "OK");
            HttpPostMessage message = new HttpPostMessage() { success = false };
            return message;
        }
    }

    private class HttpPostMessage
    {
        public bool success { get; set; }
        [JsonProperty("udlånID")]
        public int id { get; set; }
    }
}