using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;

namespace Cycle
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t1 = new Thread(qw);
            Thread t2 = new Thread(qw);
            Thread t3 = new Thread(qw);
            Thread t4 = new Thread(qw);
            t1.Start();
        }
        static string cHash(string rawData)
        {
            using (SHA256 shHash = SHA256.Create())
            {
                byte[] bytes = shHash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        static void qw()
        {
            int Attempt = 0;
            string Chars = "abcdefghijklmnopqrstuvwxyz";
            foreach (var Password in getComb(Chars.ToArray(), 5))
            {
                Attempt++;
                Console.WriteLine("Attempt №:" + Attempt + "; password:" + Password);
                if (cHash(Convert.ToString(Password)) == "74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f")
                {
                    Console.WriteLine("true " + Attempt);
                    break;
                }
            }
        }
        private static IEnumerable<string> getComb(char[] Chars, int mLen)
        {
            if (mLen < 1) yield break;
            foreach (var a in Chars)
            {
                yield return a.ToString();
                foreach (var b in getComb(Chars, mLen - 1)) yield return a + b;
            }
        }
    }
}
