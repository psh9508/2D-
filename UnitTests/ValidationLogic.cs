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
    public class ValidationLogic
    {
        string 정상바코드 = @"MSH¿0.8.0.0¿001¿YSR2000¿20081117090000
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

        string 정상바코드유형2 = @"MSH¿0.8.0.0¿244¿Mediwell¿20180710005728
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

        string 교부번호에러_1 = @"MSH¿0.8.0.0¿244¿Mediwell¿20180710005728
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

        string 교부번호에러_2 = @"MSH¿0.8.0.0¿244¿Mediwell¿20180710005728
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

        string 교부번호에러_3 = @"MSH¿0.8.0.0¿244¿Mediwell¿20180710005728
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

        string 교부번호에러_4 = @"MSH¿0.8.0.0¿244¿Mediwell¿20180710005728
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

        string MSH구분자개수부족 = @"MSH¿0.8.0.0¿001¿YSR2000
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

        string MSH구분자개수부족_유형2 = @"MSH¿0.8.0.0¿001¿YSR2000
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

        string MSH구분자개수많음 = @"MSH¿0.8.0.0¿001¿YSR2000¿20081117090000¿
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

        [Test]
        public void Form_GetErrorData_정상바코드Test_1()
        {
            var form = new _2D보험구분검증툴.Form1(null, null, null);

            var errorModel = form.GetErrorData(정상바코드);

            Assert.That(0, Is.EqualTo(errorModel.Count));
        }

        [Test]
        public void ValidationLogic_GetErrorMessage_정상바코드Test_2()
        {
            var data = 정상바코드.Replace("\r\n", "¿").Split('¿');

            var errorModel = _2D보험구분검증툴.Logic.ValidationLogic.GetErrorMessage(data);

            Assert.That(0, Is.EqualTo(errorModel.Count));
        }

        [Test]
        public void Form_GetErrorData_정상바코드유형2Test_1()
        {
            var form = new _2D보험구분검증툴.Form1(null, null, null);

            var errorModel = form.GetErrorData(정상바코드유형2);

            Assert.That(0, Is.EqualTo(errorModel.Count));
        }

        [Test]
        public void ValidationLogic_GetErrorMessage_정상바코드유형2Test_2()
        {
            if (정상바코드유형2.EndsWith("\r\n"))
            {
                var idx = 정상바코드유형2.LastIndexOf("\r\n");
                정상바코드유형2 = 정상바코드유형2.Substring(0, idx);
            }

            var data = 정상바코드유형2.Replace("\r\n", "¿").Split('¿');

            var errorModel = _2D보험구분검증툴.Logic.ValidationLogic.GetErrorMessage(data);
        }

        [Test]
        public void Form_GetErrorData_MSH구분자개수부족Test()
        {
            var form = new _2D보험구분검증툴.Form1(null, null, null);

            var errorModel = form.GetErrorData(MSH구분자개수부족_유형2);

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(errorModel.Count));
                Assert.That("MSH의 구분자 개수가 다릅니다.", Is.EqualTo(errorModel[0].메세지));
            });
        }

        [Test]
        public void Form_GetErrorData_MSH해더누락Test()
        {
            var form = new _2D보험구분검증툴.Form1(null, null, null);

            var errorModel = form.GetErrorData(MSH해더누락);

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(errorModel.Count));
                Assert.That("[MSH] 헤더가 누락 되었습니다.", Is.EqualTo(errorModel[0].메세지));
            });
        }

        [Test]
        public void Form_GetErrorData_FACIN1해더누락Test()
        {
            var form = new _2D보험구분검증툴.Form1(null, null, null);

            var errorModel = form.GetErrorData(FACIN1해더누락);

            Assert.Multiple(() =>
            {
                Assert.That(2, Is.EqualTo(errorModel.Count));
                Assert.That("[FAC] 헤더가 누락 되었습니다.", Is.EqualTo(errorModel[0].메세지));
                Assert.That("[IN1] 헤더가 누락 되었습니다.", Is.EqualTo(errorModel[1].메세지));
            });
        }

        [Test]
        public void Form_GetErrorData_약품정보모두누락Test()
        {
            var form = new _2D보험구분검증툴.Form1(null, null, null);

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
            var form = new _2D보험구분검증툴.Form1(null, null, null);

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
            var form = new _2D보험구분검증툴.Form1(null, null, null);

            var errorModel = form.GetErrorData(처방의료인공백);

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(errorModel.Count));
                Assert.That("처방의사명에 공백이 포함되어 있습니다. 공백 제거 후 입력 바랍니다.", Is.EqualTo(errorModel[0].메세지));
            });
        }

        [Test]
        public void Form_GetErrorData_교부번호_1Test()
        {
            var form = new _2D보험구분검증툴.Form1(null, null, null);

            var errorModel = form.GetErrorData(교부번호에러_1);

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(errorModel.Count));
                Assert.That("교부번호 앞 8 자리가 날짜 타입이 아닙니다. YYYYMMDD 형식으로 입력 바랍니다.", Is.EqualTo(errorModel[0].메세지));
            });
        }

        [Test]
        public void Form_GetErrorData_교부번호_2Test()
        {
            var form = new _2D보험구분검증툴.Form1(null, null, null);

            var errorModel = form.GetErrorData(교부번호에러_2);

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(errorModel.Count));
                Assert.That("교부번호 앞 8 자리가 날짜 타입이 아닙니다. YYYYMMDD 형식으로 입력 바랍니다.", Is.EqualTo(errorModel[0].메세지));
            });
        }

        [Test]
        public void Form_GetErrorData_교부번호_3Test()
        {
            var form = new _2D보험구분검증툴.Form1(null, null, null);

            var errorModel = form.GetErrorData(교부번호에러_3);

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(errorModel.Count));
                Assert.That("교부번호 앞 8 자리가 날짜 타입이 아닙니다. YYYYMMDD 형식으로 입력 바랍니다.", Is.EqualTo(errorModel[0].메세지));
            });
        }

        [Test]
        public void Form_GetErrorData_교부번호_4Test()
        {
            var form = new _2D보험구분검증툴.Form1(null, null, null);

            var errorModel = form.GetErrorData(교부번호에러_4);

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(errorModel.Count));
                Assert.That("교부번호 앞 8 자리가 날짜 타입이 아닙니다. YYYYMMDD 형식으로 입력 바랍니다.", Is.EqualTo(errorModel[0].메세지));
            });
        }

        [Test]
        public void Form_GetErrorData_RXD해더누락Test()
        {
            var form = new _2D보험구분검증툴.Form1(null, null, null);

            var errorModel = form.GetErrorData(RXD해더누락);

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(errorModel.Count));
                Assert.That("[RXD] 헤더가 누락 되었습니다.", Is.EqualTo(errorModel[0].메세지));
            });
        }


        [Test]
        public void ValidationLogic_GetErrorMessage_MSH구분자개수부족Test()
        {
            var data = MSH구분자개수부족.Replace("\r\n", "¿").Split('¿');

            var errorModel = _2D보험구분검증툴.Logic.ValidationLogic.GetErrorMessage(data);

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(errorModel.Count));
                Assert.That("MSH의 구분자 개수가 다릅니다.", Is.EqualTo(errorModel[0].메세지));
            });
        }

        [Test]
        public void MSH구분자개수많음Test()
        {
            var data = MSH구분자개수많음.Replace("\r\n", "¿").Split('¿');

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
            var form = new _2D보험구분검증툴.Form1(null, null, null);

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
            var form = new _2D보험구분검증툴.Form1(null, null, null);

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
        public void 검증하기버튼_줄바꿈문제있는경우Test_1()
        {
            #region Arrange
            // 줄바꿈 문제가 있는 바코드
            var expectedValue = "MSH¿0.8.0.0¿244¿Mediwell¿20180710005728\nFAC¿34342664¿삼성디스플레이아산부속의원¿천안시 성성동 510번지¿041)529-6888¿031)208-5999¿\nPRD¿정형선¿의과¿555555\nPID¿김구번¿9003012123456\nORC¿00001¿01¿¿3¿¿¿\nDG1¿J00¿\nIN1¿1¿¿¿75217119915¿¿¿¿¿¿¿¿¿\nRXD¿1¿1¿2¿644800190¿ ¿ 1.00¿1¿3¿ ¿\nRXD¿1¿1¿1¿650700070¿ ¿ 1.00¿3¿3¿ ¿\nRXD¿1¿1¿2¿671700110¿ ¿ 1.00¿1¿1¿ ¿\n";
            var mock검증하기 = new Mock<I검증하기>();

            mock검증하기.Setup(x => x.IsValid(It.IsAny<string>()))
                .Returns(true);
            mock검증하기.Setup(x => x.Get암호화해제Data(It.IsAny<string>()))
                .Returns(expectedValue.ToString());
            #endregion

            #region Act
            var form = new _2D보험구분검증툴.Form1(mock검증하기.Object, new 인증Logic(new 외부모듈()), new FormLogic());
            var result = form.검증하기버튼("", new 국민공단Logic());
            #endregion

            #region Assert
            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(form._오류목록.Count()));
                Assert.That(@"[\r\n]로 이우러진 줄바꿈 문자가 없습니다.[\n]문자 대신 [\r\n]문자를 사용해주시기 바랍니다.", Is.EqualTo(form._오류목록[0].메세지));
            });
            #endregion
        }

        [Test]
        public void 검증하기버튼_줄바꿈문제없는경우Test_1()
        {
            #region Arrange
            var expectedValue = new StringBuilder(1000000);
            expectedValue.Append(정상바코드);

            var mock검증하기 = new Mock<I검증하기>();

            mock검증하기.Setup(x => x.IsValid(It.IsAny<string>()))
                .Returns(true);

            mock검증하기.Setup(x => x.Get암호화해제Data(It.IsAny<string>()))
                .Returns(expectedValue.ToString());
            #endregion

            #region Act
            var form = new _2D보험구분검증툴.Form1(mock검증하기.Object, null, new FormLogic());
            var result = form.검증하기버튼("", new 국민공단Logic());
            #endregion

            #region Assert
            Assert.That(0, Is.EqualTo(form._오류목록.Count()));
            #endregion
        }

    }
}
