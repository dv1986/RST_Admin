// Developed by Hamid Siamaki
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public static class POCOConversionExtensions
    {

        private static ConcurrentDictionary<Type, ConcurrentBag<PropertyInfo>> typeProperties = new ConcurrentDictionary<Type, ConcurrentBag<PropertyInfo>>();

        #region private api
        private static T ConvertValue<T>(object inputValue)
        {
            return (T)System.Convert.ChangeType(inputValue, typeof(T));
            //return (T)inputValue;
        }
        private static ConcurrentBag<PropertyInfo> extractPropertyInfo<T>(T t)
        {
            if (!typeProperties.ContainsKey(t.GetType()))
            {
                extractTypeProperties(t);
            }
            if (!typeProperties.ContainsKey(t.GetType()))
            {
                throw new Exception("unable to extract type properties");
            }
            ConcurrentBag<PropertyInfo> properties = typeProperties[t.GetType()];
            if (properties == null)
            {
                throw new Exception("unable to extract type properties.");
            }

            if (properties.Count == 0)
            {
                throw new Exception("unable to extract type properties...");
            }

            return properties;
        }

        private static void extractTypeProperties<T>(T t)
        {
            PropertyInfo[] arr = t.GetType().GetProperties();
            if (arr == null) return;
            if (arr.Length == 0) return;
            ConcurrentBag<PropertyInfo> propertyInfos = new ConcurrentBag<PropertyInfo>();
            foreach (PropertyInfo current in arr)
            {
                propertyInfos.Add(current);
            }
            typeProperties[t.GetType()] = propertyInfos;
        }
        private static object castToEnum(DataRow dr, string columnName, Type dataType)
        {
            if (dr == null) return null;
            if (dr[columnName] == DBNull.Value) return null;

            if (dr[columnName].GetType() == typeof(string))
            {
                return Enum.Parse(dataType, dr[columnName].ToString(), true);
            }
            else if (dr[columnName].GetType() == typeof(int))
            {
                return System.Convert.ToInt32(dr[columnName]);
            }
            else
            {
                return Enum.Parse(dataType, dr[columnName].ToString(), true);
            }
        }

        #endregion

        #region public extension methods
        public static object LookUpValue(this DataRow dr, string columnName, Type dataType)
        {
            if (dataType.IsEnum)
            {
                return castToEnum(dr, columnName, dataType);
            }
            TypeCode typeCode = Type.GetTypeCode(dataType);
            switch (typeCode)
            {
                case TypeCode.Empty: return dr[columnName];
                case TypeCode.Object: return dr[columnName];
                case TypeCode.DBNull: return dr[columnName];
                case TypeCode.Boolean: return dr.LookUpValue<bool>(columnName);
                case TypeCode.Char: return dr.LookUpValue<char>(columnName);
                case TypeCode.SByte: return dr.LookUpValue<sbyte>(columnName);
                case TypeCode.Byte: return dr.LookUpValue<byte>(columnName);
                case TypeCode.Int16: return dr.LookUpValue<short>(columnName);
                case TypeCode.UInt16: return dr.LookUpValue<ushort>(columnName);
                case TypeCode.Int32: return dr.LookUpValue<int>(columnName);
                case TypeCode.UInt32: return dr.LookUpValue<uint>(columnName);
                case TypeCode.Int64: return dr.LookUpValue<long>(columnName);
                case TypeCode.UInt64: return dr.LookUpValue<ulong>(columnName);
                case TypeCode.Single: return dr.LookUpValue<float>(columnName);
                case TypeCode.Double: return dr.LookUpValue<double>(columnName);
                case TypeCode.Decimal: return dr.LookUpValue<decimal>(columnName);
                case TypeCode.DateTime: return dr.LookUpValue<DateTime>(columnName);
                case TypeCode.String:
                    if (dr.Table.Columns[columnName].DataType != typeof(string))
                        return dr.LookUpValue<object>(columnName)?.ToString();
                    else
                        return dr.LookUpValue<string>(columnName);
                default: return dr[columnName];
            }
        }
        public static T LookUpValue<T>(this DataRow dr, string columnName)
        {
            if (dr == null) return default(T);
            if (!dr.Table.Columns.Contains(columnName))
            {
                throw new Exception("column " + columnName + " is missing");
            }
            if (dr[columnName] == DBNull.Value) return default(T);

            return ConvertValue<T>(dr[columnName]);
        }
        public static T Convert<T>(this DataRow dr)
        {
            if (dr == null) throw new Exception("input data row cannot be null.");

            if (!isObject(typeof(T)))
            {
                #region value-types
                return LookUpValue<T>(dr, dr.Table.Columns[0].ColumnName);
                #endregion
            }
            else
            {
                T t = (T)Activator.CreateInstance(typeof(T), new object[] { });
                #region dynamic data type
                if (t.GetType() == typeof(ExpandoObject))

                {
                    var expandoDict = new ExpandoObject() as IDictionary<String, Object>;
                    foreach (DataColumn col in dr.Table.Columns)
                    {
                        //put every column of this row into the new dictionary

                        var value = ((dr[col.ColumnName] == DBNull.Value ? null : dr[col.ColumnName]) ?? string.Empty).ToString();
                        expandoDict.Add(col.ToString(), value);
                    }
                    t = (T)expandoDict;
                    #endregion
                }
                else
                {
                    #region typed data types
                    ConcurrentBag<PropertyInfo> properties = extractPropertyInfo(t);

                    foreach (PropertyInfo pi in properties)
                    {
                        if (dr.Table.Columns.Contains(pi.Name))
                        {
                            var value = dr.LookUpValue(pi.Name, pi.PropertyType);
                            if (value == DBNull.Value)
                            {
                                value = null;
                            }
                            pi.SetValue(t, value, null);
                        }
                    }
                    #endregion
                }
                return t;
            }
        }

        private static bool isObject(Type dataType)
        {
            TypeCode typeCode = Type.GetTypeCode(dataType);
            switch (typeCode)
            {
                case TypeCode.Empty:
                case TypeCode.DBNull:
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.DateTime:
                case TypeCode.String:
                    return false;

                case TypeCode.Object:
                    return true;

                default: return true;
            }
        }
        public static T ConvertFirstRow<T>(this DataSet ds)
        {
            if (ds == null) return default(T);
            if (ds.Tables.Count == 0) return default(T);
            return ds.Tables[0].ConvertFirstRow<T>();
        }
        public static T ConvertFirstRow<T>(this DataTable dt)
        {
            if (dt == null) return default(T);
            if (dt.Rows.Count == 0) return default(T);
            return dt.Rows[0].Convert<T>();
        }
        public static List<T> Convert<T>(this DataTable dt)
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T t = dr.Convert<T>();
                if (t == null) throw new Exception("unable to convert data-row.");
                list.Add(t);
            }
            return list;
        }
        public static ConcurrentBag<T> ConvertToConcurrentBag<T>(this DataTable dt)
        {
            ConcurrentBag<T> bag = new ConcurrentBag<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T t = dr.Convert<T>();
                if (t == null) throw new Exception("unable to convert data-row.");
                bag.Add(t);
            }
            return bag;
        }
        public static Dictionary<T, U> ConvertToDictionary<T, U>(this DataTable dt, string keyColumn, string valueColumn)
        {
            Dictionary<T, U> dic = new Dictionary<T, U>();
            foreach (DataRow dr in dt.Rows)
            {
                T key = dr.LookUpValue<T>(keyColumn);
                U value = dr.LookUpValue<U>(valueColumn);
                dic[key] = value;
            }
            return dic;
        }

        public static ConcurrentDictionary<T, U> ConvertToConcurrentDictionary<T, U>(this DataTable dt, string keyColumn, string valueColumn)
        {
            ConcurrentDictionary<T, U> dic = new ConcurrentDictionary<T, U>();
            foreach (DataRow dr in dt.Rows)
            {
                T key = dr.LookUpValue<T>(keyColumn);
                U value = dr.LookUpValue<U>(valueColumn);
                dic[key] = value;
            }
            return dic;
        }
        public static List<T> Convert<T>(this ICollection<DataRow> dt)
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in dt)
            {
                T t = dr.Convert<T>();
                if (t == null) throw new Exception("unable to convert data-row.");
                list.Add(t);
            }
            return list;
        }
        public static List<T> Convert<T>(this IEnumerable<DataRow> dt)
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in dt)
            {
                T t = dr.Convert<T>();
                if (t == null) throw new Exception("unable to convert data-row.");
                list.Add(t);
            }
            return list;
        }
        public static List<T> Convert<T>(this DataRow[] dt)
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in dt)
            {
                T t = dr.Convert<T>();
                if (t == null) throw new Exception("unable to convert data-row.");
                list.Add(t);
            }
            return list;
        }

        public static List<T> ConvertFirstDataTable<T>(this DataSet ds)
        {
            if (ds == null) return null;
            if (ds.Tables.Count == 0) return null;

            return ds.Tables[0].Convert<T>();
        }
        public static ConcurrentBag<T> ConvertFirstDataTableToConcurrentBag<T>(this DataSet ds)
        {
            if (ds == null) return null;
            if (ds.Tables.Count == 0) return null;

            return ds.Tables[0].ConvertToConcurrentBag<T>();
        }

        public static DataTable ConvertToNameValueDataTable(this Dictionary<string, string> dictionary)
        {
            if (dictionary == null) return null;
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn() { ColumnName = "Name", DataType = typeof(string), AllowDBNull = true });
            dt.Columns.Add(new DataColumn() { ColumnName = "Value", DataType = typeof(string), AllowDBNull = true });

            foreach (string current in dictionary.Keys)
            {
                dt.Rows.Add(current, dictionary[current]);
            }

            return dt;
        }
        #endregion
    }
}
