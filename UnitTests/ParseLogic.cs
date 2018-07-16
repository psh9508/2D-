using _2D보험구분검증툴.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests
{
    [TestFixture]
    public class ParseLogicTest
    {
        string[] _data = new string[]
        {
            @"MSH¿0.8.0.0¿001¿YSR2000¿20081117090000
FAC¿ys040203¿의사랑의원¿¿041-583-0123¿02-2105-5091¿
PRD¿김의사¿의사¿42409
PID¿의사랑¿7711111111111
ORC¿2008111711002¿¿23¿¿7¿¿저함량 배수처방 사유♬A45900471, 사유코드 : C
DG1¿ ¿ 
IN1¿1¿¿¿12345678¿70271724¿¿¿¿¿¿¿¿
RXD¿1¿2¿1¿661900010¿¿0.330¿3¿1¿¿
RXD¿1¿1¿1¿644900310¿¿2.500¿4¿1¿¿
RXD¿1¿1¿1¿643306160¿¿0.330¿3¿1¿¿
RXD¿1¿1¿1¿642403700¿¿0.330¿3¿1¿¿
RXD¿1¿1¿2¿641800840¿¿1.000¿1¿1¿¿
RXD¿1¿1¿2¿693200860¿¿1.000¿1¿1¿odw¿첫주는하루한두번두째주부터는1주에2번사용
RXD¿1¿1¿1¿644501350¿¿0.330¿3¿1¿¿",

            $@"MSH¿0.8.0.0¿001¿YSR2000¿99991231235999
FAC¿ys050203¿태건병원¿¿02-2105-5001¿¿
PRD¿안태건¿의사¿8988
PID¿급여2종¿3333333333337
ORC¿2017071805006¿¿03¿¿7¿¿
DG1¿      ¿      
IN1¿2¿¿2¿¿¿¿¿1¿¿¿¿¿
RXD¿1¿1¿1¿052400010¿¿0.3333¿3¿1¿¿",

            $@"MSH¿0.8.0.0¿001¿YSR2000¿99991231235999
FAC¿99355001¿태건병원¿¿02-2105-5001¿¿
PRD¿안태건¿의사¿8988
PID¿경상이자¿8909082342353
ORC¿2017082505006¿¿03¿¿7¿¿
DG1¿N830  ¿      
IN1¿1¿7¿¿¿¿¿¿1¿¿¿¿¿
RXD¿1¿1¿1¿651900530¿¿0.3333¿3¿1¿¿
RXD¿1¿1¿1¿651900720¿¿0.3333¿3¿1¿¿",


        };

        [Test]
        public void Test_1()
        {
            var model = ParseLogic.Parse(_data[0]);

            Assert.Multiple(() =>
            {
                Assert.That(model.MSH.버전정보, Is.EqualTo("0.8.0.0"));
                Assert.That(model.MSH.병원전산업체코드, Is.EqualTo("001"));
                Assert.That(model.MSH.제품정보, Is.EqualTo("YSR2000"));
                Assert.That(model.MSH.생성날짜시간, Is.EqualTo("20081117090000"));

                Assert.That(model.FAC.처방요양기관정보, Is.EqualTo("ys040203"));
                Assert.That(model.FAC.의료기관명칭, Is.EqualTo("의사랑의원"));
                Assert.That(model.FAC.요양기관주소, Is.EqualTo(""));
                Assert.That(model.FAC.의료기관전화번호, Is.EqualTo("041-583-0123"));
                Assert.That(model.FAC.의료기관팩스번호, Is.EqualTo("02-2105-5091"));

                Assert.That(model.PRD.처방의료인성명, Is.EqualTo("김의사"));
                Assert.That(model.PRD.면허종별, Is.EqualTo("의사"));
                Assert.That(model.PRD.면허번호, Is.EqualTo("42409"));

                Assert.That(model.PID.수진자성명, Is.EqualTo("의사랑"));
                Assert.That(model.PID.수진자주민등록번호, Is.EqualTo("7711111111111"));

                Assert.That(model.ORC.교부번호, Is.EqualTo("2008111711002"));
                Assert.That(model.ORC.재해발생일, Is.EqualTo(""));
                Assert.That(model.ORC.진료과목, Is.EqualTo("23"));
                Assert.That(model.ORC.특정기호, Is.EqualTo(""));
                Assert.That(model.ORC.사용기간, Is.EqualTo("7"));
                Assert.That(model.ORC.용법, Is.EqualTo(""));
                Assert.That(model.ORC.조제시참고사항, Is.EqualTo("저함량 배수처방 사유♬A45900471, 사유코드 : C"));

                Assert.That(model.DG1.상병분류기호1, Is.EqualTo("")); // 스페이스가 있어도 잘 파씽 되어야 한다.
                Assert.That(model.DG1.상병분류기호2, Is.EqualTo("")); // 스페이스가 있어도 잘 파씽 되어야 한다.

                Assert.That(model.IN1.보험구분, Is.EqualTo("1"));
                Assert.That(model.IN1.공상및보훈구분, Is.EqualTo(""));
                Assert.That(model.IN1.의료급여종별, Is.EqualTo(""));
                Assert.That(model.IN1.증번호, Is.EqualTo("12345678"));
                Assert.That(model.IN1.조합기호산개기관기호, Is.EqualTo("70271724"));
                Assert.That(model.IN1.보훈번호, Is.EqualTo(""));
                Assert.That(model.IN1.미사용1, Is.EqualTo(""));
                Assert.That(model.IN1.미사용2, Is.EqualTo(""));
                Assert.That(model.IN1.미사용3, Is.EqualTo(""));

                Assert.That(model.RXDs[0].처방구분, Is.EqualTo("1"));
                Assert.That(model.RXDs[0].급여구분, Is.EqualTo("2"));
                Assert.That(model.RXDs[0].코드구분, Is.EqualTo("1"));
                Assert.That(model.RXDs[0].청구코드사용자코드, Is.EqualTo("661900010"));
                Assert.That(model.RXDs[0].약품명, Is.EqualTo(""));
                Assert.That(model.RXDs[0].일회투약량, Is.EqualTo("0.330"));
                Assert.That(model.RXDs[0].일회투여횟수, Is.EqualTo("3"));
                Assert.That(model.RXDs[0].총투약일수, Is.EqualTo("1"));
                Assert.That(model.RXDs[0].용법코드, Is.EqualTo(""));
                Assert.That(model.RXDs[0].용법, Is.EqualTo(""));

                Assert.That(model.RXDs[1].처방구분, Is.EqualTo("1"));
                Assert.That(model.RXDs[1].급여구분, Is.EqualTo("1"));
                Assert.That(model.RXDs[1].코드구분, Is.EqualTo("1"));
                Assert.That(model.RXDs[1].청구코드사용자코드, Is.EqualTo("644900310"));
                Assert.That(model.RXDs[1].약품명, Is.EqualTo(""));
                Assert.That(model.RXDs[1].일회투약량, Is.EqualTo("2.500"));
                Assert.That(model.RXDs[1].일회투여횟수, Is.EqualTo("4"));
                Assert.That(model.RXDs[1].총투약일수, Is.EqualTo("1"));
                Assert.That(model.RXDs[1].용법코드, Is.EqualTo(""));
                Assert.That(model.RXDs[1].용법, Is.EqualTo(""));

                Assert.That(model.RXDs[5].처방구분, Is.EqualTo("1"));
                Assert.That(model.RXDs[5].급여구분, Is.EqualTo("1"));
                Assert.That(model.RXDs[5].코드구분, Is.EqualTo("2"));
                Assert.That(model.RXDs[5].청구코드사용자코드, Is.EqualTo("693200860"));
                Assert.That(model.RXDs[5].약품명, Is.EqualTo(""));
                Assert.That(model.RXDs[5].일회투약량, Is.EqualTo("1.000"));
                Assert.That(model.RXDs[5].일회투여횟수, Is.EqualTo("1"));
                Assert.That(model.RXDs[5].총투약일수, Is.EqualTo("1"));
                Assert.That(model.RXDs[5].용법코드, Is.EqualTo("odw"));
                Assert.That(model.RXDs[5].용법, Is.EqualTo("첫주는하루한두번두째주부터는1주에2번사용"));

                Assert.That(model.RXDs.Count, Is.EqualTo(7));
            });
        }

        [Test]
        public void Test_2()
        {
            var model = ParseLogic.Parse(_data[1]);

            Assert.Multiple(() =>
            {
                Assert.That(model.MSH.버전정보, Is.EqualTo("0.8.0.0"));
                Assert.That(model.MSH.병원전산업체코드, Is.EqualTo("001"));
                Assert.That(model.MSH.제품정보, Is.EqualTo("YSR2000"));
                Assert.That(model.MSH.생성날짜시간, Is.EqualTo("99991231235999"));

                Assert.That(model.FAC.처방요양기관정보, Is.EqualTo("ys050203"));
                Assert.That(model.FAC.의료기관명칭, Is.EqualTo("태건병원"));
                Assert.That(model.FAC.요양기관주소, Is.EqualTo(""));
                Assert.That(model.FAC.의료기관전화번호, Is.EqualTo("02-2105-5001"));
                Assert.That(model.FAC.의료기관팩스번호, Is.EqualTo(""));

                Assert.That(model.PRD.처방의료인성명, Is.EqualTo("안태건"));
                Assert.That(model.PRD.면허종별, Is.EqualTo("의사"));
                Assert.That(model.PRD.면허번호, Is.EqualTo("8988"));

                Assert.That(model.PID.수진자성명, Is.EqualTo("급여2종"));
                Assert.That(model.PID.수진자주민등록번호, Is.EqualTo("3333333333337"));

                Assert.That(model.ORC.교부번호, Is.EqualTo("2017071805006"));
                Assert.That(model.ORC.재해발생일, Is.EqualTo(""));
                Assert.That(model.ORC.진료과목, Is.EqualTo("03"));
                Assert.That(model.ORC.특정기호, Is.EqualTo(""));
                Assert.That(model.ORC.사용기간, Is.EqualTo("7"));
                Assert.That(model.ORC.용법, Is.EqualTo(""));
                Assert.That(model.ORC.조제시참고사항, Is.EqualTo(""));

                Assert.That(model.DG1.상병분류기호1, Is.EqualTo("")); // 스페이스가 있어도 잘 파씽 되어야 한다.
                Assert.That(model.DG1.상병분류기호2, Is.EqualTo("")); // 스페이스가 있어도 잘 파씽 되어야 한다.

                Assert.That(model.IN1.보험구분, Is.EqualTo("2"));
                Assert.That(model.IN1.공상및보훈구분, Is.EqualTo(""));
                Assert.That(model.IN1.의료급여종별, Is.EqualTo("2"));
                Assert.That(model.IN1.증번호, Is.EqualTo(""));
                Assert.That(model.IN1.조합기호산개기관기호, Is.EqualTo(""));
                Assert.That(model.IN1.보훈번호, Is.EqualTo(""));
                Assert.That(model.IN1.미사용1, Is.EqualTo("1"));
                Assert.That(model.IN1.미사용2, Is.EqualTo(""));
                Assert.That(model.IN1.미사용3, Is.EqualTo(""));

                Assert.That(model.RXDs[0].처방구분, Is.EqualTo("1"));
                Assert.That(model.RXDs[0].급여구분, Is.EqualTo("1"));
                Assert.That(model.RXDs[0].코드구분, Is.EqualTo("1"));
                Assert.That(model.RXDs[0].청구코드사용자코드, Is.EqualTo("052400010"));
                Assert.That(model.RXDs[0].약품명, Is.EqualTo(""));
                Assert.That(model.RXDs[0].일회투약량, Is.EqualTo("0.3333"));
                Assert.That(model.RXDs[0].일회투여횟수, Is.EqualTo("3"));
                Assert.That(model.RXDs[0].총투약일수, Is.EqualTo("1"));
                Assert.That(model.RXDs[0].용법코드, Is.EqualTo(""));
                Assert.That(model.RXDs[0].용법, Is.EqualTo(""));

                Assert.That(model.RXDs.Count, Is.EqualTo(1));
            });
        }

        [Test]
        public void Test_3()
        {
            var model = ParseLogic.Parse(_data[2]);

            Assert.Multiple(() =>
            {
                Assert.That(model.MSH.버전정보, Is.EqualTo("0.8.0.0"));
                Assert.That(model.MSH.병원전산업체코드, Is.EqualTo("001"));
                Assert.That(model.MSH.제품정보, Is.EqualTo("YSR2000"));
                Assert.That(model.MSH.생성날짜시간, Is.EqualTo("99991231235999"));

                Assert.That(model.FAC.처방요양기관정보, Is.EqualTo("99355001"));
                Assert.That(model.FAC.의료기관명칭, Is.EqualTo("태건병원"));
                Assert.That(model.FAC.요양기관주소, Is.EqualTo(""));
                Assert.That(model.FAC.의료기관전화번호, Is.EqualTo("02-2105-5001"));
                Assert.That(model.FAC.의료기관팩스번호, Is.EqualTo(""));

                Assert.That(model.PRD.처방의료인성명, Is.EqualTo("안태건"));
                Assert.That(model.PRD.면허종별, Is.EqualTo("의사"));
                Assert.That(model.PRD.면허번호, Is.EqualTo("8988"));

                Assert.That(model.PID.수진자성명, Is.EqualTo("경상이자"));
                Assert.That(model.PID.수진자주민등록번호, Is.EqualTo("8909082342353"));

                Assert.That(model.ORC.교부번호, Is.EqualTo("2017082505006"));
                Assert.That(model.ORC.재해발생일, Is.EqualTo(""));
                Assert.That(model.ORC.진료과목, Is.EqualTo("03"));
                Assert.That(model.ORC.특정기호, Is.EqualTo(""));
                Assert.That(model.ORC.사용기간, Is.EqualTo("7"));
                Assert.That(model.ORC.용법, Is.EqualTo(""));
                Assert.That(model.ORC.조제시참고사항, Is.EqualTo(""));

                Assert.That(model.DG1.상병분류기호1, Is.EqualTo("N830")); // 스페이스가 있어도 잘 파씽 되어야 한다.
                Assert.That(model.DG1.상병분류기호2, Is.EqualTo("")); // 스페이스가 있어도 잘 파씽 되어야 한다.

                Assert.That(model.IN1.보험구분, Is.EqualTo("1"));
                Assert.That(model.IN1.공상및보훈구분, Is.EqualTo("7"));
                Assert.That(model.IN1.의료급여종별, Is.EqualTo(""));
                Assert.That(model.IN1.증번호, Is.EqualTo(""));
                Assert.That(model.IN1.조합기호산개기관기호, Is.EqualTo(""));
                Assert.That(model.IN1.보훈번호, Is.EqualTo(""));
                Assert.That(model.IN1.미사용1, Is.EqualTo("1"));
                Assert.That(model.IN1.미사용2, Is.EqualTo(""));
                Assert.That(model.IN1.미사용3, Is.EqualTo(""));

                Assert.That(model.RXDs[0].처방구분, Is.EqualTo("1"));
                Assert.That(model.RXDs[0].급여구분, Is.EqualTo("1"));
                Assert.That(model.RXDs[0].코드구분, Is.EqualTo("1"));
                Assert.That(model.RXDs[0].청구코드사용자코드, Is.EqualTo("651900530"));
                Assert.That(model.RXDs[0].약품명, Is.EqualTo(""));
                Assert.That(model.RXDs[0].일회투약량, Is.EqualTo("0.3333"));
                Assert.That(model.RXDs[0].일회투여횟수, Is.EqualTo("3"));
                Assert.That(model.RXDs[0].총투약일수, Is.EqualTo("1"));
                Assert.That(model.RXDs[0].용법코드, Is.EqualTo(""));
                Assert.That(model.RXDs[0].용법, Is.EqualTo(""));

                Assert.That(model.RXDs[1].처방구분, Is.EqualTo("1"));
                Assert.That(model.RXDs[1].급여구분, Is.EqualTo("1"));
                Assert.That(model.RXDs[1].코드구분, Is.EqualTo("1"));
                Assert.That(model.RXDs[1].청구코드사용자코드, Is.EqualTo("651900720"));
                Assert.That(model.RXDs[1].약품명, Is.EqualTo(""));
                Assert.That(model.RXDs[1].일회투약량, Is.EqualTo("0.3333"));
                Assert.That(model.RXDs[1].일회투여횟수, Is.EqualTo("3"));
                Assert.That(model.RXDs[1].총투약일수, Is.EqualTo("1"));
                Assert.That(model.RXDs[1].용법코드, Is.EqualTo(""));
                Assert.That(model.RXDs[1].용법, Is.EqualTo(""));

                Assert.That(model.RXDs.Count, Is.EqualTo(2));
            });
        }
    }
}
