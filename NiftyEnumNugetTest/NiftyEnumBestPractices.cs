using NiftyEnum;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace NiftyEnumTest
{
    public class NiftyEnumBestPractices
    {

        private readonly ITestOutputHelper _output;

        public NiftyEnumBestPractices(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>
        /// 色
        /// </summary>
        enum ColorEnum : byte
        {
            [EnumStringValue("青")]
            Blue,
            [EnumStringValue("赤")]
            Red,
            [EnumStringValue("黄色")]
            Yellow,
            //[EnumStringValue("緑")]
            Green,
        }

        [Fact]
        public void Enumから文字列値を取得()
        {
            //DBから取得した値をEnum型に変換して扱っており、表示文字列に変換する時など

            //Enum
            var value = ColorEnum.Blue;

            //文字列値取得
            var color = value.StringValue();

            Assert.Equal("青", color);
        }


        [Fact]
        public void 値から文字列値を取得()
        {
            //DBから取得した値を表示文字列に変換する時など

            //値
            var value = (byte)1;

            //文字列値取得
            var color = EnumHelper.Convert<ColorEnum>(value).StringValue();

            Assert.Equal("赤", color);
        }

        [Fact]
        public void Enumから数値を取得()
        {
            //Enum型でやり取りしていた値をDBに登録する時など
            //TODO:これ実際は不要な気もするので要確認

            //値
            var value = ColorEnum.Yellow;

            //数値取得
            var byteNumber = (byte)value;


            Assert.Equal(2, byteNumber);
        }

        [Fact]
        public void 数値と文字列値のリストを取得()
        {
            //ドロップダウンリストやコンボボックスに使えるリストを生成する
            //TODO:実際に利用する場合はbyte型などの数値型に変換しなくても、自動的にキャストされると思うのでEnum型のままValueに使用してよさそうなので要確認

            //匿名型のリストに変換
            var list = EnumHelper.GetItems<ColorEnum>().Select((e) => new { Value = (byte)e.Value, Label = e.StringValue });
            Assert.Equal(4, list.Count());

            //コンソールに表示
            foreach (var item in list)
            {
                _output.WriteLine($"値:{item.Value} ラベル:{item.Label}");
            }
        }

        #region nuget.orgのReadMe用

        enum SampleEnum : byte
        {
            [EnumStringValue("青")]
            Blue,
            [EnumStringValue("赤")]
            Red,
            [EnumStringValue("黄色")]
            Yellow,
        }

        [Fact]
        public void ReadMe()
        {
            //srt = "赤";
            string str = SampleEnum.Red.StringValue();

            //items = { (SampleEnum.Blue, "青"), (SampleEnum.Red, "赤"), (SampleEnum.Yellow, "黄色") };
            (SampleEnum Value, string StringValue)[] items = EnumHelper.GetItems<SampleEnum>();

            //result = SampleEnum.Yellow;
            SampleEnum result = EnumHelper.Convert<SampleEnum>((byte)2);
        }

        #endregion

    }
}
