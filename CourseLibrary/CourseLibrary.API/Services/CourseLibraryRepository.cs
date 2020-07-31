using CourseLibrary.API.DbContexts;
using CourseLibrary.API.Entities;
using CourseLibrary.API.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseLibrary.API.Services
{
    public class CourseLibraryRepository : ICourseLibraryRepository, IDisposable
    {
        private readonly CourseLibraryContext _context;

        public CourseLibraryRepository(CourseLibraryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddCourse(Guid authorId, Course course)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            // always set the AuthorId to the passed-in authorId
            course.AuthorId = authorId;
            _context.Courses.Add(course);
        }

        public void DeleteCourse(Course course)
        {
            _context.Courses.Remove(course);
        }

        public Course GetCourse(Guid authorId, Guid courseId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            if (courseId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(courseId));
            }

            return _context.Courses
              .Where(c => c.AuthorId == authorId && c.Id == courseId).FirstOrDefault();
        }

        public IEnumerable<Course> GetCourses(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Courses
                        .Where(c => c.AuthorId == authorId)
                        .OrderBy(c => c.Title).ToList();
        }

        public void UpdateCourse(Course course)
        {
            // no code in this implementation
        }

        public void AddAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            // the repository fills the id (instead of using identity columns)
            author.Id = Guid.NewGuid();

            foreach (var course in author.Courses)
            {
                course.Id = Guid.NewGuid();
            }

            _context.Authors.Add(author);
        }

        public bool AuthorExists(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Authors.Any(a => a.Id == authorId);
        }

        public void DeleteAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            _context.Authors.Remove(author);
        }

        public Author GetAuthor(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Authors.FirstOrDefault(a => a.Id == authorId);
        }

        public IEnumerable<Author> GetAuthors()
        {
            return _context.Authors.ToList<Author>();
        }

        public IEnumerable<Author> GetAuthors(AuthorsResourceParameter authorsResourceParameter)
        {
            if (string.IsNullOrWhiteSpace(authorsResourceParameter.MainCategory)
                && string.IsNullOrWhiteSpace(authorsResourceParameter.SearchQuery))
            {
                return GetAuthors();
            }

            //mainCategory = mainCategory.Trim();
            //return _context.Authors.Where(a => a.MainCategory == mainCategory).ToList();

            /* It could be that we only want to apply the filter, only want to apply the 
             * search query or both. We want to cover all of these cases, so the first thing
             * we will do is cast the Author DbSet to an IQueryable of author. The Author DbSet
             * is Authors on the context. By doing this, we can work on this collection, adding
             * where clauses for filtering when needed and for searching. There is a good reason
             * to write code like this and it has something to do with deferred execution.
             * 
             * DEFERRED EXECUTION
             * Whe working with Entity-Framework Core, we use Linq to build our queries. With deferred
             * execution, the query variable itself never holds the query results and only stores
             * the query commands. Execution of the query is deferred until the query variable is
             * iterated over. So, deferred execution means that query execution occurs sometime after
             * the query is constructed. We can get this behavior by working with IQueryable 
             * implementing collections. A query variable stores query commands and not results.
             * IQueryable of T allows us to execute the query against the specific datasource,
             * and while building upon it, it creates an expression tree. But, the query itself
             * is not sent to the datasource until the iteration happens and the iteration can happen
             * in different ways: (1) using the IQueryable in a loop, (2) calling ToList(), ToArray(),
             * ToDictionary() because this means that converting the expression tree to an actual
             * list of items, (3) calling Singleton queries like Average, Count, First,
             */
            var collection = _context.Authors as IQueryable<Author>;

            if (!string.IsNullOrWhiteSpace(authorsResourceParameter.MainCategory))
            {
                authorsResourceParameter.MainCategory = authorsResourceParameter.MainCategory.Trim();
                collection = collection.Where(a => a.MainCategory == authorsResourceParameter.MainCategory);
            }

            if (!string.IsNullOrWhiteSpace(authorsResourceParameter.SearchQuery))
            {
                authorsResourceParameter.SearchQuery = authorsResourceParameter.SearchQuery.Trim();
                collection = collection.Where(a => a.MainCategory.Contains(authorsResourceParameter.SearchQuery)
                    || a.LastName.Contains(authorsResourceParameter.SearchQuery)
                    || a.FirstName.Contains(authorsResourceParameter.SearchQuery));
            }

            return collection.ToList();
        }

        public IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds)
        {
            if (authorIds == null)
            {
                throw new ArgumentNullException(nameof(authorIds));
            }

            return _context.Authors.Where(a => authorIds.Contains(a.Id))
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToList();
        }

        public void UpdateAuthor(Author author)
        {
            // no code in this implementation
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }
    }
}
