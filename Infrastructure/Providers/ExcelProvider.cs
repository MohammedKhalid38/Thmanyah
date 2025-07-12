using AutoDependencyRegistration.Attributes;
using Infrastructure.Providers.Contracts;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Data;

namespace Infrastructure.Providers;
[RegisterClassAsScoped]
public class ExcelProvider : IExcelProvider
{

    //public List<DataTable> ExcelToDataTable(IFormFile excelFile, bool includeHeader = false)
    //{
    //    List<DataTable> result = new();

    //    if (excelFile != null)
    //    {
    //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

    //        using var package = new ExcelPackage(excelFile.OpenReadStream());
    //        ExcelWorksheets worksheets = package.Workbook.Worksheets;

    //        if (worksheets == null || worksheets.Count == 0)
    //        {
    //            throw new ArgumentException("Excel file does not contain any worksheet.");
    //        }

    //        foreach (var worksheet in worksheets)
    //        {
    //            DataTable table = new();

    //            // Add columns with either header names or default column names
    //            if (includeHeader)
    //            {
    //                // Add columns based on the first row
    //                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
    //                {
    //                    table.Columns.Add(worksheet.Cells[1, col].Text.Trim());
    //                }
    //            }
    //            else
    //            {
    //                // Add default column names (Column1, Column2, ...)
    //                for (int i = 1; i <= worksheet.Dimension.End.Column; i++)
    //                {
    //                    table.Columns.Add($"Column{i}");
    //                }
    //            }

    //            // Start reading from row 2 if including headers, row 1 if not
    //            int startRow = includeHeader ? 2 : 1; // Skip header row if `includeHeader` is true

    //            for (int rowNumber = startRow; rowNumber <= worksheet.Dimension.End.Row; rowNumber++)
    //            {
    //                DataRow newRow = table.NewRow();

    //                for (int colNumber = 1; colNumber <= worksheet.Dimension.End.Column; colNumber++)
    //                {
    //                    // Assign cell value to the respective DataRow
    //                    newRow[colNumber - 1] = worksheet.Cells[rowNumber, colNumber].Text;
    //                }

    //                table.Rows.Add(newRow);
    //            }

    //            result.Add(table);
    //        }
    //    }

    //    return result;
    //}
    public List<DataTable> ExcelToDataTable(IFormFile excelFile, bool includeHeader = false)
    {
        List<DataTable> result = new();

        if (excelFile != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(excelFile.OpenReadStream());
            ExcelWorksheets worksheets = package.Workbook.Worksheets;
            if (worksheets == null || worksheets.Count == 0)
                throw new ArgumentException("Excel file does not contain any worksheet.");

            foreach (var worksheet in worksheets)
            {
                DataTable table = new();
                // Add columns with either header names or default column names
                if (includeHeader)
                {
                    // Add columns based on the first row
                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                        table.Columns.Add(worksheet.Cells[1, col].Text.Trim());
                }
                else
                {
                    // Add default column names (Column1, Column2, ...)
                    for (int i = 1; i <= worksheet.Dimension.End.Column; i++)
                        table.Columns.Add($"Column{i}");
                }

                // Start reading from row 2 if including headers, row 1 if not
                int startRow = includeHeader ? 2 : 1; // Skip header row if `includeHeader` is true

                for (int rowNumber = startRow; rowNumber <= worksheet.Dimension.End.Row; rowNumber++)
                {
                    DataRow newRow = table.NewRow();

                    bool isRowEmpty = true; // Flag to check if the row is empty

                    for (int colNumber = 1; colNumber <= worksheet.Dimension.End.Column; colNumber++)
                    {
                        var cellValue = worksheet.Cells[rowNumber, colNumber].Text;

                        if (!string.IsNullOrEmpty(cellValue))
                            isRowEmpty = false; // Found a non-empty cell

                        // Assign cell value to the respective DataRow
                        newRow[colNumber - 1] = cellValue;
                    }

                    // If the row is empty, skip it
                    if (isRowEmpty)
                        continue; // Skip adding this row to the DataTable

                    table.Rows.Add(newRow);
                }

                result.Add(table);
            }
        }

        return result;
    }

}