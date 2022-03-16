using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VVowels
{
    //Believe me, I'm just being lazy here... like seriously XD
    
    public class ShortVowels
    {
        private StringComparison sc = StringComparison.OrdinalIgnoreCase;
        private string path = Directory.GetCurrentDirectory() + "/dictionary/";
        public virtual int vbounds { get; set; } = 3;
        public virtual int ybounds { get; set; } = 2;

        public (double value, string vFormatValue) VShortVowel(string str, ShortVo shortV, [Optional] bool all)
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
                    if (!all)
                    {
                        var v = ShortVowel(wordChuncks[i], shortV, all);
                        formatt[i] = "LV = " + v.ToString("0.0000") + " || " + wordChuncks[i] + " || ";
                        tValue += v;
                    }
                    else
                    {
                        var v = ShortVowel(wordChuncks[i], shortV, all);
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
        private int counta = 0;
        private void AllMode(ShortVo shortVo)
        {
            SetDictionary(shortVo, true);
        }
        public void SetActiveVowel(string strPath)
        {
            vowelComparer = (strPath) switch
            {
                var x when x.Contains("short-a.txt", sc) => "a",
                var x when x.Contains("short-i.txt", sc) => "i",
                var x when x.Contains("short-u.txt", sc) => "u",
                var x when x.Contains("short-e.txt", sc) => "e",
                var x when x.Contains("short-o.txt", sc) => "o",
                _ => vowelComparer = string.Empty
            };
        }
        public double ShortVowel(string str, ShortVo shortV, bool all)
        {
            var val = 0.0;

            if (paths.Count == 0)
            {
                PoolDictionary(shortV, all);
            }
            else
            {
                if (!all)
                    SetDictionary(shortV, false);
            }

            if (all)
                AllMode(shortV);

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

                                    if (i + vbounds <= e.Length - 1)
                                    {
                                        tmpThree = e.Substring(i, vbounds);

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
            counta = 0;
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

        private void PoolDictionary(ShortVo shortV, [Optional] bool all)
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
            SetDictionary(shortV);
        }
        private void SetDictionary(ShortVo shortV, [Optional] bool all)
        {
            if (!all)
            {
                for (var f = 0; f < paths.Count; f++)
                {

                    if (shortV == ShortVo.A)
                    {
                        if (paths[f].path.Contains("short-a.txt"))
                        {
                            (string estring, bool ebool) esb = (paths[f].path, true);
                            paths[f] = esb;
                            vowelComparer = "a";
                        }
                    }
                    else if (shortV == ShortVo.I)
                    {
                        if (paths[f].path.Contains("short-i.txt"))
                        {
                            (string estring, bool ebool) esb = (paths[f].path, true);
                            paths[f] = esb;
                            vowelComparer = "i";
                        }
                    }
                    else if (shortV == ShortVo.U)
                    {
                        if (paths[f].path.Contains("short-u.txt"))
                        {
                            (string estring, bool ebool) esb = (paths[f].path, true);
                            paths[f] = esb;
                            vowelComparer = "u";
                        }
                    }
                    else if (shortV == ShortVo.E)
                    {
                        if (paths[f].path.Contains("short-e.txt"))
                        {
                            (string estring, bool ebool) esb = (paths[f].path, true);
                            paths[f] = esb;
                            vowelComparer = "e";
                        }
                    }
                    else if (shortV == ShortVo.O)
                    {
                        if (paths[f].path.Contains("short-o.txt"))
                        {
                            (string estring, bool ebool) esb = (paths[f].path, true);
                            paths[f] = esb;
                            vowelComparer = "o";
                        }
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