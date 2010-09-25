namespace SkyMap.Net.Criteria
{
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing.Design;

    [Serializable]
    public class TyQuery : ManyToOneDataSource, ISaveAs
    {
        private string detailPage;
        private string formPage;
        private IList<TyQueryWhere> queryWheres;
        private IList<TrFilter> filters;
        private string sql;

        public void SaveAs(UnitOfWork unitOfWork)
        {
            throw new NotImplementedException();
        }

        [DisplayName("打开详细页命令")]
        public string DetailPage
        {
            get
            {
                return this.detailPage;
            }
            set
            {
                this.detailPage = value;
            }
        }

        [DisplayName("打开表单命令")]
        public string FormPage
        {
            get
            {
                return this.formPage;
            }
            set
            {
                this.formPage = value;
            }
        }

        public IList<TyQueryWhere> QueryWheres
        {
            get
            {
                return this.queryWheres;
            }
            set
            {
                this.queryWheres = value;
            }
        }

        public IList<TrFilter> TrFilters
        {
              get 
              {
                    return this.filters;
              }
              set
              {
                    this.filters = value;
              }
        }


        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), DisplayName("SQL"), Description("1.可以在SQL语句中使用：{SYS:STAFFID}：当前用户ID，{SYS:STAFFNAME}：当前用户姓名，\r\n {SYS:DEPTID}：当前用户部门ID，{SYS:DEPTNAME}：当前用户部门名称,{SYS:NOW}：当前系统时间\r\n2.“{0}”表示将生成的Where字句")]
        public string Sql
        {
            get
            {
                return this.sql;
            }
            set
            {
                this.sql = value;
            }
        }
    }
}

