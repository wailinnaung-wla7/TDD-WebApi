using CloudCustomers.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCustomers.UnitTests.Fixtures
{
    public static class UserFixture
    {
        public static List<User> GetTestUsers() => new() {
        new User {
            Id = 1,
            Name = "Test",
            Address =  new Address {
                Street = "123 Market St",
                City = "Singapore",
                ZipCode = "213123"
            }
        },
        new User {
            Id = 2,
            Name = "John Doe",
            Address =  new Address {
                Street = "456 Main St",
                City = "New York",
                ZipCode = "10001"
            }
        },
        new User {
            Id = 3,
            Name = "Jane Smith",
            Address =  new Address {
                Street = "789 Oak St",
                City = "Los Angeles",
                ZipCode = "90001"
            }
        }
    };
    }

}
