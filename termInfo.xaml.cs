using C971.Entity;
using C971.SQLitefunctions;
using Plugin.LocalNotification;
using Microsoft.Maui.Controls.Compatibility;
namespace C971;

public partial class termInfo : ContentPage
{
    public readonly databaseFunctions gate;

    public static List<Course> Courses = new List<Course>();

    public Term currentTerm;

    public static termInfo Current;
    public termInfo(Term term)
	{
		InitializeComponent();
        gate = App.Database;
        Current = this;
        currentTerm = term;
        BindingContext = currentTerm;
        LoadSavedData();
    }


    private void notifications(Course course)
    {
        if (course.Notifications == "Yes")
        {

            var startNotification = new NotificationRequest
            {
                NotificationId = 400,
                Title = "Course Start Reminder",
                Description = $"Your course {course.Name} is starting on {course.StartDate}",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now,
                    //     NotifyTime = course.StartDate.AddDays(-7),
                }
            };
            var endNotification = new NotificationRequest
            {
                NotificationId = 500,
                Title = "Course end Reminder",
                Description = $"Your course {course.Name} is starting on {course.EndDate}",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now,
                    //   NotifyTime = course.EndDate.AddDays(-7),
                }
            };

            LocalNotificationCenter.Current.Show(startNotification);
            LocalNotificationCenter.Current.Show(endNotification);
        }
    }

    public async Task LoadSavedData()
    {
        var getCourses = await gate.getCourses(currentTerm.TermId);
        Courses.Clear();
        foreach (var course in getCourses)
        {
            Courses.Add(course);
            if (course.Notifications == "Yes")
            {
                notifications(course);
            }
        }
        LoadUI(); 
    }

    public void LoadUI()
    {
        courseStack.Children.Clear();

        foreach (var course in Courses)
        {

            var grid = new Microsoft.Maui.Controls.Grid
            {
                BackgroundColor = Colors.Orange,

            };

            Button button = new Button
            {
                 Text = course.Name,
                 BackgroundColor = Colors.DarkOrange
            };
            button.Clicked += (sender, args) => Navigation.PushAsync(new courseInfo(course));


            SwipeItem deleteItem = new SwipeItem
            {
                Text = "Delete",
                BackgroundColor = Colors.Salmon,
                BindingContext = course
            };
            deleteItem.Invoked += OnDelete;

            SwipeItem editItem = new SwipeItem
            {
                Text = "Edit",
                BackgroundColor = Colors.LightSeaGreen,
                BindingContext = course
            };
            editItem.Invoked += OnEdit;

            List<SwipeItem> items = new List<SwipeItem> { deleteItem, editItem };

            SwipeView swipeView = new SwipeView
            {
                RightItems = new SwipeItems(items),
                Content = grid
            };

            grid.Add(button);

            courseStack.Children.Add(swipeView);
        }
    }

    private void OnEdit(object sender, EventArgs e)
    {
        var item = sender as SwipeItem;
        var course = item.BindingContext as Course;
        Navigation.PushAsync(new editCourse(course));
    }

    private void OnDelete(object sender, EventArgs e)
    {
        var item = sender as SwipeItem;
        var course = item.BindingContext as Course;

        gate.deleteCourse(course);
        Courses.Remove(course);
        LoadSavedData();
    }

    private async void addCourses(object sender, EventArgs e)
    {
        if (Courses.Count < 6)
        {
            await Navigation.PushAsync(new addCourses(currentTerm));
        }
        else
        {
            await DisplayAlert("Limit reached", "You can only add up to 6 courses.", "OK");
            return;
        }

    }


}