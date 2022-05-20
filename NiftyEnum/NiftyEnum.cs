using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NiftyEnum
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

    /// <summary>
    /// メソッド定義
    /// </summary>
    public static class EnumHelper
    {

        /// <summary>
        /// EnumStringValue属性に設定した文字列を取得する
        /// 例: TestEnum.A.StringValue
        /// </summary>
        /// <param name = "enumeration" > 列挙型のフィールド </ param >
        /// <returns>未設定の場合は空文字</returns>
        public static string StringValue(this Enum value)
        {
            //指定しているフィールド取得
            var field = value.GetType().GetField(value.ToString());
            return GetEnumStringValue(field);
        }


        /// <summary>
        /// ValueTuple型(Value,StringValue)のリストを返す
        /// 例: new TestEnum().GetItems ? TODO これ拡張じゃないほうがいいか...
        /// </summary>
        /// <param name = "enumeration" ></ param >
        /// <returns></returns>
        public static IEnumerable<(T Value, string StringValue)> GetItems<T>() where T : Enum
        {
            //現在対象Enumのフィールドをすべて取得
            var fields = typeof(T).GetFields().
                Where((f) => f.Attributes == (FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.HasDefault));
            //取得した全てのフィールドを戻り値の方にして返す
            return fields.Select((f) => ((T)f.GetValue(null), GetEnumStringValue(f)));
        }

        /// <summary>
        /// 対象FieldのEnumStringValue属性から文字列を取得する
        /// </summary>
        /// <param name="field"></param>
        /// <returns>未設定の場合は空文字</returns>
        private static string GetEnumStringValue(FieldInfo field)
        {
            //対象フィールドに設定されているEnumStringValueAttribute取得
            var attribute = field.GetCustomAttribute<EnumStringValueAttribute>();
            //取得できた場合、保持している文字列値を返す
            return attribute != null ? attribute.Value : "";
        }

    }


}