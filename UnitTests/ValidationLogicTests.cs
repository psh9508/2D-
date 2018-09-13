using _2D보험구분검증툴;
using _2D보험구분검증툴.Class;
using _2D보험구분검증툴.Interface;
using _2D보험구분검증툴.Logic;
using _2D보험구분검증툴.Logic.보험구분Logic;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UnitTests
{
    [TestFixture]
    public class ValidationLogicTests
    {
        const string 정상바코드 = @"MSH¿0.8.0.0¿001¿YSR2000¿20081117090000
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
RXD¿1¿1¿1¿644501350¿¿0.330¿3¿1¿¿";

        const string 정상바코드유형2 = @"MSH¿0.8.0.0¿244¿Mediwell¿20180710005728
FAC¿34342664¿삼성디스플레이아산부속의원¿천안시 성성동 510번지¿041)529-6888¿031)208-5999¿
PRD¿정형선¿의과¿555555
PID¿김구번¿9003012123456
ORC¿2008111711002¿01¿¿3¿¿¿
DG1¿J00¿
IN1¿1¿¿¿75217119915¿¿¿¿¿¿¿¿¿
RXD¿1¿1¿2¿644800190¿ ¿ 1.00¿1¿3¿ ¿
RXD¿1¿1¿1¿650700070¿ ¿ 1.00¿3¿3¿ ¿
RXD¿1¿1¿2¿671700110¿ ¿ 1.00¿1¿1¿ ¿
"; // 마지막 줄에 줄바꿈이 있다.(유형2로 정의)

        string 환자명공백 = @"MSH¿0.8.0.0¿001¿YSR2000¿20081117090000
FAC¿ys040203¿의사랑의원¿¿041-583-0123¿02-2105-5091¿
PRD¿김의사¿의사¿42409
PID¿의 사랑¿7711111111111
ORC¿2008111711002¿¿23¿¿7¿¿저함량 배수처방 사유♬A45900471, 사유코드 : C
DG1¿ ¿ 
IN1¿1¿¿¿12345678¿70271724¿¿¿¿¿¿¿¿
RXD¿1¿2¿1¿661900010¿¿0.330¿3¿1¿¿
RXD¿1¿1¿1¿644900310¿¿2.500¿4¿1¿¿
RXD¿1¿1¿1¿643306160¿¿0.330¿3¿1¿¿
RXD¿1¿1¿1¿642403700¿¿0.330¿3¿1¿¿
RXD¿1¿1¿2¿641800840¿¿1.000¿1¿1¿¿
RXD¿1¿1¿2¿693200860¿¿1.000¿1¿1¿odw¿첫주는하루한두번두째주부터는1주에2번사용
RXD¿1¿1¿1¿644501350¿¿0.330¿3¿1¿¿";

        string 처방의료인공백 = @"MSH¿0.8.0.0¿001¿YSR2000¿20081117090000
FAC¿ys040203¿의사랑의원¿¿041-583-0123¿02-2105-5091¿
PRD¿김의 사¿의사¿42409
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
RXD¿1¿1¿1¿644501350¿¿0.330¿3¿1¿¿";

        string 약품정보모두에러 = @"MSH¿0.8.0.0¿001¿YSR2000¿20081117090000
FAC¿ys040203¿의사랑의원¿¿041-583-0123¿02-2105-5091¿
PRD¿김의사¿의사¿42409
PID¿의사랑¿7711111111111
ORC¿2008111711002¿¿23¿¿7¿¿저함량 배수처방 사유♬A45900471, 사유코드 : C
DG1¿ ¿ 
IN1¿1¿¿¿12345678¿70271724¿¿¿¿¿¿¿¿
RXD¿7¿8¿1¿661900010¿¿0.330¿3¿1¿¿
RXD¿1¿1¿1¿644900310¿¿2.500¿4¿1¿¿
RXD¿1¿8¿1¿643306160¿¿0.330¿3¿1¿¿
RXD¿1¿1¿1¿642403700¿¿0.330¿3¿1¿¿
RXD¿7¿1¿1¿641800840¿¿1.000¿1¿1¿¿
RXD¿1¿1¿0¿693200860¿¿1.000¿1¿1¿odw¿첫주는하루한두번두째주부터는1주에2번사용
RXD¿1¿8¿0¿644501350¿¿0.330¿3¿1¿¿";

        const string 교부번호에러_1 = @"MSH¿0.8.0.0¿244¿Mediwell¿20180710005728
FAC¿34342664¿삼성디스플레이아산부속의원¿천안시 성성동 510번지¿041)529-6888¿031)208-5999¿
PRD¿정형선¿의과¿555555
PID¿김구번¿9003012123456
ORC¿11002¿01¿¿3¿¿¿
DG1¿J00¿
IN1¿1¿¿¿75217119915¿¿¿¿¿¿¿¿¿
RXD¿1¿1¿2¿644800190¿ ¿ 1.00¿1¿3¿ ¿
RXD¿1¿1¿1¿650700070¿ ¿ 1.00¿3¿3¿ ¿
RXD¿1¿1¿2¿671700110¿ ¿ 1.00¿1¿1¿ ¿
"; // 마지막 줄에 줄바꿈이 있다.(유형2로 정의)

        const string 교부번호에러_2 = @"MSH¿0.8.0.0¿244¿Mediwell¿20180710005728
