using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement1.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;
        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
                {
                    new Employee(){Id=1,Name="Jose",Email="Jose@gmail.com",Department=Dept.HR},
                      new Employee(){Id=2,Name="Teena",Email="teena@gmail.com",Department=Dept.IT},
                        new Employee(){Id=3,Name="Mary",Email="mary@gmail.com",Department=Dept.IT}
                };
        }

        public Employee AddEmployee(Employee employee)
        {
           employee.Id =_employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;


        }

        public Employee Delete(int id)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == id);
            if(employee!=null)
            {
                _employeeList.Remove(employee);
            }

            return employee;
          
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int id)
        {
            return _employeeList.FirstOrDefault(emp => emp.Id == id);
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }
            return employee;
        }
    }
}
