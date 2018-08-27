//using _2D보험구분검증툴;
//using _2D보험구분검증툴.Interface;
//using _2D보험구분검증툴.Logic;
//using _2D보험구분검증툴.Logic.보험구분Logic;
//using Moq;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace UnitTests.보험구분Tests
//{
//    [TestFixture]
//    public class 국민공단LogicTests
//    {
//        private const string 정상바코드 = @"MSH¿0.8.0.0¿001¿YSR2000¿20081117090000
//FAC¿ys040203¿의사랑의원¿¿041-583-0123¿02-2105-5091¿
//PRD¿김의사¿의사¿42409
//PID¿의사랑¿7711111111111
//ORC¿2008111711002¿¿23¿¿7¿¿저함량 배수처방 사유♬A45900471, 사유코드 : C
//DG1¿ ¿ 
//IN1¿1¿¿¿12345678¿70271724¿¿¿¿¿¿¿¿
//RXD¿1¿2¿1¿661900010¿¿0.330¿3¿1¿¿
//RXD¿1¿1¿1¿644900310¿¿2.500¿4¿1¿¿
//RXD¿1¿1¿1¿643306160¿¿0.330¿3¿1¿¿
//RXD¿1¿1¿1¿642403700¿¿0.330¿3¿1¿¿
//RXD¿1¿1¿2¿641800840¿¿1.000¿1¿1¿¿
//RXD¿1¿1¿2¿693200860¿¿1.000¿1¿1¿odw¿첫주는하루한두번두째주부터는1주에2번사용
//RXD¿1¿1¿1¿644501350¿¿0.330¿3¿1¿¿";

//        private const string 정상바코드_유형2 = @"MSH¿0.8.0.0¿001¿YSR2000¿20081117090000
//FAC¿ys040203¿의사랑의원¿¿041-583-0123¿02-2105-5091¿
//PRD¿김의사¿의사¿42409
//PID¿의사랑¿7711111111111
//ORC¿2008111711002¿¿23¿¿7¿¿저함량 배수처방 사유♬A45900471, 사유코드 : C
//DG1¿ ¿ 
//IN1¿1¿¿¿12345678¿70271724¿¿¿¿¿¿¿¿
//RXD¿1¿2¿1¿661900010¿¿0.330¿3¿1¿¿
//RXD¿1¿1¿1¿644900310¿¿2.500¿4¿1¿¿
//RXD¿1¿1¿1¿643306160¿¿0.330¿3¿1¿¿
//RXD¿1¿1¿1¿642403700¿¿0.330¿3¿1¿¿
//RXD¿1¿1¿2¿641800840¿¿1.000¿1¿1¿¿
//RXD¿1¿1¿2¿693200860¿¿1.000¿1¿1¿odw¿첫주는하루한두번두째주부터는1주에2번사용
//RXD¿1¿1¿1¿644501350¿¿0.330¿3¿1¿¿

//";

//        private const string 비정상바코드_1 = @"MSH¿0.8.0.0¿001¿YSR2000¿20081117090000
//FAC¿ys040203¿의사랑의원¿¿041-583-0123¿02-2105-5091¿
//PRD¿김의사¿의사¿42409
//PID¿의사랑¿7711111111111
//ORC¿2008111711002¿¿23¿¿7¿¿저함량 배수처방 사유♬A45900471, 사유코드 : C
//DG1¿ ¿ 
//IN1¿2¿¿¿12345678¿70271724¿¿¿¿¿¿¿¿
//RXD¿1¿2¿1¿661900010¿¿0.330¿3¿1¿¿
//RXD¿1¿1¿1¿644900310¿¿2.500¿4¿1¿¿
//RXD¿1¿1¿1¿643306160¿¿0.330¿3¿1¿¿
//RXD¿1¿1¿1¿642403700¿¿0.330¿3¿1¿¿
//RXD¿1¿1¿2¿641800840¿¿1.000¿1¿1¿¿
//RXD¿1¿1¿2¿693200860¿¿1.000¿1¿1¿odw¿첫주는하루한두번두째주부터는1주에2번사용
//RXD¿1¿1¿1¿644501350¿¿0.330¿3¿1¿¿";

