using cube.fx.common.contract.model;

namespace cube.fx.bulkupload.api.Interfaces.Services
{
    public interface IService
    {
        object Input { get; set; }
        APIResponse Invoke();
        APIResponse Insert();
        APIResponse Update();
        APIResponse GetById(Guid pk);
        APIResponse FindAll();

    }
}
