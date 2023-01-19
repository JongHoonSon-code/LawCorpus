using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;

namespace LawCorpus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                lblStatus.Content = "데이터 가져오는 중....";
            }));

            await GetData();
            lblStatus.Content = "데이터 가져오기 완료!!";
        }

        public async Task GetData()
        {            
            resultBox.ItemsSource = await GetAsyncData();
        }

        private static HttpClient serviceClient = new HttpClient()
        {
            BaseAddress = new Uri("http://www.law.go.kr/DRF/lawService.do"),
        };

        private async Task<List<DataDto>> GetAsyncData()
        {
            Common.hangDict = Common.CreateHangDict();
            Common.MogDict = Common.CreateMogDict();
            int pageNo = 0;
            int totalCount;

            string data = await GetData(pageNo);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(data);

            totalCount = Convert.ToInt32(doc.GetElementsByTagName("totalCnt")[0].InnerText);
            int pageCount = (int)Math.Ceiling((decimal)(totalCount / 100));

            await ListIter(doc.GetElementsByTagName("법령ID"));
            for (int i = 1; i <= pageCount; i++)
            {
                data = await GetData(i);
                doc.LoadXml(data);
                await ListIter(doc.GetElementsByTagName("법령ID"));
            }

            using (StreamWriter wr = new StreamWriter("result.tsv", false, Encoding.UTF8))
            {
                wr.WriteLine("Source,Target");

                foreach (var item in Common.sentences)
                {
                    wr.WriteLine("{0}\t{1}", item.Key, item.Value);
                }
            }

            totalCount = Common.sentences.Count + Common.headNotMatchCount + Common.existSentenceCount + Common.nothingEnDataCount + Common.lengthExceptCount + Common.addendExceptCount;
            List<DataDto> result = new List<DataDto>();
            result.Add(new DataDto("전체문장", 100, totalCount));
            result.Add(new DataDto("수집문장", CalcRate(Common.sentences.Count, totalCount), Common.sentences.Count));
            result.Add(new DataDto("문두번호 불일치 제외", CalcRate(Common.headNotMatchCount, totalCount), Common.headNotMatchCount));
            result.Add(new DataDto("중복문장 제외", CalcRate(Common.existSentenceCount, totalCount), Common.existSentenceCount));
            result.Add(new DataDto("번역문 없는 경우 제외", CalcRate(Common.nothingEnDataCount, totalCount), Common.nothingEnDataCount));
            result.Add(new DataDto("원문-번역문 길이 5배 이상 제외", CalcRate(Common.lengthExceptCount, totalCount), Common.lengthExceptCount));
            result.Add(new DataDto("부칙관련 제외", CalcRate(Common.addendExceptCount, totalCount), Common.addendExceptCount));
            result.Add(new DataDto("* 한자 포함 문장", CalcRate(Common.removeHanjaCount, totalCount), Common.removeHanjaCount));
            result.Add(new DataDto("* 특수문자 포함 문장", CalcRate(Common.specialCharCount, totalCount), Common.specialCharCount));
            
            return result;
        }

        private int CalcRate(int count, int totalCount)
        {
            if (count > 0)
            {
                return Convert.ToInt32(Convert.ToDecimal(count) / totalCount * 100);
            }
            else
            {
                return 0;
            }
        }

        private async Task<string> GetData(int pageNo)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://www.law.go.kr/DRF/lawSearch.do");
            HttpResponseMessage response = client.GetAsync($"?OC=john9005&target=elaw&type=XML&display=100&page={pageNo}").Result;
            string result = "";

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }

            return result;
        }

        private async Task ListIter(XmlNodeList list)
        {
            foreach (XmlNode item in list)
            {
                HttpResponseMessage response = serviceClient.GetAsync($"?OC=john9005&target=law&type=XML&ID={item.InnerText}").Result;
                string resultKo = "";

                if (response.IsSuccessStatusCode)
                {
                    resultKo = await response.Content.ReadAsStringAsync();
                }

                XmlDocument docKo = new XmlDocument();
                docKo.LoadXml(resultKo);

                response = serviceClient.GetAsync($"?OC=john9005&target=elaw&type=XML&ID={item.InnerText}").Result;
                string resultEn = "";

                if (response.IsSuccessStatusCode)
                {
                    resultEn = await response.Content.ReadAsStringAsync();
                }

                XmlDocument docEn = new XmlDocument();
                if (!string.IsNullOrEmpty(resultEn))
                {
                    docEn.LoadXml(resultEn);
                    Mapping(docKo, docEn);
                }
            }
        }

        private void Mapping(XmlDocument ko, XmlDocument en)
        {

            Regex regSentence = new Regex(@"(?<=[:;.?>)]\s|(?<=[\r\n|\r|\n]))([0-9\-]+[.)](?![0-9])|\(\s?[0-9]+\s?\)|\([a-zA-Z]+\))|^Article[\s\d]+");
            Regex regJo = new Regex(@"^제[\d\s]+조의\d+|^제[\d\s]+조|^Article \d+-\d+|^Article \d+");
            Regex regHang = new Regex(@"^[①②③④⑤⑥⑦⑧⑨⑩⑪⑫⑬⑭⑮⑯⑰⑱⑲⑳㉑㉒㉓㉔㉕㉖㉗㉘㉙㉚]|^\([\d]{1,2}\)");
            Regex regHo = new Regex(@"^\d+\.|^\d+\.");
            Regex regMog = new Regex(@"^[가-호]{1}\.|^\([a-zA-Z]\)");
            Regex regDigit = new Regex(@"\d+");
            Regex regRemove = new Regex(@" {2,}");
            MatchCollection matches;
            string koTitle;
            string koContents;
            string enTitle;
            string enContents;
            string temp;
            int cnt = 0;
            int enCnt = 0;
            int koJoNo;
            int enJoNo;

            foreach (XmlElement joUnit in ko.GetElementsByTagName("조문단위"))
            {
                koTitle = string.Empty;
                koContents = string.Empty;
                enTitle = string.Empty;
                enContents = string.Empty;
                matches = null;

                if (joUnit["조문제목"] != null)
                {
                    koTitle = joUnit["조문제목"].InnerText.Trim();
                }

                if (en.GetElementsByTagName("Jo")[cnt] == null)
                {
                    break;
                }

                koJoNo = Convert.ToInt32(joUnit["조문번호"].InnerText);
                enJoNo = Convert.ToInt32(en.GetElementsByTagName("Jo")[cnt]["joNo"].InnerText);

                //조번호가 안 맞을 경우 다음 조까지 당김
                if (!koJoNo.Equals(enJoNo))
                {
                    if (koJoNo > enJoNo)
                    {
                        for (int i = cnt + 1; i < en.GetElementsByTagName("Jo").Count; i++)
                        {
                            if (en.GetElementsByTagName("Jo")[i] == null)
                            {
                                break;
                            }

                            if (koJoNo.Equals(Convert.ToInt32(en.GetElementsByTagName("Jo")[i]["joNo"].InnerText)))
                            {
                                cnt = i;
                                enJoNo = Convert.ToInt32(en.GetElementsByTagName("Jo")[cnt]["joNo"].InnerText);
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                if (!koJoNo.Equals(enJoNo))
                {
                    continue;
                }

                if (en.GetElementsByTagName("Jo")[cnt]["joTtl"] != null)
                {
                    enTitle = en.GetElementsByTagName("Jo")[cnt]["joTtl"].InnerText.Trim();
                }

                if (!string.IsNullOrEmpty(koTitle) && !string.IsNullOrEmpty(enTitle))
                {
                    Common.SaveDict(koTitle, enTitle);
                }

                if (joUnit["조문내용"] != null)
                {
                    koContents = joUnit["조문내용"].InnerText.Trim();
                }

                if (en.GetElementsByTagName("Jo")[cnt]["joCts"] != null)
                {
                    temp = en.GetElementsByTagName("Jo")[cnt]["joCts"].InnerText.Trim();
                    temp = regRemove.Replace(temp, "");
                    matches = regSentence.Matches(temp);
                    if (matches.Count == 0)
                    {
                        if (koContents.StartsWith("제"))
                        {
                            if (temp.StartsWith("CHAPTER"))
                            {
                                Common.SaveDict(koContents, temp);
                            }
                        }
                        else
                            Common.SaveDict(koContents, temp);
                    }
                    else
                    {
                        enCnt = 0;
                        Common.HeadCheck(regJo, regDigit, koContents, Common.GetMatchValue(matches, enCnt, temp));
                        enCnt++;

                        if (joUnit.InnerText.Contains("① 식품의약품안전처장은 법 제32조의2제1항 각 호 외의 부분에 따른 현지실사(이하"))
                        {

                        }

                        if (joUnit["항"] != null)
                        {
                            for (int i = 0; i < joUnit.ChildNodes.Count; i++)
                            {
                                try
                                {
                                    if (joUnit.ChildNodes[i].Name == "항")
                                    {
                                        for (int j = 0; j < joUnit.ChildNodes[i].ChildNodes.Count; j++)
                                        {
                                            if (joUnit.ChildNodes[i].ChildNodes[j].Name == "항내용")
                                            {
                                                koContents = joUnit.ChildNodes[i].ChildNodes[j].InnerText.Trim();

                                                Common.HangHeadCheck(regHang, koContents, Common.GetMatchValue(matches, enCnt, temp));
                                                enCnt++;
                                            }
                                            else if (joUnit.ChildNodes[i].ChildNodes[j].Name == "호")
                                            {
                                                for (int k = 0; k < joUnit.ChildNodes[i].ChildNodes[j].ChildNodes.Count; k++)
                                                {
                                                    if (joUnit.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "호내용")
                                                    {
                                                        koContents = joUnit.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerText.Trim();

                                                        Common.HeadCheck(regHo, regDigit, koContents, Common.GetMatchValue(matches, enCnt, temp));
                                                        enCnt++;
                                                    }
                                                    else if (joUnit.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "목")
                                                    {
                                                        for (int l = 0; l < joUnit.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes.Count; l++)
                                                        {
                                                            if (joUnit.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "목내용")
                                                            {
                                                                koContents = joUnit.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].InnerText.Trim();
                                                                string[] ko_mog_list = koContents.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                                                                if (ko_mog_list.Length == 1)
                                                                {
                                                                    Common.MogHeadCheck(regMog, koContents, Common.GetMatchValue(matches, enCnt, temp));
                                                                    enCnt++;
                                                                }
                                                                else
                                                                {
                                                                    foreach (var item in ko_mog_list)
                                                                    {
                                                                        Common.MogHeadCheck(regMog, item, Common.GetMatchValue(matches, enCnt, temp));
                                                                        enCnt++;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception Ex)
                                {

                                }

                            }
                        }
                    }
                }

                cnt++;
            }

            cnt = 0;
            string enTemp;
            string[] koAddendList;
            string[] enAddendListTemp;
            List<string> enAddendList;
            foreach (XmlElement addendUnit in ko.GetElementsByTagName("부칙단위"))
            {
                if (en.GetElementsByTagName("Ar")[cnt] == null)
                {
                    break;
                }
                enCnt = 0;
                koJoNo = Convert.ToInt32(addendUnit["부칙공포번호"].InnerText);
                enJoNo = Convert.ToInt32(en.GetElementsByTagName("Ar")[cnt]["arAncNo"].InnerText);

                //부칙번호가 안 맞을 경우 다음 조까지 당김
                if (!koJoNo.Equals(enJoNo))
                {
                    if (koJoNo > enJoNo)
                    {
                        for (int i = cnt + 1; i < en.GetElementsByTagName("Ar").Count; i++)
                        {
                            if (en.GetElementsByTagName("Ar")[i] == null)
                            {
                                break;
                            }

                            if (koJoNo.Equals(Convert.ToInt32(en.GetElementsByTagName("Ar")[i]["arAncNo"].InnerText)))
                            {
                                cnt = i;
                                enJoNo = Convert.ToInt32(en.GetElementsByTagName("Jo")[cnt]["joNo"].InnerText);
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                if (!koJoNo.Equals(enJoNo))
                {
                    continue;
                }

                temp = addendUnit["부칙내용"].InnerText;
                enTemp = en.GetElementsByTagName("Ar")[cnt]["arCts"].InnerText;

                koAddendList = temp.Split('\n');
                enAddendListTemp = enTemp.Split('\n');
                enAddendList = new List<string>();

                bool isArticle = false;
                for (int i = 0; i < enAddendListTemp.Length; i++)
                {
                    if (enAddendListTemp[i].Trim().StartsWith("Article"))
                    {
                        isArticle = true;
                        enAddendList.Add(enAddendListTemp[i].Trim());
                    }
                    else
                    {
                        if (isArticle)
                        {
                            enAddendList[enAddendList.Count - 1] = $"{enAddendListTemp[i - 1]} {enAddendListTemp[i].Trim()}";
                        }
                        else
                            enAddendList.Add(enAddendListTemp[i].Trim());
                        isArticle = false;
                    }
                }

                foreach (var koAddend in koAddendList)
                {
                    for (int i = enCnt; i < enAddendList.Count; i++)
                    {
                        if (Common.AddEndMatched(regJo, regHang, regHo, regDigit, koAddend.Trim(), enAddendList, i))
                        {
                            enCnt++;
                            break;
                        }
                    }
                }

                cnt++;
            }
        }        
    }

    public class DataDto
    {
        public DataDto(string type, int rate, int count)
        {
            Type = type;
            Rate = rate;
            Count = count;
        }

        public string Type { get; set; }
        public int Rate { get; set; }
        public int Count { get; set; }
    }
    //public class DataDto
    //{
    //    public DataDto(int totalCount, int sentenceCount, int headNotMatchCount, int existSentenceCount, int nothingEnDataCount, int lengthExceptCount,
    //        int addendExceptCount, int removeHanjaCount, int specialCharCount)
    //    {
    //        TotalCount = totalCount;
    //        SentenceCount = sentenceCount;
    //        HeadNotMatchCount = headNotMatchCount;
    //        ExistSentenceCount = existSentenceCount;
    //        NothingEnDataCount = nothingEnDataCount;
    //        LengthExceptCount = lengthExceptCount;
    //        AddendExceptCount = addendExceptCount;
    //        RemoveHanjaCount = removeHanjaCount;
    //        SpecialCharCount = specialCharCount;
    //    }
    //    public int TotalCount { get; set; }
    //    public int SentenceCount { get; set; }
    //    public int HeadNotMatchCount { get; set; }
    //    public int ExistSentenceCount { get; set; }
    //    public int NothingEnDataCount { get; set; }
    //    public int LengthExceptCount { get; set; }
    //    public int AddendExceptCount { get; set; }
    //    public int RemoveHanjaCount { get; set; }
    //    public int SpecialCharCount { get; set; }
    //}
}
