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
using Newtonsoft.Json;
using PagedList;

namespace BetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IPagedList<UserDTO>> GetUser([FromQuery]PagingParameters pagingParams)
        {
            IPagedList<UserDTO> users = await _userService.GetUsersPagedAsync(pagingParams.PageNumber, pagingParams.PageSize);

            PagingMetadata metadata = new PagingMetadata
            {
                Count = users.Count,
                PageSize = users.PageSize,
                PageNumber = users.PageNumber,
                PageCount = users.PageCount,
                HasNextPage = users.HasNextPage,
                HasPreviousPage = users.HasPreviousPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return users;
        }


        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _userService.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserPutDTO user)
        {
            int status = await _userService.UpdateUserAsync(id, user);
            if (status == 0)
            {
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostUser(User user)
        {
            await _userService.InsertUserAsync(user);
            return NoContent();
        }
    }
}
