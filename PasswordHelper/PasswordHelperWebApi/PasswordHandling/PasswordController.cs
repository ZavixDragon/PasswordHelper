using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using PasswordHelper;
using PasswordHelper.Common;

namespace PasswordHelperWebApi.PasswordHandling
{
    [Route("api/[controller]")]
    public class PasswordController : Controller
    {
        private readonly IStore<Password> _store;
        private readonly IPasswordGenerationFactory _factory;

        public PasswordController(IStore<Password> store, IPasswordGenerationFactory factory)
        {
            _store = store;
            _factory = factory;
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            try
            {
                return new JsonResult(_store.Retrieve(id)) { StatusCode = (int) HttpStatusCode.OK };
            }
            catch (InvalidOperationException ex)
            {
                return new ErrorResult(ex.Message, HttpStatusCode.NotFound);
            }
        }

        [HttpPost("generate")]
        public IActionResult Post([FromBody]GeneratePasswordRequest request)
        {
            if (request.Identifier == null)
                return new ErrorResult("The request is missing an Identifier", HttpStatusCode.BadRequest);
            try
            {
                return new JsonResult(_factory.Create(request).Get()) { StatusCode = (int) HttpStatusCode.OK };
            }
            catch (ArgumentException ex)
            {
                return new ErrorResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpPost("save")]
        public IActionResult Post([FromBody]Password value, [FromServices]IStore<Password> store)
        {
            if (value.Identifier == null || value.Value == null)
                return new ErrorResult("Can't save password because either the identifier or the password is null.", 
                    HttpStatusCode.BadRequest);
            store.Store(value.Identifier, value);
            return new ObjectResult("Success") { StatusCode = (int) HttpStatusCode.OK };
        }
    }
}
