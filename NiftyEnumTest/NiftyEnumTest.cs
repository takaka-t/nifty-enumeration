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
            [EnumStringValue("あああ")]
            AAA,
            [EnumStringValue("いいい")]
            BBB,
            [EnumStringValue("ううう")]
            CCC,
            [EnumStringValue("えええ")]
            DDD,
        }

        [Fact]
        public void Test1()
        {
            var str1 = TestEnum.AAA.StringValue();
            Assert.Equal("あああ", str1);
            Assert.NotEqual("いいい", str1);

            var items = EnumHelper.GetItems<TestEnum>();
            Assert.Equal(4, items.Count());

        }
    }
}
