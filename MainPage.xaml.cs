using C971.Entity;
using C971.SQLitefunctions;
using Microsoft.Maui.Controls.Compatibility;
using SQLite;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.Maui.Storage;

namespace C971
{
    //when running this application, if you tap on any button multiple times, you will create multiple instances of that particular UI component. there are many problems with the UI. only contained with this framework
    public partial class MainPage : ContentPage
    {
        public static databaseFunctions gate;

        public static List<Term> Terms = new List<Term>();
        public static MainPage Current { get;  set; }

        private static bool isDummyDataInitialized = false;
        public MainPage()
        {
            InitializeComponent();
            gate = App.Database; 
            Current = this;
            
            BindingContext = Terms;

            bool isDummyDataInitialized = Preferences.Get("IsDummyDataInitialized", false);
            if (!isDummyDataInitialized)
            {
                _ = dummyData();
                isDummyDataInitialized = true;
            }
            LoadSavedData();
        }

        public void clearLocalStorage()
        {
            Preferences.Remove("IsDummyDataInitialized");
        }
        private async Task dummyData()
        {
            try
            {
                var term = new Term
                {
                    Title = "Winter",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                };
                await gate.addTerm(term);

         //add course
                var insertedTerm = await gate.getTermByTitle(term.Title);

                /*     
                    var course = new Course
                    {
                        TermId = insertedTerm.TermId,
                        Name = "ML",
                        Instructor = "Anika Patel",
                        Email = "anika.patel@strimeuniversity.edu",
                        Phone = "555 - 123 - 4567",
                    //     Notes = "Dummy notes",
                        Status = "In progress",
                        Notifications = "Yes",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now
                    };
                    await gate.addCourse(course);


                    var course2 = new Course
                    {
                        TermId = insertedTerm.TermId,
                        Name = "AI",
                        Instructor = "Anika Patel",
                        Email = "anika.patel@strimeuniversity.edu",
                        Phone = "555 - 123 - 4567",
                        //     Notes = "Dummy notes",
                        Status = "In progress",
                        Notifications = "Yes",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now
                    };
                    await gate.addCourse(course2);

                    var course3 = new Course
                    {
                        TermId = insertedTerm.TermId,
                        Name = "Phy",
                        Instructor = "Anika Patel",
                        Email = "anika.patel@strimeuniversity.edu",
                        Phone = "555 - 123 - 4567",
                        //     Notes = "Dummy notes",
                        Status = "In progress",
                        Notifications = "Yes",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now
                    };
                    await gate.addCourse(course3);

                    var course4 = new Course
                    {
                        TermId = insertedTerm.TermId,
                        Name = "ML",
                        Instructor = "Anika Patel",
                        Email = "anika.patel@strimeuniversity.edu",
                        Phone = "555 - 123 - 4567",
                        //     Notes = "Dummy notes",
                        Status = "In progress",
                        Notifications = "Yes",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now
                    };
                    await gate.addCourse(course4);

                    var course5 = new Course
                    {
                        TermId = insertedTerm.TermId,
                        Name = "ML",
                        Instructor = "Anika Patel",
                        Email = "anika.patel@strimeuniversity.edu",
                        Phone = "555 - 123 - 4567",
                        //     Notes = "Dummy notes",
                        Status = "In progress",
                        Notifications = "Yes",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now
                    };
                    await gate.addCourse(course5);


                    //add exam
                    //fetch course by instructor. multiple course associated w/ other terms can have same name
                    var insertedCourse = await gate.getCourseByName(course.Instructor);

                    var exam = new Exam
                    {
                        CourseId = insertedCourse.Id,
                        Name = "OA",
                        Type = "OA",
                        Notifications = "Yes",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now
                    };
                    await gate.addExam(exam);

                    //add exam2

                    var exam2 = new Exam
                    {
                        CourseId = insertedCourse.Id,
                        Name = "PA",
                        Type = "PA",
                        Notifications = "Yes",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now
                    };
                    await gate.addExam(exam2);


                    var notes = new Notes
                    {
                        CourseId = insertedCourse.Id,
                        Title = "a",
                        Text = "a",
                 
                    };
                    await gate.addNotes(notes);
                */

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error:", ex.Message, "OK");
            }
        }

        public async Task LoadSavedData()
        {
            var getTerms = await gate.getTerms();
            Terms.Clear();
            foreach (var term in getTerms)
            {
                Terms.Add(term);
      
            }
            LoadUI();
        }

        //binding gui components bug
        private void LoadUI()
        {
            termStack.Children.Clear();  

            foreach (var term in Terms)
            {
                
                var grid = new Microsoft.Maui.Controls.Grid
                {
                    BackgroundColor = Colors.Orange,
                 
                };

                Button button = new Button
                {
                    Text = term.Title, 
                    BackgroundColor = Colors.LightSeaGreen,
                 
                };
                button.Clicked += (sender, args) => Navigation.PushAsync(new termInfo(term));

                SwipeItem editItem = new SwipeItem
                {
                    Text = "Edit",
                    BackgroundColor = Colors.DarkOrange,
                    BindingContext = term
                };
                editItem.Invoked += OnEdit;

                SwipeItem deleteItem = new SwipeItem
                { 
                    Text = "Delete",
                    BackgroundColor = Colors.DeepPink,
                    BindingContext = term
                };
                deleteItem.Invoked += OnDelete;

                List<SwipeItem> items = new List<SwipeItem> { deleteItem, editItem };

                SwipeView swipeView = new SwipeView
                {
                    RightItems = new SwipeItems(items),
                    Content = grid
                };

                grid.Add(button);
                termStack.Children.Add(swipeView);
            }
        }

        private void OnEdit(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var term = item.BindingContext as Term;
            Navigation.PushAsync(new editTerm(term));
        }

        private void OnDelete(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var term = item.BindingContext as Term;
        
        //  gate.deleteTerm(term);
            gate.cascadeDelete(term);
            Terms.Remove(term);
            LoadSavedData();
        }

        private async void addTerm(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new addTerm());
        }
      
    }
}
