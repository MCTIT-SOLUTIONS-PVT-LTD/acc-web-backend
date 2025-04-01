using AcceInfoBankingAPI.Common.Helper;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AcceInfoBankingAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DBConnectionHelper _db;
        public AuthorizationController(IConfiguration configuration, DBConnectionHelper db)
        {
            _configuration = configuration;
            _db = db;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Common.Models.RequestModel.LoginRequest loginRequest)
        {

            if (string.IsNullOrEmpty(loginRequest.Type) || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(new { success = false, message = "Missing required fields." });
            }

            if (loginRequest.Type.ToLower() == "employee")
            {
                var sql11 = @"
                SELECT c.""ContactId"", c.""UserName""
                FROM ""Contact"" c
                JOIN ""ContactRoleJn"" crj ON c.""ContactId"" = crj.""ContactId""
                JOIN ""Roles"" r ON crj.""RoleId"" = r.""Id""
                WHERE c.""UserName"" = @UserName AND r.""Name"" = 'Employee';
            ";
                string UserName = "admin";
                string roleName = "Customer";
                var sql = $@"
                SELECT c.""ContactId"", c.""UserName""
                FROM ""Contact"" c
                JOIN ""ContactRoleJn"" crj ON c.""ContactId"" = crj.""ContactId""
                JOIN ""Roles"" r ON crj.""RoleId"" = r.""Id""
                WHERE c.""UserName"" = '{UserName}' AND r.""Name"" = '{roleName}';
                ";
                var employee = await _db.QuerySingleAsync<dynamic>(sql, new { loginRequest.Username });
                if(employee != null)
                {
                    return Ok(new { message = "Success" });
                }
            }

            //// Static validation (replace with your own user validation logic)
            //if (loginRequest.Username == "admin" && loginRequest.Password == "admin")
            //{
            //    AuthHelper authHelper = new AuthHelper(_configuration);
            //    var token = authHelper.GenerateJwtToken(loginRequest.Username);
            //    return Ok(new { Token = token });
            //}

            return Unauthorized(new { message = "Invalid credentials" });
        }
        // GET: api/<AuthorizationController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AuthorizationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AuthorizationController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AuthorizationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthorizationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
