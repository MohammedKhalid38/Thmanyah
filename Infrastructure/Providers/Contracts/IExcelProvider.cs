using Microsoft.AspNetCore.Http;
using System.Data;

namespace Infrastructure.Providers.Contracts;

public interface IExcelProvider
{
    //DataTable ExcelToDataTable(IFormFile excelFile);
    List<DataTable> ExcelToDataTable(IFormFile excelFile, bool includeHeader = false);
}