using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Models
{
    static class ExceptionForMail
    {
        static public List<myException> ExceptionList = new List<myException>(); 
    }

    class myException
    {
        public string file;
        public int ex_catch_row;
        public string ex_mes;
        public string ex_method;
        public myException(string f,int row,string mes, string method )
        {
            file = f; ex_catch_row = row;ex_mes = mes; ex_method = method;
        }

        public override string ToString()
        {
            return $"{file} {ex_catch_row} : {ex_mes} \n in method {ex_method}";
        }
    }

}
