using _2D보험구분검증툴;
using _2D보험구분검증툴.Interface;
using _2D보험구분검증툴.Logic;
using _2D보험구분검증툴.Logic.보험구분Logic;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests.보험구분Tests
{
    [TestFixture]
    public class 산재LogicTests
    {
        private string 정상바코드 = @"MSH¿0.8.0.0¿01¿Ku2.0¿20171016141414
FAC¿12345678¿테스트의원¿¿02-2105-500¿¿
PRD¿의사¿의사¿12345
PID¿테스트¿111111-11111111
ORC¿2017033101004¿20101010¿¿¿3¿¿♬써스펜이알서방정 100/100 전액본인부담
DG1¿J00¿
IN1¿3¿¿¿관리번호(산재)¿¿회사이름¿¿¿¿¿¿¿
RXD¿1¿1¿1¿644900310¿¿0.3333¿3¿1¿¿식후 30분
";
        private string 정상바코드_유형2 = @"MSH¿0.8.0.0¿01¿Ku2.0¿20171016141414
FAC¿12345678¿테스트의원¿¿02-2105-500¿¿
PRD¿의사¿의사¿12345
PID¿테스트¿111111-11111111
ORC¿2017033101004¿20101010¿¿¿3¿¿♬써스펜이알서방정 100/100 전액본인부담
DG1¿J00¿
IN1¿3¿¿¿관리번호(산재)¿¿회사이름¿¿¿¿¿¿¿
RXD¿1¿1¿1¿644900310¿¿0.3333¿3¿1¿¿식후 30분";

        private string 비정상바코드 = @"MSH¿0.8.0.0¿01¿Ku2.0¿20171016141414
FAC¿12345678¿테스트의원¿¿02-2105-500¿¿
PRD¿의사¿의사¿12345
PID¿테스트¿111111-11111111
ORC¿2017033101004¿20101010¿¿¿3¿¿♬써스펜이알서방정 100/100 전액본인부담
DG1¿J00¿
IN1¿2¿¿¿관리번호(산재)¿¿회사이름¿¿¿¿¿¿¿
RXD¿1¿1¿1¿644900310¿¿0.3333¿3¿1¿¿식후 30분
";

        private string 비정상바코드_2 = @"MSH¿0.8.0.0¿01¿Ku2.0¿20171016141414
FAC¿12345678¿테스트의원¿¿02-2105-500¿¿
PRD¿의사¿의사¿12345
PID¿테스트¿111111-11111111
ORC¿2017033101004¿20101010¿¿¿3¿¿♬써스펜이알서방정 100/100 전액본인부담
DG1¿J00¿
IN1¿3¿1¿¿관리번호(산재)¿¿회사이름¿¿¿¿¿¿¿
RXD¿1¿1¿1¿644900310¿¿0.3333¿3¿1¿¿식후 30분
";

        private string 비정상바코드_3 = @"MSH¿0.8.0.0¿01¿Ku2.0¿20171016141414
FAC¿12345678¿테스트의원¿¿02-2105-500¿¿
PRD¿의사¿의사¿12345
PID¿테스트¿111111-11111111
ORC¿2017033101004¿20101010¿¿¿3¿¿♬써스펜이알서방정 100/100 전액본인부담
DG1¿J00¿
IN1¿¿¿¿관리번호(산재)¿¿회사이름¿¿¿¿¿¿¿
RXD¿1¿1¿1¿644900310¿¿0.3333¿3¿1¿¿식후 30분
";

        private string 비정상바코드_4 = @"MSH¿0.8.0.0¿01¿Ku2.0¿20171016141414
FAC¿12345678¿테스트의원¿¿02-2105-500¿¿
PRD¿의사¿의사¿12345
PID¿테스트¿111111-11111111
ORC¿2017033101004¿20101010¿¿¿3¿¿♬써스펜이알서방정 100/100 전액본인부담
DG1¿J00¿
IN1¿¿¿¿¿¿회사이름¿¿¿¿¿¿¿
RXD¿1¿1¿1¿644900310¿¿0.3333¿3¿1¿¿식후 30분
";

        private string 비정상바코드_5 = @"MSH¿0.8.0.0¿01¿Ku2.0¿20171016141414
FAC¿12345678¿테스트의원¿¿02-2105-500¿¿
PRD¿의사¿의사¿12345
PID¿테스트¿111111-11111111
ORC¿2017033101004¿20101010¿¿¿3¿¿♬써스펜이알서방정 100/100 전액본인부담
DG1¿J00¿
IN1¿3¿¿¿¿¿회사이름¿¿¿¿¿¿¿
RXD¿1¿1¿1¿644900310¿¿0.3333¿3¿1¿¿식후 30분
";


        [Test]
        public void 산재Logic_Validation_정상바코드테스트()
        {
            var logic = new 산재Logic();
            var model = ParseLogic.Parse(정상바코드);
            var retv = logic.Validation(model);

            Assert.IsTrue(retv);
        }

        [Test]
        public void 산재Logic_Validation_정상바코드테스트_유형2()
        {
            var logic = new 산재Logic();
            var model = ParseLogic.Parse(정상바코드_유형2);
            var retv = logic.Validation(model);

            Assert.IsTrue(retv);
        }


        [Test]
        public void 산재Logic_Validation_비정상바코드_1()
        {
            var logic = new 산재Logic();
            var model = ParseLogic.Parse(비정상바코드);
            var retv = logic.Validation(model);
            var errorModel = logic.GetErrorModel(model, 0);

            Assert.Multiple(() =>
            {
                Assert.IsFalse(retv);

                Assert.That(1, Is.EqualTo(errorModel.Count()));

                Assert.That(@"산재 바코드의 보험구분은 3이고 공상및보훈구분에 값이 없어야합니다.", Is.EqualTo(errorModel.First().메세지));
            });
        }

        [Test]
        public void 산재Logic_Validation_비정상바코드_2()
        {
            var logic = new 산재Logic();
            var model = ParseLogic.Parse(비정상바코드_2);
            var retv = logic.Validation(model);
            var errorModel = logic.GetErrorModel(model, 0);

            Assert.Multiple(() =>
            {
                Assert.IsFalse(retv);

                Assert.That(1, Is.EqualTo(errorModel.Count()));

                Assert.That(@"산재 바코드의 보험구분은 3이고 공상및보훈구분에 값이 없어야합니다.", Is.EqualTo(errorModel.First().메세지));
            });
        }

        [Test]
        public void 산재Logic_Validation_비정상바코드_3()
        {
            var logic = new 산재Logic();
            var model = ParseLogic.Parse(비정상바코드_3);
            var retv = logic.Validation(model);
            var errorModel = logic.GetErrorModel(model, 0);

            Assert.Multiple(() =>
            {
                Assert.IsFalse(retv);

                Assert.That(1, Is.EqualTo(errorModel.Count()));

                Assert.That(@"산재 바코드의 보험구분은 3이고 공상및보훈구분에 값이 없어야합니다.", Is.EqualTo(errorModel.First().메세지));
            });
        }

        [Test]
        public void 산재Logic_Validation_비정상바코드_4()
        {
            var logic = new 산재Logic();
            var model = ParseLogic.Parse(비정상바코드_4);
            var retv = logic.Validation(model);
            var errorModel = logic.GetErrorModel(model, 0);

            Assert.Multiple(() =>
            {
                Assert.IsFalse(retv);

                Assert.That(2, Is.EqualTo(errorModel.Count()));

                Assert.That(@"산재 바코드의 보험구분은 3이고 공상및보훈구분에 값이 없어야합니다.", Is.EqualTo(errorModel.ToArray()[0].메세지));
                Assert.That(@"산재 바코드는 증번호란에 관리번호가 필수로 들어가야 합니다.", Is.EqualTo(errorModel.ToArray()[1].메세지));
            });
        }

        [Test]
        public void 산재Logic_Validation_비정상바코드_5()
        {
            var logic = new 산재Logic();
            var model = ParseLogic.Parse(비정상바코드_5);
            var retv = logic.Validation(model);
            var errorModel = logic.GetErrorModel(model, 0);

            Assert.Multiple(() =>
            {
                Assert.IsFalse(retv);

                Assert.That(1, Is.EqualTo(errorModel.Count()));

                Assert.That(@"산재 바코드는 증번호란에 관리번호가 필수로 들어가야 합니다.", Is.EqualTo(errorModel.First().메세지));
            });
        }

        [Test]
        public void Form_검증하기버튼_정상바코드()
        {
            var form = GetForm(정상바코드);

            form.검증하기버튼("", new 산재Logic());

            Assert.That(0, Is.EqualTo(form._오류목록.Count()));
        }

        [Test]
        public void Form_검증하기버튼_정상바코드_유형2()
        {
            var form = GetForm(정상바코드_유형2);

            form.검증하기버튼("", new 산재Logic());

            Assert.That(0, Is.EqualTo(form._오류목록.Count()));
        }

        [Test]
        public void Form_검증하기버튼_비정상바코드()
        {
            var form = GetForm(비정상바코드);

            form.검증하기버튼("", new 산재Logic());

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(form._오류목록.Count()));
                Assert.That(@"산재 바코드의 보험구분은 3이고 공상및보훈구분에 값이 없어야합니다.", Is.EqualTo(form._오류목록.First().메세지));
            });
        }

        [Test]
        public void Form_검증하기버튼_비정상바코드_2()
        {
            var form = GetForm(비정상바코드_2);

            form.검증하기버튼("", new 산재Logic());

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(form._오류목록.Count()));
                Assert.That(@"산재 바코드의 보험구분은 3이고 공상및보훈구분에 값이 없어야합니다.", Is.EqualTo(form._오류목록.First().메세지));
            });
        }

        [Test]
        public void Form_검증하기버튼_비정상바코드_3()
        {
            var form = GetForm(비정상바코드_3);

            form.검증하기버튼("", new 산재Logic());

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(form._오류목록.Count()));
                Assert.That(@"산재 바코드의 보험구분은 3이고 공상및보훈구분에 값이 없어야합니다.", Is.EqualTo(form._오류목록.First().메세지));
            });
        }

        [Test]
        public void Form_검증하기버튼_비정상바코드_4()
        {
            var form = GetForm(비정상바코드_4);

            form.검증하기버튼("", new 산재Logic());

            Assert.Multiple(() =>
            {
                Assert.That(2, Is.EqualTo(form._오류목록.Count()));

                Assert.That(@"산재 바코드의 보험구분은 3이고 공상및보훈구분에 값이 없어야합니다.", Is.EqualTo(form._오류목록.ToArray()[0].메세지));
                Assert.That(@"산재 바코드는 증번호란에 관리번호가 필수로 들어가야 합니다.", Is.EqualTo(form._오류목록.ToArray()[1].메세지));
            });
        }

        [Test]
        public void Form_검증하기버튼_비정상바코드_5()
        {
            var form = GetForm(비정상바코드_5);

            form.검증하기버튼("", new 산재Logic());

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(form._오류목록.Count()));
                Assert.That(@"산재 바코드는 증번호란에 관리번호가 필수로 들어가야 합니다.", Is.EqualTo(form._오류목록.First().메세지));
            });
        }

        private Form1 GetForm(string expectedValue)
        {
            var 검증하기Mock = new Mock<I검증하기>();

            검증하기Mock.Setup(x => x.IsValid(It.IsAny<string>()))
                .Returns(true);

            검증하기Mock.Setup(x => x.Get암호화해제Data(It.IsAny<string>()))
                .Returns(expectedValue);

            var form = new Form1(검증하기Mock.Object, null, null);
            return form;
        }
    }
}
