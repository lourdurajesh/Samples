using cube.fx.common.contract.model;
using cube.fx.bulkupload.api.Services;

namespace cube.fx.bulkupload.api.Services
{
    public class DummyService : BaseService
    {
        public override APIResponse Invoke()
        {
            throw new NotImplementedException();
        }
        public override APIResponse Update()
        {
            return base.Update();
        }
    }
}