FAC¿34342664¿삼성디스플레이아산부속의원¿천안시 성성동 510번지¿041)529-6888¿031)208-5999¿
PRD¿정형선¿의과¿555555
PID¿김구번¿9003012123456
ORC¿2008131711002¿01¿¿3¿¿¿
DG1¿J00¿
IN1¿1¿¿¿75217119915¿¿¿¿¿¿¿¿¿
RXD¿1¿1¿2¿644800190¿ ¿ 1.00¿1¿3¿ ¿
RXD¿1¿1¿1¿650700070¿ ¿ 1.00¿3¿3¿ ¿
RXD¿1¿1¿2¿671700110¿ ¿ 1.00¿1¿1¿ ¿
"; // 마지막 줄에 줄바꿈이 있다.(유형2로 정의)

        const string 교부번호에러_3 = @"MSH¿0.8.0.0¿244¿Mediwell¿20180710005728
FAC¿34342664¿삼성디스플레이아산부속의원¿천안시 성성동 510번지¿041)529-6888¿031)208-5999¿
PRD¿정형선¿의과¿555555
PID¿김구번¿9003012123456
ORC¿20082129811002¿01¿¿3¿¿¿
DG1¿J00¿
IN1¿1¿¿¿75217119915¿¿¿¿¿¿¿¿¿
RXD¿1¿1¿2¿644800190¿ ¿ 1.00¿1¿3¿ ¿
RXD¿1¿1¿1¿650700070¿ ¿ 1.00¿3¿3¿ ¿
RXD¿1¿1¿2¿671700110¿ ¿ 1.00¿1¿1¿ ¿
"; // 마지막 줄에 줄바꿈이 있다.(유형2로 정의)

        const string 교부번호에러_4 = @"MSH¿0.8.0.0¿244¿Mediwell¿20180710005728
FAC¿34342664¿삼성디스플레이아산부속의원¿천안시 성성동 510번지¿041)529-6888¿031)208-5999¿
PRD¿정형선¿의과¿555555
PID¿김구번¿9003012123456
ORC¿200899999002¿01¿¿3¿¿¿
DG1¿J00¿
IN1¿1¿¿¿75217119915¿¿¿¿¿¿¿¿¿
RXD¿1¿1¿2¿644800190¿ ¿ 1.00¿1¿3¿ ¿
RXD¿1¿1¿1¿650700070¿ ¿ 1.00¿3¿3¿ ¿
RXD¿1¿1¿2¿671700110¿ ¿ 1.00¿1¿1¿ ¿
"; // 마지막 줄에 줄바꿈이 있다.(유형2로 정의)

        const string MSH구분자개수부족 = @"MSH¿0.8.0.0¿001¿YSR2000
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
RXD¿1¿1¿1¿644501350¿¿0.330¿3¿1¿¿";

        string MSH해더누락 = @"FAC¿ys040203¿의사랑의원¿¿041-583-0123¿02-2105-5091¿
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
RXD¿1¿1¿1¿644501350¿¿0.330¿3¿1¿¿";

        string FACIN1해더누락 = @"MSH¿0.8.0.0¿244¿Mediwell¿20180710005728
PRD¿정형선¿의과¿555555
PID¿김구번¿9003012123456
ORC¿00001¿01¿¿3¿¿¿
DG1¿J00¿
RXD¿1¿1¿2¿644800190¿ ¿ 1.00¿1¿3¿ ¿
RXD¿1¿1¿1¿650700070¿ ¿ 1.00¿3¿3¿ ¿
RXD¿1¿1¿2¿671700110¿ ¿ 1.00¿1¿1¿ ¿
"; // 마지막 줄에 줄바꿈이 있다.(유형2로 정의)

        string RXD해더누락 = @"MSH¿0.8.0.0¿001¿YSR2000¿20081117090000
FAC¿ys040203¿의사랑의원¿¿041-583-0123¿02-2105-5091¿
PRD¿김의사¿의사¿42409
PID¿의사랑¿7711111111111
ORC¿2008111711002¿¿23¿¿7¿¿저함량 배수처방 사유♬A45900471, 사유코드 : C
DG1¿ ¿ 
IN1¿1¿¿¿12345678¿70271724¿¿¿¿¿¿¿¿";

        const string MSH구분자개수부족_유형2 = @"MSH¿0.8.0.0¿001¿YSR2000
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
RXD¿1¿1¿1¿644501350¿¿0.330¿3¿1¿¿
";

        const string MSH구분자개수많음 = @"MSH¿0.8.0.0¿001¿YSR2000¿20081117090000¿
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
RXD¿1¿1¿1¿644501350¿¿0.330¿3¿1¿¿";

        string MSH와ORC구분자개수부족 = @"MSH¿0.8.0.0¿001¿YSR2000
FAC¿ys040203¿의사랑의원¿¿041-583-0123¿02-2105-5091¿
PRD¿김의사¿의사¿42409
PID¿의사랑¿7711111111111
ORC¿2008111711002¿23¿¿7¿¿저함량 배수처방 사유♬A45900471, 사유코드 : C
DG1¿ ¿ 
IN1¿1¿¿¿12345678¿70271724¿¿¿¿¿¿¿¿
RXD¿1¿2¿1¿661900010¿¿0.330¿3¿1¿¿
RXD¿1¿1¿1¿644900310¿¿2.500¿4¿1¿¿
RXD¿1¿1¿1¿643306160¿¿0.330¿3¿1¿¿
RXD¿1¿1¿1¿642403700¿¿0.330¿3¿1¿¿
RXD¿1¿1¿2¿641800840¿¿1.000¿1¿1¿¿
RXD¿1¿1¿2¿693200860¿¿1.000¿1¿1¿odw¿첫주는하루한두번두째주부터는1주에2번사용
RXD¿1¿1¿1¿644501350¿¿0.330¿3¿1¿¿";

        string MSH와ORC구분자개수부족_유형2 = @"MSH¿0.8.0.0¿001¿YSR2000
