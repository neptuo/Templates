using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleNG
{
    public class PersonModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public AddressModel Address { get; set; }

        public PersonModel(string firstname, string lastname, AddressModel address)
        {
            Firstname = firstname;
            Lastname = lastname;
            Address = address;
        }
    }

    public class AddressModel
    {
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public string City { get; set; }
        public int PostalCode { get; set; }

        public AddressModel(string street, int houseNumber, string city, int postalCode)
        {
            Street = street;
            HouseNumber = houseNumber;
            City = city;
            PostalCode = postalCode;
        }
    }
}
