using Data.Contexts;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace UserProvider.Functions
{
    public class UserFunction(ILogger<UserFunction> logger, DataContext context)
    {
        private readonly ILogger<UserFunction> _logger = logger;
        private readonly DataContext _context = context;


        [Function("GetUsers")]
        public async Task<IActionResult> GetUsers([HttpTrigger(AuthorizationLevel.Function, "get", Route = "users")] HttpRequest req)
        {
            var users = await _context.Users.ToListAsync();
            return new OkObjectResult(users);
        }


        [Function("GetUserById")]
        public async Task<HttpResponseData> GetUserById(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/{id}")] HttpRequestData req,
        string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return req.CreateResponse(System.Net.HttpStatusCode.NotFound);
            }

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(user);

            return response;
        }

        [Function("UpdateUser")]
        public async Task<HttpResponseData> UpdateUser(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "users/{id}")] HttpRequestData req,
        string id)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return req.CreateResponse(System.Net.HttpStatusCode.NotFound);
            }

            var updatedUser = await req.ReadFromJsonAsync<ApplicationUser>();
            if (updatedUser == null)
            {
                return req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            }

            _context.Entry(existingUser).CurrentValues.SetValues(updatedUser);
            await _context.SaveChangesAsync();

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(existingUser);

            return response;
        }



        [Function("DeleteUser")]
        public async Task<HttpResponseData> DeleteUser(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "users/{id}")] HttpRequestData req,
        string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return req.CreateResponse(System.Net.HttpStatusCode.NotFound);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return req.CreateResponse(System.Net.HttpStatusCode.NoContent);
        }


    }


}
