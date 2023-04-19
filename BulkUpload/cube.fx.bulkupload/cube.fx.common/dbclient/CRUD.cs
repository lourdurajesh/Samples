using cube.fx.common.contract.interfaces;
using cube.fx.common.contract.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cube.fx.common.dbclient
{
    public class CRUD : ICRUD
    {
        private string connectionString;
        public void SetInstance(string db, bool fromOutside = false)
        {
            if (fromOutside)
                connectionString = db;
            else
            {
                CRUDHelper cRUDHelper = new CRUDHelper(db);
                connectionString = cRUDHelper.connectionString;
            }
        }
        public T Insert<T>(T Obj) where T : class
        {

            using (var conn = new SqlConnection(connectionString))
            {
                int retryCount = 0;
                conn.Open();
            RetryStart:
                try
                {

                    List<T> listObj = new List<T>();
                    listObj.Add(Obj);
                    int rowsAffected;
                    using (SqlCommand cmd = new SqlCommand(Entity2InsertQuery<T>(listObj), conn))
                        rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        return Obj;
                    else
                        return null;
                }
                catch (SqlException e)
                {
                    if (e.Number == 1205 && retryCount < 5)  // SQL Server error code for deadlock
                    {
                        Thread.Sleep(200);
                        retryCount++;
                        goto RetryStart;
                    }
                    else
                    {
                        throw;
                    }

                }
                finally
                {
                    conn.Close();
                }
            }



        }
        public UpdateEntity Update<T>(UpdateEntity updateEntity) where T : class
        {
            int retryCount = 0;
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
            RetryStart:
                try
                {
                    int rowsAffected;
                    using (SqlCommand cmd = new SqlCommand(Entity2UpdateQuery<T>(updateEntity), conn))
                        rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        updateEntity.Success = true;
                    else
                        updateEntity.Success = false;
                }
                catch (SqlException e)
                {
                    if (e.Number == 1205 && retryCount < 5)  // SQL Server error code for deadlock
                    {
                        Thread.Sleep(300);
                        retryCount++;
                        goto RetryStart;
                    }
                    else
                    {
                        throw;
                    }

                }
                finally
                {
                    conn.Close();
                }
            }
            return updateEntity;

        }
        public T GetById<T>(KeyValueEntity guidInput) where T : class
        {
            var ds = new DataSet();
            string tName = GetTableName<T>();
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string strQuery = "SELECT * FROM " + tName + " WHERE " + guidInput.Key + " = '" + (Guid)guidInput.Value + "'";
                    using (SqlCommand cmd = new SqlCommand(strQuery, conn))
                    using (var adapter = new SqlDataAdapter(cmd))
                        adapter.Fill(ds);
                    DataTable dataTable = ds.Tables[0];
                    var TLst = JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(dataTable));
                    return TLst.FirstOrDefault();
                }
                catch
                {

                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public bool DeleteByIds<T>(KeyValueEntity deleteInput) where T : class
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    int rowsAffected;
                    using (SqlCommand cmd = new SqlCommand(Entity2DeleteQuery<T>(deleteInput), conn))
                        rowsAffected = cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {

                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
            return true;
        }
        public List<T> BulkInsert<T>(List<T> ts) where T : class
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    int rowsAffected;
                    using (SqlCommand cmd = new SqlCommand(Entity2InsertQuery<T>(ts), conn))
                        rowsAffected = cmd.ExecuteNonQuery();

                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
            return ts;

        }
        public List<UpdateEntity> BulkUpdate<T>(List<UpdateEntity> updateEntities) where T : class
        {
            using (var conn = new SqlConnection(connectionString))
            {

                updateEntities.ForEach(updateInputEntity =>
                {
                    int retryCount = 0;
                    conn.Open();
                RetryStart:
                    try
                    {                        
                        int rowsAffected;
                        using (SqlCommand cmd = new SqlCommand(Entity2UpdateQuery<T>(updateInputEntity), conn))
                            rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                            updateInputEntity.Success = true;
                        else
                            updateInputEntity.Success = false;
                    }
                    catch (SqlException e)
                    {
                        if (e.Number == 1205 && retryCount < 5)  // SQL Server error code for deadlock
                        {
                            Thread.Sleep(200);
                            retryCount++;
                            goto RetryStart;
                        }
                        else
                        {
                            throw;
                        }

                    }
                    finally
                    {
                        conn.Close();
                    }
                });
            }
            return updateEntities;
        }
        public List<T> GetMany<T>(List<FilterListDetails> filterListDetails, string sortColumn = null, string shortType = null) where T : class
        {
            var ds = new DataSet();
            string tName = GetTableName<T>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                try
                {
                    string strQuery = "SELECT * FROM " + tName + " WHERE " + GenerateQuery(filterListDetails);
                    if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(shortType))
                        strQuery += " order by " + sortColumn + " " + shortType;
                    using (SqlCommand cmd = new SqlCommand(strQuery, conn))
                    using (var adapter = new SqlDataAdapter(cmd))
                        adapter.Fill(ds);
                    DataTable dataTable = ds.Tables[0];
                    return JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(dataTable));
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        public DataTable ExecuteQuery(string strQuery)
        {
            var ds = new DataSet();
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(strQuery, conn))
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandTimeout = 600;
                        adapter.Fill(ds);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
            return ds.Tables[0];
        }

        #region Common Code
        private string Entity2UpdateQuery<T>(UpdateEntity ObjLst) where T : class
        {
            string tName = GetTableName<T>();
            StringBuilder queryBuilderFinal = new StringBuilder();
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("UPDATE " + tName + " WITH (UPDLOCK) SET ");
            ObjLst.keyValueEntities.ForEach(x => { string str = x.Value != null ? x.Value.ToString() : "";   queryBuilder.Append(x.Key + "='" + str.Replace("'", "''") + "',"); });
            queryBuilderFinal.Append(queryBuilder.ToString().Substring(0, queryBuilder.Length - 1));
            queryBuilderFinal.Append(" WHERE " + ObjLst.WhereQuery.ToUpper().Replace("WHERE", ""));
            return queryBuilderFinal.ToString();
        }
        private string Entity2InsertQuery<T>(List<T> ObjLst) where T : class
        {
            string tName = GetTableName<T>();
            StringBuilder queryBuilder = new StringBuilder();
            var obj = typeof(T);
            string columnList = string.Empty;
            foreach (PropertyInfo prop in obj.GetProperties())
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                columnList += prop.Name + ",";
            }
            columnList = columnList.Substring(0, columnList.Length - 1);
            queryBuilder.Append("INSERT INTO " + tName);
            queryBuilder.Append("(" + columnList + ") VALUES");
            ObjLst.ForEach(x =>
            {
                queryBuilder.Append("(");
                x = AddTenentCodeProperty<T>(x);
                x = AddProperty<T>(x, "CreatedBy,UploadedBy,ModifiedBy");
                x = AddDateTimeBasedOnProperty<T>(x, "CreatedDateTime,ModifiedDateTime");
                foreach (PropertyInfo prop in x.GetType().GetProperties())
                {
                    if (prop.GetValue(x) == null)
                        queryBuilder.Append(" null , ");
                    else
                    {
                        string strValue = string.Empty;
                        if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(Nullable<DateTime>))
                        {
                            DateTime dateTime;
                            bool isDatetime = DateTime.TryParse(prop.GetValue(x)?.ToString(),out dateTime);
                            strValue = isDatetime ?Convert.ToDateTime(dateTime).ToString("yyyy-MM-dd HH:mm:ss.fff"): prop.GetValue(x)?.ToString();
                        }
                        else
                            strValue = prop.GetValue(x)?.ToString().Replace("'", "''");
                            queryBuilder.Append("'" + strValue + "', ");
                    }
                }
                string temp = queryBuilder.ToString().Substring(0, queryBuilder.Length - 2);
                queryBuilder = new StringBuilder();
                queryBuilder.Append(temp);
                queryBuilder.Append("),");
            });

            return queryBuilder.ToString().Substring(0, queryBuilder.Length - 1);
        }
        private string Entity2DeleteQuery<T>(KeyValueEntity deleteInput) where T : class
        {
            string tName = GetTableName<T>();
            StringBuilder queryBuilder = new StringBuilder();
            var obj = typeof(T);
            string guidList = string.Empty;
            ((List<Guid>)deleteInput.Value).ForEach(x => { guidList += "'" + x + "',"; });
            guidList = guidList.Substring(0, guidList.Length - 1);
            queryBuilder.Append("DELETE FROM " + tName + " WHERE " + deleteInput.Key + " IN (" + guidList + ")");
            return queryBuilder.ToString();
        }
        private static string GetTableName<T>()
        {
            var tName = Config.Contexts.Where(x => x.Key.ToLower().Equals(typeof(T).Name.ToLower())).FirstOrDefault();
            return tName != null ? tName.Value : typeof(T).Name;
        }


        private string GenerateQuery(List<FilterListDetails> filterListDetails)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" ( ");
            filterListDetails.OrderBy(x => x.GroupId).ToList().ForEach(filterListDetail =>
            {
                StringBuilder stringBuilderInner = new StringBuilder();
                StringBuilder groupKeyValueBuilder = new StringBuilder();
                filterListDetail.FilterInput.ForEach(x =>
                {
                    if (x.CompareOperator.ToLower() == "in")
                        groupKeyValueBuilder.Append(x.FieldName + " " + x.CompareOperator + "( " + x.Value + " ) " + x.LogicalOperator + " ");
                    else
                    {
                        if (x.DataType != null && (x.DataType.Contains('?')) && string.IsNullOrEmpty(x.Value?.ToString()))
                        {
                            groupKeyValueBuilder.Append(x.FieldName + " " + x.CompareOperator + " NULL" + " " + x.LogicalOperator + " ");
                        }
                        else
                            groupKeyValueBuilder.Append(x.FieldName + " " + x.CompareOperator + "'" + x.Value + "'" + " " + x.LogicalOperator + " ");
                    }
                });
                stringBuilderInner.Append(groupKeyValueBuilder.ToString().Substring(0, groupKeyValueBuilder.Length - (filterListDetail.FilterInput[filterListDetail.FilterInput.Count - 1].LogicalOperator.Length + 1)));
                stringBuilderInner.Append(" ) ");
                stringBuilderInner.Append(filterListDetail.LogicalOperator + " ( ");
                stringBuilder.Append(stringBuilderInner);

            });
            int logicalOperatorLength = 0;
            logicalOperatorLength = filterListDetails[filterListDetails.Count - 1].LogicalOperator != null ? filterListDetails[filterListDetails.Count - 1].LogicalOperator.Length : 0;
            return stringBuilder.ToString().Substring(0, stringBuilder.Length - (logicalOperatorLength + 3));
        }

        public virtual T AddProperty<T>(T TObject, string fieldName)
        {
            string PropertyName = string.Empty;
            List<string> lstfieldName = new List<string>();
            try
            {
                if (!string.IsNullOrEmpty(fieldName))
                    lstfieldName = fieldName.Split(new char[] { ',' }).ToList();

                foreach (var item in lstfieldName)
                {
                    string Prefix = string.Empty;
                    foreach (var prop in TObject.GetType().GetProperties())
                    {
                        if (prop.Name.Length > 4)
                            Prefix = prop.Name.Substring(0, 4);
                        else
                            Prefix = "";
                        if (Prefix.ToUpper() != "CMN_")
                            break;

                    }
                    PropertyName = Prefix + item;

                    var UserName = string.Empty;

                    PropertyInfo p = typeof(T).GetProperty(PropertyName);

                    UserName = Config.Settings.CommonSettings.UserName;

                    // check if property exists.
                    if (p != null && !string.IsNullOrEmpty(UserName))
                    {
                        System.Type t = p.PropertyType;

                        if (t == typeof(string))
                        {
                            p.SetValue(TObject, UserName);
                        }
                    }

                }
                return TObject;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual T AddTenentCodeProperty<T>(T TObject) where T : class
        {
            var tenantCode = Config.Settings.CommonSettings.TenantCode;
            if (!string.IsNullOrEmpty(tenantCode))
            {
                if (TObject.GetType().GetProperty("TNT_TenantCode") != null)
                {
                    System.Reflection.PropertyInfo p = typeof(T).GetProperty("TNT_TenantCode");
                    if (p != null)
                    {
                        System.Type t = p.PropertyType;

                        if (t == typeof(string))
                        {
                            p.SetValue(TObject, tenantCode);
                        }
                    }
                }
                else if (TObject.GetType().GetProperty("CMN_TenantCode") != null)
                {
                    System.Reflection.PropertyInfo p = typeof(T).GetProperty("CMN_TenantCode");
                    if (p != null)
                    {
                        System.Type t = p.PropertyType;
                        if (t == typeof(string))
                        {
                            p.SetValue(TObject, tenantCode);
                        }
                    }
                }
            }
            return TObject;
        }
        public virtual T AddDateTimeBasedOnProperty<T>(T TObject, string fieldName)
        {
            string PropertyName = string.Empty;
            List<string> lstfieldName = new List<string>();
            try
            {
                if (!string.IsNullOrEmpty(fieldName))
                    lstfieldName = fieldName.Split(new char[] { ',' }).ToList();

                foreach (var item in lstfieldName)
                {
                    string Prefix = string.Empty;
                    foreach (var prop in TObject.GetType().GetProperties())
                    {
                        if (prop.Name.Length > 4)
                            Prefix = prop.Name.Substring(0, 4);
                        else
                            Prefix = "";
                        if (Prefix.ToUpper() != "CMN_")
                            break;

                    }
                    PropertyName = Prefix + item;

                    DateTime dateTime = Convert.ToDateTime(System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.FFF"));

                    if (TObject.GetType().GetProperty(PropertyName) != null)
                    {
                        PropertyInfo p = typeof(T).GetProperty(PropertyName);
                        if (p != null)
                        {
                            Type t = p.PropertyType;

                            if (t == typeof(DateTime))
                            {
                                p.SetValue(TObject, dateTime);
                            }
                            else if (t == typeof(DateTime?))
                            {
                                p.SetValue(TObject, dateTime);
                            }
                        }
                    }
                }
                return TObject;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
