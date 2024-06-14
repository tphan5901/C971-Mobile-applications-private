using C971.Entity;
using C971.SQLitefunctions;
namespace C971;

public partial class addExam : ContentPage
{
	public static Course currentCourse;
	public readonly databaseFunctions gate;
	public addExam(Course course)
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
            if (string.IsNullOrEmpty(type.Text) || string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(examStart.Date.ToString()) || string.IsNullOrEmpty(examEnd.Date.ToString()))
            {
                await DisplayAlert("Invalid", "One of the fields is empty", "ok");
                return;
            }
            string yesNo = "";
            if (notify.IsChecked == true)
            {
                yesNo = "Yes";
            }
            else
            {
                yesNo = "No";
            }
          
            Exam newExam = new Exam
            {
                CourseId = currentCourse.Id,
                Type = type.Text = "OA",
                Name = name.Text,
                Notifications = yesNo,
                StartDate = examStart.Date,
                EndDate = examEnd.Date,

            };

            int success = await gate.addExam(newExam);
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