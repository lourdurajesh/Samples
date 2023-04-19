using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube.fx.common.contract.model
{   
  
    public class UpdateEntity
    {
        public bool Success { get; set; }
        public string WhereQuery { get; set; }
        public List<KeyValueEntity> keyValueEntities { get; set; }

    }

    public class KeyValueEntity
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }
    public class FilterListDetails
    {
        public int GroupId { get; set; }
        public string Duration { get; set; }
        public string LogicalOperator { get; set; }
        public List<Input> FilterInput { get; set; }
    }

    public class Input
    {
        public string FieldName { get; set; }       
        public object Value { get; set; }       
        public string CompareOperator { get; set; }       
        public string DataType { get; set; }
        public string LogicalOperator { get; set; }
      
    }


    public class UIInput
    {        
        public string FieldName { get; set; }
        public object Value { get; set; }
        public bool IsObject { get; set; }
        public string LabelText { get; set; }
    }

    public class FilterInput
    {
        public string FilterID { get; set; }
        public List<UIInput> SearchInput { get; set; }
    }
}
