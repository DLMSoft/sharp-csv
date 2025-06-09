using System;
using System.IO;
using System.Text;

namespace DLMSoft.SharpCSV {
    public enum CSVTokenStatus {
        NONE,
        END_OF_LINE,
        END_OF_FILE
    }

    public class CSVToken {
        public CSVField Field { get; set; } = null!;
        public CSVTokenStatus Status { get; set; } = CSVTokenStatus.NONE;
    }

    public static class CSVHelper {
        #region Method : FormatCSVString
        public static string FormatCSVString(string input)
        {
            // TODO Someone plz optimze this.
            var sb = new StringBuilder();
            var needQuote = false;

            for (var i = 0; i < input.Length; i += 1) {
                if (input[i] == '\r' && input[i + 1] == '\n') {
                    sb.AppendLine();
                    needQuote = true;
                    i += 1;
                    continue;
                }
                if (input[i] == '"') {
                    sb.Append("\"\"");
                    needQuote = true;
                    continue;
                } 
                if (input[i] == ',') {
                    sb.Append(',');
                    needQuote = true;
                    continue;
                }

                sb.Append(input[i]);
            }

            return needQuote ? $"\"{sb}\"" : sb.ToString();
        }
        #endregion

        #region Method : ReadCSVToken
        public static CSVToken ReadCSVToken(this TextReader reader)
        {
            int ch, ch2;
            bool isTokenStart = false, isInQuote = false;
            var sb = new StringBuilder();
            while (true) {
                ch = reader.Read();
                if (ch < 0) {
                    return new CSVToken {
                        Field = new CSVField(sb.ToString()),
                        Status = CSVTokenStatus.END_OF_FILE
                    };
                }
                var chr = (char)ch;
                if (chr == '\r') {
                    if (isInQuote) {
                        sb.Append('\r');
                        continue;
                    }
                    ch2 = reader.Read();
                    if (ch2 == '\n') {
                        return new CSVToken {
                            Field = new CSVField(sb.ToString()),
                            Status = CSVTokenStatus.END_OF_LINE
                        };
                    }
                    sb.Append(chr);
                    if (ch2 < 0) {
                        return new CSVToken {
                            Field = new CSVField(sb.ToString()),
                            Status = CSVTokenStatus.END_OF_FILE
                        };
                    }
                    if (ch2 == ',') {
                        return new CSVToken {
                            Field = new CSVField(sb.ToString()),
                            Status = CSVTokenStatus.NONE
                        };
                    }
                    sb.Append((char)ch2);
                    continue;
                }
                if (!isTokenStart) {
                    if (char.IsWhiteSpace(chr)) continue;
                    isTokenStart = true;
                }
                if (chr == ',') {
                    if (isInQuote) {
                        sb.Append(',');
                        continue;
                    }
                    return new CSVToken {
                        Field = new CSVField(sb.ToString()),
                        Status = CSVTokenStatus.NONE
                    };
                }
                if (chr == '"') {
                    if (isInQuote) {
                        var valid = false;
                        do {
                            ch2 = reader.Read();
                            if (ch2 < 0) {
                                return new CSVToken {
                                    Field = new CSVField(sb.ToString()),
                                    Status = CSVTokenStatus.END_OF_FILE
                                };
                            }
                            if (ch2 == '\r') {
                                if (reader.Read() != '\n') throw new FormatException();
                                return new CSVToken {
                                    Field = new CSVField(sb.ToString()),
                                    Status = CSVTokenStatus.END_OF_LINE
                                };
                            }
                            if (ch2 == ',') {
                                return new CSVToken {
                                    Field = new CSVField(sb.ToString()),
                                    Status = CSVTokenStatus.NONE
                                };
                            }
                            if (ch2 == '"') {
                                sb.Append('"');
                                valid = true;
                                break;
                            }
                        }
                        while (char.IsWhiteSpace((char)ch2));
                        if (valid) continue;
                        throw new FormatException();
                    }
                    if (sb.Length != 0) throw new FormatException();
                    isInQuote = true;
                    continue;
                }
                sb.Append(chr);
            }
        }
        #endregion
    }
}