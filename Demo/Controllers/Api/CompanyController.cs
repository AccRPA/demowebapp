using Demo.Infrastructure;
using Demo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Demo.Controllers
{
    public class CompanyController : ApiController
    {
        /// <summary>
        /// Return all the companies stored in the database
        /// </summary>
        /// <returns>List of companies</returns>
        public IEnumerable<Company> Get()
        {
            IList<Company> companies = null;
            using (var ctx = new ApplicationDbContext())
            {
                companies = ctx.Company.Include("Address").ToList();
            }
            return companies;
        }

        /// <summary>
        /// Retrieve a Company by its Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Company</returns>
        public Company Get(int id)
        {
            Company company = null;
            using (var ctx = new ApplicationDbContext())
            {
                company = GetCompanyById(id, ctx);
            }
            return company;
        }

        /// <summary>
        /// Retrieve a Company by its Name 
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Company</returns>
        public Company Get(string name)
        {
            Company company = null;
            using (var ctx = new ApplicationDbContext())
            {
                if (ctx.Company.Any(x => x.Name == name))
                {
                    company = ctx.Company.Include("Address").Single(x => x.Name.ToLower().Equals(name.ToLower()));
                }
            }
            return company;
        }

        /// <summary>
        /// Insert a new Company in the database
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Operation result</returns>
        public IHttpActionResult Post([FromBody]Company data)
        {
            using (var ctx = new ApplicationDbContext())
            {
                if (null == Get(data.Name))
                {
                    ctx.Company.Add(data);
                    ctx.SaveChanges();
                    return Ok();
                }
                else
                {
                    return BadRequest(Constants.MESSAGE_COMPANY_ALREADY_EXISTS);
                }
            }
        }

        /// <summary>
        /// Update a Company by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns>Operation result</returns>
        public IHttpActionResult Put(int id, [FromBody]Company data)
        {
            using (var ctx = new ApplicationDbContext())
            {
                Company company = GetCompanyById(id, ctx);
                if (null != company)
                {
                    if (null == Get(data.Name))
                    {
                        company.Name = data.Name;
                        company.Address.Country = data.Address.Country;
                        company.Address.City = data.Address.City;
                        company.Address.ZipCode = data.Address.ZipCode;
                        ctx.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return BadRequest(Constants.MESSAGE_COMPANY_ALREADY_EXISTS);
                    }
                }
                else
                {
                    return BadRequest(string.Format(Constants.MESSAGE_ERROR_NO_ID, id));
                }
            }
        }

        /// <summary>
        /// Delete a company by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Operation result</returns>
        public IHttpActionResult Delete(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                Company company = GetCompanyById(id, ctx);
                if (null != company)
                {
                    ctx.Address.Remove(company.Address);
                    ctx.Company.Remove(company);
                    ctx.SaveChanges();
                    return Ok();
                }
                else
                {
                    return BadRequest(string.Format(Constants.MESSAGE_ERROR_NO_ID, id));
                }
            }
        }

        /// <summary>
        /// Get a Company in the same context
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        private Company GetCompanyById(int id, ApplicationDbContext dbContext)
        {
            Company company = null;
            if (dbContext.Company.Any(x => x.Id == id))
            {
                company = dbContext.Company.Include("Address").Single(x => x.Id == id);
            }
            return company;
        }
    }
}
