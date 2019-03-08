using Demo.Models;
using System.Collections.Generic;
using System.Net.Http;

namespace Demo.Infrastructure
{
    /// <summary>
    /// Interface which defines methods to use between controllers and web api 
    /// </summary>
    public interface ICompanyRepository
    { 
        List<Company> GetAll();

        Company GetCompanyById(int id);

        HttpResponseMessage AddCompany(Company data);

        HttpResponseMessage UpdateCompany(Company data);

        HttpResponseMessage DeleteCompany(int id);
    }
}
