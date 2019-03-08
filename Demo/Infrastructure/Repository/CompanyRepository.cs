using Demo.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;

namespace Demo.Infrastructure
{
    /// <summary>
    /// This class is used to establish the connection to the API 
    /// and retrieve the information in the way that controllers need
    /// </summary>
    public class CompanyRepository : ICompanyRepository
    {
        /// <summary>
        /// Add a new company only if it doesn't exist in the database
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Returns the result of the operation</returns>
        public HttpResponseMessage AddCompany(Company data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings.Get(Constants.API_URL));
                var responseTask = client.PostAsJsonAsync("company", data);
                responseTask.Wait();

                HttpResponseMessage result = responseTask.Result;
                if (!result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    result.ReasonPhrase = JObject.Parse(readTask.Result)["Message"].ToString();
                }

                return result;
            }
        }

        /// <summary>
        /// Delete a company from the database only if it exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the result of the operation</returns>
        public HttpResponseMessage DeleteCompany(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings.Get(Constants.API_URL));
                var responseTask = client.DeleteAsync(string.Format("company/{0}", id));
                responseTask.Wait();

                HttpResponseMessage result = responseTask.Result;
                if (!result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    result.ReasonPhrase = JObject.Parse(readTask.Result)["Message"].ToString();
                }

                return result;
            }
        }

        /// <summary>
        /// Get all the companies stored in the database
        /// </summary>
        /// <returns>List of companies</returns>
        public List<Company> GetAll()
        {
            List<Company> companies = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings.Get(Constants.API_URL));
                var responseTask = client.GetAsync("company");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Company>>();
                    readTask.Wait();
                    companies = readTask.Result;
                }
            }
            return companies;
        }

        /// <summary>
        /// Retrieve a company searching by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Company</returns>
        public Company GetCompanyById(int id)
        {
            Company company = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings.Get(Constants.API_URL));
                var responseTask = client.GetAsync(string.Format("company/{0}", id));
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Company>();
                    readTask.Wait();
                    company = readTask.Result;
                }
            }
            return company;
        }

        /// <summary>
        /// Update a company only if it exists in the database
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Returns the result of the operation</returns>
        public HttpResponseMessage UpdateCompany(Company data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings.Get(Constants.API_URL));
                var responseTask = client.PutAsJsonAsync(string.Format("company/{0}", data.Id), data);
                responseTask.Wait();

                HttpResponseMessage result = responseTask.Result;
                if (!result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    result.ReasonPhrase = JObject.Parse(readTask.Result)["Message"].ToString();
                }

                return result;
            }
        }
    }
}