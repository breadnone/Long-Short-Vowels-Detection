using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace VVowels
{
    public class Program
    {
        private static bool all = false;
        private static bool longVowel = false;
        private static string sVow = string.Empty;
        public static void Main()
        {
            Console.WriteLine("Check for Long Vowel(1) OR Short Vowel(2)");
            sVow = Console.ReadLine();

            if (sVow.Contains("1"))
                longVowel = true;
            else
                longVowel = false;

            Console.WriteLine("Single(1) or ALL(2) Vowels?");
            sVow = Console.ReadLine();
            string vowType = string.Empty;
            string inputStr = string.Empty;

            if (longVowel)
            {

                if (sVow.Contains("1"))
                {
                    Console.WriteLine("vowels of | a | i | u | e | o | to check? ");
                    vowType = Console.ReadLine();
                }
                if (sVow.Contains("2"))
                {
                    all = true;
                    Console.WriteLine("Set for all vowels!");
                }

                Console.WriteLine("Enter english phrases :");
                inputStr = Console.ReadLine();

                if (!String.IsNullOrEmpty(inputStr))
                    DetectVowels(inputStr, vowType);
            }
            else
            {
                if (sVow.Contains("1"))
                {
                    Console.WriteLine("vowels of | a | i | u | e | o | to check? ");
                    vowType = Console.ReadLine();
                }
                else
                {
                    all = true;
                    Console.WriteLine("Set for all vowels!");
                }

                Console.WriteLine("Enter english phrases :");
                inputStr = Console.ReadLine();

                if (!String.IsNullOrEmpty(inputStr))
                    DetectVowels(inputStr, vowType);
            }
        }

        public static void DetectVowels(string str, string vow)
        {
            //Note
            //results may vary depends on how many datas in a dictionary
            //Either way must be compared vice-versa for both Long and Short vowels
            var t = new LongVowels();
            var lv = LongVo.A;

            if (longVowel)
            {
                if (vow.Equals("a"))
                    lv = LongVo.A;
                if (vow.Equals("i"))
                    lv = LongVo.I;
                if (vow.Equals("u"))
                    lv = LongVo.U;
                if (vow.Equals("e"))
                    lv = LongVo.E;
                if (vow.Equals("o"))
                    lv = LongVo.O;
            }

            var result = (0.0, string.Empty);

            if (longVowel)            
                t.isLong = true;
            else            
                t.isLong = false;

            if (!all)
            {
                result = t.VLongVowel(str, lv);
            }
            else
            {
                result = t.VLongVowel(str, lv, all);
            }

            if (longVowel)
                Console.WriteLine("==> Long Vowel");
            else
                Console.WriteLine("==> Short Vowel");

            if (!all)
                Console.WriteLine("==> Check for vowel type : " + lv);
            else
                Console.WriteLine("==> Check for ALL Vowels");

            Console.WriteLine(result.Item2);

            all = false;
            longVowel = false;
            sVow = string.Empty;
        }
    }
}