using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DLMSoft.SharpCSV {
    public partial class CSVTable {
        #region Method : FromStream
        public static CSVTable FromStream(Stream stream, CSVOptions options = null)
        {
            CSVToken token;
            CSVTable table;
            List<CSVField> fields = new List<CSVField>();

            options ??= new CSVOptions();
        
            using var reader = new StreamReader(stream, options.Encoding);

            table = null;

            while (true) {
                token = reader.ReadCSVToken();
                fields.Add(token.Field);
                if (token.Status != CSVTokenStatus.NONE) {
                    table ??= new CSVTable(fields.Count);
                    if (options.HasHeader && table.Headers == null) {
                        table.SetHeaders((from i in fields select (string)i).ToArray());
                        fields.Clear();
                        if (token.Status == CSVTokenStatus.END_OF_FILE) break;
                        continue;
                    }
                    var nonEmpties = from f in fields where !f.IsEmpty select f;
                    if (!options.ReserveEmpty && !nonEmpties.Any()) {
                        fields.Clear();
                        if (token.Status == CSVTokenStatus.END_OF_FILE) break;
                        continue;
                    }
                    table.AppendRecord(fields.ToArray());
                    fields.Clear();
                    if (token.Status == CSVTokenStatus.END_OF_FILE) break;
                    continue;
                }
            }

            return table;
        }
        #endregion

        #region Method : FromString
        public static CSVTable FromString(string str, CSVOptions options = null)
        {
            options ??= new CSVOptions();

            var bytes = options.Encoding.GetBytes(str);
            using var stream = new MemoryStream(bytes);
        
            return FromStream(stream, options);
        }
        #endregion
    }
}