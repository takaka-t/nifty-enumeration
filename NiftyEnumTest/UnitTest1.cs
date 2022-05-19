using System;
using Xunit;

namespace NiftyEnumExtentionTest
{
    public class UnitTest1
    {

        enum AAA
        {
            a,
            b,
            c,
            d,
        }

        [Fact]
        public void Test1()
        {
            //new AAA()
            var a = AAA.b.StringValue();

            NiftyEnum.StringValue(AAA.b);

        }
    }
}
