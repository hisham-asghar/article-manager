using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
//using Generics.Helpers;
//using Generics.Models;
using LayerDb.AdoNet;
using LayerDb.Models;

namespace LayerDb
{
    public class QueryExecutor
    {
        // ADO.NET
        public static bool ExecuteDml(string query, string function = null, bool retry = true)
        {
            try
            {
                return DatabaseQueryExecuter<object>.Execute(query) >= 0;
            }
            catch (Exception ex)
            {
                if (retry && !string.IsNullOrWhiteSpace(ex.ToString()) && ex.ToString().ToLower().Contains("timeout"))
                    return ExecuteDml(query, function, false);
                //(new NotifyModel()
                //{
                //    Body = ex.ToString(),
                //    Subject =
                //        $"Exception Error - Function = {function} - Query = {query} - Exception = "
                //}).Notify();
            }

            return false;
        }
        public static T FirstOrDefault<T>(string query, string function = null, bool retry = true) where T : new()
        {
            try
            {
                return DatabaseQueryExecuter<T>.Get(query);
            }
            catch (Exception ex)
            {
                if (retry && !string.IsNullOrWhiteSpace(ex.ToString()) && ex.ToString().ToLower().Contains("timeout"))
                    return FirstOrDefault<T>(query, function, false);
                //(new NotifyModel()
                //{
                //    Body = ex.ToString(),
                //    Subject =
                //        $"Exception Error - Function = {function} - Query = {query} - Exception = "
                //}).Notify();
            }

            return default(T);
        }
        public static List<T> List<T>(string query, string function = null,bool retry = true) where T : new()
        {
            try
            {
                return DatabaseQueryExecuter<T>.GetAll(query);
            }
            catch (Exception ex)
            {
                if (retry && !string.IsNullOrWhiteSpace(ex.ToString()) && ex.ToString().ToLower().Contains("timeout"))
                    return List<T>(query, function, false);
                //(new NotifyModel()
                //{
                //    Body = ex.ToString(),
                //    Subject =
                //        $"Exception Error - Function = {function} - Query = {query} - Exception = "
                //}).Notify();
            }

            return default(List<T>);
        }

    }
}
