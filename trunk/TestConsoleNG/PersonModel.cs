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

        public PersonModel(string firstname, string lastname)
        {
            Firstname = firstname;
            Lastname = lastname;
        }
    }
}
