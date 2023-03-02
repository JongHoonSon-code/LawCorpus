﻿using Microsoft.Win32;
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

                Dictionary<string, int> chkDic = new Dictionary<string, int>();
                chkDic.Add(@"피보증인의 직위변경에 대한 사용자의 통지의무 해태의 효과", 0);
                chkDic.Add(@"피보증인인 회사 이사의 처남인 관계에서 연대보증계약의 포괄적 위임을 한 경우 위 이사의 퇴임 후에는 그 위임이 철회된 것으로 본 사례", 0);
                chkDic.Add(@"피보험차량을 양수받아 그 명의로 차량이전등록을 마친 양수인이 자동차종합보험 보통약관상 피보험자로 정한 기명피보험자의 승낙을 얻어 자동차를 사용 또는 관리중인 자에 해당하는지 여부(소극)", 0);
                chkDic.Add(@"피공탁자의 출급청구권이 채무자의 다른 채권자의 압류전부명령에 의하여 거부된 경우, 변제공탁의 효력유무", 0);
                chkDic.Add(@"필적감정 결과의 배척이 채증법칙위반이나 심리미진의 위법에 해당한다고 본 사례", 0);
                chkDic.Add(@"필수적 몰수·추징 규정이 적용되는 피고사건의 재판 가운데 몰수 또는 추징 부분만에 대하여 상소한 경우의 효력", 0);
                chkDic.Add(@"필수적 공동소송인 공유물분할청구소송에서, 공동소송인 중 1인에 소송요건의 흠이 있으면 전체 소송이 부적법하게 되는지 여부(적극)", 0);
                chkDic.Add(@"하급심에서 확정된 판결부분에 대한 경정이 상급법원에서 가능한 여부.", 0);
                chkDic.Add(@"하반기에 수입신고하면서 상반기 발행의 상공부장관 추천서를 첨부한 경우 할당관세율의 적용 가부", 0);
                chkDic.Add(@"하수급인 “갑”을 수급인인 피고 회사에서 파견한 현장 소장인양 표시하여 행동케 한 것이 상법 제24조 소정의 상호대여자와 상호사용자의 관계에 해당된다고 볼 수 있는지 여부", 0);
                chkDic.Add(@"하수급인을 수급인의 공사현장소장인 것처럼 행동하게 한 경우, 수급인의 명의대여자로서의 책임여부", 0);
                chkDic.Add(@"기존의 건축물이 제1항 각 호의 사유로 제71조부터 제80조까지, 제82조부터 제84조까지, 제84조의2, 제85조부터 제89조까지 및 「수산자원관리법 시행령」 제40조제1항에 따른 건축제한, 건폐율 또는 용적률 규정에 부적합하게 된 경우에도 해당 건축물의 기존 용도가 국토교통부령(수산자원보호구역의 경우에는 해양수산부령을 말한다)으로 정하는 바에 따라 확인되는 경우(기존 용도에 따른 영업을 폐업한 후 기존 용도 외의 용도로 사용되지 아니한 것으로 확인되는 경우를 포함한다)에는 업종을 변경하지 아니하는 경우에 한하여 기존 용도로 계속 사용할 수 있다.", 0);
                chkDic.Add(@"이 경우 기존의 건축물이 공장이나 제조업소인 경우로서 대기오염물질발생량 또는 폐수배출량이 「대기환경 보전법 시행령」 별표 1 및 「물환경보전법 시행령」 별표 13에 따른 사업장 종류별 대기오염물질발생량 또는 배출규모의 범위에서 증가하는 경우는 기존 용도로 사용하는 것으로 본다.", 0);
                chkDic.Add(@"<개정 2015. 7. 6., 2018. 1. 16.>", 0);
                chkDic.Add(@" 다음 각 호의 어느 하나에 해당하는 자(제1호부터 제5호까지의 어느 하나의 자에 해당하지 아니하게 된 날부터 1년이 경과하지 아니한 자를 포함한다)는 주식등에 대한 공개매수(제133조제1항의 공개매수를 말한다.이하 이 항에서 같다)의 실시 또는 중지에 관한 미공개정보(대통령령으로 정하는 방법에 따라 불특정 다수인이 알 수 있도록 공개되기 전의 것을 말한다.이하 이 항에서 같다)를 그 주식등과 관련된 특정증권등의 매매, 그 밖의 거래에 이용하거나 타인에게 이용하게 하여서는 아니 된다.", 0);
                chkDic.Add(@"다만, 공개매수를 하려는 자(이하 이 조에서 “공개매수예정자”라 한다)가 공개매수공고 이후에도 상당한 기간 동안 주식등을 보유하는 등 주식등에 대한 공개매수의 실시 또는 중지에 관한 미공개정보를 그 주식등과 관련된 특정증권등의 매매, 그 밖의 거래에 이용할 의사가 없다고 인정되는 경우에는 그러하지 아니하다.", 0);
                chkDic.Add(@"「여신전문금융업법」에 의한 신용카드업자(직불카드업자 및 기명식선불카드업자를 포함한다), 「전자금융거래법」에 따른 금융기관 및 전자금융업자(이하 이 조에서 ""신용카드업자등""이라 한다)는 신용카드회원ㆍ직불카드회원ㆍ기명식선불카드회원ㆍ직불전자지급수단이용자ㆍ기명식선불전자지급수단이용자ㆍ기명식전자화폐이용자(이하 이 항에서 ""신용카드회원등""이라 한다)가 신용카드등사용금액의 합계액 및 소득공제 대상금액이 기재된 확인서(이하 이 조에서 ""신용카드등사용금액확인서""라 한다)의 발급을 요청하는 경우에는 지체없이 이를 발급하여야 한다.", 0);
                chkDic.Add(@"<개정 2000. 1. 10., 2003. 12. 30., 2005. 2. 19., 2007. 2. 28., 2010. 2. 18.>", 0);
                chkDic.Add(@" 6급이하공개경쟁채용시험등의 제1차시험에서는 각 과목 만점의 40퍼센트 이상 득점한 사람 중에서 선발예정인원의 5배수의 범위에서 시험성적을 고려하여 점수가 높은 사람부터 차례로 합격자를 결정한다.", 0);
                chkDic.Add(@"다만, 7급 공개경쟁채용시험(이에 상당하는 외무공무원 공개경쟁채용시험을 포함한다)의 제1차시험에서는 별표 3에서 정한 영어능력검정시험과 별표 4에서 정한 한국사능력검정시험에서 각각 기준점수 및 기준등급 이상 취득한 사람으로서 영어과목과 한국사과목을 제외한 나머지 과목에서 각 과목 만점의 40퍼센트 이상 득점한 사람 중에서 선발예정인원의 10배수의 범위에서 시험성적을 고려하여 점수가 높은 사람부터 차례로 합격자를 결정한다.", 0);
                chkDic.Add(@"<개정 2015. 5. 6., 2018. 12. 18.>", 0);
                chkDic.Add(@" 거주자가 제1항제1호에 따른 공익사업의 시행자 및 같은 항 제2호에 따른 사업시행자(이하 이 조에서 ""사업시행자""라 한다)로 지정되기 전의 사업자(이하 이 항에서 ""지정 전 사업자""라 한다)에게 2년 이상 보유한 토지등(제1항제1호의 공익사업에 필요한 토지등 또는 같은 항 제2호에 따른 정비구역의 토지등을 말한다. 이하 이 항에서 같다)을 2015년 12월 31일 이전에 양도하고 해당 토지등을 양도한 날이 속하는 과세기간의 개인지방소득 과세표준신고(예정신고를 포함한다)를 법정신고기한까지 한 경우로서 지정 전 사업자가 그 토지등의 양도일부터 5년 이내에 사업시행자로 지정받은 경우에는 대통령령으로 정하는 바에 따라 제1항에 따른 양도소득분 개인지방소득세 감면을 받을 수 있다.", 0);
                chkDic.Add(@"이 경우 감면할 양도소득분 개인지방소득세의 계산은 감면율 등이 변경되더라도 양도 당시 법률에 따른다.", 0);
                chkDic.Add(@"부동산 또는 자동차(「자동차관리법」 제5조에 따라 등록된 자동차를 말한다.이하 이 항에서 같다) 매도용으로 인감증명서를 발급받으려는 자는 별지 제14호서식의 인감증명서 중 부동산 매수자 또는 자동차 매수자란에 기재하려는 부동산 또는 자동차 매수자의 성명·주소 및 주민등록번호(법인인 경우에는 법인명, 주된 사무소의 소재지 및 법인등록번호를 말한다)를 관계공무원에게 구술이나 서면으로 제공하고, 그 기재사항을 확인한 후 발급신청자 서명란에 서명한다.", 0);
                chkDic.Add(@"다만, 재외국민이 부동산 매도용으로 인감증명서를 발급받는 경우에는 별지 제13호서식의 세무서장 확인란에 이전할 부동산의 종류와 소재지를 기재하고, 소관증명청의 소재지 또는 부동산소재지를 관할하는 세무서장의 확인을 받아야 한다.", 0);
                chkDic.Add(@"<개정 2013.12.17., 2016.1.12.>", 0);
                chkDic.Add(@"법 제26조제1항제1호에 따른 보험급여액의 징수는 보험가입신고를 하여야 할 기한이 끝난 날의 다음 날부터 보험가입신고를 한 날까지의 기간 중에 발생한 재해에 대한 요양급여ㆍ휴업급여ㆍ장해급여ㆍ간병급여ㆍ유족급여ㆍ상병보상연금에 대하여 하며, 징수할 금액은 가입신고를 게을리한 기간 중에 발생한 재해에 대하여 지급 결정한 보험급여 금액의 100분의 50에 해당하는 금액(사업주가 가입신고를 게을리한 기간 중에 납부하여야 하였던 산재보험료의 5배를 초과할 수 없다)으로 한다.", 0);
                chkDic.Add(@"다만, 요양을 시작한 날(재해 발생과 동시에 사망한 경우에는 그 재해발생일)부터 1년이 되는 날이 속하는 달의 말일까지의 기간 중에 급여청구사유가 발생한 보험급여로 한정한다.", 0);
                chkDic.Add(@"<개정 2017. 12. 26.>", 0);
                chkDic.Add(@" 물류산업을 경영하는 중소기업인 법인(이하 이 조에서 ""제휴물류법인""이라 한다)의 주주(그 법인의 발행주식총수의 100분의 10 이상을 보유한 주주를 말한다. 이하 이 조에서 같다)가 소유하는 제휴물류법인의 주식을 다음 각 호의 요건을 갖추어 2009년 12월 31일 이전에 물류산업을 경영하는 다른 중소기업인 법인(「자본시장과 금융투자업에 관한 법률」에 따른 주권상장법인은 제외하며, 이하 이 조에서 ""제휴상대물류법인""이라 한다)이 보유한 자기주식과 교환하거나 제휴상대물류법인에 현물출자하고 그 법인으로부터 출자가액에 상당하는 주식을 새로 받음으로써 발생하는 양도차익에 대해서는 대통령령으로 정하는 바에 따라 그 주주가 주식교환 또는 현물출자(이하 이 조에서 ""주식교환등""이라 한다)로 인하여 취득한 제휴상대물류법인의 주식을 처분할 때까지 양도소득세의 과세를 이연받을 수 있다.", 0);
                chkDic.Add(@"<개정 2011. 12. 31.>", 0);
                chkDic.Add(@" 법 제61조제4항에서 ""대통령령으로 정하는 방법""이란 그 밖의 시설물 및 구축물(토지 또는 건물과 일괄하여 평가하는 것을 제외한다)에 대하여 그것을 다시 건축하거나 다시 취득할 경우에 소요되는 가액(이하 이 항에서 ""재취득가액등""이라 한다)에서 그것의 설치일부터 평가기준일까지의 기획재정부령으로 정하는 감가상각비상당액을 뺀 것을 말한다.", 0);
                chkDic.Add(@"이 경우 재취득가액등을 산정하기 어려운 경우에는 「지방세법 시행령」 제4조제1항에 따른 가액을 해당 시설물 및 구축물의 가액(「지방세법 시행령」 제6조 각 호에 규정된 특수부대설비에 대하여 「지방세법 시행령」 제4조제1항에 따라 해당 시설물 및 구축물과 별도로 평가한 가액이 있는 경우에는 이를 가산한 가액을 말한다)으로 할 수 있다.", 0);
                chkDic.Add(@"<개정 1998. 12. 31., 2003. 12. 30., 2005. 8. 5., 2008. 2. 29., 2010. 2. 18., 2010. 9. 20.>", 0);
                chkDic.Add(@" 제1항에 따른 수당은 제1항 각 호의 지급대상기간 중 징계처분(이 영의 적용을 받지 아니하는 다른 공무원의 신분에서 받은 징계처분을 포함한다)을 받은 공무원에게는 지급하지 아니하며, 신규 임용된 공무원과 직위해제나 휴직처분을 받은 공무원의 경우에는 해당 지급대상기간 중 공무원으로 실제 근무한 기간(직위해제 처분기간은 실제 근무하지 아니한 기간으로 보고, 「공무원보수규정」 제15조제1호ㆍ제4호ㆍ제5호 및 제6호에 따른 휴직기간과 공무상 질병 또는 부상에 따른 휴직기간 및 「교육공무원법」 제12조제1항제5호에 따라 특별채용된 사람의 사립학교 근무기간은 실제 근무한 기간으로 본다)에 따라 다음 계산식에 의해 지급하되, 실제 근무한 기간을 계산할 때 15일 이상은 1개월로 계산하고 15일 미만은 계산하지 아니한다.", 0);
                chkDic.Add(@"<개정 2010. 1. 7., 2017. 1. 6.>", 0);
                chkDic.Add(@" 경찰공무원의 징계는 징계위원회의 의결을 거쳐 징계위원회가 설치된 소속 기관의 장이 하되, 「국가공무원법」에 따라 국무총리 소속으로 설치된 징계위원회에서 의결한 징계는 경찰청장 또는 해양경찰청장이 한다.", 0);
                chkDic.Add(@"다만, 파면ㆍ해임ㆍ강등 및 정직은 징계위원회의 의결을 거쳐 해당 경찰공무원의 임용권자가 하되, 경무관 이상의 강등 및 정직과 경정 이상의 파면 및 해임은 경찰청장 또는 해양경찰청장의 제청으로 행정안전부장관 또는 해양수산부장관과 국무총리를 거쳐 대통령이 하고, 총경 및 경정의 강등 및 정직은 경찰청장 또는 해양경찰청장이 한다.", 0);
                chkDic.Add(@"다만, 시장이나 군수가 지방교통약자 이동편의 증진계획의 내용을 다른 교통 관련 계획에 반영하여 수립한 경우에는 국토교통부장관의 승인을 받아 해당 지방교통약자 이동편의 증진계획을 따로 수립하지 아니할 수 있다.", 0);
                chkDic.Add(@"<개정 2013. 3. 23.>", 0);
                chkDic.Add(@"법 제83조ㆍ법 제89조제1항제2호ㆍ법 제90조ㆍ법 제91조ㆍ법 제93조ㆍ법 제95조 및 법 제98조의 규정에 의하여 용도세율의 적용을 승인받은 물품이나 관세의 감면을 받은 물품을 당해 조항에 규정하는 기간 내에, 법 제107조의 규정에 의하여 관세의 분할납부의 승인을 얻은 물품을 그 분할납부기간 만료 전에 그 설치 또는 사용장소를 변경하고자 하는 때에는 변경전의 관할지 세관장에게 다음 각호의 사항을 기재한 설치 또는 사용장소변경신고서를 제출하고, 제출일부터 1월내에 변경된 설치 또는 사용장소에 이를 반입하여야 한다.", 0);
                chkDic.Add(@"다만, 재해ㆍ노사분규 등의 긴급한 사유로 자기소유의 국내의 다른 장소로 당해 물품의 설치 또는 사용장소를 변경하고자 하는 경우에는 관할지 세관장에게 신고하고, 변경된 설치 또는 사용장소에 반입한 후 1월 이내에 설치 또는 사용장소변경신고서를 제출하여야 한다.", 0);
                chkDic.Add(@"<개정 2017. 3. 27.>", 0);
                chkDic.Add(@"「국정감사 및 조사에 관한 법률」 제17조에 따른 징계사유에 해당할 때", 0);
                chkDic.Add(@"그 법인의 재무ㆍ회계ㆍ기획ㆍ연구개발에 관련된 업무에 종사하고 있는 직원", 0);
                chkDic.Add(@"전자등록기관의 상근임원은 계좌관리기관의 임직원이 아닌 사람이어야 한다.", 0);
                chkDic.Add(@"교류협력사업에 필요한 시설 경비 및 식재료의 보관·운송과 관련되는 경비", 0);
                chkDic.Add(@"「중소기업창업 지원법」 제2조제1호에 따른 창업을 위하여 전용하는 경우", 0);
                chkDic.Add(@"문화재의 연혁, 구조, 양식, 보존상태 및 주변상황 등 수리대상의 현황", 0);
                chkDic.Add(@"협회의 설립 후 임원이 임명될 때까지 필요한 업무는 발기인이 수행한다.", 0);
                chkDic.Add(@"「장애인복지법」에 따라 등록된 시각ㆍ청각장애인이 생활하는 가정의 수상기", 0);
                chkDic.Add(@"여비의 항목과 그 금액은 대법원규칙으로 정하는 범위에서 법원이 정한다.", 0);
                chkDic.Add(@"상임이사는 보건복지부령으로 정하는 추천 절차를 거쳐 이사장이 임명한다.", 0);
                chkDic.Add(@"가구유형ㆍ가구소득 등 장애인과 비장애인의 비교조사를 위하여 필요한 사항", 0);
                chkDic.Add(@"순환보직, 성과평가, 승진 등의 인사관리는 투명하고 공정하게 운영할 것", 0);
                chkDic.Add(@"기술이전ㆍ사업화를 촉진하기 위한 사업의 추진 및 기반 확충에 관한 사항", 0);
                chkDic.Add(@"별표 11에 따른 업무의 정지 또는 사용금지 처분기준과 과징금 부과기준", 0);
                chkDic.Add(@"원본증명기관 지정의 기준 및 절차에 필요한 사항은 대통령령으로 정한다.", 0);
                chkDic.Add(@"그 미생물에 관계되는 발명에 대한 특허출원이 공개되거나 설정등록된 경우", 0);
                chkDic.Add(@"법령에 규정된 중앙행정기관의 권한에 속하는 사무의 전수조사에 관한 사항", 0);
                chkDic.Add(@"제4조(토양오염조사기관 및 누출검사기관 지정권자 변경에 관한 경과조치)", 0);
                chkDic.Add(@"경쟁제한적인 법령 및 행정처분의 협의ㆍ조정등 경쟁촉진정책에 관한 사항", 0);
                chkDic.Add(@"제6조에 따른 특수임무유공자, 그 유족 또는 가족의 등록에 관한 사무", 0);
                chkDic.Add(@"공로자 또는 그 유족에 대한 제5조에 따른 공로금의 지급에 관한 사항", 0);
                chkDic.Add(@"제1항 본문에 따른 휴업 또는 휴지의 허가기준은 다음 각 호와 같다.", 0);
                chkDic.Add(@"법 제11조에 따른 주택조합 설립인가를 받은 날부터 3개월이 지난 날", 0);
                chkDic.Add(@"법 제81조제1항에 따른 자료ㆍ정보의 제공 요청 및 이용에 관한 사무", 0);
                chkDic.Add(@"그 밖에 농촌융복합산업 추진에 필요한 농림축산식품부령으로 정하는 사항", 0);
                chkDic.Add(@"여성농어업인의 안정적인 농어업생산활동 기반을 마련하기 위한 고충 상담", 0);
                chkDic.Add(@"등기관이 등기를 마친 경우 그 등기는 접수한 때부터 효력을 발생한다.", 0);
                chkDic.Add(@"민간투자유치를 의무화하는 사업에서 투자계약이 변경·무효화·양도된 경우", 0);
                chkDic.Add(@"동명이인의 가능성도 있으므로 당사자의 성명만으로는 당사자특정에 충분하다고 할 수 없고, 다른 정보들과 결합되어야 한다.", 0);
                chkDic.Add(@"근대 이전 봉건 시대의 토지사상으로 대표적인 것에는 서양의 분할 소유권 사상과 동양의 왕토사상이 있다.", 0);
                chkDic.Add(@"그러나 원천지국에서 전부 또는 부분적인 과세권을 가지고 있는 때에는 거주지국가는 이중과세를 배제시킬 의무가 있다.", 0);
                chkDic.Add(@"신규 유망 사업의 발굴을 통해 초일류 글로벌 기업으로의 도약을 위한 성장 기반을 마련할 것으로 전망된다.", 0);
                chkDic.Add(@"하원이 발의한 법안에서는 민간기업들과 연방 정부 간 정보 공유는 가능한 특정 국가 기관을 허브로 지정하지 않았다.", 0);
                chkDic.Add(@"전라북도의 지방세 비중이 인구비율에 비해 가장 낮은 모습을 보인다.", 0);
                chkDic.Add(@"이에 따라, 특허 분쟁 재판에 대한 승인 및 집행은 특허를 보호하는 국가의 국내법이 정하는 범위 내에서만 보호된다.", 0);
                chkDic.Add(@"드론에 카메라와 온도 측정 센서를 부착해서 해일 등의 재난을 예보하고 있다.", 0);
                chkDic.Add(@"증인 정은 을이 당일 현금이 부족하여 6,000 위안을 빌린 사실을 진술하였다.", 0);
                chkDic.Add(@"금융회사의 위험관리가 더욱 중요해지는 상황에서 그 취지는 타당하다.", 0);
                chkDic.Add(@"상법이 민법의 특별법임을 고려할 때 민법의 개정이 병행되어야 할 것이다.", 0);
                chkDic.Add(@"부패재산몰수법에서 제9조에서 제14조에서는 집행된 부패재산의 반환과 관련하여 규정하고 있다.", 0);
                chkDic.Add(@"말하자면 1965년 차고스 군도의 분리가 이번 권고적 의견의 요청에 이르게 된 근본 원인인 셈이다.", 0);
                chkDic.Add(@"실체법상의 권리 의무의 귀속 주체가 항상 소송의 당사자가 되는 것은 아니다.", 0);
                chkDic.Add(@"이사가 소신 있는 경영 판단을 할 수 있게 해 준다는 점에서 매우 유용한 책임제한 제도라할 수 있다.", 0);
                chkDic.Add(@"자금상의 한계에 부딪히는 것도 아니며, 비교적 큰 규모의 시장 활동에 대해서도 제소할 수 있다.", 0);
                chkDic.Add(@"다만, 미성년자의 삭제 청구권은 제3자의 게시물이 아닌 자신의 게시물에 한정된다.", 0);
                chkDic.Add(@"이는 잘못된 의료광고는 국민의 보건건과 건강권을 위협하며 회복 불가능한 손해를 야기할 수 있다.", 0);
                chkDic.Add(@"이는 우리나라 납세자들의 수직적 조세 형평성에 대한 부정적 인식이 성실한 납세의 정당성을 약화시키거나 탈세를 정당화하는 핑계로 널리 이용되지 않고 있음을 의미한다.", 0);
                chkDic.Add(@"대한항공과 아시아나항공은 각각 2008년 7월과 10월에 마일리지 유효기간제도를 도입했다.", 0);
                chkDic.Add(@"갑이 을을 상대로 금 1,000만 원의 대여금청구의 소를 제기하자 을은 변론기일에 그 돈을 받은 사실이 있다고 주장하였다.", 0);
                chkDic.Add(@"그렇다면, AAA의 인과 관계론에 대한 책임론설적 이해 방식은 단순한 실수가 아닌 것임은 분명해 보인다.", 0);
                chkDic.Add(@"독일 주식법의 경우, 일본 회사법과 같이 기업 집단의 내부 통제에 관하여 직선적인 표현을 하고 있지는 않다.", 0);
                chkDic.Add(@"역사적 제도주의에서는 국가 권력을 획득한 집단의 정책적 선호와 이를 관철시키는 불균등한 권력구조를 인식하려 한다.", 0);
                chkDic.Add(@"법관의 독립성은 법관 스스로가 지켜야 할 직무상의 특별한 의무이기도 하다.", 0);
                chkDic.Add(@"이때 표현이라고 하는 것에 대하여 회화를 예로 든다면 캔버스와 같은 화폭에 물감 등을 이용하여 붓과 같은 도구로 그려내는 것을 의미한다는 것에는 이견이 없을 것이다.", 0);
                chkDic.Add(@"이렇게 서로 다른 상호 법 구조를 비교하면서 차이점에 대하여 다름으로 인정하지 않고 옳고 그름으로 파악하는 것은 잘못되었다는 것이다.", 0);
                chkDic.Add(@"이는 도산 관련 법률의 필요성에 대한 인식 부족 등으로 인한 것이기도 하였다.", 0);
                chkDic.Add(@"시민이 이런 공동체 가치를 수용하고 실천하게 시민사회를 활성화하는 역할을 해야 한다.", 0);
                chkDic.Add(@"빅 데이터가 경제적 가치를 가지고, 산업계에서 그 가치를 인정받는 것은 사실이다.", 0);
                chkDic.Add(@"이사를 책임 위험으로부터 보호하여 적극적인 경영상 결정을 하도록 장려할 수 있기 때문이다.", 0);
                chkDic.Add(@"그만큼 학교자치에 관한 조례는 항상 위법성 논란의 중심에 놓여 있었다.", 0);
                chkDic.Add(@"모의고사는 로스쿨 실무 교육의 일응의 기준 척도 역할을 하고, 채점기준표는 학생들의 학습방향타 역할을 하고 있는 현실이다.", 0);
                chkDic.Add(@"조세법 위반행위도 변호사의 신뢰성과 적격성에 부정적인 영향을 주는 범죄행위에 포함된다.", 0);
                chkDic.Add(@"익명 처리 정보는 더 이상 개인정보가 되지 않아서 개인정보 보호법의 적용을 받지 않기 때문이다.", 0);
                chkDic.Add(@"이 경우에는 평등 심사를 먼저 할 것이 아니라, 관련 기본권이 중대하게 제한되는 경우이다.", 0);
                chkDic.Add(@"자체로 재산권으로서의 성질을 가지고 있고 다른 재산과 마찬가지로 처분하는 것이 당연히 인정되어 있다.", 0);
                chkDic.Add(@"B2B 전용 사이버몰이 아닌 일반 사이버몰에서의 거래의 경우, 사업자라 하더라도 일반 소비자와 동일한 지위 및 거래조건으로 거래하기 때문에 전자 상거래 소비자 보호법상 소비자에 해당하게 된다.", 0);
                chkDic.Add(@"청구권은 상대적 권리로서 특정 사인 간에 인정되기 때문에 그 성질상 타인의 청구권 보전을 위한 자구행위는 인정될 수 없다.", 0);
                chkDic.Add(@"중국의 신장웨이우얼 자치구 수도 우루무치시 사이바커구에 위치한 공원 인근 지역에서 2014년 5월 22일 오전 7시 50분께 폭발사고가 일어나 사상자 수는 최소 125명(사망 31명)에 이른다.", 0);
                chkDic.Add(@"정의된 범죄는 이미 존재하는 국제법의 규칙에 의해 범죄였다.", 0);
                chkDic.Add(@"무엇보다, 국민의 안정적인 노후를 책임져야 할 국민 연금이 기업 지배 구조의 개선을 주도해야 하는 근거를 찾기 어렵다.", 0);
                chkDic.Add(@"앞에서 말한 법 원칙은 형벌을 통해 천부적이고 극히 개인적인 권리를 박탈하는 것을 정당화할 수 없다.", 0);
                chkDic.Add(@"2006년에 청년의사에서 시행한 조사결과 봉직의의 일일 근무시간은 평균 10시간 25분이었다.", 0);
                chkDic.Add(@"사후적 판단보다는 개인 정보 보호에 지장이 없는 한도에서 AI, 빅데이터 자료를 활용하여 부정 목적의 다수 보험 가입을 사전에 차단하는 방안을 강구하여야 한다.", 0);
                chkDic.Add(@"형사법의 기본 원칙을 위반한 경우는 중과실에 해당한다고 볼 수 있기 때문이다.", 0);
                chkDic.Add(@"현행 지방세 체계에서 지방자치단체들이 자율적으로 지방세 정책을 적극 운영할 수 있는 현실적인 재량 여지는 어느 정도 있다고 생각하는가?", 0);
                chkDic.Add(@"이를 법적으로 보면, 시공의 기능을 넘어 시행의 영역에 해당되는 것들이었지만 구법 체계는 시공과 시행을 엄격하게 구별하지 않았다.", 0);
                chkDic.Add(@"가상의 경쟁 가격은 이름 그대로 가상의 가격일 뿐이다.", 0);
                chkDic.Add(@"인공 지능은 인간만의 고유한 영역으로 여겨지던 창작을 바탕으로 한 지식재산 관련 산업에까지 영역을 확장해 가고 있다.", 0);
                chkDic.Add(@"이때 사상이라는 손해에 대한 미필적 고의도 전혀 없었다면 상법 제659조의 면책사유로 볼 수 없다.", 0);
                chkDic.Add(@"형식적으로는 별개의 위헌 심사 기준이라 하면서도 실질적으로는 본질 침해 금지 원칙의 존재를 부인하는 상태에 있었다.", 0);
                chkDic.Add(@"반대로 중요한 일임에도 어느 부처도 맡지 않고 있는 사무는 무엇인지 찾아 포함시킬 필요도 있다.", 0);
                chkDic.Add(@"개인 정보는 이러한 우리 헌법의 과제를 달성하는 데 필요한 수단이 되고 있다.", 0);
                chkDic.Add(@"행위자 내지 일반인이 결과 발생의 조건이 될 피해자의 행위를 예견하기 어려운 것은 무엇보다 그 행위가 그의 의사 결정에 기초하고 있기 때문이다.", 0);
                chkDic.Add(@"이하에서는 개성 공단에서의 행정 형법 적용 여부를 검토하는 이 글의 주제와 관련하여, 행정 형법에 형법 제3조가 동일하게 적용되는지에 대해 형법 제8조의 해석론을 기초로 살펴보도록 하겠다.", 0);
                chkDic.Add(@"그와 같은 법률 지식이 없거나 부족한 일반 분쟁당사자에게 해당 분쟁해결에 필요한 법령과 관련하여 구체적인 용어를 사용하여 검색할 것을 기대할 수는 없다.", 0);
                chkDic.Add(@"비중요 정보처리시스템의 경우에는 예외적으로 클라우드 컴퓨팅 서비스 이용이 가능하다.", 0);
                chkDic.Add(@"매일 피해자 134명에게 총 12.2억 원의 피해가 발생한 것으로 1인당 평균 9.1백만 원의 피해를 입은 꼴이다.", 0);
                chkDic.Add(@"법인세법상 비영리법인을 비영리내국법인과 비영리외국법인으로 나누어 아래와 같이 정의하고 있다.", 0);
                chkDic.Add(@"이것은 심사 담당자들의 대부분이 의사들로 구성되어 있는 미국이나 유럽의 예와는 상당한 차이가 존재하는 대목이다.", 0);
                chkDic.Add(@"신용사기 전략의 강압적 측면과 신용사기 전략의 위협적인 요소는 극소화 전략을 통해 나타날 수 있다.", 0);
                chkDic.Add(@"조약 체결·비준에 대한 국회 동의권은 국회의 입법권과 더불어 조약 내용의 형성 및 심의 권한의 근거가 된다.", 0);
                chkDic.Add(@"누구도 세계화의 흐름을 가로 막지는 못할 것이다.", 0);
                chkDic.Add(@"일본 개정법이 새롭게도입한 절대적 효력설을 실제 판례 사안에 대입해 분석함으로써 우리 민법의 개정방향을설정하거나 검토하는데 도움이 될 것으로 생각된다.", 0);
                chkDic.Add(@"이에 따라 앞으로 BBB의 지분 확대와 관련하여 금융위원회의 대주주 적격성 판단이 주목된다.", 0);
                chkDic.Add(@"정기선사의 회생 절차 개시는 여러 관련 당사자들에게 불측의 손해를 야기하게 된다.", 0);
                chkDic.Add(@"승인번호 20번은 31개의 이용 허락 신청(저작물)에 대해 승인한 경우인데, 보상금은 각리 이용자가 한 것이다.", 0);
                chkDic.Add(@"감사 위원회 모범규준에서도 권고하고 있는 내부 감사 조직을 상법에 도입할 필요가 있다.", 0);
                chkDic.Add(@"특히, 만기 시 재연장이 가능하여 반영구적 자본과 유사한 성격이며 자기 자본 비율 산출 시 기본 자본에 해당한다.", 0);
                chkDic.Add(@"먼저, 부가 비배우자 사이의 인공수정을 동의하지 아니한 경우에 부의 친생부인권은 인정될 것이다.", 0);
                chkDic.Add(@"원천징수의무자에 대한 징수처분에 대해 원천납세의무자는 소의 이익이 없다.", 0);
                chkDic.Add(@"지역구 의석 수와 비례대표 의석수의 비율을 최소한 3:1 정도로 하여 조정하면 될 것으로 본다.", 0);
                chkDic.Add(@"다수 법관이 이러한 교육을 수료하겠다고 동의할 것인가라는 문제부터, 겨우 200만 정도의 체류외국인의 문제를 해결하기 위해 모든 법관이 이러한 교육을 받는다는 것은 지나친 낭비라고 볼 수도 있을 것이기 때문이다.", 0);
                chkDic.Add(@"미국에서는 보전 권리자의 귀책을 판단함에 있어 소극적, 적극적 귀책 기준을 적용하고 있다.", 0);
                chkDic.Add(@"이로 인해 피해자가 적어도 지속적인 공포감이나 두려움을 느끼게 된다.", 0);
                chkDic.Add(@"필요적 공범 내지 대향범, 양벌규정이 적용되는 사업주와 종업원 관계를 모두 포함하는 개념이다.", 0);
                chkDic.Add(@"앞에서 본 바와 같이 Aitcho 제도의 Barrientos 섬에 서식하는 이끼 층 한가운데에 관광객들이 밟고 다니는 길이 만들어졌다는 사실은 환경적, 생태적 피해가 현실적으로 발생하고 있다는 확실한 증거가 되고 있다.", 0);
                chkDic.Add(@"이로 인해 사람을 사망에 이르게 하면 장형 100대의 형벌과 사망자의 장례를 위한 은10냥을 강제징수한다.", 0);
                chkDic.Add(@"대법원 판례에서는 ""잔금이나 등기가 이행되지 않아 사실상 취득하지 못한 경우에는 당사자 간의 합의 해제로 계약이 소급하여 소멸하였다면 신고 납부 방식에 의해 취득세를 납다"" 고 판시하고 있다.", 0);
                chkDic.Add(@"현 대통령의 임기를 단축하기 위하여 부칙에 그 근거 조항을 두어 해결하자는 방안도 제시된다.", 0);
                chkDic.Add(@"판례도 일관되게 상법 제42조는 영업양도에만 적용된다 하고 있다.", 0);
                chkDic.Add(@"소송심의회는 위원장이 필요하다고 인정 할 때 위원장이 소집하며, 재적위원 과반수의 출석으로 개의하고, 출석위원 3분의 2이상의 찬성으로 의결한다.", 0);
                chkDic.Add(@"제4조에 따른 융자대상에 해당되는 자로서 융자를 받고자 하는 자는 별지 제3호서식의 중소기업육성기금 융자신청서를 작성하여 구청장에게 제출하여야 한다.", 0);
                chkDic.Add(@"「지방세기본법」 제135조에 따른 재산세 납세관리인의 신고 또는 납세관리인을 지정한 경우에는 별지 제14호 서식의 납세관리인 관리대장을 작성하여야 한다.", 0);
                chkDic.Add(@"“공영개발사업”이란 구청장인 시행자가 사업시행계획에 의한 공공시설을 정비하고, 직접 건축물정비 또는 주택 및 부대복리시설을 건설하여 공급하는 사업방법을 말한다.", 0);
                chkDic.Add(@"다만, 장애인 전용주차구획의 발견이 쉬운 경우에는 그러하지 아니하다.", 0);
                chkDic.Add(@"제1항에 따라 파견을 요청받은 관계기관의 장은 파견할 직원의 성명ㆍ소속ㆍ연락처 등을 기재한 근무자 명단을 대책본부장에게 신속하게 통보하여야 한다.", 0);
                chkDic.Add(@"관서의 장은 일상경비출납원으로 하여금 출납폐쇄기한 경과 후 20일 이내에 지급실적보고서(별지 제110호서식)를 작성하게 하여 이를 주관국장·단장에게 보고하여야 한다.", 0);
                chkDic.Add(@"구청장은 제6조에 따라 지원 신청한 사업 등을 심의·의결하기 위하여 위원장과 부위원장을 포함한 15명 이내의 위원으로 위원회를 구성·운영한다.", 0);
                chkDic.Add(@"서울특별시관악구청장(이하 “구청장”이라 한다)은 당해 기관의 업무 성격과 고객의 특성에 따라 부서별·업무분야별로 동시 또는 단계적으로 헌장을 제정하여야 한다.", 0);
                chkDic.Add(@"제12조제1항 단서조항 및 각 호에 따라 비용추계서 작성을 생략하는 경우에는 별지 제2호서식의 비용추계서 미첨부 사유서를 제출하여야 한다.", 0);
                chkDic.Add(@"구청장은 「식생활교육지원법」(이하 “법”이라 한다) 제16조 규정에 따라 5년마다 관악구 식생활 교육 계획(이하 “구 계획”이라 한다)을 수립하여야 한다.", 0);
                chkDic.Add(@"구청장은 시행규칙 제4조에 따라 업무상 조정·감독을 받는 민간인에 대한 비밀취급인가 시에는 사전에 국가정보원장과 협의 보안대책을 강구하여야 한다.", 0);
                chkDic.Add(@"구청장은 환경미화원 대표와의 서면합의에 의해 월차유급 휴가일 또는 연차유급 휴가일에 갈음하여 특정 근로일에 환경미화원을 휴무시킬 수 있다.", 0);
                chkDic.Add(@"주관과장이 시설공사를 시행하고자 할 때에는 설계지침서에 의하여 설계서를 작성하여야 하며 서울특별시건설심의위원회조례에서 정하는 바에 따라 심사를 받아야 한다.", 0);
                chkDic.Add(@"센터의 장은 장애인으로 하고, 종사자 자격기준 및 센터의 운영에 필요한 사항은 법 시행규칙 제39조의2 및 제42조를 준용한다.", 0);
                chkDic.Add(@"“정책실명제 중점관리 대상사업”이란 구민의 관심이 높고 대외적 영향이 큰 사업 중 추진사항과 관련자를 공개하는 사업 등을 말한다.", 0);
                chkDic.Add(@"“발전사업”이란 「전기사업법」 제2조제3호에 따른 전기를 생산하여 이를 전력시장을 통하여 전기판매사업자에게 공급하는 것을 주된 목적으로 하는 사업을 말한다.", 0);
                chkDic.Add(@"“서울특별시 성동구 자기주도학습 지원센터”(이하 “지원센터”라 한다)란 서울특별시 성동구(이하 “구”라 한다)가 구민의 자기주도학습 지원을 위해 설치·운영하는 시설을 말한다.", 0);
                chkDic.Add(@"“구매목표율”이란 영 제28조제1항에 따라 우선구매 대상기관 등이 해당 연도에 구매하는 해당 품목의 총 구매물량에서 차지하는 비율을 말한다.", 0);
                chkDic.Add(@"“사용”이라 함은 「서울특별시 동대문구 자치회관 설치 및 운영 조례」(이하 “조례”라 한다) 제2조제1호의 자치회관 시설을 사용하는 행위를 말한다.", 0);
                chkDic.Add(@"“주민협의체”란 도시재생활성화지역의 도시재생 계획수립 및 사업시행 과정에 참여하고 적극적으로 의견을 제시하기 위하여 구성한 자발적인 주민 협력조직을 말한다.", 0);
                chkDic.Add(@"「외국인투자 촉진법」에 따른 외국인투자기업이 영 제32조 제3항에 따라 대부료를 납부하는 경우 이자율은 행정안전부장관이 정하여 고시하는 이자율로 한다.", 0);
                chkDic.Add(@"「지방재정법」(이하 “법”이라 한다) 제91조 및 「지방재정법 시행령」(이하 “영”이라 한다)제134조 제2항의 회계 관계 공무원은 다음 각호와 같이 지정한다.", 0);
                chkDic.Add(@"「지방재정법 시행령」제44조제2항에 따른 서울특별시 마포구 투자심사위원회(이하 “위원회”라 한다)는 위원장과 부위원장 각 1명을 포함하여 15명 이내의 위원으로 구성한다.", 0);
                chkDic.Add(@"각 자치회관은 조례 제5조제2항에 따라 지역특성에 맞는 프로그램을 2개 이상 선정·운영하여야 하며 상·하반기 실적을 구청장에게 보고하여야 한다.", 0);
                chkDic.Add(@"검사위원에게는 예산의 범위에서 수당을 지급할 수 있으며, 수당의 지급은 실제로 검사를 시작한 날부터 검사의견서를 제출한 날까지로 한다.", 0);
                chkDic.Add(@"다만, 민원인이 공개를 원하지 않을 경우에는 제외한다.", 0);
                chkDic.Add(@"공무원인 위원이 부득이한 사정으로 구도시계획위원회의 회의에 참석하지 못하는 경우에는 해당 기관의 소속 공무원이 대리하여 참석할 수 있다.", 0);
                chkDic.Add(@"피고가 2005. 2. 7.원고 및 나머지 선정자들에 대하여 한 개선명령을 취소한다.", 0);
                chkDic.Add(@"제1심 판결 중 반소에 관하여 환송 후 피고(반소원고)들이 변경한 청구에 따라 아래에서 지급을 명하는 금액에 해당하는 피고(반소원고)들의 패소 부분을 취소한다.", 0);
                chkDic.Add(@"피고인 1, 피고인 2, 피고인 3이 위 각 벌금을 납입하지 아니하는 경우 각 50,000원을 1일로 환산한 기간 위 피고인들을 노역장에 유치한다.", 0);
                chkDic.Add(@"납세고지서의 하자를 사전에 보완할 수 있는 서면의 요건", 0);
                chkDic.Add(@"납세고지서의 하자에 대한 구두 보정이 허용되는지 여부", 0);
                chkDic.Add(@"납세고지서의 공제액란에 일부 착오기재 내지 누락이 있는 경우, 납세고지의 효력", 0);
                chkDic.Add(@"납세고지서의 공시송달의 효력을 다투는 납세자의 주장에 대하여 이를 인정할 증거가 없다고 판시하는 것이 잘못인지 여부", 0);
                chkDic.Add(@"납세고지서의 과세귀속연도를 잘못 기재하였으나 심사청구 직후 착오기재에 대한 정정통지를 함으로써 과세처분의 하자가 치유되었다고 본 사례", 0);
                chkDic.Add(@"납세고지서나 납부통지서에 필요적 기재사항의 기재에 관한 입증책임", 0);
                chkDic.Add(@"납세고지서상 납세의무자의 확정이 과세관청의 사후확인에 따라 좌우되는지 여부", 0);
                chkDic.Add(@"납세고지서상의 납세의무자의 표시가 납세의무자의 동일성을 식별할 수 없을 정도로 불분명한 경우, 그 납세고지서의 송달이 적법한 납세고지로서의 효력을 갖는지 여부(소극)", 0);
                chkDic.Add(@"홍콩의 절도 법령[Theft Ordinance(Cap 210)]은 타인에게 속한 재물(재산)을 그로부터 영구히 박탈하려는 의도로 부정하게 착복하는 범죄에 대한 처벌규정을 두고 있다.", 0);
                chkDic.Add(@"피고인 1은 회계2파트 직원들의 임금이 지급되는 방식에 대해서 알지 못하였고, 위와 같은 방식의 임금 지급을 지시한 바도 없다.", 0);
                chkDic.Add(@"공소외 4 주식회사 측에서는 2009. 4.경 임대료까지 공소외 4 주식회사에서 부담하기로 약속한 마당에 굳이 공동사용 계약서까지 소급하여 작성할 하등의 이유나 필요는 없는 것으로 보인다.", 0);
                chkDic.Add(@"위 피고인들은 공소외 1 저축은행 또는 공소외 4 저축은행의 임직원으로서 위 각 은행의 사무를 처리하는 자의 지위에 있다.", 0);
                chkDic.Add(@"문자메시지를 보낸 경우는 반드시 통화내역에 남는데, 피고인이 피해자에게 문자메시지를 보낸 것이 통화내역에 없기는 하다.", 0);
                chkDic.Add(@"공소외 17은 트위터피드에 자신의 계정을 모두 등록해놓고 사용하였다고 하면서, 언론사 기사 등을 트위터 피드로 걸어놓으라는 지시가 있었다고 진술하였다.", 0);
                chkDic.Add(@"원고 소프트웨어를 사용하는 경우에도 라이선스 받은 동시 사용자 수를 넘는 사용자가 피고 소프트웨어를 사용하게 될 수는 없다.", 0);
                chkDic.Add(@"제가 회장으로 있고, 공소외 13이 대표이사로 있었는데, 피고인 2가 사용한 ‘고문’은 회장 바로 밑 대표이사 위 정도 된다고 보시면 된다.", 0);

                using (StreamWriter wr = new StreamWriter("valied_result.tsv", false, Encoding.UTF8))
                {
                    //wr.WriteLine("Source\tTarget");

                    foreach (var item in Common.sentences)
                    {
                        if (!chkDic.ContainsKey(item.Key))
                        {
                            wr.WriteLine("{0}\t{1}", item.Key, item.Value);
                        }
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
