using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainings2.Models
{
    public class Person
    {
        public int personid { get; set; }
        public string login { get; set; }
        public string name { get; set; }
        public string sname { get; set; }
        public int phone { get; set; }
        public char sex { get; set; }
        public int age { get; set; }

        public Person()
        {
        }

        public Person(int personid, string login, string name, string sname, int phone, char sex, int age)
        {
            this.personid = personid;
            this.login = login ?? throw new ArgumentNullException(nameof(login));
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.sname = sname ?? throw new ArgumentNullException(nameof(sname));
            this.phone = phone;
            this.sex = sex;
            this.age = age;
        }
    }
}
