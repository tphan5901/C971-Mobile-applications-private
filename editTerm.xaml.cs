using C971.Entity;
using C971.SQLitefunctions;
namespace C971;

public partial class editTerm : ContentPage
{
    public readonly databaseFunctions gate;
    public Term currentTerm;
    public editTerm(Term term)
    {
        InitializeComponent();
        gate = App.Database;
        currentTerm = term;
        BindingContext = currentTerm;
    }

    public async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(startDate.Date.ToString()) || string.IsNullOrEmpty(endDate.Date.ToString()))
        {
            await DisplayAlert("Message", "One of the fields is empty", "ok");
        }
        else
        {
            Term updateTerm = new Term
            {
                TermId = currentTerm.TermId,
                Title = name.Text,
                StartDate = startDate.Date,
                EndDate = endDate.Date
            };
            await gate.updateTerm(updateTerm);
            await Navigation.PopAsync();
            await MainPage.Current.LoadSavedData();
        }
    }
    private async void ExitButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

}