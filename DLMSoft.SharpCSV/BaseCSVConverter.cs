using System;

namespace DLMSoft.SharpCSV {
    public class BaseCSVConverter : ICSVConverter {
        public bool CanConvert(Type type)
        {
            return type == typeof(sbyte)
                || type == typeof(byte)
                || type == typeof(short)
                || type == typeof(ushort)
                || type == typeof(int)
                || type == typeof(uint)
                || type == typeof(long)
                || type == typeof(ulong)
                || type == typeof(float)
                || type == typeof(double)
                || type == typeof(DateTime)
                || type == typeof(Guid)
                || type.IsEnum;
        }

        public object Parse(string input, Type resultType, object defaultResult)
        {
            if (string.IsNullOrEmpty(input)) {
                return defaultResult;
            }
            if (resultType == typeof(sbyte)) {
                if (sbyte.TryParse(input, out var result))
                    return result;
                return defaultResult;
            }
            if (resultType == typeof(byte)) {
                if (byte.TryParse(input, out var result))
                    return result;
                return defaultResult;
            }
            if (resultType == typeof(short)) {
                if (short.TryParse(input, out var result))
                    return result;
                return defaultResult;
            }
            if (resultType == typeof(ushort)) {
                if (ushort.TryParse(input, out var result))
                    return result;
                return defaultResult;
            }
            if (resultType == typeof(int)) {
                if (int.TryParse(input, out var result))
                    return result;
                return defaultResult;
            }
            if (resultType == typeof(uint)) {
                if (uint.TryParse(input, out var result))
                    return result;
                return defaultResult;
            }
            if (resultType == typeof(long)) {
                if (long.TryParse(input, out var result))
                    return result;
                return defaultResult;
            }
            if (resultType == typeof(ulong)) {
                if (ulong.TryParse(input, out var result))
                    return result;
                return defaultResult;
            }
            if (resultType == typeof(float)) {
                if (float.TryParse(input, out var result))
                    return result;
                return defaultResult;
            }
            if (resultType == typeof(double)) {
                if (double.TryParse(input, out var result))
                    return result;
                return defaultResult;
            }
            if (resultType == typeof(Guid)) {
                if (Guid.TryParse(input, out var result))
                    return result;
                return defaultResult;
            }
            if (resultType == typeof(DateTime)) {
                if (DateTime.TryParse(input, out var result))
                    return result;
                return defaultResult;
            }
            if (resultType.IsEnum) {
                if (!Enum.TryParse(resultType, input, out var result))
                    return result;
                return defaultResult;
            }
            throw new InvalidCastException();
        }

        public string GetString(object input, Type inputType)
        {
            return input?.ToString() ?? string.Empty;
        }
    }
}