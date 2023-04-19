using cube.fx.common.contract.model;
using cube.fx.common.contract.model.des;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube.fx.common.contract.interfaces
{
   public interface IDesActionLogHandler
    {
        APIResponse Insert(DesActionLog desActionLog);
        APIResponse Update(UpdateEntity updateEntity);
    }
}
