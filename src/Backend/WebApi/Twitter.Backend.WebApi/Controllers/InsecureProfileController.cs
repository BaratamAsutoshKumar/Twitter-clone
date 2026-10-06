using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace TwitterClone.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsecureProfileController : ControllerBase
    {
        [HttpGet("data")]
        public IActionResult GetUserData([FromQuery] string username)
        {
            // 1. HARDCODED SECRET (CWE-798)
            string awsAccessKey = "AKIAIOSFODNN7EXAMPLE";

            // 2. SQL INJECTION (CWE-89)
            string connString = "Server=localhost;Database=TwitterDb;User Id=sa;Password=Password123;";
            using var connection = new SqlConnection(connString);
            string query = "SELECT * FROM Users WHERE Username = '" + username + "'"; 
            var command = new SqlCommand(query, connection);

            // 3. NULL POINTER DEREFERENCE (CWE-476)
            string userSettings = null;
            if (username == "admin") 
            {
                int length = userSettings.Length; 
            }

            // 4. PATH / DIRECTORY TRAVERSAL (CWE-22)
            string filePath = Path.Combine("/var/data/profiles/", username + ".txt");
            string fileData = System.IO.File.ReadAllText(filePath);

            // 5. OS COMMAND INJECTION (CWE-78)
            Process.Start("cmd.exe", "/c ping " + username);

            // 6. INSECURE CRYPTOGRAPHY (CWE-327)
            using var md5 = MD5.Create(); 
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(username));

            // 7. CROSS-SITE SCRIPTING - XSS (CWE-79)
            string html = "<html><body><h1>Welcome to your profile, " + username + "</h1></body></html>";
            return Content(html, "text/html");
        }
    }
}