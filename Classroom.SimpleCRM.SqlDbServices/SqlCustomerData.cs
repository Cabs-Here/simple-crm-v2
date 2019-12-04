using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Classroom.SimpleCRM.SqlDbServices
{
    public class SqlCustomerData : ICustomerData
    {
        private readonly CrmDbContext context;
        private ColumnMapping mappingCustomer;

        public SqlCustomerData(CrmDbContext context)
        {
            this.context = context;
            mappingCustomer = new ColumnMapping(new[] {
                new ColumnMappingValue("FirstName"),
                new ColumnMappingValue("LastName"),
                new ColumnMappingValue("Name", new[] { "LastName", "FirstName" }), //custom sort option
                new ColumnMappingValue("EmailAddress"),
                new ColumnMappingValue("PhoneNumber"),
                new ColumnMappingValue("Status"),
                new ColumnMappingValue("LastContactDate")
            }); //TODO: create an overload to build base mappings using reflection and allow custom additions
        }

        public Customer Get(int customerId)
        {
            return context.Customer.FirstOrDefault(x => x.CustomerId == customerId);
        }
        public List<Customer> Get(int accountId, CustomerListParameters options)
        {
            var sortedResults = context.Customer
                //TODO: after auth is added .Where(x => x.AccountId == accountId)
                .ApplySort(options.OrderBy, mappingCustomer);

            if (!string.IsNullOrWhiteSpace(options.LastName))
            {
                sortedResults = sortedResults
                    .Where(x => x.LastName.ToLowerInvariant() == options.LastName.Trim().ToLowerInvariant());
            }
            // add other where clauses when more filters are added.
            if (!string.IsNullOrWhiteSpace(options.Term))
            {
                sortedResults = sortedResults
                    .Where(x => (x.FirstName + " " + x.LastName).ToLowerInvariant().Contains(options.Term.ToLowerInvariant())
                        || x.EmailAddress.ToLowerInvariant().Contains(options.Term.ToLowerInvariant()));
            }

            return sortedResults.Skip((options.Page - 1) * options.Take)
                .Take(options.Take)
                .ToList();
        }
        public void Add(Customer item)
        {
            context.Add(item); //not inserted to database yet, just pending insert
        }
        public void Update(Customer item)
        {
            //do nothing, changes are tracked automatically
        }
        public void Delete(Customer item)
        {
            context.Remove(item);
        }
        public void Commit()
        {
            context.SaveChanges();
        }
    }
}