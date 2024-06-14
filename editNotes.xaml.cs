using C971.Entity;
using C971.SQLitefunctions;

namespace C971;

public partial class editNotes : ContentPage
{
    public static Notes currentNotes;

    public readonly databaseFunctions gate;
    public editNotes(Notes note)
	{
		InitializeComponent();
        gate = App.Database;
        currentNotes = note;
        BindingContext = currentNotes;
    }

    private async void ExitButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(note.Text) || string.IsNullOrEmpty(title.Text) )
            {
                await DisplayAlert("Invalid", "One of the fields is empty", "ok");
                return;
            }
         
            Notes newNotes = new Notes
            {
                Id = currentNotes.Id,
                CourseId = currentNotes.CourseId,
                Title = title.Text,
                Text = note.Text,
             
            };

            await gate.updateNotes(newNotes);
            await courseInfo.Current.LoadSavedData();
            await DisplayAlert("Success", "Update successful", "ok");        
            await Navigation.PopAsync();
            
        }
        catch (Exception ex)
        {

            await DisplayAlert("Message", "Upload failed", "ok", ex.Message);
        }
    }


}