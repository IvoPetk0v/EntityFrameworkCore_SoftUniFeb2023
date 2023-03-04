using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var context = new SoftUniContext();

            // Console.WriteLine(GetEmployeesFullInformation(context));

            // Console.WriteLine(GetEmployeesWithSalaryOver50000(context));

            // Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));

            // Console.WriteLine(AddNewAddressToEmployee(context));

            // Console.Write(GetEmployeesInPeriod(context));

            // Console.WriteLine(GetAddressesByTown(context));

            // Console.WriteLine(GetEmployee147(context));

            // Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));

            // Console.WriteLine(GetLatestProjects(context));

            // Console.WriteLine(IncreaseSalaries(context));

            // Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));

            // Console.WriteLine(DeleteProjectById(context));

            // Console.WriteLine(RemoveTown(context));
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                 .OrderBy(e => e.EmployeeId)
                 .Select(e => new
                 {
                     e.FirstName,
                     e.LastName,
                     e.MiddleName,
                     e.JobTitle,
                     e.Salary
                 })
                 .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ToArray();
            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    Department = e.Department.Name,
                    e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.Department} - ${e.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            Employee? employee = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");

            employee!.Address = newAddress;

            context.SaveChanges();

            var employeeAddresses = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address!.AddressText)
                .ToArray();

            return String.Join(Environment.NewLine, employeeAddresses);
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employeesWithProjects = context.Employees
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager!.FirstName,
                    ManagerLastName = e.Manager!.LastName,
                    Projects = e.EmployeesProjects
                        .Where(ep => ep.Project.StartDate.Year >= 2001 &&
                                     ep.Project.StartDate.Year <= 2003)
                        .Select(ep => new
                        {
                            ProjectName = ep.Project.Name,
                            StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                            EndDate = ep.Project.EndDate.HasValue
                                ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                                : "not finished"
                        })
                        .ToArray()
                })
                .ToArray();

            foreach (var e in employeesWithProjects)
            {
                sb
                    .AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                foreach (var p in e.Projects)
                {
                    sb
                        .AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var addresses = context.Addresses
                 .Select(a => new
                 {
                     a.AddressText,
                     TownName = a.Town!.Name,
                     EmployeeCount = a.Employees.Count
                 })
               .OrderByDescending(a => a.EmployeeCount)
               .ThenBy(a => a.TownName)
               .ThenBy(a => a.AddressText)
               .Take(10)
               .ToArray();

            foreach (var a in addresses)
            {
                sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeeCount} employees");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employeeId = 147;

            var employeeById = context.Employees
                .Where(e => e.EmployeeId == employeeId)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    EmployeeProjectsName = e.EmployeesProjects
                    .Select(ep => ep.Project.Name)
                    .OrderBy(a => a)
                    .ToArray()
                })
                .ToArray().First();

            string employeeProjects = String.Join(Environment.NewLine, employeeById.EmployeeProjectsName);

            sb.AppendLine($"{employeeById.FirstName} {employeeById.LastName} - {employeeById.JobTitle}");
            sb.AppendLine(employeeProjects);

            return sb.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var departaments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    d.Name,
                    depManagerFirstName = d.Manager.FirstName,
                    depManagerLastName = d.Manager.LastName,
                    depEmployees = d.Employees
                    .Select(e => e)
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToArray()
                })
                .ToArray();

            foreach (var d in departaments)
            {
                sb.AppendLine($"{d.Name} - {d.depManagerFirstName}  {d.depManagerLastName}");

                foreach (var e in d.depEmployees)
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    ProjectStartDate = p.StartDate
                    .ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                })
                .ToArray();

            foreach (var p in projects)
            {
                sb.AppendLine(p.Name);
                sb.AppendLine(p.Description);
                sb.AppendLine(p.ProjectStartDate);
            }

            return sb.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context.Employees
                .Include(x => x.Department)
                .Where(x => x.Department.Name == "Engineering" ||
                            x.Department.Name == "Tool Design" ||
                            x.Department.Name == "Marketing" ||
                            x.Department.Name == "Information Services")
                .ToList();

            foreach (var employee in employees)
            {
                employee.Salary *= 1.12m;
            }

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees.OrderBy(x => x.FirstName).ThenBy(x => x.LastName))
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                 .Where(e => e.FirstName.Substring(0, 2).ToLower() == "sa")
                 .Select(e => new
                 {
                     e.FirstName,
                     e.LastName,
                     e.JobTitle,
                     e.Salary
                 })
                 .OrderBy(e => e.FirstName)
                 .ThenBy(e => e.LastName)
                 .ToList();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var projectForDel = context.Projects.Find(2);

            var epForDel = context.EmployeesProjects
                   .Where(ep => ep.ProjectId == 2);

            context.EmployeesProjects.RemoveRange(epForDel);
            context.Projects.Remove(projectForDel!);
            context.SaveChanges();

            var projectsName = context.Projects
                .Take(10)
                .Select(p => p.Name)
                .ToArray();
            return String.Join(Environment.NewLine, projectsName);
        }

        public static string RemoveTown(SoftUniContext context)
        {

            var addressesInSeatle = context.Addresses
                .Where(t => t.Town!.Name == "Seattle")
                .ToArray();
            var addressesForDel = context.Addresses
                 .Where(t => t.Town.Name == "Seattle");

            foreach (var e in context.Employees)
            {
                if (addressesInSeatle.Any(a => a.AddressId == e.AddressId))
                {
                    e.AddressId = null;
                }
            }

            context.Addresses.RemoveRange(addressesForDel);
            context.Towns.Remove(context.Towns.First(t => t.Name == "Seattle"));
            context.SaveChanges();

            return $"{addressesInSeatle.Count()} addresses in Seattle were deleted";
        }

    }
}