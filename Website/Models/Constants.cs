using LayerBao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class Constants
    {
        public class UserRoles
        {
            public const string Reviewer = "Reviewer";
            public const string Reader = "Reader";
            public const string Admin = "Admin";
        }
        private static string DefaultDatabase = null;
        public static string GetDefaultDatabase()
        {
            if (string.IsNullOrWhiteSpace(DefaultDatabase))
            {
                DefaultDatabase = SiteMetaBao.GetCurrentDatabaseName();
            }
            return DefaultDatabase;
        }
        public static string ResetDefaultDatabase()
        {
            DefaultDatabase = SiteMetaBao.GetCurrentDatabaseName();
            return DefaultDatabase;
        }
    }
}