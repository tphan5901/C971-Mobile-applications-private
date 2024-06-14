using C971.Entity;
using C971.SQLitefunctions;
using Microsoft.Maui.Controls.Compatibility;
using Plugin.LocalNotification;
namespace C971;

public partial class courseInfo : ContentPage
{
    public readonly databaseFunctions gate;
    public static List<Exam> Exam = new List<Exam>();
    public static List<Notes> Notes = new List<Notes>();
    public Course currentCourse;
    public static courseInfo Current;

    public courseInfo(Course course)
    {
        InitializeComponent();
        gate = App.Database;
        currentCourse = course;
        Current = this;
        BindingContext = currentCourse;
        LoadSavedData();
    }

    private void notifications(Exam exam)
    {
        if (exam.Notifications == "Yes")
        {
            var startNotification = new NotificationRequest
            {
                NotificationId = 200,
                Title = "Exam reminder",
                Description = $"Your Exam: {exam.Type} is starting on {exam.StartDate}",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now,
                    //    NotifyTime = exam.StartDate.AddDays(-5),
                }
            };
            var endNotification = new NotificationRequest
            {
                NotificationId = 300,
                Title = "Exam end reminder",
                Description = $"Your course {exam.Type} is starting on {exam.EndDate}",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now,
                    //      NotifyTime = exam.EndDate.AddDays(-5),
                }
            };

            LocalNotificationCenter.Current.Show(startNotification);
            LocalNotificationCenter.Current.Show(endNotification);
        }
    }

    public async Task LoadSavedData()
    {
            var getExam = await gate.getExam(currentCourse.Id);
            Exam.Clear();
            foreach (var exam in getExam)
            {
                Exam.Add(exam);
                notifications(exam);
            }

            var getNotes = await gate.getNotes(currentCourse.Id);
            Notes.Clear();
            foreach (var notes in getNotes)
            {
                Notes.Add(notes);
            }

        UIcomponent(); 
    }

    public void UIcomponent()
    {
        notesStack.Children.Clear();

        foreach (var note in Notes)
        {
            var grid = new Microsoft.Maui.Controls.Grid
            {
                BackgroundColor = Colors.Black,
            };

            Button notesButton = new Button
            {
                BackgroundColor = Colors.LemonChiffon,
                TextColor = Colors.Black,
                Text = note.Title,
            };

            notesButton.Clicked += (sender, args) => Navigation.PushAsync(new notesInfo(note));

            SwipeItem editItem = new SwipeItem
            {
                Text = "Edit",
                BackgroundColor = Colors.LightSeaGreen,
                BindingContext = note
            };

            editItem.Invoked += editNote;

            SwipeItem deleteItem = new SwipeItem
            {
                Text = "Delete",
                BackgroundColor = Colors.LightSalmon,
                BindingContext = note
            };

            deleteItem.Invoked += deleteNote;

            List<SwipeItem> items = new List<SwipeItem> { deleteItem, editItem };

            SwipeView swipeView = new SwipeView
            {
                RightItems = new SwipeItems(items),
                Content = grid
            };

            grid.Add(notesButton);
            notesStack.Children.Add(swipeView);
        }

        //exams
        examStack.Children.Clear();

            foreach (var exam in Exam)
            {
                var grid = new Microsoft.Maui.Controls.Grid
                {
                    BackgroundColor = Colors.Black,
                };

                Button button = new Button
                {
                    BackgroundColor = Colors.DarkOrange,
                    Text = exam.Type,
                };

                button.Clicked += (sender, args) => Navigation.PushAsync(new examInfo(exam));

                SwipeItem editItem = new SwipeItem
                {
                    Text = "Edit",
                    BackgroundColor = Colors.LightSeaGreen,
                    BindingContext = exam
                };

                editItem.Invoked += OnEdit;

                SwipeItem deleteItem = new SwipeItem
                {
                    Text = "Delete",
                    BackgroundColor = Colors.LightSalmon,
                    BindingContext = exam
                };

                deleteItem.Invoked += OnDelete;

                List<SwipeItem> items = new List<SwipeItem> { deleteItem, editItem };

                SwipeView swipeView = new SwipeView
                {
                    RightItems = new SwipeItems(items),
                    Content = grid
                };

                grid.Add(button);
                examStack.Children.Add(swipeView);
            }
        }

    //edit/delete notes
        private void editNote(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var note = item.BindingContext as Notes;
            Navigation.PushAsync(new editNotes(note));
        }

        private async void deleteNote(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var note = item.BindingContext as Notes;
            await gate.deleteNotes(note);
            Notes.Remove(note);
            await LoadSavedData();
        }

    //exams edit/delete
    private void OnEdit(object sender, EventArgs e)
    {
        var item = sender as SwipeItem;
        var exam = item.BindingContext as Exam;
        Navigation.PushAsync(new editExam(exam));
    }

    private async void OnDelete(object sender, EventArgs e)
    {
        var item = sender as SwipeItem;
        var exam = item.BindingContext as Exam;
        await gate.deleteExam(exam);
        Exam.Remove(exam);
        await LoadSavedData();
    }

    private async void addOA(object sender, EventArgs e)
    {
        if (Exam.Count < 2)
        {
            await Navigation.PushAsync(new addExam(currentCourse));
        }
        else
        {
            await DisplayAlert("Limit reached", "You can only add up to 2 exams.", "OK");
            return;
        }
    }

    private async void addNotes(object sender, EventArgs e)
    {
       await Navigation.PushAsync(new addNotes(currentCourse));    
    }

    private async void addPA(object sender, EventArgs e)
    {
        if (Exam.Count < 2)
        {
            await Navigation.PushAsync(new addPA(currentCourse));
        }
        else
        {
            await DisplayAlert("Limit reached", "You can only add up to 2 exams.", "OK");
            return;
        }
    }

/*
    private async void shareNotes(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(currentCourse.Notes))
        {
           
            await Share.Default.RequestAsync(new ShareTextRequest
            {
                Title = "Share Notes",
                Text = currentCourse.Notes
            });
        }
        else 
        {
            await DisplayAlert("No Notes", "There are no notes to share.", "OK");
        }
    }
*/

}