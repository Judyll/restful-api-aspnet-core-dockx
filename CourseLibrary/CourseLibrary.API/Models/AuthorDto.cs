using System;

namespace CourseLibrary.API.Models
{
    /*This is the one that will be returned by our controller action*/
    public class AuthorDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string MainCategory { get; set; }
    }
}
