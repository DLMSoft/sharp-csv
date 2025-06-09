using System;
using System.Collections.Generic;

namespace DLMSoft.SharpCSV {
    public sealed partial class CSVRecord {
        #region Properties
        public CSVTable Table { get; }
        #endregion

        #region Constructor
        internal CSVRecord(CSVTable table)
        {
            Table = table;
            values_ = new CSVField[table.ColumnCount];
        }
        #endregion

        #region Method : SetValues
        public void SetValues(params CSVField[] values)
        {
            Array.Copy(values, values_, Math.Min(values_.Length, values.Length));
        }
        #endregion

        #region Indexers
        public CSVField this[int index] {
            get {
                if (index < 0 && index >= values_.Length) throw new IndexOutOfRangeException();
                return values_[index];
            }
            set {
                if (index < 0 && index >= values_.Length) throw new IndexOutOfRangeException();
                values_[index] = value;
            }
        }

        public CSVField this[string fieldName] {
            get {
                var index = Table.GetFieldIndexByName(fieldName);
                if (index == -1) throw new KeyNotFoundException();
                return values_[index];
            }
            set {
                var index = Table.GetFieldIndexByName(fieldName);
                if (index == -1) throw new KeyNotFoundException();
                values_[index] = value;
            }
        }
        #endregion

        #region Method : Get
        public T Get<T>(int index)
        {
            if (index < 0 || index >= values_.Length) throw new ArgumentOutOfRangeException(nameof(index));
            return values_[index].Get<T>();
        }

        public T Get<T>(string fieldName)
        {
            var index = Table.GetFieldIndexByName(fieldName);
            if (index == -1) throw new KeyNotFoundException();
            return Get<T>(index);
        }
        #endregion

        #region Method : Set
        public void Set<T>(int index, T value)
        {
            if (index < 0 || index >= values_.Length) throw new ArgumentOutOfRangeException(nameof(index));
            values_[index].Set(value);
        }

        public void Set<T>(string fieldName, T value)
        {
            var index = Table.GetFieldIndexByName(fieldName);
            if (index == -1) throw new KeyNotFoundException();
            Set(index, value);
        }
        #endregion

        #region Fields
        readonly CSVField[] values_;
        #endregion
    }
}