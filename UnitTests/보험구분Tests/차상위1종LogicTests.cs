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
    public class 차상위1종LogicTests
    {
        const string 정상바코드 = @"MSH¿0.8.0.0¿01¿Ku2.0¿20171016141414
FAC¿12345678¿의원¿¿02-2105-5002¿¿
PRD¿의사¿의사¿12345
PID¿차상위¿111111-11111111
ORC¿2017033101004¿¿¿¿3¿¿♬본인부담경감-차상위(C)
DG1¿J00¿
IN1¿1¿16¿¿123456789¿¿¿¿¿¿¿¿¿
RXD¿1¿1¿1¿653601420¿¿1¿1¿1¿¿식후 30분";

        const string 정상바코드_유형2 = @"MSH¿0.8.0.0¿01¿Ku2.0¿20171016141414
FAC¿12345678¿의원¿¿02-2105-5002¿¿
PRD¿의사¿의사¿12345
PID¿차상위¿111111-11111111
ORC¿2017033101004¿¿¿¿3¿¿♬본인부담경감-차상위(C)
DG1¿J00¿
IN1¿1¿16¿¿123456789¿¿¿¿¿¿¿¿¿
RXD¿1¿1¿1¿653601420¿¿1¿1¿1¿¿식후 30분
";

        const string 정상바코드_유형3 = @"MSH¿0.8.0.0¿01¿Ku2.0¿20171016141414
FAC¿12345678¿의원¿¿02-2105-5002¿¿
PRD¿의사¿의사¿12345
PID¿차상위¿111111-11111111
ORC¿2017033101004¿¿¿¿3¿¿♬본인부담경감-차상위(C)
DG1¿J00¿
IN1¿1¿16¿¿123456789¿¿¿¿¿¿¿¿¿
RXD¿1¿1¿1¿653601420¿¿1¿1¿1¿¿식후 30분

";

        const string 비정상바코드 = @"MSH¿0.8.0.0¿01¿Ku2.0¿20171016141414
FAC¿12345678¿의원¿¿02-2105-5002¿¿
PRD¿의사¿의사¿12345
PID¿차상위¿111111-11111111
ORC¿2017033101004¿¿¿¿3¿¿♬본인부담경감-차상위(C)
DG1¿J00¿
IN1¿1¿¿¿123456789¿¿¿¿¿¿¿¿¿
RXD¿1¿1¿1¿653601420¿¿1¿1¿1¿¿식후 30분";

        const string 비정상바코드_유형2 = @"MSH¿0.8.0.0¿01¿Ku2.0¿20171016141414
FAC¿12345678¿의원¿¿02-2105-5002¿¿
PRD¿의사¿의사¿12345
PID¿차상위¿111111-11111111
ORC¿2017033101004¿¿¿¿3¿¿♬본인부담경감-차상위(C)
DG1¿J00¿
IN1¿1¿¿¿123456789¿¿¿¿¿¿¿¿¿
RXD¿1¿1¿1¿653601420¿¿1¿1¿1¿¿식후 30분
";

        const string 비정상바코드_유형2_2 = @"MSH¿0.8.0.0¿01¿Ku2.0¿20171016141414
FAC¿12345678¿의원¿¿02-2105-5002¿¿
PRD¿의사¿의사¿12345
PID¿차상위¿111111-11111111
ORC¿2017033101004¿¿¿¿3¿¿♬본인부담경감-차상위(C)
DG1¿J00¿
IN1¿2¿16¿¿123456789¿¿¿¿¿¿¿¿¿
RXD¿1¿1¿1¿653601420¿¿1¿1¿1¿¿식후 30분
";

        const string 비정상바코드_유형3 = @"MSH¿0.8.0.0¿01¿Ku2.0¿20171016141414
FAC¿12345678¿의원¿¿02-2105-5002¿¿
PRD¿의사¿의사¿12345
PID¿차상위¿111111-11111111
ORC¿2017033101004¿¿¿¿3¿¿♬본인부담경감-차상위(C)
DG1¿J00¿
IN1¿2¿18¿¿123456789¿¿¿¿¿¿¿¿¿
RXD¿1¿1¿1¿653601420¿¿1¿1¿1¿¿식후 30분

";

        const string 비정상바코드_유형3_2 = @"MSH¿0.8.0.0¿01¿Ku2.0¿20171016141414
FAC¿12345678¿의원¿¿02-2105-5002¿¿
PRD¿의사¿의사¿12345
PID¿차상위¿111111-11111111
ORC¿2017033101004¿¿¿¿3¿¿♬본인부담경감-차상위(C)
DG1¿J00¿
IN1¿2¿¿¿123456789¿¿¿¿¿¿¿¿¿
RXD¿1¿1¿1¿653601420¿¿1¿1¿1¿¿식후 30분

";

        [TestCase(정상바코드)]
        [TestCase(정상바코드_유형2)]
        [TestCase(정상바코드_유형3)]
        public void 차상위Logic_Validation_정상바코드(string data)
        {
            var logic = new 차상위1Logic();
            var model = ParseLogic.Parse(data);
            var retv = logic.Validation(model);

            Assert.IsTrue(retv);
        }

        [TestCase(비정상바코드)]
        [TestCase(비정상바코드_유형2)]
        [TestCase(비정상바코드_유형2_2)]
        [TestCase(비정상바코드_유형3)]
        [TestCase(비정상바코드_유형3_2)]
        public void 차상위Logic_Validation_비정상바코드_1(string data)
        {
            var logic = new 차상위1Logic();
            var model = ParseLogic.Parse(비정상바코드);
            var retv = logic.Validation(model);
            var errorModel = logic.GetErrorModel(model, 0);

            Assert.Multiple(() =>
            {
                Assert.IsFalse(retv);

                Assert.That(1, Is.EqualTo(errorModel.Count()));

                Assert.That(@"차상위 보험은 보험구분이 1이고 공상및보훈구분은 16(차상위 1종(C)) 이어야 합니다.", Is.EqualTo(errorModel.First().메세지));
            });
        }

        [TestCase(정상바코드)]
        [TestCase(정상바코드_유형2)]
        [TestCase(정상바코드_유형3)]
        public void Form_검증하기버튼_정상바코드(string data)
        {
            var form = GetForm(data);

            form.검증하기버튼("", new 차상위1Logic());

            Assert.That(0, Is.EqualTo(form._오류목록.Count()));
        }

        [TestCase(비정상바코드)]
        [TestCase(비정상바코드_유형2)]
        [TestCase(비정상바코드_유형2_2)]
        [TestCase(비정상바코드_유형3)]
        [TestCase(비정상바코드_유형3_2)]
        public void Form_검증하기버튼_비정상바코드_유형2(string data)
        {
            var form = GetForm(data);

            form.검증하기버튼("", new 차상위1Logic());

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(form._오류목록.Count()));
                Assert.That(@"차상위 보험은 보험구분이 1이고 공상및보훈구분은 16(차상위 1종(C)) 이어야 합니다.", Is.EqualTo(form._오류목록.First().메세지));
            });
        }

        private Form1 GetForm(string expectedValue)
        {
            var 검증하기Mock = new Mock<I검증하기>();
            var formMock = new Mock<IForm>();

            검증하기Mock.Setup(x => x.IsValid(It.IsAny<string>()))
                .Returns(true);

            검증하기Mock.Setup(x => x.Get암호화해제Data(It.IsAny<string>()))
                .Returns(expectedValue);

            var form = new Form1(검증하기Mock.Object, null, formMock.Object);
            return form;
        }

        #region Private Funcs

        #endregion

    }
}
