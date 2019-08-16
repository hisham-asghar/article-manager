using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayerDb.AdoNet
{
    public class ObjectMapper<T> where T : new()
    {
        public virtual List<T> MapReaderToObjectList(SqlDataReader reader)
        {
            var list = new List<T>();
            while (reader.Read())
            {
                var item = ItemMapper(reader);
                list.Add(item);
            }
            return list;
        }

        private static T ItemMapper(IDataRecord reader)
        {
            var item = new T();
            var t = item.GetType();
            foreach (var property in t.GetProperties())
            {
                try
                {
                    //var type = property.PropertyType;
                    var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    var readerValue = string.Empty;

                    var columnName = property.Name;
                    var attribute = property.GetCustomAttributes(typeof(Column), true);
                    if (attribute.Length > 0)
                    {
                        var attributeValues = (Column)attribute[0];
                        columnName = attributeValues.Name;
                    }


                    if (reader[columnName] != DBNull.Value)
                    {
                        readerValue = reader[columnName].ToString();
                    }

                    if (!string.IsNullOrEmpty(readerValue))
                    {
                        property.SetValue(item, readerValue.To(type), null);
                    }
                    else if (type.Name == Enums.DataType.String.ToString())
                    {
                        property.SetValue(item, readerValue.To(type), null);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            return item;
        }

        public T MapReaderToObject(SqlDataReader reader)
        {
            return !reader.Read()
                ? default(T) :
                ItemMapper(reader);
        }
    }
}
