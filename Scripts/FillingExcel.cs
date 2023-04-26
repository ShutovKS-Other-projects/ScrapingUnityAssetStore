using OfficeOpenXml;

namespace ParsingMarketPlaces;

public class FillingExcel
{
    private FileInfo _file;
    private ExcelPackage _excelPackage;
    private ExcelWorksheet _worksheet;

    public FillingExcel(string path)
    {
        _file = new FileInfo(path);
        _excelPackage = new ExcelPackage(_file);

        if (File.Exists(path))
        {
            var isThis = false;
            foreach (var excelWorksheet in _excelPackage.Workbook.Worksheets)
            {
                if (excelWorksheet.Name != "DataBase Assets") continue;
                _worksheet = excelWorksheet;
                isThis = true;
            }

            if (!isThis)
                _excelPackage.Workbook.Worksheets.Add("DataBase Assets");
        }
        else
        {
            _excelPackage.Workbook.Worksheets.Add("DataBase Assets");
            foreach (var excelWorksheet in _excelPackage.Workbook.Worksheets)
            {
                if (excelWorksheet.Name != "DataBase Assets") continue;
                _worksheet = excelWorksheet;
                return;
            }
        }
    }

    //TODO: is value
}