using ComputerRenting.Model;

namespace ComputerRenting.View;

public partial class ProfilPage : ContentPage
{
    public Bruger bruger { get; set; }
    public ProfilPage(Bruger user)
    {
        bruger = user;
        InitializeComponent();
        NavnLabel.Text = bruger.navn;
        AdresseLabel.Text = bruger.adresse + " " + bruger.postNrByNavn + " " + bruger.postNrID;
        TelefonLabel.Text = "87654321";
        EmailLabel.Text = bruger.email;
        ElevNummerLabel.Text = bruger.elevNummer;
        KlasseLabel.Text = bruger.stamKlasseNavn;
    }
}