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
            string inputStr = "velvie walks through the woods picking strawberries";
            DetectLongVowels(inputStr);
        }

        public static void DetectLongVowels(string str)
        {
            var t = new LongVowels();
            var lv = LongVo.A; //Looking for long vowels 'A'

            //2 = max bounds e.g: longString = "Unbelieveable" -> range of 3 -> |unb|nbe|bel|eli|lie|iev|eve|vea|eab|abl|ble|
            t.vbounds = 2; //set max bounds/ OPTIONAL!
            var result = t.VLongVowel(str, lv);
    
            Console.WriteLine(result.vFormatValue);
        } 
    }
}