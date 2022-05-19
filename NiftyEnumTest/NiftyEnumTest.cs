using NiftyEnum;
using System;
using Xunit;

namespace NiftyEnumExtentionTest
{
    public class NiftyEnumTest
    {

        enum AAA
        {
            [EnumStringValue("aaa")]
            aaa,
            bbb,
            ccc,
            ddd,
        }

        [Fact]
        public void Test1()
        {
            //new AAA()
            var a = AAA.bbb.StringValue();
            Methods.GetItems<AAA>();
            //NiftyEnum.StringValue(AAA.b);

        }
    }
}
