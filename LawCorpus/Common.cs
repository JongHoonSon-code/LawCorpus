using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LawCorpus
{
    public static class Common
    {
        public static Regex regHanja = new Regex(@"\([\u2e80-\u2eff\u31c0-\u31ef\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fbf\uf900-\ufaff]+\)|[\u2e80-\u2eff\u31c0-\u31ef\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fbf\uf900-\ufaff]+|\([一-龥]+\)|[一-龥]+");
        public static Regex regSpecialChar = new Regex(@"&lt;|&gt;|<BR>|\n|\t|<img id=""\d""></img>|\[[a-zA-Z\s0-9.,]+\](?=[\r\n])|;$");
        public static Regex regSplit = new Regex(@"([\.:]\s)(?=(<|다만|다만,|단,|이 경우|Provided|In))");
        public static Regex regLength = new Regex(@" ㆍ");
        public static Dictionary<string, string> sentences = new Dictionary<string, string>();
        public static Dictionary<string, string> hangDict = new Dictionary<string, string>();
        public static Dictionary<string, string> MogDict = new Dictionary<string, string>();
        public static int headNotMatchCount = 0;
        public static int existSentenceCount = 0;
        public static int nothingEnDataCount = 0;
        public static int lengthExceptCount = 0;
        public static int addendExceptCount = 0;
        public static int removeHanjaCount = 0;
        public static int specialCharCount = 0;
        

        public static bool AddEndMatched(Regex regJo, Regex regHang, Regex regHo, Regex regDigit, string koAddend, List<string> enAddendList, int enCnt)
        {
            if (koAddend.Trim().StartsWith("부칙") && enAddendList[enCnt].StartsWith("ADDEND"))
            {
                SaveDict(koAddend, enAddendList[enCnt]);
                return true;
            }
            else if (regJo.IsMatch(koAddend) && regJo.IsMatch(enAddendList[enCnt]))
            {
                if (HeadCheck(regJo, regDigit, koAddend, enAddendList[enCnt]))
                    return true;
                else
                {
                    headNotMatchCount++;
                    return false;
                }
            }
            else if (regHang.IsMatch(koAddend) && regHang.IsMatch(enAddendList[enCnt]))
            {
                if (HangHeadCheck(regHang, koAddend, enAddendList[enCnt]))
                    return true;
                else
                {
                    headNotMatchCount++;
                    return false;
                }
            }
            else if (regHo.IsMatch(koAddend) && regHo.IsMatch(enAddendList[enCnt]))
            {
                if (HeadCheck(regJo, regDigit, koAddend, enAddendList[enCnt]))
                    return true;
                else
                {
                    headNotMatchCount++;
                    return false;
                }
            }
            addendExceptCount++;
            return false;
        }
        public static string GetMatchValue(MatchCollection matchs, int cnt, string temp)
        {
            if (matchs.Count <= cnt)
            {
                nothingEnDataCount++;
                return "";
            }
            if (matchs.Count > cnt + 1)
            {
                return temp.Substring(matchs[cnt].Index, matchs[cnt + 1].Index - matchs[cnt].Index).Trim();
            }
            else
                return temp.Substring(matchs[cnt].Index).Trim();
        }

        public static bool HeadNumCheck(Regex regDigit, string koHead, string enHead)
        {
            var koMatches = regDigit.Matches(koHead);
            var enMatches = regDigit.Matches(enHead);

            if (koMatches.Count == enMatches.Count)
            {
                for (int i = 0; i < koMatches.Count; i++)
                {
                    if (!koMatches[i].Value.Equals(enMatches[i].Value))
                    {
                        return false;
                    }
                }
                return true;
            }
            else return false;
        }

        public static bool HeadCheck(Regex reg, Regex regDigit, string koCon, string enCont)
        {
            string koHead = reg.Match(koCon).Value;
            string enHead = reg.Match(enCont).Value;
            if (HeadNumCheck(regDigit, koHead, enHead))
            {
                SaveDict(koCon, enCont);
                return true;
            }
            else
            {
                headNotMatchCount++;
                return false;
            }
        }

        public static bool HangHeadCheck(Regex reg, string koCon, string enCont)
        {
            string koHead = reg.Match(koCon).Value;
            string enHead = reg.Match(enCont).Value;
            if (hangDict.ContainsKey(koHead) && hangDict[koHead].Equals(enHead))
            {
                SaveDict(koCon, enCont);
                return true;
            }
            else
            {
                headNotMatchCount++;
                return false;
            }
        }

        public static bool MogHeadCheck(Regex reg, string koCon, string enCont)
        {
            string koHead = reg.Match(koCon).Value;
            string enHead = reg.Match(enCont).Value;
            if (MogDict.ContainsKey(koHead) && MogDict[koHead].Equals(enHead))
            {
                SaveDict(koCon, enCont);
                return true;
            }
            else
            {
                headNotMatchCount++;
                return false;
            }
        }

        public static void SaveDict(string koCon, string enCon)
        {
            if (regHanja.IsMatch(koCon))
            {
                koCon = regHanja.Replace(koCon, "");
                removeHanjaCount++;
            }
            if (regSpecialChar.IsMatch(koCon))
            {
                koCon = regSpecialChar.Replace(koCon, "");
                specialCharCount++;
            }
            if (regSpecialChar.IsMatch(enCon))
            {
                enCon = regSpecialChar.Replace(enCon, "");
                specialCharCount++;
            }

            koCon = koCon.Trim();
            enCon = enCon.Trim();
            string tempKo;
            string tempEn;

            if (regLength.Matches(koCon).Count * 5 >= regLength.Matches(enCon).Count && koCon.Length * 10 > enCon.Length && koCon.Length <= enCon.Length)
            {
                var koMaches = regSplit.Matches(koCon);
                var enMaches = regSplit.Matches(enCon);
                int koPos = 0;
                int enPos = 0;
                if (koMaches.Count > 0 && koMaches.Count == enMaches.Count)
                {
                    for (int i = 0; i < koMaches.Count; i++)
                    {
                        tempKo = koCon.Substring(koPos, koMaches[i].Index + 1 - koPos).Trim();
                        tempEn = enCon.Substring(enPos, enMaches[i].Index + 1 - enPos).Trim();
                        AddDic(tempKo, tempEn);

                        koPos = koMaches[i].Index + 1;
                        enPos = enMaches[i].Index + 1;
                        if (i + 1 == koMaches.Count)
                        {
                            tempKo = koCon.Substring(koMaches[i].Index + 1).Trim();
                            tempEn = enCon.Substring(enMaches[i].Index + 1).Trim();
                            AddDic(tempKo, tempEn);
                        }
                    }
                }
                else
                    AddDic(koCon, enCon);
            }
            else lengthExceptCount++;
        }

        public static void AddDic(string koCon, string enCon)
        {
            if (!sentences.ContainsKey(koCon))
            {
                if ((koCon.Length > 2000 || enCon.Length > 2000))
                {
                    lengthExceptCount++;
                }
                else
                    sentences.Add(koCon, enCon);
            }
            else
                existSentenceCount++;
        }

        public static Dictionary<string, string> CreateHangDict()
        {
            var dict = new Dictionary<string, string>();
            //dict.Add(@"^제[\d\s]+조", @"^Article [\d]+");
            dict.Add("①", "(1)");
            dict.Add("②", "(2)");
            dict.Add("③", "(3)");
            dict.Add("④", "(4)");
            dict.Add("⑤", "(5)");
            dict.Add("⑥", "(6)");
            dict.Add("⑦", "(7)");
            dict.Add("⑧", "(8)");
            dict.Add("⑨", "(9)");
            dict.Add("⑩", "(10)");
            dict.Add("⑪", "(11)");
            dict.Add("⑫", "(12)");
            dict.Add("⑬", "(13)");
            dict.Add("⑭", "(14)");
            dict.Add("⑮", "(15)");
            dict.Add("⑯", "(16)");
            dict.Add("⑰", "(17)");
            dict.Add("⑱", "(18)");
            dict.Add("⑲", "(19)");
            dict.Add("⑳", "(20)");
            dict.Add("㉑", "(21)");
            dict.Add("㉒", "(22)");
            dict.Add("㉓", "(23)");
            dict.Add("㉔", "(24)");
            dict.Add("㉕", "(25)");
            dict.Add("㉖", "(26)");
            dict.Add("㉗", "(27)");
            dict.Add("㉘", "(28)");
            dict.Add("㉙", "(29)");
            dict.Add("㉚", "(30)");
            //dict.Add(@)"^\d+\.", @"^\d+\.");
            //dict.Add(@"^[ㄱ-힣]{1}\.", @"^\([a-zA-Z]\)");

            return dict;
        }

        public static Dictionary<string, string> CreateMogDict()
        {
            var dict = new Dictionary<string, string>();
            dict.Add("가.", "(a)");
            dict.Add("나.", "(b)");
            dict.Add("다.", "(c)");
            dict.Add("라.", "(d)");
            dict.Add("마.", "(e)");
            dict.Add("바.", "(f)");
            dict.Add("사.", "(g)");
            dict.Add("아.", "(h)");
            dict.Add("자.", "(i)");
            dict.Add("차.", "(j)");
            dict.Add("카.", "(k)");
            dict.Add("타.", "(l)");
            dict.Add("파.", "(m)");
            dict.Add("하.", "(n)");
            dict.Add("거.", "(o)");
            dict.Add("너.", "(p)");
            dict.Add("더.", "(q)");
            dict.Add("러.", "(r)");
            dict.Add("머.", "(s)");
            dict.Add("버.", "(t)");
            dict.Add("서.", "(u)");
            dict.Add("어.", "(v)");
            dict.Add("저.", "(w)");
            dict.Add("처.", "(x)");
            dict.Add("커.", "(y)");
            dict.Add("터.", "(z)");
            dict.Add("퍼.", "(aa)");
            dict.Add("허.", "(bb)");
            dict.Add("고.", "(cc)");
            dict.Add("노.", "(dd)");
            dict.Add("도.", "(ee)");
            dict.Add("로.", "(ff)");
            dict.Add("모.", "(gg)");
            dict.Add("보.", "(hh)");
            dict.Add("소.", "(ii)");
            dict.Add("오.", "(jj)");
            dict.Add("조.", "(kk)");
            dict.Add("초.", "(ll)");
            dict.Add("코.", "(mm)");
            dict.Add("토.", "(nn)");
            dict.Add("포.", "(oo)");
            dict.Add("호.", "(pp)");

            return dict;
        }
    }
}
