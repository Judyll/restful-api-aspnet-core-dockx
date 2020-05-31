using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Controllers
{
    /*This isn't strictly necessary but by doing so, we are configuring this controller
     with features and behaviour aimed at improving the development experience when
    building APIs. Think about requiring attribute routing, automatically returning a
    400 Bad Request on bad input, and returning problem details on errors. We'll encounter
    quite a few of those throughout the course. So, if building an API, the APIController
    attribute will definitely help.*/
    [ApiController]
    /*The ControllerBase class contains basic functionality controllers need, like
     access to the model state, the current user, and common methods for returning
    responses. We could also inherit from Controller, but by doing so, we also add
    support for Views (Razor) which isn't needed for building APIs.*/
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
        }

        /*We know we want to response to a get request. So let's use the HttpGet attribute.
         We want this controller action executed when we send a request to api/authors. That's
        what we passed in as route templates*/
        [HttpGet("api/authors")]
        /*IActionResult defines a contract that represents the result of an action method.
        */
        public IActionResult GetAuthors()
        {
            var authorsFromRepo = _courseLibraryRepository.GetAuthors();
            return new JsonResult(authorsFromRepo);
        }
    }
}
