using C971.SQLitefunctions;
namespace C971
{
    public partial class App : Application
    {
        public static databaseFunctions Database { get; set; }
        public App()
        {
            InitializeComponent();
            try
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Courseplan33r.db3");
     
                Database = new databaseFunctions(dbPath);

                removeSQLiteDatabase();
                MainPage = new AppShell();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization failed: {ex.Message}");
              
            }
        }

        public static void removeSQLiteDatabase()
        {     
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Courseplan.db3");
            try
            {
                if (File.Exists(databasePath))
                {
                    File.Delete(databasePath);
                    Console.WriteLine("SQLite database deleted successfully.");
                }
                else
                {
                    Console.WriteLine("SQLite database not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting SQLite database file: {ex.Message}");
            }
        }


    }
}
