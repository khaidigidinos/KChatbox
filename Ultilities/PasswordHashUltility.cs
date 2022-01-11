using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SignalRApi.Ultilities
{
    public class PasswordHashUltility
    {
        public static string HashPassowrd(string input) => string.Concat(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(input)).Select(item => item.ToString("x2")));
    }
}
