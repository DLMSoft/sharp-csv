using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DLMSoft.SharpCSV {
    /// <summary>
    /// 表示 CSV 文档对象模型的类。
    /// </summary>
    public sealed partial class CSVTable: IEnumerable<CSVRecord> {
        #region Properties
        public int ColumnCount { get; private set; }
        public string[] Headers { get; private set; }
        public int RecordCount => records_.Count;
        public ICollection<CSVRecord> Records => records_;
        public bool HasHeader => Headers != null;
        #endregion

        #region Constructor
        public CSVTable(int columnCount)
        {
            ColumnCount = columnCount;
        }

        public CSVTable(string[] columnHeaders)
        {
            ColumnCount = columnHeaders.Length;
            Headers = columnHeaders.ToArray();
        }
        #endregion

        #region Indexers
        public CSVRecord this[int index]
        {
            get {
                if (index < 0 || index > records_.Count) throw new IndexOutOfRangeException();
                return records_[index];
            }
        }
        #endregion

        #region Method : SetHeaders
        public void SetHeaders(string[] headers)
        {
            if (ColumnCount != headers.Length) throw new ArgumentOutOfRangeException(nameof(headers));
            Headers = headers.ToArray();
        }
        #endregion

        #region Method : RegisterGlobalConverter
        public static void RegisterGlobalConverter<TConverter>() where TConverter : ICSVConverter, new() 
        {
            var converterType = typeof(TConverter);
            var existed = from c in converters_ where c.GetType() == converterType select c;
            if (existed.Any()) throw new InvalidOperationException($"Converter {converterType.Name} already registered !");
            var converter = new TConverter();
            converters_.Add(converter);
        }
        #endregion

        #region Method : GetConverter
        public static ICSVConverter GetConverter(Type type)
        {
            var result = from i in converters_ where i.CanConvert(type) select i;
            return result.FirstOrDefault();
        }
        #endregion

        #region Method : AppendRecord
        public CSVRecord AppendRecord()
        {
            var result = new CSVRecord(this);
            records_.Add(result);
            return result;
        }

        public CSVRecord AppendRecord(params CSVField[] fields)
        {
            var result = new CSVRecord(this);
            result.SetValues(fields);
            records_.Add(result);
            return result;
        }
        #endregion

        #region Method : InsertRecord
        public CSVRecord InsertRecord(int index)
        {
            if (index >= records_.Count) return AppendRecord();
            var result = new CSVRecord(this);
            if (index >= 0) {
                records_.Insert(index, result);
            }
            else if (index < -records_.Count + 1) {
                records_.Insert(0, result);
            }
            else {
                records_.Insert(records_.Count + index, result);
            }
            return result;
        }

        public CSVRecord InsertRecord(int index, params CSVField[] fields)
        {
            if (index >= records_.Count) return AppendRecord();
            var result = new CSVRecord(this);
            result.SetValues(fields);
            if (index >= 0) {
                records_.Insert(index, result);
            }
            else if (index < -records_.Count) {
                records_.Insert(0, result);
            }
            else {
                records_.Insert(records_.Count + index, result);
            }
            return result;
        }
        #endregion

        #region Method : RemoveRecord
        public bool RemoveRecord(int index)
        {
            if (index < -records_.Count) return false;
            if (index >= records_.Count) return false;

            if (index < 0) {
                records_.RemoveAt(records_.Count + index);
                return true;
            }

            records_.RemoveAt(index);
            return true;
        }

        public int RemoveRecord(Predicate<CSVRecord> condition)
        {
            return records_.RemoveAll(condition);
        }
        #endregion

        #region Method : GetFieldIndexByName
        public int GetFieldIndexByName(string name) {
            if (Headers == null) throw new InvalidOperationException();
            for (var i = 0; i < Headers.Length; i += 1) {
                if (Headers[i] == name) return i;
            }
            return -1;
        }
        #endregion

        #region Implements of IEnumerable<CSVRecord>
        #region Method : GetEnumerator
        public IEnumerator<CSVRecord> GetEnumerator() {
            return records_.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
        #endregion

        #region Fields
        static readonly List<ICSVConverter> converters_ = new List<ICSVConverter>(new ICSVConverter[] { new BaseCSVConverter() });
        readonly List<CSVRecord> records_ = new List<CSVRecord>();
        #endregion
    }
}