//        private const string 비정상바코드_2 = @"MSH¿0.8.0.0¿001¿YSR2000¿20081117090000
//FAC¿ys040203¿의사랑의원¿¿041-583-0123¿02-2105-5091¿
//PRD¿김의사¿의사¿42409
//PID¿의사랑¿7711111111111
//ORC¿2008111711002¿¿23¿¿7¿¿저함량 배수처방 사유♬A45900471, 사유코드 : C
//DG1¿ ¿ 
//IN1¿1¿1¿¿12345678¿70271724¿¿¿¿¿¿¿¿
//RXD¿1¿2¿1¿661900010¿¿0.330¿3¿1¿¿
//RXD¿1¿1¿1¿644900310¿¿2.500¿4¿1¿¿
//RXD¿1¿1¿1¿643306160¿¿0.330¿3¿1¿¿
//RXD¿1¿1¿1¿642403700¿¿0.330¿3¿1¿¿
//RXD¿1¿1¿2¿641800840¿¿1.000¿1¿1¿¿
//RXD¿1¿1¿2¿693200860¿¿1.000¿1¿1¿odw¿첫주는하루한두번두째주부터는1주에2번사용
//RXD¿1¿1¿1¿644501350¿¿0.330¿3¿1¿¿";

//        private const string 비정상바코드_3 = @"MSH¿0.8.0.0¿001¿YSR2000¿20081117090000
//FAC¿ys040203¿의사랑의원¿¿041-583-0123¿02-2105-5091¿
//PRD¿김의사¿의사¿42409
//PID¿의사랑¿7711111111111
//ORC¿2008111711002¿¿23¿¿7¿¿저함량 배수처방 사유♬A45900471, 사유코드 : C
//DG1¿ ¿ 
//IN1¿2¿1¿¿12345678¿70271724¿¿¿¿¿¿¿¿
//RXD¿1¿2¿1¿661900010¿¿0.330¿3¿1¿¿
//RXD¿1¿1¿1¿644900310¿¿2.500¿4¿1¿¿
//RXD¿1¿1¿1¿643306160¿¿0.330¿3¿1¿¿
//RXD¿1¿1¿1¿642403700¿¿0.330¿3¿1¿¿
//RXD¿1¿1¿2¿641800840¿¿1.000¿1¿1¿¿
//RXD¿1¿1¿2¿693200860¿¿1.000¿1¿1¿odw¿첫주는하루한두번두째주부터는1주에2번사용
//RXD¿1¿1¿1¿644501350¿¿0.330¿3¿1¿¿

//";


//        [TestCase(정상바코드)]
//        [TestCase(정상바코드_유형2)]
//        public void 국민공단Logic_Validation_정상바코드테스트(string data)
//        {
//            var logic = new 국민공단Logic();
//            var model = ParseLogic.Parse(data);
//            var retv = logic.Validation(model);

//            Assert.IsTrue(retv);
//        }

//        [TestCase(비정상바코드_1)]
//        [TestCase(비정상바코드_2)]
//        [TestCase(비정상바코드_3)]
//        public void 국민공단Logic_Validation_비정상바코드(string data)
//        {
//            var logic = new 국민공단Logic();
//            var model = ParseLogic.Parse(data);
//            var retv = logic.Validation(model);
//            var errorModel = logic.GetErrorModel(model, 0);

//            Assert.Multiple(() =>
//            {
//                Assert.IsFalse(retv);

//                Assert.That(1, Is.EqualTo(errorModel.Count()));

//                Assert.That(@"국민공단은 보험구분이 1이고 공상및보훈구분에 값이 없어야합니다.", Is.EqualTo(errorModel.First().메세지));
//            });
//        }

     
//        [TestCase(정상바코드)]
//        [TestCase(정상바코드_유형2)]
//        public void Form_검증하기버튼_정상바코드(string data)
//        {
//            var 검증하기Mock = new Mock<I검증하기>();

//            검증하기Mock.Setup(x => x.IsValid(It.IsAny<string>()))
//                .Returns(true);

//            검증하기Mock.Setup(x => x.Get암호화해제Data(It.IsAny<string>()))
//                .Returns(data);

//            var form = new Form1(검증하기Mock.Object, null, new FormLogic());

//            form.검증하기버튼("", new 국민공단Logic());

//            Assert.That(0, Is.EqualTo(form._오류목록.Count()));
//        }


//        [TestCase(비정상바코드_1)]
//        [TestCase(비정상바코드_2)]
//        [TestCase(비정상바코드_3)]
//        public void Form_검증하기버튼_비정상바코드(string data)
//        {
//            var 검증하기Mock = new Mock<I검증하기>();

//            검증하기Mock.Setup(x => x.IsValid(It.IsAny<string>()))
//                .Returns(true);

//            검증하기Mock.Setup(x => x.Get암호화해제Data(It.IsAny<string>()))
//                .Returns(data);

//            var form = new Form1(검증하기Mock.Object, null, null);

//            form.검증하기버튼("", new 국민공단Logic());

//            Assert.Multiple(() =>
//            {
//                Assert.That(1, Is.EqualTo(form._오류목록.Count()));
//                Assert.That(@"국민공단은 보험구분이 1이고 공상및보훈구분에 값이 없어야합니다.", Is.EqualTo(form._오류목록.First().메세지));
//            });
//        }
//    }
//}
