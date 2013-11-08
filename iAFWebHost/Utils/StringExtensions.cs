using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace iAFWebHost.Utils
{
    public static class StringExtensions
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
    }
}