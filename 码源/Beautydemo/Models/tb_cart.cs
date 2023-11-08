//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Beautydemo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class tb_cart
    {
        [DisplayName("购物车编号")]
        public int cid { get; set; }
        [DisplayName("用户姓名")]
        public string name { get; set; }
        [DisplayName("商品编号")]
        public int pid { get; set; }
        [DisplayName("商品名称")]
        public string pname { get; set; }
        [DisplayName("商品单价")]
        public Nullable<decimal> price { get; set; }
        [DisplayName("商品数量")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "数量不可为空")]
        [DataType(DataType.Text, ErrorMessage = "数量格式错误")]
        public Nullable<int> nums { get; set; }
        [DisplayName("商品图片")]
        public string photo { get; set; }
    
        public virtual tb_product tb_product { get; set; }
    }
}
