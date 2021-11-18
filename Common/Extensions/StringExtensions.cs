using System;
using System.Json;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidJson(this string s)
        {
            try
            {
                var tmpObj = JsonValue.Parse(s);

                return true;
            }
            catch 
            {   
                return false;
            }
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool IsNotNullOrEmpty(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

    }
}