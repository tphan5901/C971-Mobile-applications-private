using C971.Entity;
using C971.SQLitefunctions;
namespace C971;

public partial class notesInfo : ContentPage
{
    public static Notes currentNotes;

    public readonly databaseFunctions gate;

    public notesInfo Current;
    public notesInfo(Notes note)
	{
		InitializeComponent();
        gate = App.Database;
        Current = this;
        currentNotes = note;
        BindingContext = currentNotes;
    }

    private async void shareNotes(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(currentNotes.Text))
        {
            await Share.Default.RequestAsync(new ShareTextRequest
            {       
                Title = currentNotes.Title,
                Text = currentNotes.Text,
            });
        }
        else
        {
            await DisplayAlert("No Notes", "There are no notes to share.", "OK");
        }
    }

}