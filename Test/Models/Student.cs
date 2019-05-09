using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Student
    {
        string lname;
        string fname;
        string sname;
        int age;
        string group;
        string phone;
        string mail;
        DateTime DT_Start;
        DateTime DT_End;
        public Student()        { DT_Start = DateTime.Now;      }

        public string Lname { get => lname; set => lname = value; }
        public string Fname { get => fname; set => fname = value; }
        public string Sname { get => sname; set => sname = value; }
        public int Age { get => age; set => age = value; }
        public string Group { get => group; set => group = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Mail { get => mail; set => mail = value; }
        public DateTime Start { get => DT_Start;  }
        public DateTime End { get => DT_End; set => DT_End = value; }

        public override string ToString()
        {
            return $"{lname} {fname} {sname}";
        }
    }
}
