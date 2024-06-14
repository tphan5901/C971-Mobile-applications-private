using C971.Entity;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace C971.SQLitefunctions
{
    public class databaseFunctions
    {
        private readonly SQLiteAsyncConnection _connection;

        public databaseFunctions(string path)
        {
            _connection = new SQLiteAsyncConnection(path);
            _connection.CreateTableAsync<Term>();
            _connection.CreateTableAsync<Course>();
            _connection.CreateTableAsync<Exam>();
            _connection.CreateTableAsync<Notes>();
        }

        public async Task cascadeDelete(Term term)
        {
            var courses = await getCourses(term.TermId);

            foreach (var course in courses)
            {
                var exams = await getExam(course.Id);
                foreach (var exam in exams)
                {
                    await deleteExam(exam);
                }

                var notes = await getNotes(course.Id);
                foreach (var note in notes)
                {
                    await deleteNotes(note);
                }
                await deleteCourse(course);
            }
            await deleteTerm(term);
        }

        public Task<List<Term>> getTerms()
        {
            return _connection.Table<Term>().ToListAsync();
        }

        public Task<int> addTerm(Term term)
        {
            return _connection.InsertAsync(term);
        }

        public Task<Term> getTermByTitle(string title)
        {
            return _connection.Table<Term>().FirstOrDefaultAsync(t => t.Title == title);
        }

        public Task<int> addCourse(Course course)
        {
            return _connection.InsertAsync(course);
        }

        public Task<Course> getCourseByName(string instructor)
        {
            return _connection.Table<Course>().FirstOrDefaultAsync(c => c.Instructor == instructor);
        }

        public Task<int> addExam(Exam exam)
        {
            return _connection.InsertAsync(exam);
        }

        public Task<List<Exam>> getExam(int courseId)
        {
            return _connection.Table<Exam>().Where(e => e.CourseId == courseId).ToListAsync();
        }

        public Task<int> addNotes(Notes note)
        {
            return _connection.InsertAsync(note);
        }

        public Task<List<Notes>> getNotes(int courseId)
        {
            return _connection.Table<Notes>().Where(e => e.CourseId == courseId).ToListAsync();
        }

        public Task updateNotes(Notes note)
        {
            return _connection.UpdateAsync(note);
        }
        public Task deleteNotes(Notes note)
        {
            return _connection.DeleteAsync(note);
        }
        public Task updateTerm(Term term)
        {
            return _connection.UpdateAsync(term);
        }

        public Task deleteTerm(Term term)
        {
            return _connection.DeleteAsync(term);
        }

        public Task<List<Course>> getCourses(int termId)
        {
            return _connection.Table<Course>().Where(c => c.TermId == termId).ToListAsync();
        }

        public Task updateCourse(Course course)
        {
            return _connection.UpdateAsync(course);
        }

        public Task deleteCourse(Course course)
        {
            return _connection.DeleteAsync(course);
        }

        public Task updateExam(Exam exam)
        {
            return _connection.UpdateAsync(exam);
        }
        public Task deleteExam(Exam exam)
        {
            return _connection.DeleteAsync(exam);
        }

    }
}
