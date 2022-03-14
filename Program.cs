using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace VVowels
{
    public class Program
    {
        private static bool all = false;
        public static void Main()
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

        public static void DetectLongVowels(string str, string vow)
        {
            //Note
            //results may vary depends on how many datas in a dictionary
            //Either way must be compared vice-versa for both Long and Short vowels
            var t = new LongVowels();

            //LongVo.I, LongVo.A, LongVo.O, LongVo.E, LongVo.U
            var lv = LongVo.A;
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

            var result = (0.0, string.Empty);

            if(!all)
                result = t.VLongVowel(str, lv);
            else
                result = t.VLongVowel(str, lv, all);

            Console.WriteLine("==> Long Vowel");

            if(!all)
                Console.WriteLine("==> Check for vowel type : " + lv);
            else
                Console.WriteLine("==> Check for ALL Vowels");

            Console.WriteLine(result.Item2);

            all = false;
        } 
    }
}