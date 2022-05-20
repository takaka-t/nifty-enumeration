using NiftyEnum;
//using static NiftyEnum.Methods;
using System;
using System.Linq;
using Xunit;

namespace NiftyEnumExtentionTest
{
    public class NiftyEnumTest
    {

        enum TestEnum
        {
            [EnumStringValue("‚ ‚ ‚ ")]
            AAA,
            [EnumStringValue("‚¢‚¢‚¢")]
            BBB,
            [EnumStringValue("‚¤‚¤‚¤")]
            CCC,
            [EnumStringValue("‚¦‚¦‚¦")]
            DDD,
        }

        [Fact]
        public void Test1()
        {
            var str1 = TestEnum.AAA.StringValue();
            Assert.Equal("‚ ‚ ‚ ", str1);
            Assert.NotEqual("‚¢‚¢‚¢", str1);

            var items = EnumHelper.GetItems<TestEnum>();
            Assert.Equal(4, items.Count());

        }
    }
}
