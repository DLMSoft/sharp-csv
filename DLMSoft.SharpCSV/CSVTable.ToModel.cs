using System.Collections.Generic;
using System.Linq;

namespace DLMSoft.SharpCSV {
    public partial class CSVTable {
        public IEnumerable<TModel> ToModel<TModel>() where TModel : new()
        {
            var results = from i in records_ select i.ToModel<TModel>();
            return results.ToArray();
        }
    }
}