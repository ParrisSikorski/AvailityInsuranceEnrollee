using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace AvailityInsuranceEnrollee
{
    public class CSVFileParser
    {
        Ilogger _logger;
        private string _fileLocation;

        public CSVFileParser(string fileLocation)
        {
            _fileLocation = fileLocation;
        }
        public IEnumerable<InsuranceCompanyEnrollee> FileParser()
        {
            var InsuranceList = new List<InsuranceCompanyEnrollee>();
            
            try
            {
                foreach (string file in Directory.EnumerateFiles(_fileLocation, "*.csv"))
                {
                    using (var reader = new StreamReader(file))
                    {
                        string line = string.Empty;
                        string headerLine = reader.ReadLine();
                        while ((line = reader.ReadLine()) != null)
                        {
                            var enrollmentRecord = new InsuranceCompanyEnrollee();
                            var splitLine = line.Split(",");
                            //userId,firstName,lastName,version,insuranceCompany
                            if (splitLine.Length == 4)
                            {
                                enrollmentRecord.userId = splitLine[0];
                                enrollmentRecord.firstAndLastName = splitLine[1];
                                enrollmentRecord.version = Int32.Parse(splitLine[2]);
                                enrollmentRecord.insuranceCompany = splitLine[3];
                            }
                            InsuranceList.Add(enrollmentRecord);
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }
            

            return InsuranceList;
        }
    }
}
