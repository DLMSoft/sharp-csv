using System;

namespace DLMSoft.SharpCSV {
    #region Class : CSVFieldAttribute
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class CSVFieldAttribute : Attribute {
        #region Properties
        public string Name { get; }
        public int Index { get; } = -1;
        #endregion
    
        #region Constructors
        public CSVFieldAttribute(string name)
        {
            Name = name;
        }

        public CSVFieldAttribute(int index)
        {
            Index = index;
        }

        public CSVFieldAttribute() {}
        #endregion
    }
    #endregion

    #region Class : CSVConverterAttribute
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class CSVConverterAttribute : Attribute {
        #region Properties
        public Type ConverterType { get; }
        #endregion

        #region Constructor
        public CSVConverterAttribute(Type type)
        {
            var baseType = typeof(ICSVConverter);
            if (!baseType.IsAssignableFrom(type)) throw new InvalidOperationException();
            ConverterType = type;
        }
        #endregion

        #region Method : InstantiateConverter
        public ICSVConverter InstantiateConverter()
        {
            return (Activator.CreateInstance(ConverterType) as ICSVConverter)!;
        }
        #endregion
    }

    #endregion
}