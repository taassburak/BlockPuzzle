using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace PassionPunch
{
    public static class Utilities
    {
        /// <summary>
        /// Debug or not, depend on a simple switch
        /// </summary>
        /// <param name="behavior"></param>
        /// <param name="message"></param>
        public static void Print(this MonoBehaviour behavior, string message)
        {
#if PP_DEBUG
            var tag = Application.identifier.Split('.');
            Debug.Log($"<{tag[tag.Length - 1]}>{behavior}: {message}");
#endif
        }

        public static string MakeSafeForCode(this string str)
        {
            str = Regex.Replace(str, "[^a-zA-Z0-9_]", "_", RegexOptions.Compiled);
            if (char.IsDigit(str[0]))
            {
                str = "_" + str;
            }

            return str;
        }

        public static bool IsGenericList(this object o)
        {
            var oType = o.GetType();
            return (oType.IsGenericType && (oType.GetGenericTypeDefinition() == typeof(List<>)));
        }
    }
}