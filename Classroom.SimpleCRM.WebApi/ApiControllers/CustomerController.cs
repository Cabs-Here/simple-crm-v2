using System;
using System.Linq;
using Classroom.SimpleCRM.WebApi.Filters;
using Classroom.SimpleCRM.WebApi.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;

namespace Classroom.SimpleCRM.WebApi.ApiControllers
{
    [Route("api/customers")]
    [Authorize(Policy = "ApiUser")]
    public class CustomerController : Controller
    {
        private readonly ICustomerData _customerData;
        private readonly IUrlHelper _urlHelper;

        public CustomerController(ICustomerData customerData,
            IUrlHelper urlHelper)
        {
            _customerData = customerData;
            _urlHelper = urlHelper;
        }
        /// <summary>
        /// Gets all customers visible in the account of the current user
        /// </summary>
        /// <returns></returns>
        
        [HttpGet("", Name = "GetCustomers")] //  ./api/customers
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
        public IActionResult GetCustomers([FromQuery]CustomerListParameters resourceParameters)
        {
            if (resourceParameters.Page < 1)
            {
                resourceParameters.Page = 1; //0 is default when not specified
            }

            if (resourceParameters.Take == 0)
            {
                resourceParameters.Take = 50; //default when value is not specified.
            }
            else if (resourceParameters.Take > 250)
            {
                return new ValidationFailedResult("A request can only take maximum of 250 items.");
            }

            var customers = _customerData.Get(0, resourceParameters);

            var pagination = new PaginationModel
            {
                Previous = CreateCustomersResourceUri(resourceParameters, -1),
                Next = CreateCustomersResourceUri(resourceParameters, 1),
                Take = resourceParameters.Take,
                Page = resourceParameters.Page
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagination));
            

            var models = customers.Select(c => new CustomerDisplayViewModel(c));
            return Ok(models);
        }
        private string CreateCustomersResourceUri(CustomerListParameters resourceParameters, int pageAdjust)
        {
            if (resourceParameters.Page + pageAdjust <= 0)
                return null;
            return _urlHelper.Link("GetCustomers",
                new
                {
                    take = resourceParameters.Take,
                    page = resourceParameters.Page + pageAdjust,
                    orderBy = resourceParameters.OrderBy,
                    lastName = resourceParameters.LastName,
                    term = resourceParameters.Term
                });
        } 

        /// <summary>
        /// Retrieves a single customer by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")] //  ./api/customers/:id
        [HttpGet]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
        public IActionResult Get(int id)
        {
            var customer = _customerData.Get(id);
            if (customer == null)
            {
                return NotFound();
            }
            var model = new CustomerDisplayViewModel(customer);
            Response.Headers.Add("ETag", "\"" + customer.LastContactDate.ToString() + "\"");
            return Ok(model);
            
        }
        [HttpPost("")] //  ./api/customers
        public IActionResult Create([FromBody]CustomerCreateViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {   //rules failed, return a well formed error
                return new ValidationFailedResult(ModelState);
            }

            var customer = new Customer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                PreferredContactMethod = model.PreferredContactMethod
                LastContactDate = DateTime.UtcNow
            };

            _customerData.Add(customer);
            _customerData.Commit();
            Response.Headers.Add("ETag", "\"" + customer.LastContactDate.ToString() + "\"");
            return Ok(new CustomerDisplayViewModel(customer)); //includes new auto-assigned id
        }
        [HttpPut("{id}")] //  ./api/customers/:id
        public IActionResult Update(int id, [FromBody]CustomerUpdateViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {   //rules failed, return a well formed error
                return new ValidationFailedResult(ModelState);
            }

            var customer = _customerData.Get(id);
            if (customer == null)
            {
                return NotFound();
            }

            string ifMatch = Request.Headers["If-Match"];
            if (ifMatch != customer.LastContactDate.ToString())
            {
                return StatusCode(422, "This record has been updated by another user. Please refresh and try again.");
            }

            //update only editable properties from model
            customer.EmailAddress = model.EmailAddress;
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.PhoneNumber = model.PhoneNumber;
            customer.PreferredContactMethod = model.PreferredContactMethod;
            customer.LastContactDate = DateTime.UtcNow;

            _customerData.Update(customer);
            _customerData.Commit();
            return Ok(customer); //server version, updated per request
        }
        [HttpDelete("{id}")] //  ./api/customers/:id
        public IActionResult Delete(int id)
        {
            var customer = _customerData.Get(id);
            if (customer == null)
            {
                return NotFound();
            }

            _customerData.Delete(customer);
            _customerData.Commit();
            return NoContent();
        }
    }
}