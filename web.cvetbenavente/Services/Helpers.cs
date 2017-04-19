using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.cvetbenavente.Services
{
    public class Helpers
    {
        public static string NormalizeString(string str)
        {
            byte[] tempBytes;

            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(str);
            return System.Text.Encoding.UTF8.GetString(tempBytes);
        }
    }
}
