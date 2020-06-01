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

    /*Consistency is important. For example, if we want to consume all our resources by
     staring with "api" and all the resources in this controller should start with 
     "api/authors", then we can use the "Route" attribute at the controller level for this.
     So, we only need to define it once, and not on each actions.*/
    [Route("api/authors")]

    /*When the controller is created to one of Microsoft's templates, you might see the
     the below route definition. That results in exactly the same way as writing "api/authors".
     [controller] will be substituted with the prefix of our controller class "authors".
     But, if we where to have a refactoring our our controller name, the URI for our
     authors resource would automatically change as well. And, sometimes, the name of the
     class might not necessarily match part of our route.*/
    // [Route("api/[controller]")]

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
        what we passed in as route templates.*/
        // [HttpGet("api/authors")]
        /*Defining the route templates in the controller level and not on each action*/
        [HttpGet()]
        /*IActionResult defines a contract that represents the result of an action method.
        */
        public IActionResult GetAuthors()
        {
            var authorsFromRepo = _courseLibraryRepository.GetAuthors();
            return new JsonResult(authorsFromRepo);
        }
    }
}