FAC¿ys040203¿의사랑의원¿¿041-583-0123¿02-2105-5091¿
PRD¿김의사¿의사¿42409
PID¿의사랑¿7711111111111
ORC¿2008111711002¿23¿¿7¿¿저함량 배수처방 사유♬A45900471, 사유코드 : C
DG1¿ ¿ 
IN1¿1¿¿¿12345678¿70271724¿¿¿¿¿¿¿¿
RXD¿1¿2¿1¿661900010¿¿0.330¿3¿1¿¿
RXD¿1¿1¿1¿644900310¿¿2.500¿4¿1¿¿
RXD¿1¿1¿1¿643306160¿¿0.330¿3¿1¿¿
RXD¿1¿1¿1¿642403700¿¿0.330¿3¿1¿¿
RXD¿1¿1¿2¿641800840¿¿1.000¿1¿1¿¿
RXD¿1¿1¿2¿693200860¿¿1.000¿1¿1¿odw¿첫주는하루한두번두째주부터는1주에2번사용
RXD¿1¿1¿1¿644501350¿¿0.330¿3¿1¿¿
";

        string MSH와ORC와IN1구분자개수부족 = @"MSH¿0.8.0.0¿001¿YSR2000
FAC¿ys040203¿의사랑의원¿¿041-583-0123¿02-2105-5091¿
PRD¿김의사¿의사¿42409
PID¿의사랑¿7711111111111
ORC¿2008111711002¿23¿¿7¿¿저함량 배수처방 사유♬A45900471, 사유코드 : C
DG1¿ ¿ 
IN1¿1¿¿¿12345678¿70271724¿¿¿¿¿¿¿
RXD¿1¿2¿1¿661900010¿¿0.330¿3¿1¿¿
RXD¿1¿1¿1¿644900310¿¿2.500¿4¿1¿¿
RXD¿1¿1¿1¿643306160¿¿0.330¿3¿1¿¿
RXD¿1¿1¿1¿642403700¿¿0.330¿3¿1¿¿
RXD¿1¿1¿2¿641800840¿¿1.000¿1¿1¿¿
RXD¿1¿1¿2¿693200860¿¿1.000¿1¿1¿odw¿첫주는하루한두번두째주부터는1주에2번사용
RXD¿1¿1¿1¿644501350¿¿0.330¿3¿1¿¿";

        string RXD구분자개수부족 = @"MSH¿0.8.0.0¿001¿YSR2000¿20081117090000
FAC¿ys040203¿의사랑의원¿¿041-583-0123¿02-2105-5091¿
PRD¿김의사¿의사¿42409
PID¿의사랑¿7711111111111
ORC¿2008111711002¿¿23¿¿7¿¿저함량 배수처방 사유♬A45900471, 사유코드 : C
DG1¿ ¿ 
IN1¿1¿¿¿12345678¿70271724¿¿¿¿¿¿¿¿
RXD¿1¿2¿1¿661900010¿¿0.330¿3¿1¿
RXD¿1¿1¿1¿644900310¿¿2.500¿4¿1¿¿
RXD¿1¿1¿1¿643306160¿¿0.330¿3¿1¿
RXD¿1¿1¿1¿642403700¿¿0.330¿3¿1¿¿
RXD¿1¿1¿2¿641800840¿¿1.000¿1¿1¿¿
RXD¿1¿1¿2¿693200860¿¿1.000¿1¿1¿odw첫주는하루한두번두째주부터는1주에2번사용
RXD¿1¿1¿1¿644501350¿¿0.330¿3¿1¿";

        string RXD구분자개수부족_유형2 = @"MSH¿0.8.0.0¿001¿YSR2000¿20081117090000
FAC¿ys040203¿의사랑의원¿¿041-583-0123¿02-2105-5091¿
PRD¿김의사¿의사¿42409
PID¿의사랑¿7711111111111
ORC¿2008111711002¿¿23¿¿7¿¿저함량 배수처방 사유♬A45900471, 사유코드 : C
DG1¿ ¿ 
IN1¿1¿¿¿12345678¿70271724¿¿¿¿¿¿¿¿
RXD¿1¿2¿1¿661900010¿¿0.330¿3¿1¿
RXD¿1¿1¿1¿644900310¿¿2.500¿4¿1¿¿
RXD¿1¿1¿1¿643306160¿¿0.330¿3¿1¿
RXD¿1¿1¿1¿642403700¿¿0.330¿3¿1¿¿
RXD¿1¿1¿2¿641800840¿¿1.000¿1¿1¿¿
RXD¿1¿1¿2¿693200860¿¿1.000¿1¿1¿odw첫주는하루한두번두째주부터는1주에2번사용
RXD¿1¿1¿1¿644501350¿¿0.330¿3¿1¿
";

        string 약품명생략되지않은바코드 = @"MSH¿0.8.0.0¿001¿YSR2000¿20081117090000
