using LawCorpus.Dto;
using Microsoft.Win32;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LawCorpus
{
    /// <summary>
    /// Translation.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Translation : Window
    {
        private string googleKey;
        private string papagoKey;
        private string papagoKeyId;
        private string bleuUrl;
        private string transUrl;
        public Translation()
        {
            InitializeComponent();            
            googleKey = "Your google key";
            papagoKey = "Your papago key";
            papagoKeyId = "Your papago key id";
            bleuUrl = "bleu URL";
            transUrl = "trans URL";
        }

        private async void trans_Click(object sender, RoutedEventArgs e)
        {
            var googleTrans = await GetGoogleTranslation(source.Text);
            var modelTrans = await GetTranslate(source.Text, modelType.Text);

            model.Text = modelTrans.text.Trim();
            google.Text = googleTrans.data.translations[0].translatedText;            

            if (!string.IsNullOrEmpty(target.Text.Trim()))
            {
                List<string> candidate = new();
                List<string> reference = new();

                candidate.Add(model.Text);
                candidate.Add(google.Text);
                reference.Add(target.Text);


                var bleuParam = new BleuParam()
                {
                    Candidate = candidate,
                    Reference = reference,
                    SplitType = 0
                };

                var bleuResult = await GetBleu(bleuParam);
                if (bleuResult.Count > 1)
                {
                    bleuModel.Text = $"{bleuResult[0].score:#.##}";
                    bleuGoogle.Text = $"{bleuResult[1].score:#.##}";
                }
            }
        }

        public async Task<GoogleDto> GetGoogleTranslation(string sentence)
        {
            GoogleDto result;
            string responseData;

            try
            {
                HttpClient client = new HttpClient();

                var buffer = Encoding.UTF8.GetBytes($"key={googleKey}&q={System.Web.HttpUtility.UrlEncode(sentence)}&source=ko&target=en&model=nmt");
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var response = await client.PostAsync("https://translation.googleapis.com/language/translate/v2", byteContent);

                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();
                    responseData = System.Web.HttpUtility.HtmlDecode(responseData);
                    result = JsonSerializer.Deserialize<GoogleDto>(responseData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    result = new GoogleDto();
                }
            }
            catch (Exception)
            {
                result = new GoogleDto();
            }
            
            return result;
        }

        public async Task<PapagoDto> GetPapagoTranslation(string sentence)
        {
            PapagoDto result;
            string responseData;

            try
            {
                HttpClient client = new HttpClient();

                var buffer = Encoding.UTF8.GetBytes($"source=ko&target=en&text={sentence}");
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                byteContent.Headers.Add("X-NCP-APIGW-API-KEY-ID", papagoKeyId);
                byteContent.Headers.Add("X-NCP-APIGW-API-KEY", papagoKey);

                var response = await client.PostAsync("https://naveropenapi.apigw.ntruss.com/nmt/v1/translation", byteContent);

                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();
                    responseData = System.Web.HttpUtility.HtmlDecode(responseData);
                    result = JsonSerializer.Deserialize<PapagoDto>(responseData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    result = new PapagoDto();
                }
            }
            catch (Exception)
            {
                result = new PapagoDto();
            }

            return result;
        }
        private async Task<TransformerModel> GetTranslate(string sentence, string modelType)
        {
            TransformerModel model;
            string responseData;

            HttpClient client = new HttpClient();

            var obj = new { sentence };
            var json = JsonSerializer.Serialize(obj);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            string port = "6000";
            if (modelType == "30-30")
            {
                port = "6001";
            }
            else if (modelType == "KoBART")
            {
                port = "6002";
            }

            var response = await client.PostAsync($"{transUrl}:{port}/predict", data);

            if (response.IsSuccessStatusCode)
            {
                responseData = await response.Content.ReadAsStringAsync();
                model = JsonSerializer.Deserialize<TransformerModel>(responseData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            else
            {
                model = new TransformerModel();
            }

            return model;
        }

        public async Task<List<BleuDto>> GetBleu(BleuParam param)
        {
            List<BleuDto> model;
            string responseData;

            HttpClient client = new HttpClient();

            var todoItemJson = new StringContent(
                JsonSerializer.Serialize(param),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync($"{bleuUrl}/GetBleuScore", todoItemJson);

            if (response.IsSuccessStatusCode)
            {
                responseData = await response.Content.ReadAsStringAsync();
                model = JsonSerializer.Deserialize<List<BleuDto>>(responseData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            else
            {
                model = new List<BleuDto>();
            }
            return model;
        }

        private async void batchTrans_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                if (System.IO.Path.GetExtension(openFileDialog.FileName).Equals(".txt"))
                {
                    string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                    TransformerModel trans;
                    //PapagoDto trans;
                    //GoogleDto trans;
                    StreamWriter resultFile = new StreamWriter(System.IO.Path.Combine(directoryPath, "result.txt"));
                    foreach (string line in File.ReadLines(openFileDialog.FileName))
                    {
                        trans = await GetTranslate(line, modelType.Text);
                        resultFile.WriteLine($"{line}\t{trans.text}");
                        //trans = await GetPapagoTranslation(line);
                        //resultFile.WriteLine($"{line}\t{trans.message.result.translatedText}");
                        //trans = await GetGoogleTranslation(line);
                        //resultFile.WriteLine($"{line}\t{trans.data.translations[0].translatedText}");
                    }
                    resultFile.Close();
                    MessageBox.Show("번역이 완료되었습니다.");
                }
                else
                    MessageBox.Show("txt 파일만 가능합니다.");
            }
        }

        private async void bleuCalc_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                if (System.IO.Path.GetExtension(openFileDialog.FileName).Equals(".xlsx"))
                {               
                    string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                    string fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                    string resultFileName = $"계산 결과_{DateTime.Now.ToShortDateString()}.xlsx";

                    using (var fs = new FileStream(System.IO.Path.Combine(directoryPath, resultFileName), FileMode.Create, FileAccess.Write))
                    {
                        IWorkbook workbook;
                        workbook = new XSSFWorkbook();
                        ISheet excelSheet = workbook.CreateSheet(resultFileName);
                        int rowNo = 0;
                        IRow createRow = excelSheet.CreateRow(rowNo);
                        createRow = excelSheet.CreateRow(rowNo);
                        createRow.CreateCell(0).SetCellValue("원문");
                        createRow.CreateCell(1).SetCellValue("정답문장1");
                        createRow.CreateCell(2).SetCellValue("정답문장2");
                        createRow.CreateCell(3).SetCellValue("정답문장3");
                        createRow.CreateCell(4).SetCellValue("ChatGPT");
                        createRow.CreateCell(5).SetCellValue("ChatGPT 점수");
                        createRow.CreateCell(6).SetCellValue("DeepL");
                        createRow.CreateCell(7).SetCellValue("DeepL 점수");
                        createRow.CreateCell(8).SetCellValue("Papago");
                        createRow.CreateCell(9).SetCellValue("Papago 점수");
                        createRow.CreateCell(10).SetCellValue("Google");
                        createRow.CreateCell(11).SetCellValue("Google 점수");
                        createRow.CreateCell(12).SetCellValue("Transformer");
                        createRow.CreateCell(13).SetCellValue("Transformer 점수");
                        createRow.CreateCell(14).SetCellValue("KoBART");
                        createRow.CreateCell(15).SetCellValue("KoBART 점수");
                        rowNo++;

                        ISheet sheet;
                        using (var stream = new FileStream(System.IO.Path.Combine(directoryPath, fileName), FileMode.Open))
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                            sheet = hssfwb.GetSheetAt(0);

                            List<BleuDto> model;
                            var param = new BleuParam();
                            param.Reference = new List<string>();
                            param.Candidate = new List<string>();
                            IRow row;
                            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                            {
                                row = sheet.GetRow(i);

                                param.Reference.Clear();
                                param.Candidate.Clear();
                                param.Reference.Add(row.GetCell(1).ToString().ToLower().Trim());
                                param.Reference.Add(row.GetCell(2).ToString().ToLower().Trim());
                                param.Reference.Add(row.GetCell(3).ToString().ToLower().Trim());
                                param.Candidate.Add(row.GetCell(4).ToString().ToLower().Trim());
                                param.Candidate.Add(row.GetCell(5).ToString().ToLower().Trim());
                                param.Candidate.Add(row.GetCell(6).ToString().ToLower().Trim());
                                param.Candidate.Add(row.GetCell(7).ToString().ToLower().Trim());
                                param.Candidate.Add(row.GetCell(8).ToString().ToLower().Trim());
                                param.Candidate.Add(row.GetCell(9).ToString().ToLower().Trim());

                                model = await GetBleu(param);

                                createRow = excelSheet.CreateRow(rowNo);                                
                                createRow.CreateCell(0).SetCellValue(row.GetCell(0).ToString());
                                createRow.CreateCell(1).SetCellValue(row.GetCell(1).ToString());
                                int cnt = 4;
                                rowNo++;                                
                                foreach (var item in model)
                                {
                                    createRow.CreateCell(cnt).SetCellValue(item.sentence);
                                    cnt++;
                                    createRow.CreateCell(cnt).SetCellType(CellType.Numeric);
                                    createRow.CreateCell(cnt).SetCellValue(item.score);
                                    cnt++;
                                }
                            }
                        }

                        workbook.Write(fs, true);
                    }

                #region [기존거]
                /*
                using (var fs = new FileStream(System.IO.Path.Combine(directoryPath, resultFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet(resultFileName);
                    int rowNo = 0;
                    IRow createRow = excelSheet.CreateRow(rowNo);

                    createRow.CreateCell(0).SetCellValue("원문");
                    createRow.CreateCell(1).SetCellValue("정답문장");
                    createRow.CreateCell(2).SetCellValue("평가문장");
                    createRow.CreateCell(3).SetCellValue("점수");

                    ISheet sheet;
                    using (var stream = new FileStream(System.IO.Path.Combine(directoryPath, fileName), FileMode.Open))
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);

                        List<BleuDto> model;
                        var param = new BleuParam();
                        param.Reference = new List<string>();
                        param.Candidate = new List<string>();
                        IRow row;
                        string url = string.Empty;
                        string source = string.Empty;
                        bool isCalc = false;
                        for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                        {
                            row = sheet.GetRow(i);                                
                            if (row == null) continue;
                            if (param.Reference.Count > 0 && param.Candidate.Count > 0)
                            {
                                if (isCalc && row.GetCell(0) != null)
                                {
                                    model = await GetBleu(param);
                                    int cnt = 0;
                                    foreach (var item in model)
                                    {
                                        rowNo++;
                                        createRow = excelSheet.CreateRow(rowNo);
                                        if (cnt == 0)
                                        {
                                            createRow.CreateCell(0).SetCellValue(source);
                                        }
                                        if (param.Reference.Count > cnt)
                                        {
                                            createRow.CreateCell(1).SetCellValue(param.Reference[cnt]);
                                        }
                                        cnt++;
                                        createRow.CreateCell(2).SetCellValue(item.sentence);
                                        createRow.CreateCell(3).SetCellType(CellType.Numeric);
                                        createRow.CreateCell(3).SetCellValue(item.score);
                                    }
                                    if (row.GetCell(0).ToString().Equals("End"))
                                    {
                                        break;
                                    }
                                }
                            }
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                            if (row.GetCell(2) != null)
                            {                                    
                                if (row.GetCell(0) != null)
                                {
                                    source = row.GetCell(0).ToString();
                                    param.Reference.Clear();
                                    param.Candidate.Clear();
                                    param.SplitType = 0;
                                    isCalc = false;
                                }
                                if (row.GetCell(1) != null)
                                {
                                    param.Reference.Add(row.GetCell(1).ToString());
                                }
                                param.Candidate.Add(row.GetCell(2).ToString());
                                isCalc = true;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    workbook.Write(fs, true);
                }
                */
                #endregion

                MessageBox.Show("계산이 완료되었습니다.");
                }
                else
                    MessageBox.Show("xlsx 파일만 가능합니다.");
            }
        }

        private async void bleuFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                if (System.IO.Path.GetExtension(openFileDialog.FileName).Equals(".txt"))
                {
                    string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                    string resultFileName = $"BLEU 계산용.xlsx";
                    TransformerModel trans30_30;
                    TransformerModel trans20_20;
                    TransformerModel trans15_15;
                    GoogleDto google;
                    StreamWriter resultFile = new StreamWriter(System.IO.Path.Combine(directoryPath, "result.txt"));

                    using (var fs = new FileStream(System.IO.Path.Combine(directoryPath, resultFileName), FileMode.Create, FileAccess.Write))
                    {
                        IWorkbook workbook;
                        workbook = new XSSFWorkbook();
                        ISheet excelSheet = workbook.CreateSheet(resultFileName);
                        int rowNo = 0;
                        IRow createRow = excelSheet.CreateRow(rowNo);

                        createRow.CreateCell(0).SetCellValue("원문");
                        createRow.CreateCell(1).SetCellValue("정답문장");
                        createRow.CreateCell(2).SetCellValue("평가문장");
                        createRow.CreateCell(3).SetCellValue("점수");

                        foreach (string line in File.ReadLines(openFileDialog.FileName))
                        {
                            try
                            {                                
                                string[] temp;
                                temp = line.Split('\t', StringSplitOptions.RemoveEmptyEntries);
                                if (temp.Length == 2)
                                {
                                    temp[0] = temp[0].Trim();
                                    trans30_30 = await GetTranslate(temp[0], "30-30");
                                    trans20_20 = await GetTranslate(temp[0], "KoBART");
                                    //trans15_15 = await GetTranslate(temp[0], "15-15");
                                    google = await GetGoogleTranslation(temp[0]);

                                    rowNo++;
                                    createRow = excelSheet.CreateRow(rowNo);
                                    createRow.CreateCell(0).SetCellValue(temp[0]);
                                    createRow.CreateCell(1).SetCellValue(temp[1]);
                                    createRow.CreateCell(2).SetCellValue(google.data.translations[0].translatedText);
                                    rowNo++;
                                    createRow = excelSheet.CreateRow(rowNo);
                                    createRow.CreateCell(2).SetCellValue(trans30_30.text.Trim());
                                    rowNo++;
                                    createRow = excelSheet.CreateRow(rowNo);
                                    createRow.CreateCell(2).SetCellValue(trans20_20.text.Trim());
                                    //rowNo++;
                                    //createRow = excelSheet.CreateRow(rowNo);
                                    //createRow.CreateCell(2).SetCellValue(trans15_15.text);
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                        workbook.Write(fs, true);
                    }
                    resultFile.Close();
                    MessageBox.Show("파일 생성이 완료되었습니다.");
                }
                else
                    MessageBox.Show("txt 파일만 가능합니다.");
            }
        }
    }
}
