﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ecomm_project_01.DataAccess.Repository.iRepository
{
    public interface ISPCall:IDisposable
    {
        void Execute(string ProcedureName , DynamicParameters param = null);

        T Single<T>(string ProcedureName, DynamicParameters param= null);

        T OneRecord<T>(string ProcedureName, DynamicParameters param = null);

        IEnumerable<T> List<T>(string ProcedureName, DynamicParameters param = null);

        Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>
            (string ProcedureName, DynamicParameters param = null);
    }
}
