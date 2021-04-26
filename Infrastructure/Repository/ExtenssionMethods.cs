using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public static class ExtenssionMethods
    {
        public static object DBDateTimeTrueFormat(this string itmdate, string format = "dd/MM/yyyy")
        {
            if (string.IsNullOrEmpty(itmdate))
                return DBNull.Value;
            DateTime dt = new DateTime();
            if (DateTime.TryParseExact(itmdate, format, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                return dt;
            return DBNull.Value;


        }
        public static T ExtractAndCastTo<T>(this IDataReader dr, string fieldName)
        {
            T data = default(T);
            try
            {
                if (dr != null)
                {
                    if (dr[fieldName] != DBNull.Value)

                    {
                        data = (T)dr[fieldName];
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error has occured in Extract and Cast Data from DataReader field name: {fieldName ?? ""}", ex);
            }
            return data;

        }

        #region ValidateColumnExistExtractAndCastTo
        /// <summary>
        /// Method to check if reader is not null,
        /// ecides whether a specific column name exists or not,
        /// the column is not null
        /// </summary>
        /// <typeparam name="T">column type</typeparam>
        /// <param name="dr">DataReader</param>
        /// <param name="fieldName">Column Name</param>
        /// <returns>Column value with defined type</returns>
        /// <condition>
        /// If all above summary rules follows or not
        /// </condition>
        public static T ValidateColumnExistExtractAndCastTo<T>(this IDataReader dr, string fieldName)
        {
            //Create required type object and assign default value
            T data = default(T);
            try
            {
                if (dr != null)
                    //Validate if the reader has the required column/field name
                    if ((Enumerable.Range(0, dr.FieldCount).Any(i => string.Equals(dr.GetName(i), fieldName, StringComparison.OrdinalIgnoreCase))))
                        //Validate the reader column
                        if (dr[fieldName] != DBNull.Value)
                        {
                            if (typeof(T) == typeof(DateTime))
                                data = (T)Convert.ChangeType(dr[fieldName], typeof(T));
                            else if (typeof(T) == typeof(Int32))
                                data = (T)Convert.ChangeType(dr[fieldName], typeof(T));
                            else if (typeof(T) == typeof(Int16))
                                data = (T)Convert.ChangeType(dr[fieldName], typeof(T));
                            else if (typeof(T) == typeof(decimal))
                                data = (T)Convert.ChangeType(dr[fieldName], typeof(T));
                            else
                                data = (T)Convert.ChangeType(dr[fieldName], typeof(T));

                            //data = (T)dr[fieldName];
                        }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error has occured in Extract and Cast Data from DataReader field name: {fieldName ?? ""}", ex);
            }
            return data;
        }
        #endregion ValidateColumnExistExtractAndCastTo

        #region ExtractAndCastTo
        /// <summary>
        /// Method to check if reader is not null,
        /// the column is not null
        /// </summary>
        /// <typeparam name="T">column type</typeparam>
        /// <param name="dr">DataReader</param>
        /// <param name="colIndex">Reader Index</param>
        /// <returns>Column value based on index with defined type</returns>
        /// <condition>
        /// If all above summary rules follows or not
        /// </condition>
        public static T ExtractAndCastTo<T>(this IDataReader dr, int colIndex)
        {
            //Create required type object and assign default value
            T data = default(T);
            
            try
            {
                if (dr != null)
                    //Validate the reader column
                    if (!dr.IsDBNull(colIndex))
                    {
                        if (typeof(T) == typeof(DateTime))
                            data = (T)Convert.ChangeType(dr.GetDateTime(colIndex), typeof(T));
                        else if (typeof(T) == typeof(Int32))
                            data = (T)Convert.ChangeType(dr.GetInt32(colIndex), typeof(T));
                        else if (typeof(T) == typeof(Int16))
                            data = (T)Convert.ChangeType(dr.GetInt16(colIndex), typeof(T));
                        else if (typeof(T) == typeof(decimal))
                            data = (T)Convert.ChangeType(dr.GetDecimal(colIndex), typeof(T));
                        else
                            data = (T)Convert.ChangeType(dr.GetString(colIndex), typeof(T));
                    }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error has occured in Extract and Cast Data from DataReader Index: {colIndex.ToString() ?? ""}", ex);
            }
            return data;
        }
        #endregion ExtractAndCastTo

        public static Nullable<T> ExtractAndCastToNull<T>(this IDataReader dr, string fieldName) where T : struct
        {

            try
            {
                object columnValue = dr[fieldName];

                if (!(columnValue is DBNull))
                    return (T)columnValue;



            }
            catch (Exception ex)
            {
                throw new Exception($"Error has occured in Extract and Cast Data from DataReader field name: {fieldName ?? ""}", ex);
            }
            return null;

        }


        public static T ExtractAndCastTo<T>(this IDataRecord dr, string fieldName)
        {
            T data = default(T);
            try
            {
                if (dr != null)
                {
                    if (dr[fieldName] != DBNull.Value)

                    {
                        data = (T)dr[fieldName];
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error has occured in Extract and Cast Data from DataReader field name: {fieldName ?? ""}", ex);
            }
            return data;

        }
        public static Nullable<T> ExtractAndCastToNull<T>(this IDataRecord dr, string fieldName) where T : struct
        {

            try
            {
                object columnValue = dr[fieldName];

                if (!(columnValue is DBNull))
                    return (T)columnValue;



            }
            catch (Exception ex)
            {
                throw new Exception($"Error has occured in Extract and Cast Data from DataReader field name: {fieldName ?? ""}", ex);
            }
            return null;

        }
        public static decimal? DecimalZeroToNull(this decimal? item)
        {
            if (item.HasValue && item.Value == 0)
                return null;
            return item;
        }
        public static IDataReader SafeExecuteReader(this IDbCommand cmd)
        {
            IDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in ExecuteReader Command: {cmd.CommandText}", ex);
            }
            return reader;

        }
    }
}
