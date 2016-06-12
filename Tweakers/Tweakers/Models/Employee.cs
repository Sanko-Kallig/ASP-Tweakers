using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tweakers.Models
{
    public class Employee : Account
    {
        public string EmployeeFunction { get; set; }

        public List<string> Skills { get; set; }

        public Employee(string employeeFunction, List<string> skills)
        {
            EmployeeFunction = employeeFunction;
            Skills = skills;
        }
    }
}