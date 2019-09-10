using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TokGamesApi
{
    public static class StringExtensions
    {
        public static string GetEnvironmentVariable(string name)
        {
            return System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }

        /// <summary>
        /// Returns the input string in lowercase alphanumeric format. All spaces are replaced with underscores.
        /// </summary>
        public static string ToIdFormat(this string item)
        {
            item = item?.Trim().ToLower().Replace("/", "").Replace(" ", "_").Replace("&", "and").Replace("é", "e");
            item = Regex.Replace(item, "[^0-9A-Za-z]", "");
            return item;
        }

        /// <summary>
        /// Returns the input string with the first character converted to uppercase
        /// </summary>
        public static string FirstLetterToUpperCase(this string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException("There is no first letter");

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>
        /// Converts DateTime to Integer
        /// </summary>
        public static long ToUnixTime(this DateTime dateTime)
        {
            DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (int)Math.Round((dateTime - d).TotalSeconds);
        }

        /// <summary>
        /// Converts longeger to DateTime
        /// </summary>
        public static DateTime ToDateTime(this long dateTime)
        {
            return dateTime.ToDateTime();
        }

        /// <summary>
        /// Returns the an update string to update the count of a document (Example "$inc", ("total", 1))
        /// </summary>
        public static string BuildUpdateString(string updateType, Dictionary<string, long> fields = null)
        {
            if (string.IsNullOrEmpty(updateType))
                return null;

            string fieldString = "";
            fieldString += "{";

            int i = 0;
            foreach (var key in fields.Keys)
            {
                fieldString += "\"" + key + "\":" + fields[key];

                if (i + 1 < fields.Keys.Count)
                    fieldString += ",";
                else
                    fieldString += "}";

                ++i;
            }

            //for (int i = 0; i < fields.Count; ++i)
            //{
            //    fieldString += "\"" + fields[i].Item1 + "\":" + fields[i].Item2;

            //    if (i + 1 < fields.Count)
            //        fieldString += ",";
            //    else
            //        fieldString += "}";
            //}

            string val = "{\"" + updateType + "\":" + fieldString + "}"; //{\"coins_current\":1,\"arr[1]\":1}
            return val;
        }

        /// <summary>
        /// Returns the an update string to update the count of a document (Example "$set", ("total", "y"))
        /// </summary>
        public static string BuildUpdateStringField(string updateType, List<(string, string)> fields = null)
        {
            if (string.IsNullOrEmpty(updateType))
                return null;

            string fieldString = "";
            fieldString += "{";
            for (int i = 0; i < fields.Count; ++i)
            {
                fieldString += "\"" + fields[i].Item1 + "\":" + fields[i].Item2;

                if (i + 1 < fields.Count)
                    fieldString += ",";
                else
                    fieldString += "}";
            }

            string val = "{\"" + updateType + "\":" + fieldString + "}"; //{\"coins_current\":1,\"arr[1]\":1}
            return val;
        }

        /// <summary>
        /// Converts UserCounter
    




    }

    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
