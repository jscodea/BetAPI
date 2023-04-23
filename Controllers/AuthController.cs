using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BetAPI.Data;
using BetAPI.Models;
using BetAPI.DTO;
using BetAPI.Services;
using BetAPI.Filters;
using PagedList;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace BetAPI.Controllers
{
    [Route("api/[controller]")]
    [RenderableExceptionFilter]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        // POST: api/Auth/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            bool authenticated = await _authService.LoginAsync(username, password);

            if (! authenticated)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
