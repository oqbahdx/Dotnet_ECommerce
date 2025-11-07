using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Address
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Country { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string Street { get; private set; } = string.Empty;
        public string PostalCode { get; private set; } = string.Empty;

        public bool IsDefault { get; private set; }

        // Relation
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;

        private Address() { }

        public Address(string country, string city, string street, string postalCode, bool isDefault = false)
        {
            Country = country;
            City = city;
            Street = street;
            PostalCode = postalCode;
            IsDefault = isDefault;
        }
    }
}
