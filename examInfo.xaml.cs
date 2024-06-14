namespace C971;
using C971.Entity;
using C971.SQLitefunctions;
using Microsoft.Maui.Controls.Compatibility;

public partial class examInfo : ContentPage
{
    public static Exam currentExam;

    public readonly databaseFunctions gate;

    public examInfo Current;

    public static string yesNo;
    public examInfo(Exam exam)
	{
		InitializeComponent();
        gate = App.Database;
        Current = this;
        currentExam = exam;
        BindingContext = currentExam;
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

}