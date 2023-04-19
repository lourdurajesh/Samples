namespace cube.fx.bulkupload.api.Controllers
{
    internal class ServiceNameAttribute : Attribute
    {
        public string ServiceName;

        public ServiceNameAttribute(string name)
        {
            this.ServiceName = name;
        }
    }
}