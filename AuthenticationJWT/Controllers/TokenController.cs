using AuthenticationJWT.Data;
using AuthenticationJWT.Models;
using System;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace AuthenticationJWT.Controllers
{
    public class TokenController : ApiController
    {
        private DBContext db = new DBContext();

        [AllowAnonymous]
        public IHttpActionResult Get(string Email, string Password)
        {
            if (CheckUser(Email, Password))
            {
                object response = new
                {
                    token = JwtManager.GenerateToken(Email)
                };
                return Ok(response)  ;

            }
           
            return Unauthorized();
        }
      
        public bool CheckUser(string Email, string Password)
        {

            //var v = db.Users.FirstOrDefault(user => user.Email == Email);
            //if (v != null && v.VerifyPasswordHash(Password, user.PasswordHash, user.PasswordSalt))
            //{
            //    return true;
            //VerifyPasswordHash(Password, user.PasswordHash, user.PasswordSalt) == true
            //}
            Users users = db.Users.FirstOrDefault(user => user.Email == Email);
            if (users != null && VerifyPasswordHash(Password, users.PasswordHash, users.PasswordSalt)) {
                return true;

            }

            return false;
        }
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password is required !");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
