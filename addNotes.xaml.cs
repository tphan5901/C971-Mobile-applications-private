using C971.Entity;
using C971.SQLitefunctions;
namespace C971;

public partial class addNotes : ContentPage
{
    public static Course currentCourse;
    public readonly databaseFunctions gate;
    public addNotes(Course course)
	{
		InitializeComponent();
        currentCourse = course;
        gate = App.Database;
	}

    private async void ExitButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(notes.Text) || string.IsNullOrEmpty(title.Text))
            {
                await DisplayAlert("Invalid", "One of the fields is empty", "ok");
                return;
            }
          

            Notes newNotes = new Notes
            {
                CourseId = currentCourse.Id,
                Title = title.Text,
                Text = notes.Text,

            };

            int success = await gate.addNotes(newNotes);
            if (success > 0)
            {
                await DisplayAlert("Success", "Upload successful", "ok");
                courseInfo.Current.LoadSavedData();
                await Navigation.PopAsync();
            }
        }
        catch (Exception ex)
        {

            await DisplayAlert("Message", "Upload failed", "ok", ex.Message);
        }
    }

}