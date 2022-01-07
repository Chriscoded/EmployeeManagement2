using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _EmployeeList;

        public MockEmployeeRepository()
        {
            _EmployeeList = new List<Employee>()
            {
                new Employee{Id = 1, Name = "Christopher", Email = "amaemechris@gmail.com", Department = Dept.HR},
                new Employee{Id = 2, Name = "Ben", Email = "ben@gmail.com", Department = Dept.IT},
                new Employee{Id = 3, Name = "Jude", Email = "jude@gmail.com", Department = Dept.Payroll}
            };
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return _EmployeeList;
        }

        public Employee GetEmployee(int Id)

        {
            return _EmployeeList.FirstOrDefault(e => e.Id == Id);
        }

        public Employee Add(Employee employee)
        {
           employee.Id =  _EmployeeList.Max(e => e.Id) + 1;
            _EmployeeList.Add(employee);
            return employee;
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employee = _EmployeeList.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = _EmployeeList.FirstOrDefault(e => e.Id == id);
            if(employee != null)
            {
                _EmployeeList.Remove(employee);
            }
            return employee;
        }
    }
}
