using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
            return Ok(authorsFromRepo);
        }

        /*As we learned, the good practice for designing a URI for this resource  is to
         use plural noun, authors, followed by the forward slash, followed by the id.
         The route attribute at controller level takes care of api/authors. So, our 
         HttpGet attribute should does only contain the id. The forward slash is added
         automatically. That id is dependent on the parameter you want to access. It's a
         parameter that changes. To reflect that, we surround it with curly braces. We will
         need to access this parameter in our action method, and we can get that by adding
         the parameter to the method signature the same name we gave it on the route
         template.*/
        [HttpGet("{authorId}")]

        /*Quick tip, if you happen to run into a case where you can have Ids of multiple
         types for the resource, for example, an integer or guid, you can use route contraints
         to disambiguate between routes. "guid" is a route contraints. This will only match
         if the input after "authors" can be casted in guid.*/
        // [HttpGet("{authorId:guid}")]
        public IActionResult GetAuthor(Guid authorId)
        {
            var authorFromRepo = _courseLibraryRepository.GetAuthor(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }
            
            return Ok(authorFromRepo);
        }
    }
}
