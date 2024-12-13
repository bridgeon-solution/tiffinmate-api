using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.DAL.Entities
{
    public class TokenHelper
    {
       
            public static string GenerateRefreshToken()
            {
                var randomNumber = new byte[32];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(randomNumber);
                }
                return Convert.ToBase64String(randomNumber);
            }



        }
    }
