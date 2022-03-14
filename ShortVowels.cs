using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
namespace VVowels
{
    public class ShortVowels
    {
        private StringComparison sc = StringComparison.OrdinalIgnoreCase;
        private string path = Directory.GetCurrentDirectory() + "/dictionary/";
        public virtual int vbounds { get; set; } = 3;
        public virtual int ybounds { get; set; } = 2;

        public (double value, string vFormatValue) VShortVowel(string str, ShortVo shortV)
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
                    var v = ShortVowel(wordChuncks[i], shortV);
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
        private int pathCounter = 0;
        private List<(string path, bool enable)> paths = new List<(string path, bool enable)>();
        private string vowelComparer = string.Empty;
        public double ShortVowel(string str, ShortVo shortV)
        {
            var val = 0.0;

            if (paths.Count == 0)
            {
                PoolDictionary();
            }
            else
            {
                SetDictionary(shortV);
            }

            if (!str.Contains(vowelComparer))
                return val += 0;

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
                                if (i + ybounds <= cutE.Length - 1)
                                {
                                    //this seems unnecessary, but it needs to skip the middel single character
                                    var yy = cutE[i + (ybounds - ybounds)] + " " + cutE[i + ybounds];

                                    Console.WriteLine(yy[0] + "|||||||||||||||" + cutE[i + ybounds]);

                                    if (str[i] == yy[0] && str[i + ybounds] == yy[ybounds])
                                        val += 0.2;
                                }

                                if (i + vbounds <= cutE.Length - 1)
                                {
                                    var sInput = str.Substring(i, vbounds);
                                    var sDict = cutE.Substring(i, vbounds);

                                    Console.WriteLine("AA : " + sInput);
                                    Console.WriteLine("BB : " + sDict);

                                    if (sInput.Contains(vowelComparer, sc) && sDict.Contains(vowelComparer, sc) && sInput.Equals(sDict, sc))
                                    {
                                        val += 0.2;
                                        Console.WriteLine("+3 sequence matching : " + sDict);
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

            DefaultStat();
            return val / (double)100;
        }

        private void DefaultStat()
        {
            vbounds = 2;
            ybounds = 2;
            vowelComparer = string.Empty;

            //Aight, tbh I'm just being lazy here :)
            for (var f = 0; f < paths.Count; f++)
            {
                (string estring, bool ebool) esb = (paths[f].path, false);
                paths[f] = esb;
            }
        }

        private void PoolDictionary()
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
        }
        private void SetDictionary(ShortVo shortV)
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
    }
}