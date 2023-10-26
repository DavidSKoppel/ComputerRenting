using CommunityToolkit.Maui.Views;
using ComputerRenting.Model;

namespace ComputerRenting.View;

public partial class RentComputerPopUp : Popup
{
	public RentComputerPopUp(FreeComputer free)
	{
		InitializeComponent();
        ComputerNavnLabel.Text = free.computer + " " + free.model;
    }
    void OnYesButtonClicked(object? sender, EventArgs e) 
	{
		var rent = lendingDatePicker.Date;
        Close(rent);
    }
    void OnNoButtonClicked(object? sender, EventArgs e) => Close(false);
}