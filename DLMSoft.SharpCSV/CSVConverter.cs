using System;
using System.Diagnostics.CodeAnalysis;

namespace DLMSoft.SharpCSV {
    #region Interface : ICSVConverter
    public interface ICSVConverter {
        bool CanConvert(Type type);
        [return: MaybeNull]
        object Parse(string input, Type resultType, object defaultValue);
        string GetString([MaybeNull]object input, Type inputType);
    }
    #endregion

    public abstract class CSVConverter<TResult> : ICSVConverter {
        #region Abstract methods
        [return: MaybeNull]
        public abstract TResult Parse(string input);
        public abstract string GetString([MaybeNullWhen(true)]TResult input);
        #endregion
        #region Implements of ICSVConverter
        #region Method : CanConvert
        public bool CanConvert(Type type)
        {
            return type == typeof(TResult);
        }
        #endregion
        #region Method : Parse
        [return: MaybeNull]
        public object Parse(string input, Type resultType, object defaultValue) {
            if (resultType != typeof(TResult)) {
                return defaultValue;
            }

            return Parse(input);
        }
        #endregion
        #region Method : GetString
        [return: NotNull]
        public string GetString([MaybeNullWhen(true)]object input, Type inputType) {
            if (inputType != typeof(TResult)) return string.Empty;

            return GetString(input == null ? default : (TResult)input);
        }
        #endregion
        #endregion
    }
}