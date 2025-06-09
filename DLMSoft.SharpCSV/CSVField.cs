using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace DLMSoft.SharpCSV {
    /// <summary>
    /// 表示 CSV 字段的结构。
    /// </summary>
    public struct CSVField {
        /// <summary>
        /// 表示 CSV 字段中的字符串值。
        /// </summary>
        public string value;

        #region Porperties
        /// <summary>
        /// 获取 CSV 的值是否为空。
        /// </summary>
        public readonly bool IsEmpty => value == string.Empty;
        #endregion

        #region Constructors
        /// <summary>
        /// 初始化 CSV 字段。
        /// </summary>
        /// <param name="raw">字段原值。</param>
        public CSVField(string raw)
        {
            value = raw;
        }
        #endregion

        #region Method : Get
        /// <summary>
        /// 通过值的类型及类型转换器实例获取字段值。
        /// </summary>
        /// <param name="type">值的类型。</param>
        /// <param name="converter">CSV 类型转换器实例。</param>
        /// <returns>返回类型为 <paramref name="type"/> 的值。</returns>
        /// <exception cref="InvalidCastException">指定的转换器不适用于值的类型。</exception>
        [Pure]
        public readonly object Get(Type type, ICSVConverter converter)
        {
            if (!converter.CanConvert(type)) throw new InvalidCastException();
            var defaultValue = type.IsValueType ? Activator.CreateInstance(type) : null;
            return converter.Parse(value, type, defaultValue);
        }

        /// <summary>
        /// 通过值的类型获取字段值。
        /// </summary>
        /// <param name="type">值的类型。</param>
        /// <returns>返回类型为 <paramref name="type"/> 的值。</returns>
        [Pure]
        public readonly object Get(Type type)
        {
            if (type == typeof(string)) return value;
            var defaultValue = type.IsValueType ? Activator.CreateInstance(type) : null;
            var converter = CSVTable.GetConverter(type);
            if (converter == null) return defaultValue;
            return converter.Parse(value, type, defaultValue);
        }

        /// <summary>
        /// 获取字段值。
        /// </summary>
        /// <typeparam name="TResult">值的类型。</typeparam>
        /// <returns>返回类型为 <typeparamref name="TResult"/> 的值。</returns>
        [Pure]
        public readonly TResult Get<TResult>()
        {
            var type = typeof(TResult);
            return (TResult)Get(type);
        }
        #endregion

        #region Method : Set
        /// <summary>
        /// 通过类型转换器实例设置值。
        /// </summary>
        /// <param name="converter">CSV 类型转换器实例。</param>
        /// <param name="value">值内容。</param>
        /// <exception cref="InvalidCastException"></exception>
        public void Set(ICSVConverter converter, object value)
        {
            if (value == null) {
                this.value = string.Empty;
                return;
            }
            var type = value.GetType();
            if (!converter.CanConvert(type)) throw new InvalidCastException();
            this.value = converter.GetString(value, type);
        }

        public void Set(Type type, object value)
        {
            var converter = CSVTable.GetConverter(type);
            if (converter == null) {
                this.value = value?.ToString() ?? string.Empty;
                return;
            }
            this.value = converter.GetString(value, type);
        }

        public void Set<TResult>(TResult value)
        {
            var type = typeof(TResult);
            Set(type, value);
        }
        #endregion

        #region Method : FromValue
        public static CSVField FromValue<TValue>(TValue value)
        {
            var result = new CSVField();
            result.Set(value);
            return result;
        }
        #endregion

        #region Implicit Conversions
        public static implicit operator string(CSVField value) => value.value;
        public static implicit operator byte(CSVField value) => value.Get<byte>();
        public static implicit operator sbyte(CSVField value) => value.Get<sbyte>();
        public static implicit operator short(CSVField value) => value.Get<short>();
        public static implicit operator ushort(CSVField value) => value.Get<ushort>();
        public static implicit operator int(CSVField value) => value.Get<int>();
        public static implicit operator uint(CSVField value) => value.Get<uint>();
        public static implicit operator long(CSVField value) => value.Get<long>();
        public static implicit operator ulong(CSVField value) => value.Get<ulong>();
        public static implicit operator float(CSVField value) => value.Get<float>();
        public static implicit operator double(CSVField value) => value.Get<double>();
        public static implicit operator CSVField(string value) {
            var result = new CSVField(value);
            return result;
        }
        public static implicit operator CSVField(byte value) {
            var result = new CSVField();
            result.Set(value);
            return result;
        }
        public static implicit operator CSVField(sbyte value) {
            var result = new CSVField();
            result.Set(value);
            return result;
        }
        public static implicit operator CSVField(short value) {
            var result = new CSVField();
            result.Set(value);
            return result;
        }
        public static implicit operator CSVField(ushort value) {
            var result = new CSVField();
            result.Set(value);
            return result;
        }
        public static implicit operator CSVField(int value) {
            var result = new CSVField();
            result.Set(value);
            return result;
        }
        public static implicit operator CSVField(uint value) {
            var result = new CSVField();
            result.Set(value);
            return result;
        }
        public static implicit operator CSVField(long value) {
            var result = new CSVField();
            result.Set(value);
            return result;
        }
        public static implicit operator CSVField(ulong value) {
            var result = new CSVField();
            result.Set(value);
            return result;
        }
        public static implicit operator CSVField(float value) {
            var result = new CSVField();
            result.Set(value);
            return result;
        }
        public static implicit operator CSVField(double value) {
            var result = new CSVField();
            result.Set(value);
            return result;
        }
        #endregion

        #region Overrides of Object
        #region Method : ToString
        [Pure]
        public override readonly string ToString() => value;
        #endregion

        #region Method : GetHashCode
        [Pure]
        public override readonly int GetHashCode() => value.GetHashCode();
        #endregion

        #region Method : Equals
        [Pure]
        public override readonly bool Equals([NotNullWhen(true)] object obj)
        {
            if (obj == null || !(obj is CSVField field)) return false;
            return field.value == value;
        }

        [Pure]
        public static bool operator ==(CSVField left, CSVField right)
        {
            return left.Equals(right);
        }

        [Pure]
        public static bool operator !=(CSVField left, CSVField right)
        {
            return !(left == right);
        }
        #endregion
        #endregion
    }
}