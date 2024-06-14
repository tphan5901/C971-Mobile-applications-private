using C971.Entity;
using C971.SQLitefunctions;

namespace C971;

public partial class addCourses : ContentPage
{
    public static Term currentTerm;

    public readonly databaseFunctions gate;

    public static string Email;

    public static string Phone;
    public addCourses(Term term)
	{
		InitializeComponent();
        gate = App.Database;
        currentTerm = term;
        var pickerItems = new List<string>
            { 
                "Completed",
                "Not Completed",
                "In progress",

            };
        picker.ItemsSource = pickerItems;
    }

    private async void ExitButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    private async void SaveButton_Clicked(object sender, EventArgs e)
    {

        if (string.IsNullOrEmpty(classname.Text) || string.IsNullOrEmpty(name.Text) || picker.SelectedItem == null || string.IsNullOrEmpty(email.Text) || string.IsNullOrEmpty(phone.Text) || string.IsNullOrEmpty(classStart.Date.ToString()) || string.IsNullOrEmpty(classEnd.Date.ToString()))
        {
            await DisplayAlert("Invalid", "One of the fields is empty", "ok");
            return;
        }
        else
        {
            string yesNo = "";
            if (notify.IsChecked == true)
            {
                yesNo = "Yes";
            }
            else
            {
                yesNo = "No";
            }

            Email = email.Text;
            bool containsNotAllowedChars = await containsCharacters(Email);
            if (containsNotAllowedChars)
            {
                await DisplayAlert("Message", "Email should not contain the following: %#!$^&*()-+{}:;'\",/?", "ok");
                return;
            }

            Phone = phone.Text;
            bool isValidPhone = validatePhone(Phone);
            if (!isValidPhone)
            {
                await DisplayAlert("Message", "Phone number should only contain digits and dashes (-)", "OK");
                return;
            }

            Course newClass = new Course
            {
                TermId = currentTerm.TermId,
                Name = classname.Text,
                Instructor = name.Text,
                Email = Email,
                Phone = Phone,
            //    Notes = notes.Text,
                Status = picker.SelectedItem.ToString(),
                Notifications = yesNo,
                StartDate = classStart.Date,
                EndDate = classEnd.Date,

            };

            int success = await gate.addCourse(newClass);
            if (success > 0)
            {
                await DisplayAlert("Success", "Upload successful", "ok");
                termInfo.Current.LoadSavedData();
                await Navigation.PopAsync();
            }
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
            string notAllowedChar = "%#!$^&*()-+{}:;'\",/?";

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


}