FAC¿ys040203¿의사랑의원¿¿041-583-0123¿02-2105-5091¿
PRD¿김의사¿의사¿42409
PID¿의사랑¿7711111111111
ORC¿2008111711002¿¿23¿¿7¿¿저함량 배수처방 사유♬A45900471, 사유코드 : C
DG1¿ ¿ 
IN1¿1¿¿¿12345678¿70271724¿¿¿¿¿¿¿¿
RXD¿1¿2¿1¿661900010¿약품명¿0.330¿3¿1¿¿
RXD¿1¿1¿1¿644900310¿¿2.500¿4¿1¿¿
RXD¿1¿1¿1¿643306160¿¿0.330¿3¿1¿¿
RXD¿1¿1¿1¿642403700¿약품명¿0.330¿3¿1¿¿
RXD¿1¿1¿2¿641800840¿¿1.000¿1¿1¿¿
RXD¿1¿1¿2¿693200860¿¿1.000¿1¿1¿odw¿첫주는하루한두번두째주부터는1주에2번사용
RXD¿1¿1¿1¿644501350¿약품명¿0.330¿3¿1¿¿";

        string 줄바꿈비정상 = @"MSH¿0.8.0.0¿161¿PHOENIX¿20180810091319FAC¿10240075¿지누스¿서울 송파구 위례성대로 34 (방이동, 지산빌딩) 123-123¿010-411-1110¿02-411-1199¿lej @hyny.co.krPRD¿의사¿의사¿12345PID¿테스트¿1111111111111ORC¿2018081000005¿¿1¿¿3¿TID - 1일 3회 식후 30분에 복용하십시오¿DG1¿J00¿IN1¿1¿1¿¿123456789¿¿¿¿¿¿¿¿¿RXD¿1¿1¿1¿644900310¿¿0.333¿3¿1¿TID¿";

        const string 모든헤더없음_1 = @"MSH¿0.8.0.0¿155¿챠트프로¿20180821113655
FAC¿12362093¿병원과컴퓨터¿¿032-561-7575¿032-567-4298¿
PRD¿일반과¿의사¿12345
PID¿이태규¿6909191446718
ORD¿2018082100001¿¿¿V193¿7¿¿중증질환 : V193
DG1¿¿¿1¿¿¿12345¿1011¿서울제1지구¿¿ ¿ ¿ ¿ ¿ ¿ 
RXD¿1¿1¿1¿620500040¿ ¿ 2.5¿4¿1¿ ¿
RXD¿1¿1¿1¿621802770¿ ¿ 0.3333¿3¿1¿ ¿
RXD¿1¿1¿1¿622801960¿ ¿ 1¿3¿1¿ ¿";

        const string 모든헤더없음_2 = @"MSH¿0.8.0.0¿155¿챠트프로¿20180821113655
FAC¿12362093¿병원과컴퓨터¿¿032-561-7575¿032-567-4298¿
PRD¿일반과¿의사¿12345
PI¿이태규¿6909191446718
ORC¿2018082100001¿¿¿V193¿7¿¿중증질환 : V193
DG1¿¿¿1¿¿¿12345¿1011¿서울제1지구¿¿ ¿ ¿ ¿ ¿ ¿ 
RXD¿1¿1¿1¿620500040¿ ¿ 2.5¿4¿1¿ ¿
RXD¿1¿1¿1¿621802770¿ ¿ 0.3333¿3¿1¿ ¿
RXD¿1¿1¿1¿622801960¿ ¿ 1¿3¿1¿ ¿";

        const string 모든헤더없음_3 = @"MSH¿0.8.0.0¿155¿챠트프로¿20180821113655
FAC¿12362093¿병원과컴퓨터¿¿032-561-7575¿032-567-4298¿
PID¿이태규¿6909191446718
ORC¿2018082100001¿¿¿V193¿7¿¿중증질환 : V193
DG1¿¿¿1¿¿¿12345¿1011¿서울제1지구¿¿ ¿ ¿ ¿ ¿ ¿ 
RXD¿1¿1¿1¿620500040¿ ¿ 2.5¿4¿1¿ ¿
RXD¿1¿1¿1¿621802770¿ ¿ 0.3333¿3¿1¿ ¿
RXD¿1¿1¿1¿622801960¿ ¿ 1¿3¿1¿ ¿";

        const string 모든헤더없음_4 = @"MSH¿0.8.0.0¿155¿챠트프로¿20180821113655
FAD¿12362093¿병원과컴퓨터¿¿032-561-7575¿032-567-4298¿
PRD¿일반과¿의사¿12345
PID¿이태규¿6909191446718
ORC¿2018082100001¿¿¿V193¿7¿¿중증질환 : V193
DG1¿¿¿1¿¿¿12345¿1011¿서울제1지구¿¿ ¿ ¿ ¿ ¿ ¿ 
RXD¿1¿1¿1¿620500040¿ ¿ 2.5¿4¿1¿ ¿
RXD¿1¿1¿1¿621802770¿ ¿ 0.3333¿3¿1¿ ¿
RXD¿1¿1¿1¿622801960¿ ¿ 1¿3¿1¿ ¿";

        const string 모든헤더없음_5 = @"MSH¿0.8.0.0¿155¿챠트프로¿20180821113655
