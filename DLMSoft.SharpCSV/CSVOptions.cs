using System.Text;

namespace DLMSoft.SharpCSV {
    public sealed class CSVOptions {
        public bool HasHeader { get; set; } = false;
        public bool ReserveEmpty { get; set; } = false;
        public Encoding Encoding { get; set; } = Encoding.Default;
    }
}