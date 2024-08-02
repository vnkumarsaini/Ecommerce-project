using Dapper;
using Ecomm_project_01.DataAccess.Data;
using Ecomm_project_01.DataAccess.Repository.iRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm_project_01.DataAccess.Repository
{
    internal class SPCall : ISPCall
    {
        private readonly ApplicationDbContext _context;
        private static string ConnectionString = "";
        public SPCall(ApplicationDbContext context)
        {
            _context = context;
            ConnectionString = _context.Database.GetDbConnection().ConnectionString;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Execute(string ProcedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlcon = new SqlConnection(ConnectionString))
            {
                sqlcon.Open();
                sqlcon.Execute(ProcedureName, param, 
                commandType:CommandType.StoredProcedure);
            }
        }

        public IEnumerable<T> List<T>(string ProcedureName, DynamicParameters param = null)
        {
            using(SqlConnection sqlcon = new SqlConnection(ConnectionString))
            {
                sqlcon.Open();
                return sqlcon.Query<T>( ProcedureName,param, commandType: CommandType.StoredProcedure);
            }
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string ProcedureName, DynamicParameters param = null)
        {
            using(SqlConnection sqlCon= new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                var result = sqlCon.QueryMultiple(ProcedureName, param,
                    commandType: CommandType.StoredProcedure);
                var item1 = result.Read<T1>();
                var item2 = result.Read<T2>();

                if(item1 != null && item2 != null)
                {
                    return new Tuple<IEnumerable<T1>, IEnumerable < T2 >>(item1,item2);
                }
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(),new List<T2>());

            }
        }

        public T OneRecord<T>(string ProcedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlcon = new SqlConnection(ConnectionString))
            {
                sqlcon.Open();
                var value = sqlcon.Query<T>(ProcedureName, param, commandType: CommandType.StoredProcedure);
                return value.FirstOrDefault();
            }
        }

        public T Single<T>(string ProcedureName, DynamicParameters param = null)
        {
           using(SqlConnection sqlcon = new SqlConnection(ConnectionString))
            {
                sqlcon.Open();
                return sqlcon.ExecuteScalar<T>(ProcedureName, param,
                    commandType:CommandType.StoredProcedure);
            }
        }
    }
}
