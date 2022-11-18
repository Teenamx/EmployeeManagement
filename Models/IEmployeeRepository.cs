using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement1.Models
{
   public interface IEmployeeRepository
    {
        Employee GetEmployee(int id);
        IEnumerable<Employee> GetAllEmployees();

        Employee AddEmployee(Employee employeee);

        Employee Update(Employee employeeChanges);

        Employee Delete(int id);

    }
}
