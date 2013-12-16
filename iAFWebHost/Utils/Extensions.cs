using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace iAFWebHost.Utils
{
    public static class Extensions
    {
        public static string sBase58Alphabet = "123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";

        public static string EncodeBase58(this ulong numberToShorten)
        {
            String sConverted = "";
            ulong iAlphabetLength = (ulong)sBase58Alphabet.Length;

            while (numberToShorten > 0)
            {
                ulong lNumberRemainder = (numberToShorten % iAlphabetLength);
                numberToShorten = (numberToShorten / iAlphabetLength);
                sConverted = sBase58Alphabet[Convert.ToInt32(lNumberRemainder)] + sConverted;
            }

            return sConverted;
        }

        public static ulong DecodeBase58(this string base58StringToExpand)
        {
            ulong lConverted = 0;
            ulong lTemporaryNumberConverter = 1;

            while (base58StringToExpand.Length > 0)
            {
                String sCurrentCharacter = base58StringToExpand.Substring(base58StringToExpand.Length - 1);
                lConverted = lConverted + (lTemporaryNumberConverter * (ulong)sBase58Alphabet.IndexOf(sCurrentCharacter));
                lTemporaryNumberConverter = lTemporaryNumberConverter * (ulong)sBase58Alphabet.Length;
                base58StringToExpand = base58StringToExpand.Substring(0, base58StringToExpand.Length - 1);
            }

            return lConverted;
        }

        public static string MD5(this string input)
        {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrEmpty(input))
            {
                Encoder enc = System.Text.Encoding.Unicode.GetEncoder();

                byte[] rawBytes = new byte[input.Length * 2];
                enc.GetBytes(input.ToCharArray(), 0, input.Length, rawBytes, 0, true);

                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] result = md5.ComputeHash(rawBytes);


                for (int a = 0; a < result.Length; a++)
                {
                    sb.Append(result[a].ToString("X2"));
                }
            }
            return sb.ToString();
        }

        public static bool IsShortCode(this string input)
        {
            bool validation = false;
            if (!String.IsNullOrEmpty(input) && input.Length < 30)
            {
                validation = true;
                char[] inputArray = input.ToCharArray();
                for (int i = 0; i < inputArray.Length; i++)
                {
                    if (sBase58Alphabet.Contains(inputArray[i]) == false)
                    {
                        validation = false;
                        break;
                    }
                }
            }
            return validation;
        }

        /// <summary>
        /// Determines whether the collection is null or contains no elements.
        /// </summary>
        /// <typeparam name="T">The IEnumerable type.</typeparam>
        /// <param name="enumerable">The enumerable, which may be null or empty.</param>
        /// <returns>
        ///     <c>true</c> if the IEnumerable is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }
            /* If this is a list, use the Count property for efficiency. 
             * The Count property is O(1) while IEnumerable.Count() is O(N). */
            var collection = enumerable as ICollection<T>;
            if (collection != null)
            {
                return collection.Count < 1;
            }
            return !enumerable.Any();
        }

        public static TData DeserializeFromString<TData>(this string settings)
        {
            byte[] b = Convert.FromBase64String(settings);
            using (var stream = new MemoryStream(b))
            {
                var formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                return (TData)formatter.Deserialize(stream);
            }
        }

        public static string SerializeToString<TData>(this TData settings)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, settings);
                stream.Flush();
                stream.Position = 0;
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string BuildExceptionMessage(this Exception exception)
        {
            Exception logException = exception;
            if (exception.InnerException != null)
                logException = exception.InnerException;

            string strErrorMsg = Environment.NewLine + "Error in Path :" + System.Web.HttpContext.Current.Request.Path;

            // Get the QueryString along with the Virtual Path
            strErrorMsg += Environment.NewLine + "Raw Url :" + System.Web.HttpContext.Current.Request.RawUrl;

            // Get the error message
            strErrorMsg += Environment.NewLine + "Message :" + logException.Message;

            // Source of the message
            strErrorMsg += Environment.NewLine + "Source :" + logException.Source;

            // Stack Trace of the error
            strErrorMsg += Environment.NewLine + "Stack Trace :" + logException.StackTrace;

            // Method where the error occurred
            strErrorMsg += Environment.NewLine + "TargetSite :" + logException.TargetSite;
            return strErrorMsg;
        }

        public static string EscapeStringValue(this string value)
        {
            const char BACK_SLASH = '\\';
            const char SLASH = '/';
            const char DBL_QUOTE = '"';

            var output = new StringBuilder(value.Length);
            foreach (char c in value)
            {
                switch (c)
                {
                    case SLASH:
                        output.AppendFormat("{0}{1}", BACK_SLASH, SLASH);
                        break;

                    case BACK_SLASH:
                        output.AppendFormat("{0}{0}", BACK_SLASH);
                        break;

                    case DBL_QUOTE:
                        output.AppendFormat("{0}{1}", BACK_SLASH, DBL_QUOTE);
                        break;

                    default:
                        output.Append(c);
                        break;
                }
            }

            return output.ToString();
        }
    }
}