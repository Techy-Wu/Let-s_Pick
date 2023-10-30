using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Security.Cryptography;

namespace Lottery_Picking
{
    class Class1
    {
        public static string path = System.Environment.CurrentDirectory.ToString();
        public static int lang = 1;         //0-En, 1-Zh
        public static int num;
        public static int target_num;
        public static int line;
        public static ArrayList al = new ArrayList();
        public static ArrayList sl_name = new ArrayList();
        public static ArrayList sl_num = new ArrayList();
        public static ArrayList sl_type = new ArrayList();      // false-最多不超过  true-至少不低于
        public static Random ran = new Random(Int32.Parse(DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString()));
        public static string version = "1.0.0.0";
        public static string machine;
        public static bool ac_info = false;

        public static string Md5(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(str)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }
    }
}
