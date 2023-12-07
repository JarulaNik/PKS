using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

class Employee
{
    public string ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public List<Job> Jobs { get; set; } = [];
    public List<Salary> Salaries { get; set; } = [];
}

class Job
{
    public string JobTitle { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Department JobDepartment { get; set; } // Now the department is represented as an enum

    // Updated enum for departments
    public enum Department
    {
        IT,
        Logistics,
        Management,
        Supply
    }
}

class Salary
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal Total { get; set; }
}

class Program
{
    private static List<Employee> employees = new List<Employee>();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("1. Добавить сотрудника");
            Console.WriteLine("2. Удалить сотрудника");
            Console.WriteLine("3. Поиск сотрудника по фамилии");
            Console.WriteLine("4. Трудовая история сотрудника");
            Console.WriteLine("5. Изменение дат начала и окончания работы сотрудника");
            Console.WriteLine("6. История зарплаты сотрудника");
            Console.WriteLine("7. Заработная плата сотрудника");
            Console.WriteLine("8. Экспорт в XML-файл");
            Console.WriteLine("9. Показать всех сотрудников");
            Console.WriteLine("10. Сотрудники, работающие в нескольких отделах");
            Console.WriteLine("11. Года с наибольшим и наименьшим числом приема и увольнения сотрудников");
            Console.WriteLine("12. Сотрудники с юбилеем в этом году");
            Console.WriteLine("0. Выход");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AddEmployee();
                    break;
                case 2:
                    DeleteEmployee();
                    break;
                case 3:
                    SearchEmployee();
                    break;
                case 4:
                    EmployeeWorkHistory();
                    break;
                case 5:
                    ChangeWorkDates();
                    break;
                case 6:
                    DisplaySalaryHistory();
                    break;
                case 7:
                    EmployeePayroll();
                    break;
                case 8:
                    ExportToXml();
                    break;
                case 9:
                    ShowAllEmployees();
                    break;
                case 10:
                    EmployeesInMultipleDepartments();
                    break;
                case 11:
                    YearsWithMostAndFewestEmployees();
                    break;
                case 12:
                    EmployeesWithAnniversary();
                    break;
                case 0:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте еще раз.");
                    break;
            }
        }
    }

    private static void AddEmployee()
    {
        Console.WriteLine("Enter employee ID:");
        string id = Console.ReadLine();

        if (employees.Any(e => e.ID == id))
        {
            Console.WriteLine("Employee with this ID already exists.");
            return;
        }

        Console.WriteLine("Enter employee first name:");
        string firstName = Console.ReadLine();

        Console.WriteLine("Enter employee last name:");
        string lastName = Console.ReadLine();

        Console.WriteLine("Enter employee date of birth (yyyy-MM-dd):");
        DateTime dateOfBirth = DateTime.Parse(Console.ReadLine());

        Employee newEmployee = new Employee
        {
            ID = id,
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = dateOfBirth
        };

        Console.WriteLine("Choose the department where the employee will work (IT, Logistics, Management, Supply):");
        string departmentInput = Console.ReadLine();

        if (Enum.TryParse<Job.Department>(departmentInput, true, out var selectedDepartment))
        {
            Job newJob = new Job
            {
                JobDepartment = selectedDepartment
            };

            newEmployee.Jobs.Add(newJob);
            employees.Add(newEmployee);

            Console.WriteLine($"Employee {firstName} {lastName} added successfully to the {selectedDepartment} department.");
        }
        else
        {
            Console.WriteLine("Invalid department choice. Employee not added.");
        }
    }


    private static void DeleteEmployee()
    {
        Console.WriteLine("Enter the ID of the employee to delete:");
        string idToDelete = Console.ReadLine();

        Employee employeeToDelete = employees.FirstOrDefault(e => e.ID == idToDelete);

        if (employeeToDelete != null)
        {
            employees.Remove(employeeToDelete);
            Console.WriteLine($"Employee {employeeToDelete.FirstName} {employeeToDelete.LastName} deleted successfully.");
        }
        else
        {
            Console.WriteLine("Employee not found.");
        }
    }

    private static void SearchEmployee()
    {
        Console.WriteLine("Enter the last name of the employee to search:");
        string lastName = Console.ReadLine();

        Employee foundEmployee = employees.FirstOrDefault(e => e.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase));

        Console.WriteLine("--------------------------------------------------");

        if (foundEmployee != null)
        {
            Console.WriteLine($"Found employee: {foundEmployee.FirstName} {foundEmployee.LastName}");

            var departments = foundEmployee.Jobs.Select(j => j.JobDepartment.ToString());
            string departmentList = string.Join(", ", departments);

            Console.WriteLine($"Department(s): {departmentList}");
            Console.WriteLine($"Date of Birth: {foundEmployee.DateOfBirth:yyyy-MM-dd}");
        }
        else
        {
            Console.WriteLine("No employees found.");
        }

        Console.WriteLine("--------------------------------------------------");
    }




    private static void EmployeeWorkHistory()
    {
        Console.WriteLine("Enter the ID of the employee to display work history:");
        string employeeId = Console.ReadLine();

        Employee employee = employees.FirstOrDefault(e => e.ID == employeeId);

        if (employee != null)
        {
            Console.WriteLine($"Work history for {employee.FirstName} {employee.LastName}:");

            foreach (var job in employee.Jobs)
            {
                Console.WriteLine($"{job.JobDepartment} => {job.StartDate:dd/MM/yyyy}-{job.EndDate:dd/MM/yyyy}");
            }

            Console.WriteLine("Enter the name of the department to update work dates (or enter 'exit' to exit):");
            string departmentName = Console.ReadLine();

            if (departmentName.ToLower() != "exit")
            {
                var selectedJob = employee.Jobs.FirstOrDefault(j => j.JobDepartment.ToString().ToLower(CultureInfo.InvariantCulture) == departmentName.ToLower(CultureInfo.InvariantCulture));


                if (selectedJob != null)
                {
                    Console.WriteLine($"You've selected job at {selectedJob.JobDepartment} => {selectedJob.StartDate:dd/MM/yyyy}-{selectedJob.EndDate:dd/MM/yyyy}");

                    // Дальнейший код для обновления дат
                }
                else
                {
                    Console.WriteLine("Invalid department name. Exiting without updating.");
                }
            }
            else
            {
                Console.WriteLine("Exiting without updating.");
            }
        }
        else
        {
            Console.WriteLine("Employee not found.");
        }
    }

    private static void ChangeWorkDates()
    {
        Console.WriteLine("Enter the ID of the employee to change work dates:");
        string employeeId = Console.ReadLine();

        Employee employee = employees.FirstOrDefault(e => e.ID == employeeId);

        if (employee != null)
        {
            Console.WriteLine("Enter the name of the department to update work dates (or enter 'exit' to exit):");
            string departmentName = Console.ReadLine();

            if (!string.Equals(departmentName, "exit", StringComparison.OrdinalIgnoreCase))
            {
                var selectedJob = employee.Jobs.FirstOrDefault(j => j.JobDepartment != null && j.JobDepartment.ToString().ToLower(CultureInfo.InvariantCulture) == departmentName.ToLower(CultureInfo.InvariantCulture));




                if (selectedJob != null)
                {
                    Console.WriteLine($"You've selected job at {selectedJob.JobDepartment} => {selectedJob.StartDate:dd/MM/yyyy}-{selectedJob.EndDate:dd/MM/yyyy}");

                    Console.WriteLine("Enter the new start date (yyyy-MM-dd):");
                    DateTime newStartDate = DateTime.Parse(Console.ReadLine());

                    Console.WriteLine("Enter the new end date (yyyy-MM-dd):");
                    DateTime newEndDate = DateTime.Parse(Console.ReadLine());

                    selectedJob.StartDate = newStartDate;
                    selectedJob.EndDate = newEndDate;

                    Console.WriteLine("Work dates updated successfully.");
                }
                else
                {
                    Console.WriteLine("Exiting without updating. Department not found.");
                }
            }
            else
            {
                Console.WriteLine("Exiting without updating.");
            }
        }
        else
        {
            Console.WriteLine("Employee not found.");
        }
    }





    private static void DisplaySalaryHistory()
    {
        Console.WriteLine("Enter the ID of the employee to display salary history:");
        string employeeId = Console.ReadLine();

        Employee employee = employees.FirstOrDefault(e => e.ID == employeeId);

        if (employee != null)
        {
            Console.WriteLine($"Salary history for {employee.FirstName} {employee.LastName}:");

            foreach (var salary in employee.Salaries.OrderBy(s => new DateTime(s.Year, s.Month, 1)))

            {
                Console.WriteLine($"{salary.Year}-{salary.Month:00} => {salary.Total} rubles");
            }
        }
        else
        {
            Console.WriteLine("Employee not found.");
        }
    }

    private static void EmployeePayroll()
    {
        Console.WriteLine("Enter the ID of the employee to calculate payroll:");
        string employeeId = Console.ReadLine();

        Employee employee = employees.FirstOrDefault(e => e.ID == employeeId);

        if (employee != null)
        {
            Console.WriteLine($"Payroll for {employee.FirstName} {employee.LastName}:");

            Console.WriteLine("Enter the start year for the period:");
            int startYear = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the end year for the period:");
            int endYear = int.Parse(Console.ReadLine());

            decimal totalPayroll = 0;

            foreach (var salary in employee.Salaries.Where(s => s.Year >= startYear && s.Year <= endYear))
            {
                totalPayroll += salary.Total;
            }

            Console.WriteLine($"Total payroll for the period {startYear}-{endYear}: {totalPayroll} rubles");
        }
        else
        {
            Console.WriteLine("Employee not found.");
        }
    }

    private static void ExportToXml()
    {
        Console.WriteLine("Enter the file path to save the XML file:");
        string filePath = Console.ReadLine();

        XElement xmlRoot = new XElement("Employees");

        foreach (var employee in employees)
        {
            XElement xmlEmployee = new XElement("Employee",
                new XElement("ID", employee.ID),
                new XElement("FirstName", employee.FirstName),
                new XElement("LastName", employee.LastName),
                new XElement("DateOfBirth", employee.DateOfBirth.ToString("yyyy-MM-dd"))
            );

            XElement xmlJobs = new XElement("JobList",
                from job in employee.Jobs
                select new XElement("Job",
                    new XElement("JobTitle", job.JobTitle),
                    new XElement("StartDate", job.StartDate.ToString("yyyy-MM-dd")),
                    new XElement("EndDate", job.EndDate.ToString("yyyy-MM-dd")),
                    new XElement("Department", job.JobDepartment)
                )
            );

            XElement xmlSalaries = new XElement("SalaryList",
                from salary in employee.Salaries
                select new XElement("Salary",
                    new XElement("Year", salary.Year),
                    new XElement("Month", salary.Month),
                    new XElement("Total", salary.Total)
                )
            );

            xmlEmployee.Add(xmlJobs);
            xmlEmployee.Add(xmlSalaries);

            xmlRoot.Add(xmlEmployee);
        }

        XDocument xmlDoc = new XDocument(xmlRoot);
        xmlDoc.Save(filePath);

        Console.WriteLine($"Data exported to XML file: {filePath}");
    }

    private static void ShowAllEmployees()
    {
        var distinctEmployees = employees.GroupBy(e => new { e.FirstName, e.LastName })
                                        .Select(g => g.First())
                                        .ToList();

        Console.WriteLine("All employees:");

        foreach (var employee in distinctEmployees)
        {
            Console.WriteLine($"{employee.FirstName} {employee.LastName} => {string.Join(", ", employee.Jobs.Select(j => j.JobDepartment))}");
        }
    }

    private static void EmployeesInMultipleDepartments()
    {
        var employeesInMultipleDepartments = employees
            .Where(e => e.Jobs.GroupBy(j => j.JobDepartment).Count() > 1)
            .ToList();

        Console.WriteLine("Employees working in more than one department:");

        foreach (var employee in employeesInMultipleDepartments)
        {
            Console.WriteLine($"{employee.FirstName} {employee.LastName}");
        }
    }

    private static void YearsWithMostAndFewestEmployees()
    {
        var hiringYears = employees.SelectMany(e => e.Jobs.Select(j => j.StartDate.Year));
        var dismissalYears = employees.SelectMany(e => e.Jobs.Select(j => j.EndDate.Year));

        var hiringYearCounts = hiringYears.GroupBy(y => y).ToDictionary(g => g.Key, g => g.Count());
        var dismissalYearCounts = dismissalYears.GroupBy(y => y).ToDictionary(g => g.Key, g => g.Count());

        int maxHiringYear = hiringYearCounts.FirstOrDefault(x => x.Value == hiringYearCounts.Values.Max()).Key;
        int minDismissalYear = dismissalYearCounts.FirstOrDefault(x => x.Value == dismissalYearCounts.Values.Min()).Key;

        Console.WriteLine($"Year with the most employees hired: {maxHiringYear}");
        Console.WriteLine($"Year with the fewest employees dismissed: {minDismissalYear}");
    }

    private static void EmployeesWithAnniversary()
    {
        Console.WriteLine("Enter the current year:");
        int currentYear = int.Parse(Console.ReadLine());

        var employeesWithAnniversary = employees
            .Where(e => (currentYear - e.DateOfBirth.Year) % 5 == 0)
            .ToList();

        Console.WriteLine("Employees with anniversary this year:");

        foreach (var employee in employeesWithAnniversary)
        {
            int yearsSinceBirth = currentYear - employee.DateOfBirth.Year;
            Console.WriteLine($"{employee.FirstName} {employee.LastName} => {yearsSinceBirth} years");
        }
    }
}



