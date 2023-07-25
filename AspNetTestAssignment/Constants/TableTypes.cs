namespace AspNetTestAssignment.Constants
{
    public enum TableType
    {
        Companies,
        CompanyHistory,
        Notes,
        Employees
    }

    public struct ColumnInfo
    {
        public string Name { get; set; }
        public string ShowName { get; set; }
        public string Width { get; set; }
    }

    public class TableTypes
    {
        public static Dictionary<TableType, List<ColumnInfo>> Columns => new Dictionary<TableType, List<ColumnInfo>>()
        {
            { TableType.Companies, new List<ColumnInfo>()
            {
                new ColumnInfo(){ Name = "Name", ShowName = "Company Name", Width = "40%" },
                new ColumnInfo(){ Name = "City", ShowName = "City", Width = "20%" },
                new ColumnInfo(){ Name = "State", ShowName = "State", Width = "20%" },
                new ColumnInfo(){ Name = "Phone", ShowName = "Phone", Width = "20%" },
            } },
            { TableType.CompanyHistory, new List<ColumnInfo>()
            {
                new ColumnInfo(){ Name = "OrderDate", ShowName = "Order Date", Width = "50%" },
                new ColumnInfo(){ Name = "City", ShowName = "Store City", Width = "50%" },
            } },
            { TableType.Notes, new List<ColumnInfo>()
            {
                new ColumnInfo(){ Name = "InvoiceNumber", ShowName = "Invoice Number", Width = "50%" },
                new ColumnInfo(){ Name = "EmployeeName", ShowName = "Employee", Width = "50%" },
            } },
            { TableType.Employees, new List<ColumnInfo>()
            {
                new ColumnInfo(){ Name = "FirstName", ShowName = "First Name", Width = "50%" },
                new ColumnInfo(){ Name = "LastName", ShowName = "Last Name", Width = "50%" },
            } },
        };

        public static Dictionary<TableType, bool> HasAddButton => new Dictionary<TableType, bool>()
        {
            { TableType.Companies, true },
            { TableType.CompanyHistory, false },
            { TableType.Notes, true },
            { TableType.Employees, true },
        };

        public static Dictionary<TableType, bool> HasEditButton => new Dictionary<TableType, bool>()
        {
            { TableType.Companies, true },
            { TableType.CompanyHistory, false },
            { TableType.Notes, false },
            { TableType.Employees, true },
        };

        public static Dictionary<TableType, bool> HasRefreshButton => new Dictionary<TableType, bool>()
        {
            { TableType.Companies, false },
            { TableType.CompanyHistory, true },
            { TableType.Notes, true },
            { TableType.Employees, true },
        };

        public static Dictionary<TableType, bool> HasRemoveButton => new Dictionary<TableType, bool>()
        {
            { TableType.Companies, false },
            { TableType.CompanyHistory, false },
            { TableType.Notes, true },
            { TableType.Employees, true },
        };

        public static Dictionary<TableType, string> Title => new Dictionary<TableType, string>()
        {
            { TableType.Companies, "Companies" },
            { TableType.CompanyHistory, "History" },
            { TableType.Notes, "Notes" },
            { TableType.Employees, "Employees" },
        };
    }
}
