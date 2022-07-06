using System;

namespace AvailityInsuranceEnrollee
{
    class Program
    {
        static void Main(string[] args)
        {
            var folderPath = System.IO.Directory.GetCurrentDirectory();
            var thisFolder = string.Format("{0}\\{1}", folderPath.Substring(0, folderPath.IndexOf("bin")), "CSVFiles");

            var parser = new CSVFileParser(thisFolder);

            var insuranceList = parser.FileParser();

            var writer = new InsuranceFileWriter();
            writer.CreateInsuranceFiles(insuranceList);
        }
    }
}
