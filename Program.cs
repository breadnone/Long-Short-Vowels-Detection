using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace VVowels
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("vowels of | a | i | u | e | o | to check? ");
            string vowType = Console.ReadLine();

            if(String.IsNullOrEmpty(vowType))
                return;

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

            var result = t.VLongVowel(str, lv);

            Console.WriteLine("==> Long Vowel");
            Console.WriteLine("==> Check for vowel type : " + lv);
            Console.WriteLine(result.vFormatValue);
        } 
    }
}