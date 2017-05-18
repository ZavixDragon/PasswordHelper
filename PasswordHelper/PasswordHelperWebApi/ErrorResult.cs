using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace PasswordHelperWebApi
{
    public class ErrorResult : ObjectResult
    {
        public ErrorResult(string message, HttpStatusCode code) : base(message)
        {
            this.StatusCode = (int) code;
        }
    }
}
