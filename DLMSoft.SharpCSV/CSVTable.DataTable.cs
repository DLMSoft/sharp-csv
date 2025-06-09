using System.Data;

namespace DLMSoft.SharpCSV {
    public partial class CSVTable {
        public DataTable ToDataTable() {
            var result = new DataTable();
            if (Headers != null) {
                foreach (var col in Headers) {
                    result.Columns.Add(col);
                }
            }
            else {
                for (var i = 0; i < ColumnCount; i += 1) {
                    result.Columns.Add();
                }
            }
        
            foreach (var record in this) {
                var row = result.NewRow();
                for (var i = 0; i < ColumnCount; i += 1) {
                    row[i] = (string)record[i];
                }
            }

            return result;
        }
    }
}