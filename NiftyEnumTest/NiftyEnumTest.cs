using NiftyEnum;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace NiftyEnumTest
{
    public class NiftyEnumTest
    {

        private readonly ITestOutputHelper _output;

        public NiftyEnumTest(ITestOutputHelper output)
        {
            _output = output;
        }

        enum TestEnum
        {
            [EnumStringValue("あああ")]
            AAA,
            [EnumStringValue("いいい")]
            BBB,
            [EnumStringValue("ううう")]
            CCC,
            //[EnumStringValue("えええ")]
            DDD,
        }

        [Fact]
        public void NiftyEnumのStringValueテスト()
        {
            //文字列値取得
            var aaa = TestEnum.AAA.StringValue();
            Assert.Equal("あああ", aaa);

            //EnumStringValueが未設定の場合
            var ddd = TestEnum.DDD.StringValue();
            Assert.Equal("", ddd);
        }

        [Fact]
        public void NiftyEnumのGetItemsテスト()
        {
            //取得数正しいか
            var items = EnumHelper.GetItems<TestEnum>();
            Assert.Equal(4, items.Count());

            //1番目の要素で検証
            var bbb = items[1];
            Assert.Equal(TestEnum.BBB, bbb.Value);

            //こんな書き方もできるのか
            var (Value, StringValue) = items[2];//.ElementAt(2);
            Assert.Equal("ううう", StringValue);
        }

        [Fact]
        public void NiftyEnumのConvertテスト()
        {
            //範囲内の数値
            var aaa = EnumHelper.Convert<TestEnum>(0);
            Assert.Equal(TestEnum.AAA, aaa);
            //範囲外の数値
            Assert.Throws<InvalidCastException>(() => EnumHelper.Convert<TestEnum>(5));

            //存在するフィールド名
            aaa = EnumHelper.Convert<TestEnum>("AAA");
            Assert.Equal(TestEnum.AAA, aaa);
            //存在しないフィールド名
            Assert.Throws<InvalidCastException>(() => EnumHelper.Convert<TestEnum>("AAB"));
        }

        [Fact]
        public void EnumのToStingとGetValues()
        {
            //ToStringはフィールド名がとれる
            var aaaStr = TestEnum.AAA.ToString();
            Assert.Equal("AAA", aaaStr);

            //全件取得
            var values = Enum.GetValues<TestEnum>();
            Assert.Equal(4, values.Length);
            Assert.Equal(TestEnum.BBB, values[1]);
        }

        [Fact]
        public void Enumのキャスト()
        {
            //数値をキャストすることで変換可能
            var bbb = (TestEnum)1;
            Assert.Equal(TestEnum.BBB, bbb);

            //範囲外はそのまま数値の5がかえる
            var result = (TestEnum)5;
            _output.WriteLine(result.ToString());
            Assert.IsType<TestEnum>(result);

            //文字列はTryParseじゃないとキャストできないっぽい
            //string value = "AAA"; string型だとキャスト自体がコンパイルエラーになる
            object value = "AAA";
            Assert.Throws<InvalidCastException>(() => (TestEnum)value);
        }

        [Fact]
        public void EnumのTryParse()
        {
            //TryParseもある、なぜかstring型をうけとる
            bool result = Enum.TryParse(2.ToString(), out TestEnum kokoko);
            Assert.Equal(TestEnum.CCC, kokoko);
            Assert.True(result);

            //TryParseで範囲外してもtrueがかえって、この場合だとoutに5が入る
            bool result2 = Enum.TryParse(5.ToString(), out TestEnum kekek);
            Assert.NotEqual(TestEnum.CCC, kekek);
            Assert.True(result2);

            //フィールド名でもOK
            bool result3 = Enum.TryParse("DDD", out TestEnum kukuku);
            Assert.Equal(TestEnum.DDD, kukuku);
            Assert.True(result3);

            //存在しないフィールド名はさすがにfalseみたい
            bool result4 = Enum.TryParse("DDE", out TestEnum kakaka);
            Assert.NotEqual(TestEnum.DDD, kakaka);
            Assert.False(result4);

            //IsDefinedで定義されているか確認してからのキャストが一番安パイ → Convert関数用意
        }

        [Fact]
        public void EnumのIsDefined()
        {
            //TryParseがなんでもtrueになりそうなので、
            //IsDefinedで定義されているか確認してからのキャストが一番安パイ → Convert関数用意
            var isDefined1 = Enum.IsDefined(typeof(TestEnum), 5);
            Assert.False(isDefined1);
            var isDefined2 = Enum.IsDefined(typeof(TestEnum), 2);
            Assert.True(isDefined2);
            var isDefined3 = Enum.IsDefined(typeof(TestEnum), "AAA");
            Assert.True(isDefined3);
            var isDefined4 = Enum.IsDefined(typeof(TestEnum), "AAB");
            Assert.False(isDefined4);

            //Enumで定義された型と値が異なる場合は、例外が発生する
            Assert.Throws<ArgumentException>(() => Enum.IsDefined(typeof(TestEnum), (byte)2));
            //string型はフィールド名と照合するため問題ない、が下記の例は値の文字列なのでFalse
            var isDefined5 = Enum.IsDefined(typeof(TestEnum), 2.ToString());
            Assert.False(isDefined5);
        }

    }
}
