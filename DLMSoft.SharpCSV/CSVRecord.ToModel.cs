using System;
using System.Linq;
using System.Reflection;

namespace DLMSoft.SharpCSV {
    #region Class : ModelField
    class ModelField {
        public PropertyInfo Property { get; set; }
        public string FieldName { get; set; }
        public int FieldIndex { get; set; } = -1;
    }
    #endregion

    public partial class CSVRecord {
        public TModel ToModel<TModel>() where TModel : new()
        {
            var modelType = typeof(TModel);
            var props = modelType.GetProperties();
            var fieldProps = from p in props let i = p.GetCustomAttribute<CSVFieldAttribute>() where i != null select new ModelField {
                Property = p,
                FieldName = i.Name,
                FieldIndex = i.Index
            };

            var result = new TModel();

            foreach (var field in fieldProps) {
                if (!field.Property.CanWrite) throw new InvalidOperationException();
                var converter = field.Property.GetCustomAttribute<CSVConverterAttribute>()?.InstantiateConverter();
                var valueType = field.Property.PropertyType;
                if (field.FieldName != null) {
                    if (converter != null) {
                        field.Property.SetValue(result, this[field.FieldName].Get(valueType, converter));
                        continue;
                    }
                    field.Property.SetValue(result, this[field.FieldName].Get(valueType));
                    continue;
                }
                if (field.FieldIndex != -1) {
                    if (converter != null) {
                        field.Property.SetValue(result, this[field.FieldIndex].Get(valueType, converter));
                        continue;
                    }
                    field.Property.SetValue(result, this[field.FieldIndex].Get(valueType));
                    continue;
                }
                var fieldName = field.Property.Name;
                if (converter != null) {
                    field.Property.SetValue(result, this[fieldName].Get(valueType, converter));
                    continue;
                }
                field.Property.SetValue(result, this[fieldName].Get(valueType));
            }

            return result;
        }
    }
}