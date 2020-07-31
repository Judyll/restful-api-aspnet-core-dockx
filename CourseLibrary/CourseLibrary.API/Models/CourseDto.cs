using System;

namespace CourseLibrary.API.Models
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        /* We can add the 'public AuthorDto Author { get; set; }' property here letting AutoMapper
         * take care of the mapping for us. But, in this case, that would result in the same
         * AuthorDto being returned for each course and that's a redundant information. It will hurt
         * performance, sending it over to the wire for each course. So, we are not going to include
         * the full AuthorDto
         */
        public Guid AuthorId { get; set; }
    }
}
