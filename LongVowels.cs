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
                    if (!all)
                    {
                        var v = LongVowel(wordChuncks[i], longV, all);
                        formatt[i] = "LV = " + v.ToString("0.0000") + " || " + wordChuncks[i] + " || ";
                        tValue += v;
                    }
                    else
                    {
                        var v = LongVowel(wordChuncks[i], longV, all);
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
        private string[] vowlsList = new string[] { "a", "i", "u", "e", "o" };
        private void AllMode(LongVo longV)
        {
            SetDictionary(longV, true);
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

            if(all)
            AllMode(longV);

            foreach (var fpath in paths)
            {
                if (File.Exists(path + fpath.path) && fpath.enable)
                {
                    if(all)
                    {
                        if (fpath.path.Contains("long-a.txt", sc))                    
                            vowelComparer = "a";                    
                        else if (fpath.path.Contains("long-i.txt", sc))
                            vowelComparer = "i";
                        else if (fpath.path.Contains("long-u.txt", sc))
                            vowelComparer = "u";
                        else if (fpath.path.Contains("long-e.txt", sc))
                            vowelComparer = "e";
                        else if (fpath.path.Contains("long-o.txt", sc))
                            vowelComparer = "o";
                    }

                    foreach (var e in File.ReadLines(path + fpath.path))
                    {
                        if (!String.IsNullOrEmpty(e))
                        {
                            int strLen = str.Length;
                            string cutE = string.Empty;

                            if (e.Length > strLen)
                            {
                                cutE = e[0..strLen];
                            }
                            else
                            {
                                cutE = e;
                            }
                            for (int i = 0; i < cutE.Length; i++)
                            {
                                if (i + vbounds <= cutE.Length - 1)
                                {
                                    var sInput = str.Substring(i, vbounds);
                                    var sDict = cutE.Substring(i, vbounds);

                                    var midVow = char.ToLower(sInput[1]);

                                    Console.WriteLine("==========> MID : " + midVow);
                                    Console.WriteLine("IN : " + sInput);
                                    Console.WriteLine("OU : " + sDict);

                                    if (midVow == vowelComparer[0])
                                    {
                                        if (sInput[0] == sDict[0])
                                        {
                                            val += 0.2;
                                            Console.WriteLine("+3 sequence matching : " + sDict);
                                        }
                                        else
                                        {
                                            val += 0.05;
                                            Console.WriteLine("+3 sequence matching : " + sDict);
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
            if (!all)
            {
                for (var f = 0; f < paths.Count; f++)
                {

                    if (longV == LongVo.A)
                    {
                        if (paths[f].path.Contains("long-a.txt"))
                        {
                            (string estring, bool ebool) esb = (paths[f].path, true);
                            paths[f] = esb;
                            vowelComparer = "a";
                        }
                    }
                    else if (longV == LongVo.I)
                    {
                        if (paths[f].path.Contains("long-i.txt"))
                        {
                            (string estring, bool ebool) esb = (paths[f].path, true);
                            paths[f] = esb;
                            vowelComparer = "i";
                        }
                    }
                    else if (longV == LongVo.U)
                    {
                        if (paths[f].path.Contains("long-u.txt"))
                        {
                            (string estring, bool ebool) esb = (paths[f].path, true);
                            paths[f] = esb;
                            vowelComparer = "u";
                        }
                    }
                    else if (longV == LongVo.E)
                    {
                        if (paths[f].path.Contains("long-e.txt"))
                        {
                            (string estring, bool ebool) esb = (paths[f].path, true);
                            paths[f] = esb;
                            vowelComparer = "e";
                        }
                    }
                    else if (longV == LongVo.O)
                    {
                        if (paths[f].path.Contains("long-o.txt"))
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