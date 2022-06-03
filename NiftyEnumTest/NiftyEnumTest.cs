using NiftyEnum;
using System;
using System.Linq;
using Xunit;

namespace NiftyEnumTest
{
    public class NiftyEnumTest
    {

        enum TestEnum
        {
            [EnumStringValue("������")]
            AAA,
            [EnumStringValue("������")]
            BBB,
            [EnumStringValue("������")]
            CCC,
            //[EnumStringValue("������")]
            DDD,
        }

        [Fact]
        public void NiftyEnum��StringValue�e�X�g()
        {
            //������l�擾
            var aaa = TestEnum.AAA.StringValue();
            Assert.Equal("������", aaa);

            //EnumStringValue�����ݒ�̏ꍇ
            var ddd = TestEnum.DDD.StringValue();
            Assert.Equal("", ddd);
        }

        [Fact]
        public void NiftyEnum��GetItems�e�X�g()
        {
            //�擾����������
            var items = EnumHelper.GetItems<TestEnum>();
            Assert.Equal(4, items.Count());

            //1�Ԗڂ̗v�f�Ō���
            var bbb = items[1];
            Assert.Equal(TestEnum.BBB, bbb.Value);

            //����ȏ��������ł���̂�
            var (Value, StringValue) = items[2];//.ElementAt(2);
            Assert.Equal("������", StringValue);
        }

        [Fact]
        public void NiftyEnum��Convert�e�X�g()
        {
            //�͈͓��̐��l
            var aaa = EnumHelper.Convert<TestEnum>(0);
            Assert.Equal(TestEnum.AAA, aaa);
            //�͈͊O�̐��l
            Assert.Throws<InvalidCastException>(() => EnumHelper.Convert<TestEnum>(5));

            //���݂���t�B�[���h��
            aaa = EnumHelper.Convert<TestEnum>("AAA");
            Assert.Equal(TestEnum.AAA, aaa);
            //���݂��Ȃ��t�B�[���h��
            Assert.Throws<InvalidCastException>(() => EnumHelper.Convert<TestEnum>("AAB"));
        }

        [Fact]
        public void Enum��ToSting��GetValues()
        {
            //ToString�̓t�B�[���h�����Ƃ��
            var aaaStr = TestEnum.AAA.ToString();
            Assert.Equal("AAA", aaaStr);

            //�S���擾
            var values = Enum.GetValues<TestEnum>();
            Assert.Equal(4, values.Length);
            Assert.Equal(TestEnum.BBB, values[1]);
        }

        [Fact]
        public void Enum�̃L���X�g()
        {
            //���l���L���X�g���邱�Ƃŕϊ��\
            var bbb = (TestEnum)1;
            Assert.Equal(TestEnum.BBB, bbb);

            //�͈͊O�͂��̂܂ܐ��l��5��������
            var result = (TestEnum)5;
            Console.WriteLine(result);
            Assert.IsType<TestEnum>(result);

            //�������TryParse����Ȃ��ƃL���X�g�ł��Ȃ����ۂ�
            //string value = "AAA"; string�^���ƃL���X�g���̂��R���p�C���G���[�ɂȂ�
            object value = "AAA";
            Assert.Throws<InvalidCastException>(() => (TestEnum)value);
        }

        [Fact]
        public void Enum��TryParse()
        {
            //TryParse������A�Ȃ���string�^�������Ƃ�
            bool result = Enum.TryParse(2.ToString(), out TestEnum kokoko);
            Assert.Equal(TestEnum.CCC, kokoko);
            Assert.True(result);

            //TryParse�Ŕ͈͊O���Ă�true���������āA���̏ꍇ����out��5������
            bool result2 = Enum.TryParse(5.ToString(), out TestEnum kekek);
            Assert.NotEqual(TestEnum.CCC, kekek);
            Assert.True(result2);

            //�t�B�[���h���ł�OK
            bool result3 = Enum.TryParse("DDD", out TestEnum kukuku);
            Assert.Equal(TestEnum.DDD, kukuku);
            Assert.True(result3);

            //���݂��Ȃ��t�B�[���h���͂�������false�݂���
            bool result4 = Enum.TryParse("DDE", out TestEnum kakaka);
            Assert.NotEqual(TestEnum.DDD, kakaka);
            Assert.False(result4);

            //IsDefined�Œ�`����Ă��邩�m�F���Ă���̃L���X�g����Ԉ��p�C �� Convert�֐��p��
        }

        [Fact]
        public void Enum��IsDefined()
        {
            //TryParse���Ȃ�ł�true�ɂȂ肻���Ȃ̂ŁA
            //IsDefined�Œ�`����Ă��邩�m�F���Ă���̃L���X�g����Ԉ��p�C �� Convert�֐��p��
            var isDefined1 = Enum.IsDefined(typeof(TestEnum), 5);
            Assert.False(isDefined1);
            var isDefined2 = Enum.IsDefined(typeof(TestEnum), 2);
            Assert.True(isDefined2);
            var isDefined3 = Enum.IsDefined(typeof(TestEnum), "AAA");
            Assert.True(isDefined3);
            var isDefined4 = Enum.IsDefined(typeof(TestEnum), "AAB");
            Assert.False(isDefined4);
        }

    }
}
