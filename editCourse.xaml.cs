using C971.Entity;
using C971.SQLitefunctions;
using System.Security.Cryptography.X509Certificates;
namespace C971;

public partial class editCourse : ContentPage
{
	public readonly databaseFunctions gate;

	public static Course currentCourse;

    public static string yesNo;

    public static string Phone;
	public editCourse(Course course)
	{
		InitializeComponent();
		gate = App.Database;
		currentCourse = course;
        BindingContext = currentCourse;
        yesNo = currentCourse.Notifications;
        if (yesNo == "Yes")
        {
            notify.IsChecked = true;
        }
        else
        {
            notify.IsChecked = false;
        }
        List<string> pickerItems = new List<string> { currentCourse.Status };
        populatePicker(pickerItems, currentCourse.Status);

        picker.ItemsSource = pickerItems;
        picker.SelectedItem = currentCourse.Status;
    }

    public static void populatePicker(List<string> pickerItems, string currentStatus)
    {
        var predefinedStatuses = new List<string>
        {
            "Completed",
            "Not Completed",
            "In progress"
        };

        if (predefinedStatuses.Contains(currentStatus))
        {
            pickerItems.AddRange(predefinedStatuses.Where(status => status != currentStatus));
        }
        else
        {
            pickerItems.AddRange(predefinedStatuses);
        }
    }

    public async void SaveButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(instructor.Text) || picker.SelectedItem == null || string.IsNullOrEmpty(email.Text) || string.IsNullOrEmpty(phone.Text) || string.IsNullOrEmpty(startDate.Date.ToString()) || string.IsNullOrEmpty(endDate.Date.ToString()))
            {
                await DisplayAlert("Invalid", "One of the fields is empty", "ok");
                return;
            }
            string noYes = "";
            if (notify.IsChecked == true)
            {
                noYes = "Yes";
            }
            else
            {
                noYes = "No";
            }
            string Email = email.Text;

            bool containsNotAllowedChars = await containsCharacters(Email);
            if (containsNotAllowedChars)
            {
                await DisplayAlert("Message", "Email should not contain the following: %#$^&*()-+{}:;'\"/? ", "ok");
                return;
            }

            Phone = phone.Text;
            bool isValidPhone = validatePhone(Phone);
            if (!isValidPhone)
            {
                await DisplayAlert("Message", "Phone number should contain digits and dashes (-)", "OK");
                return;
            }

            Course newCourse = new Course
                {
                    Id = currentCourse.Id,
                    TermId = currentCourse.TermId,
                    Name = name.Text,
                    StartDate = startDate.Date,
                    EndDate = endDate.Date,
                    Status = picker.SelectedItem.ToString(),
               //     Notes = note.Text,
                    Instructor = instructor.Text,
                    Phone = Phone,
                    Email = email.Text,
                    Notifications = noYes,
                };

                await gate.updateCourse(newCourse);
                termInfo.Current.LoadSavedData();
                await DisplayAlert("Message", "Updated", "ok");
                await Navigation.PopAsync();
        }

        catch (Exception ex)
        {
            await DisplayAlert("Message", "Update failed", "ok", ex.Message);
        }
    }

    private bool validatePhone(string input)
    {
        foreach (char c in input)
        {
            if (!char.IsDigit(c) && c != '-' && c != ' ')
            {
                return false;
            }
        }
        return true;
    }

    public static async Task<bool> containsCharacters(string input)
    {
        return await Task.Run(() =>
        {
            string notAllowedChar = "%#$^&*()-+{}:;'\"/?";

            foreach (char c in input)
            {
                if (notAllowedChar.Contains(c))
                {
                    Console.WriteLine("Email should not contain the following: %#!$^&*()-+{}:;'\",/?");
                    return true;
                }
            }
            return false;
        });
    }

    private async void ExitButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }


}