using C971.Entity;
using C971.SQLitefunctions;
namespace C971;

public partial class editExam : ContentPage
{
    public static Exam currentExam;

    public readonly databaseFunctions gate;

    public static string yesNo;
    public editExam(Exam exam)
    {
        InitializeComponent();
        gate = App.Database;
        currentExam = exam;
        BindingContext = currentExam; //binds UI to data obj to access properties



        yesNo = currentExam.Notifications;
        if (yesNo == "Yes")
        {
            notify.IsChecked = true;
        }
        else
        {
            notify.IsChecked = false;
        }
    }

    public async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(type.Text) || string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(startDate.Date.ToString()) || string.IsNullOrEmpty(endDate.Date.ToString()))
        {
            await DisplayAlert("Invalid", "One of the fields is empty", "ok");
            return;
        }
        else
        {
            string noYes = "";
            if (notify.IsChecked == true)
            {
                noYes = "Yes";
            }
            else
            {
                noYes = "No";
            }
            Exam updateTerm = new Exam
            {
                Id = currentExam.Id,
                CourseId = currentExam.CourseId,
                Type = type.Text,
                Name = name.Text,
                StartDate = startDate.Date,
                EndDate = endDate.Date,
                Notifications = noYes,
            };

            await gate.updateExam(updateTerm);
            await courseInfo.Current.LoadSavedData();
            await Navigation.PopAsync();
        }
    }
    private async void ExitButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

}