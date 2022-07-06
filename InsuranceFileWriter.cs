using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AvailityInsuranceEnrollee
{
    public class InsuranceFileWriter
    {
        public void CreateInsuranceFiles(IEnumerable<InsuranceCompanyEnrollee> InsuranceList)
        {
            try
            {
                var insuranceCompanies = (from i in InsuranceList
                                          group i by new { i.insuranceCompany }
                          into grp
                                          select new InsuranceCompanyEnrollee
                                          {
                                              insuranceCompany = grp.Key.insuranceCompany
                                          }).ToList();
                foreach (var company in insuranceCompanies)
                {
                    var companyEnrollee = new List<InsuranceCompanyEnrollee>();
                    foreach (var enrollee in InsuranceList)
                    {
                        if (company.insuranceCompany.ToString() == enrollee.insuranceCompany)
                            companyEnrollee.Add(enrollee);
                    }

                    var removedDuplicates = (from e in companyEnrollee
                                             group e by e.userId into grouped
                                             orderby grouped.Key
                                             select new InsuranceCompanyEnrollee
                                             {
                                                 userId = grouped.Key,
                                                 version = grouped.Max(g => g.version),
                                                 firstAndLastName = grouped.Max(g => g.firstAndLastName),
                                                 insuranceCompany = grouped.Max(g => g.insuranceCompany)
                                             }).ToList();

                    var orderedList = removedDuplicates.OrderBy(x => x.firstAndLastName).ToList();
                    var folderPath = System.IO.Directory.GetCurrentDirectory();
                    var thisFolder = string.Format("{0}\\{1}", folderPath.Substring(0, folderPath.IndexOf("bin")), "FormattedInsuranceFiles");
                    string formattedDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                    var file = Path.Combine(thisFolder, string.Format("{0}_{1}_Insurance.txt", company.insuranceCompany.ToString(), formattedDate));

                    if (!System.IO.Directory.Exists(thisFolder))
                    {
                        Directory.CreateDirectory(thisFolder);
                    }
                    if (!File.Exists(file))
                    {
                        using (var writer = new StreamWriter(file))
                        {
                            writer.WriteLine("userId,firstAndLastName,version,insuranceCompany");
                            foreach (var x in orderedList)
                            {
                                writer.WriteLine(String.Format("{0},{1},{2},{3}", x.userId, x.firstAndLastName, x.version, company.insuranceCompany.ToString()));
                            }
                        }
                    }

                    else
                    {
                        Console.WriteLine("File {0} already exists", file);
                    }

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }

        }
    }
}
