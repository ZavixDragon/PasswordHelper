using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using PasswordHelper;
using PasswordHelper.Common;

namespace PasswordHelperWebApi
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
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.NotFound };
            }
        }

        [HttpPost("generate")]
        public IActionResult Post([FromBody]GeneratePasswordRequest request)
        {
            try
            {
                return new ObjectResult(_factory.Create(request).Get()) { StatusCode = (int) HttpStatusCode.OK };
            }
            catch (ArgumentException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int) HttpStatusCode.BadRequest };
            }
        }

        [HttpPost("save")]
        public IActionResult Post([FromBody]Password value, [FromServices]IStore<Password> store)
        {
            if (value.Identifier == null || value.Value == null)
                return new ObjectResult("Can't save password because either the identifier or the password is null.")
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            store.Store(value.Identifier, value);
            return new ObjectResult("Success") { StatusCode = (int) HttpStatusCode.OK };
        }
    }
}
