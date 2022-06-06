using System;
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
        /// 対象のフィールのEnumStringValue属性に設定した文字列を取得する
        /// </summary>
        /// <remarks>例:TestEnum.AAA.StringValue();</remarks>
        /// <param name="value">列挙型のフィールド</param>
        /// <returns>未設定の場合は空文字</returns>
        public static string StringValue(this Enum value)
        {
            //指定しているフィールド取得
            var field = value.GetType().GetField(value.ToString());
            return GetEnumStringValue(field);
        }

        /// <summary>
        /// 対象のEnumをValueTuple型(Value,StringValue)の配列として返す
        /// </summary>
        /// <remarks>例:EnumHelper.GetItems&lt;TestEnum&gt;();</remarks>
        /// <typeparam name="T">指定のEnum</typeparam>
        /// <returns></returns>
        public static (T Value, string StringValue)[] GetItems<T>() where T : struct, Enum
        {
            //現在対象Enumのフィールドをすべて取得 型情報のフィールド(?)は除外
            var fields = typeof(T).GetFields().Where((f) => f.Attributes == (FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.HasDefault));
            //取得した全てのフィールドを戻り値の方にして返す
            return fields.Select((f) => ((T)f.GetValue(null), GetEnumStringValue(f))).OrderBy((f) => f.Item1).ToArray();
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

        /// <summary>
        /// 指定のEnumに値を変換する<br/>
        /// ※通常のキャストとの違いは、範囲外の数値で例外が発生することと、フィールド名としての文字列値からも変換できる
        /// </summary>
        /// <remarks>
        /// 例:EnumHelper.Convert&lt;TestEnum&gt;(3)
        /// </remarks>
        /// <typeparam name="T">指定のEnum</typeparam>
        /// <param name="value">値</param>
        /// <returns>範囲外の値は例外(InvalidCastException)</returns>
        public static T Convert<T>(object value) where T : struct, Enum
        {
            //未定義の場合は例外
            if (Enum.IsDefined(typeof(T), value) == false)
            {
                throw new InvalidCastException("範囲外の値です。");
            }

            //string型の変換にも対応するためTryParseで変換
            _ = Enum.TryParse(value.ToString(), out T result);
            return result;
        }
    }

}