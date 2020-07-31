using AutoMapper;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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
        private readonly IMapper _mapper;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository,
            IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        /*We know we want to response to a get request. So let's use the HttpGet attribute.
         We want this controller action executed when we send a request to api/authors. That's
        what we passed in as route templates.*/
        // [HttpGet("api/authors")]
        /*Defining the route templates in the controller level and not on each action*/
        [HttpGet()]
        /* HEAD is identical to GET with a notable difference that the API shouldn't return
         * a response body, so no response payload. It can be used to obtain information
         * on the resource as the response headers are returned. In a system that supports caching,
         * this might be used to obtain information whether the resource is still valid or
         * whether the resource is recently modified. Another usecase is to check whether the
         * resource is existing at all.
         * HEAD must return the same response as GET method but without the response body.
         */
        [HttpHead()]
        /*IActionResult defines a contract that represents the result of an action method.
        */
        /*ActionResult<T> explicity define the return type of the method, the other pieces
         of code can infer the actions expected return type. That allows them to integrate
        better of our actions. This isn't the benefit that is immediately obvious. It only
        becomes obvious when implementing something like Swashbuckle for documenting the API,
        which relies on getting information on how our API behaves. Next to that, both <T>
        and ActionResult can be implicitly cast to an ActionResult of T, and that too simplifies
        syntax. In other words, when you can use ActionResult of T, use it.*/

        /* We are now implementing a filter on mainCategory when getting authors.
         * The first thing to do is to allow the consumer of the API to pass in the value for the
         * mainCategory to the query string. This does not match any parameter name from the
         * route template so, thanks to the ApiController attribute, this will be bound by the
         * query string. [FromQuery] attribute is optional but this will make the code more
         * readable. In case the action parameter name is different from the key in the query string,
         * you can use the Name property FromQuery attribute [FromQuery(Name = "differentMainCategory")].
         * We will leave the default value as null. "string" is a reference type so null is the default.
         * By default, we don't want to apply "mainCategory" filters to our authors collection.
         */
        /* We will now add searching functionality for our Authors collection. We will accept a search query
         * and will return all authors for which one of the string fields contains the search query. First 
         * we need to ensure that the search query can be passed in. That means adding a search query
         * string to our action parameter list for the GetAuthors action. We do need to combine it 
         * with the filtering. The consumer might do a filter or search or combine both.
         */
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors(AuthorsResourceParameter authorsResourceParameter)
        {
            var authorsFromRepo = _courseLibraryRepository.GetAuthors(authorsResourceParameter);

            // We are now commenting this codes since we will now use AutoMapper
            //var authors = new List<AuthorDto>();

            //foreach (var author in authorsFromRepo) 
            //{
            //    authors.Add(new AuthorDto()
            //    {
            //        Id = author.Id,
            //        Name = $"{author.FirstName} {author.LastName}",
            //        MainCategory = author.MainCategory,
            //        Age = author.DateOfBirth.GetCurrentAge()
            //    }); ;
            //}

            //return Ok(authors);

            /* We want to map the authorsFromRepo variable. We pass to the type we want
             * to get back.
             */
            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));
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

            return Ok(_mapper.Map<AuthorDto>(authorFromRepo));
        }
    }
}
