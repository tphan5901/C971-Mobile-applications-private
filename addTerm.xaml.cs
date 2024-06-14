using C971.Entity;
using C971.SQLitefunctions;
namespace C971;

public partial class addTerm : ContentPage
{

    private readonly databaseFunctions gate;
    public addTerm()
	{
		InitializeComponent();

        gate = App.Database;
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(startDate.Date.ToString()) || string.IsNullOrEmpty(endDate.Date.ToString()))
            {
                await DisplayAlert("Message", "One of the fields is empty", "ok");
            }
            else
            {
                var term = new Term
                {
                    Title = name.Text,
                    StartDate = startDate.Date,
                    EndDate = endDate.Date
                };
                int success = await gate.addTerm(term);
                if (success > 0)
                {
                    await DisplayAlert("Success", "Upload successful", "ok");
                    MainPage.Current.LoadSavedData();
                    await Navigation.PopAsync();
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("failed", "Upload failed", "ok", ex.Message);
        }

     }

        private async void ExitButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

}

