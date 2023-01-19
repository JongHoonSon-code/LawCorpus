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
    /// SplitFile.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SplitFile : Window
    {
        public SplitFile()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);

                //validation 데이터 추출을 위한 랜덤값 생성(전체 데이터의 10%)
                Dictionary<int, int> validRows = new Dictionary<int, int>();
                int randValue;
                Random rand = new Random();
                while(validRows.Count < 130000)
                {
                    randValue = rand.Next(1300000);
                    if (!validRows.ContainsKey(randValue))
                    {
                        validRows.Add(randValue, 0);
                    }
                }

                int row = 0;
                StreamWriter trainFile = new StreamWriter(System.IO.Path.Combine(directoryPath, "train.txt"));
                StreamWriter validFile = new StreamWriter(System.IO.Path.Combine(directoryPath, "valid.txt"));
                //전체 데이터를 반복하며 랜덤생성한 값과 일치한 row는 valid.txt로 아니면 train.txt로 저장하여 학습/검증 데이터 분리
                foreach (string line in File.ReadLines(openFileDialog.FileName))
                {
                    if (validRows.ContainsKey(row))
                        validFile.WriteLine(line);
                    else
                        trainFile.WriteLine(line);
                    row++;
                }
                trainFile.Close();
                validFile.Close();

                MessageBox.Show("파일 분리가 완료되었습니다.");
            }
        }
    }
}
