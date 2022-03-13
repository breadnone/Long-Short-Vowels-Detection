using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;

namespace VVowels
{
    public class LongVowels
    {
        private static StringComparison sc = StringComparison.OrdinalIgnoreCase;
        private static string path = Directory.GetCurrentDirectory() + "/dictionary/";
        public double minValLong { get; set; } = 0.2;
        public virtual int vbounds { get; set; } = 2;

        public (double value, string vFormatValue) VLongVowel(string str, LongVo longV)
        {
            var t = (0.0, string.Empty);

            if (!String.IsNullOrEmpty(str))
            {
                if (str[str.Length - 1] == ' ')
                {
                    int e = str.Length - 2;
                    str = str[0..e];
                }
                if (str[0] == ' ')
                {
                    int e = str.Length - 1;
                    str = str[1..e];
                }

                var wordChuncks = str.Split(' ');
                var formatt = new string[wordChuncks.Length];
                double tValue = 0.0;

                for (int i = 0; i < wordChuncks.Length; i++)
                {
                    var v = LongVowelAll(wordChuncks[i], longV);
                    formatt[i] = v.ToString("0.0000") + " || " + wordChuncks[i] + " || ";
                    tValue += v;
                }

                //join 
                if (formatt.Length > 0)
                {
                    t = (tValue, String.Join(" ", formatt));
                }

            }
            return t;
        }
        private static int pathCounter = 0;
        private static List<(string path, bool enable)> paths = new List<(string path, bool enable)>();
        public double LongVowelAll(string str, LongVo longV)
        {
            var val = 0.0;

            LVClass lvc = new LVClass();

            if (paths.Count == 0)
            {
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
            }
            else
            {
                //Aight, I'm just being lazy here :)
                for (var f = 0; f < paths.Count; f++)
                {
                    (string estring, bool ebool) esb = (paths[f].path, false);
                    paths[f] = esb;
                }
                for (var f = 0; f < paths.Count; f++)
                {
                    if (longV == LongVo.A)
                    {
                        if (paths[f].path.Contains("long-a.txt"))
                        {
                            (string estring, bool ebool) esb = (paths[f].path, true);
                            paths[f] = esb;
                        }
                    }
                    else if (longV == LongVo.I)
                    {
                        if (paths[f].path.Contains("long-i.txt"))
                        {
                            (string estring, bool ebool) esb = (paths[f].path, true);
                            paths[f] = esb;
                        }
                    }
                    else if (longV == LongVo.U)
                    {
                        if (paths[f].path.Contains("long-u.txt"))
                        {
                            (string estring, bool ebool) esb = (paths[f].path, true);
                            paths[f] = esb;
                        }
                    }
                    else if (longV == LongVo.E)
                    {
                        if (paths[f].path.Contains("long-e.txt"))
                        {
                            (string estring, bool ebool) esb = (paths[f].path, true);
                            paths[f] = esb;
                        }
                    }
                    else if (longV == LongVo.O)
                    {
                        if (paths[f].path.Contains("long-o.txt"))
                        {
                            (string estring, bool ebool) esb = (paths[f].path, true);
                            paths[f] = esb;
                        }
                    }
                }
            }
            foreach (var fpath in paths)
            {
                if (File.Exists(path + fpath.path) && fpath.enable)
                {
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
                                    if (i + 2 <= cutE.Length - 1)
                                    {
                                        var yy = cutE[i] + " " + cutE[i + 2];
                                        Console.WriteLine(cutE[i] + "|||||||||||||||" + cutE[i + 2]);
                                        if (str[i] == yy[0] && str[i + 2] == cutE[i + 2])
                                            val += 0.2;
                                    }

                                    var sInput = str.Substring(i, vbounds);
                                    var sDict = cutE.Substring(i, vbounds);

                                    Console.WriteLine("AA : " + sInput);
                                    Console.WriteLine("BB : " + sDict);

                                    if (sInput.Equals(sDict, sc))
                                    {
                                        val += 0.2;
                                        Console.WriteLine("simillar +3 sequence match : " + sDict);
                                    }
                                }
                                else
                                {
                                    val += 0;
                                }
                            }
                        }
                        if (str.Equals(e, sc))
                            val += 0.4;
                    }
                }
            }
            vbounds = 2;
            return val / (double)100;
        }
    }
}