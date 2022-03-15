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

            if(sVow.Contains("1"))
            {
                longVowel = true;
            }

            if(longVowel)
            {
                Console.WriteLine("Single(1) or ALL(2) Vowels?");
                string sVow = Console.ReadLine();

                string vowType = string.Empty;

                if(sVow.Contains("1"))
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
                string inputStr = Console.ReadLine();

                if(!String.IsNullOrEmpty(inputStr))
                    DetectLongVowels(inputStr, vowType);
            }
            else
            {
                Console.WriteLine("Single(1) or ALL(2) Vowels?");
                string sVow = Console.ReadLine();

                string vowType = string.Empty;

                if(sVow.Contains("1"))
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
                string inputStr = Console.ReadLine();

                if(!String.IsNullOrEmpty(inputStr))
                    DetectLongVowels(inputStr, vowType);
            }
        }

        public static void DetectLongVowels(string str, string vow)
        {
            //Note
            //results may vary depends on how many datas in a dictionary
            //Either way must be compared vice-versa for both Long and Short vowels
            var t = new LongVowels();
            var s = new ShortVowels();
            var lv = LongVo.A;
            var sv = ShortVo.A;

            if(longVowel)
            {
                //LongVo.I, LongVo.A, LongVo.O, LongVo.E, LongVo.U
                
                if(vow.Equals("a"))
                    lv = LongVo.A;
                if(vow.Equals("i"))
                    lv = LongVo.I;
                if(vow.Equals("u"))
                    lv = LongVo.U;
                if(vow.Equals("e"))
                    lv = LongVo.E;
                if(vow.Equals("o"))
                    lv = LongVo.O;
            }
            else
            {
                //ShortVo.I, ShortVo.A, ShortVo.O, ShortVo.E, ShortVo.U
                
                if(vow.Equals("a"))
                    sv = ShortVo.A;
                if(sv.Equals("i"))
                    sv = ShortVo.I;
                if(vow.Equals("u"))
                    sv = ShortVo.U;
                if(vow.Equals("e"))
                    sv = ShortVo.E;
                if(vow.Equals("o"))
                    sv = ShortVo.O;
            }
            var result = (0.0, string.Empty);

            if(!all)
            {
                if(longVowel)
                    result = t.VLongVowel(str, lv);
                else
                    result = s.VShortVowel(str, sv);
            }
            else
            {
                if(longVowel)
                    result = t.VLongVowel(str, lv, all);
                else
                    result = s.VShortVowel(str, sv, all);
            }

            Console.WriteLine("==> Long Vowel");

            if(!all)
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