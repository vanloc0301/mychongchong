namespace SkyMap.Net.SqlOM.Render
{
    using SkyMap.Net.SqlOM;
    using System;

    public interface ISqlOmRenderer
    {
        string RenderDelete(DeleteQuery query);
        string RenderInsert(InsertQuery query);
        string RenderPage(int pageIndex, int pageSize, int totalRowCount, SelectQuery query);
        string RenderRowCount(SelectQuery query);
        string RenderSelect(SelectQuery query);
        string RenderUnion(SqlUnion union);
        string RenderUpdate(UpdateQuery query);

        string DateFormat { get; set; }
    }
}