FAD¿12362093¿병원과컴퓨터¿¿032-561-7575¿032-567-4298¿
PRD¿일반과¿의사¿12345
PID¿이태규¿6909191446718
ORC¿2018082100001¿¿¿V193¿7¿¿중증질환 : V193
DG1¿¿¿1¿¿¿12345¿1011¿서울제1지구¿¿ ¿ ¿ ¿ ¿ ¿ 
RXS¿1¿1¿1¿620500040¿ ¿ 2.5¿4¿1¿ ¿
RXS¿1¿1¿1¿621802770¿ ¿ 0.3333¿3¿1¿ ¿
RXS¿1¿1¿1¿622801960¿ ¿ 1¿3¿1¿ ¿";

        const string 모든헤더없음_6 = @"MSH¿0.8.0.0¿155¿챠트프로¿20180821113655
FAD¿12362093¿병원과컴퓨터¿¿032-561-7575¿032-567-4298¿
PRD¿일반과¿의사¿12345
PID¿이태규¿6909191446718
ORC¿2018082100001¿¿¿V193¿7¿¿중증질환 : V193
DG1¿¿¿1¿¿¿12345¿1011¿서울제1지구¿¿ ¿ ¿ ¿ ¿ ¿ ";

        const string 사용기간숫자아닌경우 = @"MSH¿0.8.0.0¿291¿PHIS¿20180903090320
