using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    /// Validation.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Validation : Window
    {
        public Validation()
        {
            InitializeComponent();
        }

        private void btnValidation_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                string[] temp;
                Common.hangDict = Common.CreateHangDict();
                Common.MogDict = Common.CreateMogDict();

                char delimeter = Convert.ToChar(9);
                //전체 데이터를 반복하며 랜덤생성한 값과 일치한 row는 valid.txt로 아니면 train.txt로 저장하여 학습/검증 데이터 분리
                foreach (string line in File.ReadLines(openFileDialog.FileName))
                {
                    
                    temp = line.Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
                    if (temp.Length == 2)
                    {
                        Common.SaveDict(temp[0], temp[1]);
                    }
                    
                }

                using (StreamWriter wr = new StreamWriter("valied_result.tsv", false, Encoding.UTF8))
                {
                    wr.WriteLine("Source\tTarget");

                    foreach (var item in Common.sentences)
                    {
                        wr.WriteLine("{0}\t{1}", item.Key, item.Value);
                    }
                }

                using (StreamWriter wr = new StreamWriter("valied_result_var.txt", false, Encoding.UTF8))
                {
                    wr.WriteLine($"중복문장 제외:{Common.existSentenceCount}");
                    wr.WriteLine($"번역문 없는 경우 제외:{Common.nothingEnDataCount}");
                    wr.WriteLine($"원문-번역문 길이 차이 제외:{Common.lengthExceptCount}");
                    wr.WriteLine($"* 한자 포함 문장:{Common.removeHanjaCount}");
                    wr.WriteLine($"* 특수문자 포함 문장:{Common.specialCharCount}");
                }

                MessageBox.Show("파일 검증이 완료되었습니다.");
            }
        }
    }
}
