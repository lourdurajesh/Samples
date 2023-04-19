using cube.fx.common.contract.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube.fx.common.contract.interfaces
{
    public interface ICRUD
    {
        void SetInstance(string db, bool fromOutside=false);
        T Insert<T>(T Obj) where T : class;
        UpdateEntity Update<T>(UpdateEntity Obj) where T : class;
        T GetById<T>(KeyValueEntity guidInput) where T : class;
        bool DeleteByIds<T>(KeyValueEntity deleteInputs) where T : class;
        List<T> BulkInsert<T>(List<T> Obj) where T : class;
        List<UpdateEntity> BulkUpdate<T>(List<UpdateEntity> Obj) where T : class;
        List<T> GetMany<T>(List<FilterListDetails> filterListDetails, string sortColumn = null, string shortType = null) where T : class;
        DataTable ExecuteQuery(string strQuery);
    }
}