FAC¿38700085¿의령군보건소¿경상남도 의령군 의령읍 서동리 534¿055-570-4005¿055-570-4018¿gu0609@korea.kr
PRD¿정희용¿의사¿130812
PID¿김종호¿5101041891910
ORC¿2018090310001¿¿¿¿7일¿¿
DG1¿I109¿
IN1¿1¿¿¿30073936919¿¿¿¿¿¿¿¿¿
RXD¿1¿1¿1¿643306540¿¿1¿1¿90¿¿
RXD¿1¿1¿1¿641100270¿¿1¿1¿90¿¿
";


        [SetUp]
        public void SetUp()
        {
            MessageHelper.IsTest = true;
        }

        [TearDown]
        public void TearDown()
        {
            MessageHelper.IsTest = false;
        }

        [TestCase(정상바코드)]
        [TestCase(정상바코드유형2)]
        public void Form_GetErrorData_정상바코드Test_1(string data)
        {
            var form = new Form1(null, null);

            var errorModel = form.GetErrorData(data);

            Assert.That(0, Is.EqualTo(errorModel.Count));
        }

        [TestCase(정상바코드)]
        [TestCase(정상바코드유형2)]
        public void ValidationLogic_GetErrorMessage_정상바코드유형Test(string barcode)
        {
            if (barcode.EndsWith("\r\n"))
            {
                var idx = barcode.LastIndexOf("\r\n");
                barcode = barcode.Substring(0, idx);
            }

            var data = barcode.Replace("\r\n", "¿").Split('¿');

            var errorModel = _2D보험구분검증툴.Logic.ValidationLogic.GetErrorMessage(data);

            Assert.That(0, Is.EqualTo(errorModel.Count));
        }


        [TestCase(MSH구분자개수부족)]
        [TestCase(MSH구분자개수많음)]
        [TestCase(MSH구분자개수부족_유형2)]
        public void Form_GetErrorData_MSH구분자개수부족Test(string data)
        {
            var form = new Form1(null, null);

            var errorModel = form.GetErrorData(data);

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(errorModel.Count));
                Assert.That("MSH의 구분자 개수가 다릅니다.", Is.EqualTo(errorModel[0].메세지));
            });
        }

        [Test]
        public void Form_GetErrorData_MSH해더누락Test()
        {
            var form = new Form1(null, null);

            var errorModel = form.GetErrorData(MSH해더누락);

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(errorModel.Count));
                Assert.That("[MSH] 헤더가 누락 되었습니다. 혹은 헤더 뒤에 구분자가 잘 들어있는지 확인해주세요.", Is.EqualTo(errorModel[0].메세지));
            });
        }

        [Test]
        public void Form_GetErrorData_FACIN1해더누락Test()
        {
            var form = new Form1(null, null);

            var errorModel = form.GetErrorData(FACIN1해더누락);

            Assert.Multiple(() =>
            {
                Assert.That(2, Is.EqualTo(errorModel.Count));
                Assert.That("[FAC] 헤더가 누락 되었습니다. 혹은 헤더 뒤에 구분자가 잘 들어있는지 확인해주세요.", Is.EqualTo(errorModel[0].메세지));
                Assert.That("[IN1] 헤더가 누락 되었습니다. 혹은 헤더 뒤에 구분자가 잘 들어있는지 확인해주세요.", Is.EqualTo(errorModel[1].메세지));
            });
        }

        [Test]
        public void Form_GetErrorData_약품정보모두누락Test()
        {
            var form = new Form1(null, null);

            var errorModel = form.GetErrorData(약품정보모두에러);

            Assert.Multiple(() =>
            {
                Assert.That(7, Is.EqualTo(errorModel.Count));
                Assert.That("1번째 약품의 처방구분값이 유효하지 않습니다.API문서의 데이터 범위를 다시한번 확인 바랍니다.", Is.EqualTo(errorModel[0].메세지));
                Assert.That("1번째 약품의 급여구분값이 유효하지 않습니다. API문서의 데이터 범위를 다시한번 확인 바랍니다.", Is.EqualTo(errorModel[1].메세지));
                Assert.That("3번째 약품의 급여구분값이 유효하지 않습니다. API문서의 데이터 범위를 다시한번 확인 바랍니다.", Is.EqualTo(errorModel[2].메세지));
                Assert.That("5번째 약품의 처방구분값이 유효하지 않습니다.API문서의 데이터 범위를 다시한번 확인 바랍니다.", Is.EqualTo(errorModel[3].메세지));
                Assert.That("6번째 약품의 코드구분값이 유효하지 않습니다. API문서의 데이터 범위를 다시한번 확인 바랍니다.", Is.EqualTo(errorModel[4].메세지));
                Assert.That("7번째 약품의 급여구분값이 유효하지 않습니다. API문서의 데이터 범위를 다시한번 확인 바랍니다.", Is.EqualTo(errorModel[5].메세지));
                Assert.That("7번째 약품의 코드구분값이 유효하지 않습니다. API문서의 데이터 범위를 다시한번 확인 바랍니다.", Is.EqualTo(errorModel[6].메세지));
            });
        }

        [Test]
        public void Form_GetErrorData_환자이름공백Test()
        {
            var form = new Form1(null, null);

            var errorModel = form.GetErrorData(환자명공백);

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(errorModel.Count));
                Assert.That("환자명에 공백이 포함되어 있습니다. 공백 제거 후 입력 바랍니다.", Is.EqualTo(errorModel[0].메세지));
            });
        }

        [Test]
        public void Form_GetErrorData_처방의이름공백Test()
        {
            var form = new Form1(null, null);

            var errorModel = form.GetErrorData(처방의료인공백);

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(errorModel.Count));
                Assert.That("처방의사명에 공백이 포함되어 있습니다. 공백 제거 후 입력 바랍니다.", Is.EqualTo(errorModel[0].메세지));
            });
        }

        [TestCase(교부번호에러_1)]
        [TestCase(교부번호에러_2)]
        [TestCase(교부번호에러_3)]
        [TestCase(교부번호에러_4)]
        public void Form_GetErrorData_교부번호_1Test(string data)
        {
            var form = new Form1(null, null);

            var errorModel = form.GetErrorData(data);

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(errorModel.Count));
                Assert.That("교부번호 앞 8 자리가 날짜 타입이 아닙니다. YYYYMMDD 형식으로 입력 바랍니다.", Is.EqualTo(errorModel[0].메세지));
            });
        }

        [Test]
        public void Form_GetErrorData_RXD해더누락Test()
        {
            var form = new Form1(null, null);

            var errorModel = form.GetErrorData(RXD해더누락);

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(errorModel.Count));
                Assert.That("[RXD] 헤더가 누락 되었습니다. 혹은 헤더 뒤에 구분자가 잘 들어있는지 확인해주세요.", Is.EqualTo(errorModel[0].메세지));
            });
        }


        [TestCase(MSH구분자개수부족)]
        [TestCase(MSH구분자개수많음)]
        [TestCase(MSH구분자개수부족_유형2)]
        public void ValidationLogic_GetErrorMessage_MSH구분자개수Test(string barcode)
        {
            // 가장 마지막에 \r\n이 있다면 삭제해준다.
            while (barcode.EndsWith("\r\n"))
            {
                var idx = barcode.LastIndexOf("\r\n");
                barcode = barcode.Substring(0, idx);
            }

            var data = barcode.Replace("\r\n", "¿").Split('¿');

            var errorModel = _2D보험구분검증툴.Logic.ValidationLogic.GetErrorMessage(data);

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(errorModel.Count));
                Assert.That("MSH의 구분자 개수가 다릅니다.", Is.EqualTo(errorModel[0].메세지));
            });
        }


        [Test]
        public void Form_GetErrorData_MSH와ORC구분자개수부족Test()
        {
            var form = new Form1(null, null);

            var errorModel = form.GetErrorData(MSH와ORC구분자개수부족_유형2);

            Assert.Multiple(() =>
            {
                Assert.That(2, Is.EqualTo(errorModel.Count));
                Assert.That("MSH의 구분자 개수가 다릅니다.", Is.EqualTo(errorModel[0].메세지));
                Assert.That("ORC의 구분자 개수가 다릅니다.", Is.EqualTo(errorModel[1].메세지));
            });
        }

        [Test]
        public void ValidationLogic_GetErrorMessage_MSH와ORC구분자개수부족Test()
        {
            var data = MSH와ORC구분자개수부족.Replace("\r\n", "¿").Split('¿');

            var errorModel = _2D보험구분검증툴.Logic.ValidationLogic.GetErrorMessage(data);

            Assert.Multiple(() =>
            {
                Assert.That(2, Is.EqualTo(errorModel.Count));
                Assert.That("MSH의 구분자 개수가 다릅니다.", Is.EqualTo(errorModel[0].메세지));
                Assert.That("ORC의 구분자 개수가 다릅니다.", Is.EqualTo(errorModel[1].메세지));
            });
        }

        [Test]
        public void MSH와ORC와IN1구분자개수부족Test()
        {
            var data = MSH와ORC와IN1구분자개수부족.Replace("\r\n", "¿").Split('¿');

            var errorModel = _2D보험구분검증툴.Logic.ValidationLogic.GetErrorMessage(data);

            Assert.Multiple(() =>
            {
                Assert.That(3, Is.EqualTo(errorModel.Count));
                Assert.That("MSH의 구분자 개수가 다릅니다.", Is.EqualTo(errorModel[0].메세지));
                Assert.That("ORC의 구분자 개수가 다릅니다.", Is.EqualTo(errorModel[1].메세지));
                Assert.That("IN1의 구분자 개수가 다릅니다.", Is.EqualTo(errorModel[2].메세지));
            });
        }

        [Test]
        public void Form_GetErrorData_RXD구분자개수부족Test()
        {
            var form = new Form1(null, null);

            var errorModel = form.GetErrorData(RXD구분자개수부족_유형2);

            Assert.Multiple(() =>
            {
                Assert.That(4, Is.EqualTo(errorModel.Count));
                Assert.That("1번째 RXD의 구분자 개수가 다릅니다.", Is.EqualTo(errorModel[0].메세지));
                Assert.That("3번째 RXD의 구분자 개수가 다릅니다.", Is.EqualTo(errorModel[1].메세지));
                Assert.That("6번째 RXD의 구분자 개수가 다릅니다.", Is.EqualTo(errorModel[2].메세지));
                Assert.That("7번째 RXD의 구분자 개수가 다릅니다.", Is.EqualTo(errorModel[3].메세지));
            });
        }

        [Test]
        public void ValidationLogic_GetErrorMessage_RXD구분자개수부족Test()
        {
            var data = RXD구분자개수부족.Replace("\r\n", "¿").Split('¿');

            var errorModel = _2D보험구분검증툴.Logic.ValidationLogic.GetErrorMessage(data);

            Assert.Multiple(() =>
            {
                Assert.That(4, Is.EqualTo(errorModel.Count));
                Assert.That("1번째 RXD의 구분자 개수가 다릅니다.", Is.EqualTo(errorModel[0].메세지));
                Assert.That("3번째 RXD의 구분자 개수가 다릅니다.", Is.EqualTo(errorModel[1].메세지));
                Assert.That("6번째 RXD의 구분자 개수가 다릅니다.", Is.EqualTo(errorModel[2].메세지));
                Assert.That("7번째 RXD의 구분자 개수가 다릅니다.", Is.EqualTo(errorModel[3].메세지));
            });
        }

        [Test]
        public void 약품명생략여부()
        {
            var data = 약품명생략되지않은바코드.Replace("\r\n", "¿").Split('¿');

            var errorModel = _2D보험구분검증툴.Logic.ValidationLogic.GetErrorMessage(data);

            Assert.Multiple(() =>
            {
                Assert.That(3, Is.EqualTo(errorModel.Count));
                Assert.That("1번째 RXD헤더의 약품명이 생략되지 않았습니다. 청구코드가 있는 경우 약품명은 생략되어야합니다.", Is.EqualTo(errorModel[0].메세지));
                Assert.That("4번째 RXD헤더의 약품명이 생략되지 않았습니다. 청구코드가 있는 경우 약품명은 생략되어야합니다.", Is.EqualTo(errorModel[1].메세지));
                Assert.That("7번째 RXD헤더의 약품명이 생략되지 않았습니다. 청구코드가 있는 경우 약품명은 생략되어야합니다.", Is.EqualTo(errorModel[2].메세지));
            });
        }

        [Test]
        public void 사용기간숫자아닌경우Test()
        {
            var data = 사용기간숫자아닌경우.Replace("\r\n", "¿").Split('¿');

            var errorModel = _2D보험구분검증툴.Logic.ValidationLogic.GetErrorMessage(data);

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(errorModel.Count));
                Assert.That(errorModel[0].메세지, Is.EqualTo("ORC헤더의 사용기간은 정수값만 들어가야 합니다."));
            });
        }

        //[Test]
        //public void 검증하기버튼_줄바꿈문제있는경우Test_1()
        //{
        //    #region Arrange
        //    // 줄바꿈 문제가 있는 바코드
        //    var expectedValue = "MSH¿0.8.0.0¿244¿Mediwell¿20180710005728\nFAC¿34342664¿삼성디스플레이아산부속의원¿천안시 성성동 510번지¿041)529-6888¿031)208-5999¿\nPRD¿정형선¿의과¿555555\nPID¿김구번¿9003012123456\nORC¿00001¿01¿¿3¿¿¿\nDG1¿J00¿\nIN1¿1¿¿¿75217119915¿¿¿¿¿¿¿¿¿\nRXD¿1¿1¿2¿644800190¿ ¿ 1.00¿1¿3¿ ¿\nRXD¿1¿1¿1¿650700070¿ ¿ 1.00¿3¿3¿ ¿\nRXD¿1¿1¿2¿671700110¿ ¿ 1.00¿1¿1¿ ¿\n";
        //    var mock검증하기 = new Mock<I검증하기>();

        //    mock검증하기.Setup(x => x.IsValid(It.IsAny<string>()))
        //        .Returns(true);
        //    mock검증하기.Setup(x => x.Get암호화해제Data(It.IsAny<string>()))
        //        .Returns(expectedValue.ToString());
        //    #endregion

        //    #region Act
        //    var form = new Form1(mock검증하기.Object, new 인증Logic(new 외부모듈()), new FormLogic());
        //    var result = form.검증하기버튼("", new 국민공단Logic());
        //    #endregion

        //    #region Assert
        //    Assert.That(false, Is.EqualTo(result));
        //    #endregion
        //}

        //[Test]
        //public void ValidationLogic_줄바꿈문제있는경우Test_1()
        //{
        //    #region Arrange
        //    // 줄바꿈 문제가 있는 바코드
        //    var expectedValue = "MSH¿0.8.0.0¿244¿Mediwell¿20180710005728\nFAC¿34342664¿삼성디스플레이아산부속의원¿천안시 성성동 510번지¿041)529-6888¿031)208-5999¿\nPRD¿정형선¿의과¿555555\nPID¿김구번¿9003012123456\nORC¿00001¿01¿¿3¿¿¿\nDG1¿J00¿\nIN1¿1¿¿¿75217119915¿¿¿¿¿¿¿¿¿\nRXD¿1¿1¿2¿644800190¿ ¿ 1.00¿1¿3¿ ¿\nRXD¿1¿1¿1¿650700070¿ ¿ 1.00¿3¿3¿ ¿\nRXD¿1¿1¿2¿671700110¿ ¿ 1.00¿1¿1¿ ¿\n";
        //    var mock검증하기 = new Mock<I검증하기>();

        //    mock검증하기.Setup(x => x.IsValid(It.IsAny<string>()))
        //        .Returns(true);
        //    mock검증하기.Setup(x => x.Get암호화해제Data(It.IsAny<string>()))
        //        .Returns(expectedValue.ToString());
        //    #endregion

        //    #region Act
        //    var form = new _2D보험구분검증툴.Form1(mock검증하기.Object, new 인증Logic(new 외부모듈()), new FormLogic());
        //    var result = form.검증하기버튼("", new 국민공단Logic());
        //    #endregion

        //    #region Assert
        //    //Assert.That(ValidationLogic.HasAllProperHeaders(, Is.EqualTo(result));
        //    #endregion
        //}


        //[Test]
        //public void 검증하기버튼_줄바꿈문제없는경우Test_1()
        //{
        //    #region Arrange
        //    var expectedValue = new StringBuilder(1000000);
        //    expectedValue.Append(정상바코드);

        //    var mock검증하기 = new Mock<I검증하기>();

        //    mock검증하기.Setup(x => x.IsValid(It.IsAny<string>()))
        //        .Returns(true);

        //    mock검증하기.Setup(x => x.Get암호화해제Data(It.IsAny<string>()))
        //        .Returns(expectedValue.ToString());
        //    #endregion

        //    #region Act
        //    var form = new _2D보험구분검증툴.Form1(mock검증하기.Object, null, new FormLogic());
        //    var result = form.검증하기버튼("", new 국민공단Logic());
        //    #endregion

        //    #region Assert
        //    Assert.That(0, Is.EqualTo(form._오류목록.Count()));
        //    #endregion
        //}

        //[Test]
        //public void Form_Has줄바꿈Error()
        //{
        //    #region Arrange
        //    var mock검증하기 = new Mock<I검증하기>();

        //    mock검증하기.Setup(x => x.IsValid(It.IsAny<string>()))
        //        .Returns(true);

        //    mock검증하기.Setup(x => x.Get암호화해제Data(It.IsAny<string>()))
        //        .Returns(줄바꿈비정상);
        //    #endregion

        //    #region Act
        //    var form = new _2D보험구분검증툴.Form1(mock검증하기.Object, null, new FormLogic());
        //    var result = form.검증하기버튼("", null);
        //    #endregion

        //    #region Assert
        //    Assert.That(false, Is.EqualTo(result));
        //    #endregion
        //}



        //[TestCase(모든헤더없음_1)]
        //[TestCase(모든헤더없음_2)]
        //[TestCase(모든헤더없음_3)]
        //[TestCase(모든헤더없음_4)]
        //[TestCase(모든헤더없음_5)]
        //[TestCase(모든헤더없음_6)]
        //public void Form_HasAllProperHeaders(string barcode)
        //{
        //    #region Arrange
        //    var mock검증하기 = new Mock<I검증하기>();

        //    mock검증하기.Setup(x => x.IsValid(It.IsAny<string>()))
        //        .Returns(true);

        //    mock검증하기.Setup(x => x.Get암호화해제Data(It.IsAny<string>()))
        //        .Returns(barcode);
        //    #endregion

        //    #region Act
        //    var form = new _2D보험구분검증툴.Form1(mock검증하기.Object, null, new FormLogic());
        //    var result = form.검증하기버튼("", null);
        //    #endregion
        //    //MyLayoutTestException
        //    #region Assert
        //    Assert.That(false, Is.EqualTo(result));
        //    #endregion
        //}

        //[TestCase(모든헤더없음_1, @"ORC, IN1헤더가 존재하지 않습니다. 헤더의 이름과 존재유무를 확인해주세요.")]
        ////[TestCase(모든헤더없음_2)]
        ////[TestCase(모든헤더없음_3)]
        ////[TestCase(모든헤더없음_4)]
        ////[TestCase(모든헤더없음_5)]
        ////[TestCase(모든헤더없음_6)]
        //public void ValidationLogic_HasAllProperHeaders(string barcode, string expectedErrorMessage)
        //{
        //    #region Arrange
        //    var mock검증하기 = new Mock<I검증하기>();

        //    mock검증하기.Setup(x => x.IsValid(It.IsAny<string>()))
        //        .Returns(true);

        //    mock검증하기.Setup(x => x.Get암호화해제Data(It.IsAny<string>()))
        //        .Returns(barcode);
        //    #endregion

        //    #region Act
        //    var form = new _2D보험구분검증툴.Form1(mock검증하기.Object, null, new FormLogic());
        //    #endregion

        //    #region Assert
        //    //Assert.That(() => form.BasicLayoutTest(barcode), Throws.TypeOf<MyLayoutTestException>());
        //    Assert.That(() => form.BasicLayoutTest(barcode), Throws.TypeOf<MyLayoutTestException>().With.Message.EqualTo(expectedErrorMessage));
        //    #endregion
        //}
    }
}
