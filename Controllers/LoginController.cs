using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using bodyspace_api.Models;
using Microsoft.Extensions.Logging;

namespace bodyspace_api.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private static HttpClient _client = new HttpClient();
        private readonly ILogger _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        // POST api/Login HTTP/1.1
        // Content-Type: application/json; charset=utf-8
        // {"username": "bob", "password": "secret"}
        [HttpPost]
        public LoginResult Post([FromBody] LoginParam creds)
        {
            var clearText = creds.Password;
            var hashText = Md5Hash(clearText);
            var json = $"{{\"username\": \"{creds.UserName}\", \"rememberMe\": false, \"password\": \"{hashText}\"}}";

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var res = _client.PostAsync("https://api.bodybuilding.com/login", content).Result)
            {
                var resultJson = res.Content.ReadAsStringAsync().Result;
                var loginResult = JsonConvert.DeserializeObject<LoginResult>(resultJson);

                var kvps = new List<KeyValuePair<string, string>>();
                foreach (var header in res.Headers)
                {
                    foreach (var val in header.Value)
                    {
                        kvps.Add(new KeyValuePair<string, string>(header.Key, val));
                    }
                }
                loginResult.ResponseHeaders = kvps;

                return loginResult;
            }
        }

        private string Md5Hash(string clearText) 
        {
            using (var hasher = MD5.Create())
            {
                var clearBytes = Encoding.UTF8.GetBytes(clearText);
                var hashBytes = hasher.ComputeHash(clearBytes);

                var sb = new StringBuilder();
                foreach (var hashByte in hashBytes)
                {
                    sb.Append(hashByte.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
