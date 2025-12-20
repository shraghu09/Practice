using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using AuthAPI.Models;

namespace AuthAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly string _connectionString;

        public AuthController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MySqlConnection");
        }

        // 1️⃣ REGISTER
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(
                "INSERT INTO users(name,email,password) VALUES(@name,@email,@password)", conn);

            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", user.Password);

            cmd.ExecuteNonQuery();
            return Ok("User registered successfully");
        }

        // 2️⃣ LOGIN
        [HttpPost("login")]
        public IActionResult Login(LoginRequest login)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(
                "SELECT * FROM users WHERE email=@email AND password=@password", conn);

            cmd.Parameters.AddWithValue("@email", login.Email);
            cmd.Parameters.AddWithValue("@password", login.Password);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return Unauthorized("Invalid email or password");

            return Ok(new { token = "test-token-123" });
        }

        // 3️⃣ DISPLAY ALL USERS
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = new List<object>();
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand("SELECT id,name,email FROM users", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new
                {
                    Id = reader["id"],
                    Name = reader["name"],
                    Email = reader["email"]
                });
            }

            return Ok(users);
        }
    }
}
