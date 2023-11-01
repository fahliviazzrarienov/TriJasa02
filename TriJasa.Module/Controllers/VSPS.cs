using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Helpers;
using TriJasa.Module.BusinessObjects;
//using Npgsql;
//using NpgsqlTypes;

namespace TriJasa.Module.Controllers {
    public class VSPS : ViewController {
        protected override void OnActivated() {
            base.OnActivated();
            //Dennis:
            //Session sesion = ((XPObjectSpace)Application.CreateObjectSpace(typeof(Article))).Session;
            //var resultSet = sesion.ExecuteQuery(@"select * from ""Article"" N0 where(N0.""GCRecord"" is null and((Strpos(N0.""Title"", 'boOboo') > 0) or(Strpos(N0.""Content"", 'boOboo') > 0)))");
            //returns 1 record
            //var result = sesion.FindObject<Article>(CriteriaOperator.Parse("Contains(Title, ?) OR Contains(Content, ?)", "boOboo", "boOboo"));
            //returns 0 records with PostgreSqlConnectionProvider and 1 with PostgreSqlConnectionProviderEx.
        }
    }
    public class PostgreSqlConnectionProviderEx : PostgreSqlConnectionProvider {
        public new const string XpoProviderTypeString = "PostgresEx";
        public PostgreSqlConnectionProviderEx(IDbConnection connection, AutoCreateOption autoCreateOption) : base(connection, autoCreateOption) {
        }
        public override string FormatFunction(FunctionOperatorType operatorType, params string[] operands) {
            if(operatorType == FunctionOperatorType.Contains) {
                return string.Format(CultureInfo.InvariantCulture, "(Strpos(Lower({0}), Lower({1})) > 0)", operands[0], operands[1]);
            }
            return base.FormatFunction(operatorType, operands);
        }
        public new static IDataStore CreateProviderFromString(string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect) {
            IDbConnection connection = CreateConnection(connectionString);
            objectsToDisposeOnDisconnect = new IDisposable[] { connection };
            return CreateProviderFromConnection(connection, autoCreateOption);
        }
        public new static IDataStore CreateProviderFromConnection(IDbConnection connection, AutoCreateOption autoCreateOption) {
            return new PostgreSqlConnectionProviderEx(connection, autoCreateOption);
        }
        //protected override IDataParameter CreateParameter(IDbCommand command, object value, string name)
        //{
        //    NpgsqlParameter param = base.CreateParameter(command, value, name);
        //    if (param.DbType == DbType.String)
        //        param.NpgsqlDbType = NpgsqlDbType.Citext;
        //    return param;
        //}
        //protected override string GetSqlCreateColumnTypeForString(DBTable table, DBColumn column)
        //{
        //    return "citext";
        //}
        public new static void Register() {
            try {
                DataStoreBase.RegisterDataStoreProvider(XpoProviderTypeString, new DataStoreCreationFromStringDelegate(CreateProviderFromString));
            }
            catch(ArgumentException e) {
                Tracing.Tracer.LogText(e.Message);
                Tracing.Tracer.LogText("A connection provider with the same name ( {0} ) has already been registered", XpoProviderTypeString);
            }
        }
    }
}
