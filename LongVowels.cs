using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Runtime.InteropServices;

namespace VVowels
{
    public class LongVowels
    {
        private StringComparison sc = StringComparison.OrdinalIgnoreCase;
        private string path = Directory.GetCurrentDirectory() + "/dictionary/";
        public virtual int vbounds { get; set; } = 3;
        public virtual int ybounds { get; set; } = 2;
        public bool isLong { get; set; }

        public (double value, string vFormatValue) VLongVowel(string str, LongVo longV, [Optional] bool all)
        {
            var t = (0.0, string.Empty);

            if (!String.IsNullOrEmpty(str))
            {
                var wordChuncks = str.Split(' ');
                var dVlas = new double[wordChuncks.Length];
                var formatt = new string[wordChuncks.Length];

                double tValue = 0.0;

                for (int i = 0; i < wordChuncks.Length; i++)
                {
                    var v = LongVowel(wordChuncks[i], longV, all);
                    if (!all)
                    {
                        formatt[i] = "LV = " + v.ToString("0.0000") + " || " + wordChuncks[i] + " || ";
                        tValue += v;
                    }
                    else
                    {
                        dVlas[i] += v;
                        formatt[i] = "LV = " + dVlas[i].ToString("0.0000") + " || " + wordChuncks[i] + " || ";
                    }
                }

                //join 
                if (formatt.Length > 0)
                {
                    t = (tValue, String.Join(" ", formatt));
                }
            }
            DefaultStat();
            return t;
        }
        private int pathCounter = 0;
        private List<(string path, bool enable)> paths = new List<(string path, bool enable)>();
        private string vowelComparer = string.Empty;
        private void AllMode(LongVo longV)
        {
            SetDictionary(longV, true);
        }
        public void SetActiveVowel(string strPath)
        {
            string strr = string.Empty;

            if (isLong)
            {
                isLong = true;
                strr = "long";
            }
            else
            {
                isLong = false;
                strr = "short";
            }

            vowelComparer = (strPath) switch
            {
                var x when x.Contains(strr + "-a.txt", sc) => "a",
                var x when x.Contains(strr + "-i.txt", sc) => "i",
                var x when x.Contains(strr + "-u.txt", sc) => "u",
                var x when x.Contains(strr + "-e.txt", sc) => "e",
                var x when x.Contains(strr + "-o.txt", sc) => "o",
                _ => vowelComparer = string.Empty
            };

        }
        public double LongVowel(string str, LongVo longV, bool all)
        {
            var val = 0.0;

            if (paths.Count == 0)
            {
                PoolDictionary(longV, all);
            }
            else
            {
                if (!all)
                    SetDictionary(longV, false);
            }

            if (all)
                AllMode(longV);

            foreach (var fpath in paths)
            {
                if (File.Exists(path + fpath.path) && fpath.enable)
                {
                    if (all)
                        SetActiveVowel(fpath.path);

                    foreach (var e in File.ReadLines(path + fpath.path))
                    {
                        if (!String.IsNullOrEmpty(e))
                        {
                            for (int i = 0; i < e.Length; i++)
                            {
                                var startVow = string.Empty;
                                var midVow = string.Empty;
                                var endVow = string.Empty;

                                if (i + vbounds <= e.Length - 1)
                                {
                                    string tmpThree = string.Empty;
                                    tmpThree = e.Substring(i, vbounds);

                                    if(!tmpThree.Contains("") || !tmpThree.Contains(" ") || tmpThree != null)
                                    {
                                        if (tmpThree[0] == vowelComparer[0]) startVow = tmpThree;
                                        else if (tmpThree[1] == vowelComparer[0]) midVow = tmpThree;
                                        else if (tmpThree[2] == vowelComparer[0]) endVow = tmpThree;
                                    }
                                    
                                    if (!String.IsNullOrEmpty(startVow) || !String.IsNullOrEmpty(midVow) || !String.IsNullOrEmpty(endVow))
                                    {
                                        //For fun
                                        List<string> fxx = new List<string>(str.Length);
                                        for (int t = 0; t < str.Length; t++)
                                        {
                                            if (t + vbounds <= str.Length - 1 && !String.IsNullOrEmpty(tmpThree))
                                            {
                                                var tt = str.Substring(t, vbounds);

                                                if (!String.IsNullOrEmpty(startVow) && startVow.Equals(tt))
                                                {
                                                    val += 0.2;
                                                    Console.WriteLine("Start vowel +3 : " + tt + " =============> START");
                                                }
                                                else if (!String.IsNullOrEmpty(midVow) && midVow.Equals(tt))
                                                {
                                                    val += 0.2;
                                                    Console.WriteLine("Middle vowel +3 : " + tt + " =============> MID");
                                                }
                                                else if (!String.IsNullOrEmpty(endVow) && endVow.Equals(tt))
                                                {
                                                    val += 0.2;
                                                    Console.WriteLine("Last vowel +3 : " + tt + " =============> LAST");
                                                }
                                                else
                                                {
                                                    //NOT FOUND
                                                    fxx.Add("===");
                                                    var f = String.Join("", fxx);
                                                    Console.WriteLine("Middle vowel +3 : " + tt + " " + f + "mismatch!");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (str.Equals(e, sc))
                            val += 0.4;
                    }
                }
            }
            return val / (double)100;
        }

        private void DefaultStat()
        {
            vbounds = 3;
            ybounds = 2;
            vowelComparer = string.Empty;

            //Aight, tbh I'm just being lazy here :)
            for (var f = 0; f < paths.Count; f++)
            {
                (string estring, bool ebool) esb = (paths[f].path, false);
                paths[f] = esb;
            }
        }

        private void PoolDictionary(LongVo longV, [Optional] bool all)
        {
            LVClass lvc = new LVClass();

            var fieldValues = lvc.GetType()
                    .GetFields()
                    .Select(field => field.GetValue(lvc))
                    .ToList();

            (string str, bool state) newInst = (string.Empty, false);

            foreach (var bb in fieldValues)
            {
                if (bb != null)
                {
                    newInst.str = bb as string;
                    newInst.state = false;
                    paths.Add(newInst);
                }
            }
            SetDictionary(longV);
        }
        private void SetDictionary(LongVo longV, [Optional] bool all)
        {
            string strr = string.Empty;

            if (isLong)
                strr = "long";
            else
                strr = "short";

            if (!all)
            {
                for (var f = 0; f < paths.Count; f++)
                {
                    if (longV == LongVo.A && paths[f].path.Contains(strr + "-a.txt"))
                    {
                        (string estring, bool ebool) esb = (paths[f].path, true);
                        paths[f] = esb;
                        vowelComparer = "a";
                    }
                    else if (longV == LongVo.I && paths[f].path.Contains(strr + "-i.txt"))
                    {
                        (string estring, bool ebool) esb = (paths[f].path, true);
                        paths[f] = esb;
                        vowelComparer = "i";
                    }
                    else if (longV == LongVo.U && paths[f].path.Contains(strr + "-u.txt"))
                    {
                        (string estring, bool ebool) esb = (paths[f].path, true);
                        paths[f] = esb;
                        vowelComparer = "u";
                    }
                    else if (longV == LongVo.E && paths[f].path.Contains(strr + "-e.txt"))
                    {
                        (string estring, bool ebool) esb = (paths[f].path, true);
                        paths[f] = esb;
                        vowelComparer = "e";
                    }
                    else if (longV == LongVo.O && paths[f].path.Contains(strr + "-o.txt"))
                    {
                        (string estring, bool ebool) esb = (paths[f].path, true);
                        paths[f] = esb;
                        vowelComparer = "o";
                    }
                }
            }

            else
            {
                for (int i = 0; i < paths.Count; i++)
                {
                    (string estring, bool ebool) esb = (paths[i].path, true);
                    paths[i] = esb;
                }
            }
        }
    }
}