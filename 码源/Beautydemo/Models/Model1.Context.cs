﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BeautyEntities2 : DbContext
    {
        public BeautyEntities2()
            : base("name=BeautyEntities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tb_admin> tb_admin { get; set; }
        public virtual DbSet<tb_cart> tb_cart { get; set; }
        public virtual DbSet<tb_message> tb_message { get; set; }
        public virtual DbSet<tb_orderdetails> tb_orderdetails { get; set; }
        public virtual DbSet<tb_orders> tb_orders { get; set; }
        public virtual DbSet<tb_product> tb_product { get; set; }
        public virtual DbSet<tb_user> tb_user { get; set; }
    }
}
