using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NiftyEnumExtention
{
    /// <summary>
    /// Enumの各フィールド用
    /// 文字列値保持用Attribute
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class EnumStringValueAttribute : Attribute
    {
        public string Value { get; }

        public EnumStringValueAttribute(string value)
        {
            this.Value = value;
        }
    }

    public static class EnumExtention
    {
        /// <summary>
        /// EnumStringValue属性に設定した文字列を取得する
        /// 例: TestEnum.A.StringValue
        /// </summary>
        /// <param name = "enumeration" > 列挙型のフィールド </ param >
        /// <returns>未設定の場合は空文字</returns>
        public static string StringValue(this Enum enumeration)
        {
            //指定しているフィールド取得
            var field = enumeration.GetType().GetField(enumeration.ToString());
            return GetEnumStringValue(field);
        }


        /// <summary>
        /// Combobox等のDataSource用
        /// 匿名クラス{Value,StringValueのリストを返す(仮)
        /// 例: new TestEnum().GetItems
        /// </summary>
        /// <param name = "enumeration" ></ param >
        /// <returns></returns>
        public static IEnumerable<(T Value, string StringValue)> GetItems<T>(this T enumeration) where T : Enum
        {
            //現在対象Enumのフィールドをすべて取得
            var fields = T.GetType();
        }

        /// < summary >
        /// 対象FieldのEnumStringValue属性から文字列を取得する
        /// </ summary >
        /// < param name = "field" ></ param >
        /// < returns > 未設定の場合は空文字 </ returns >
        private static string GetEnumStringValue(FieldInfo field)
        {
            var attribute = field.GetCustomAttribute<EnumStringValueAttribute>();
            return attribute != null ? attribute.Value : "";
        }

    }

}
