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
    public List<Job> Jobs { get; set; } = new List<Job>();
    public List<Salary> Salaries { get; set; } = new List<Salary>();

}

class Job
{
    public string JobTitle { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Department JobDepartment { get; set; }


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
            Console.WriteLine("--------------------------");
            Console.WriteLine("2. Удалить сотрудника");
            Console.WriteLine("--------------------------");
            Console.WriteLine("3. Поиск сотрудника по фамилии");
            Console.WriteLine("--------------------------");
            Console.WriteLine("4. Трудовая история сотрудника");
            Console.WriteLine("--------------------------");
            Console.WriteLine("5. История зарплаты сотрудника");
            Console.WriteLine("--------------------------");
            Console.WriteLine("6. Заработная плата сотрудника");
            Console.WriteLine("--------------------------");
            Console.WriteLine("7. Экспорт в XML-файл");
            Console.WriteLine("--------------------------");
            Console.WriteLine("8. Показать всех сотрудников");
            Console.WriteLine("--------------------------");
            Console.WriteLine("9. Сотрудники, работающие в нескольких отделах");
            Console.WriteLine("--------------------------");
            Console.WriteLine("10. Года с наибольшим и наименьшим числом приема и увольнения сотрудников");
            Console.WriteLine("--------------------------");
            Console.WriteLine("11. Сотрудники с юбилеем в этом году");
            Console.WriteLine("--------------------------");
            Console.WriteLine("0. Выход");
            Console.WriteLine("--------------------------");
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
                    DisplaySalaryHistory();
                    break;
                case 6:
                    EmployeePayroll();
                    break;
                case 7:
                    ExportToXml();
                    break;
                case 8:
                    ShowAllEmployees();
                    break;
                case 9:
                    EmployeesInMultipleDepartments();
                    break;
                case 10:
                    YearsWithMostAndFewestEmployees();
                    break;
                case 11:
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


    private static int GetEmployeeCountByDepartment(string department)
    {
        return employees.Count(e => e.Jobs.Any(j => j.JobDepartment.ToString() == department));
    }

    private static int GetYoungEmployeeCountByDepartment(string department)
    {
        int currentYear = DateTime.Today.Year;
        return employees.Count(e => e.Jobs.Any(j => j.JobDepartment.ToString() == department) && (currentYear - e.DateOfBirth.Year) < 30);
    }


    private static void AddEmployee()
    {
        Console.WriteLine("Введите порядковый номер сотрудника, которого Вы хотите добавить:");
        string id = Console.ReadLine();

        if (employees.Any(e => e.ID == id))
        {
            Console.WriteLine("Сотрудник с таким номером уже существует.");
            return;
        }

        Console.WriteLine("Введите его имя:");
        string firstName = Console.ReadLine();

        Console.WriteLine("Введите его фамилию:");
        string lastName = Console.ReadLine();

        DateTime dateOfBirth = DateTime.MinValue;

        while (dateOfBirth == DateTime.MinValue)
        {
            Console.WriteLine("Введите дату рождения сотрудника (в формате гггг-мм-дд):");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime dob))
            {
                dateOfBirth = dob;
            }
            else
            {
                Console.WriteLine("Неверный формат даты. Попробуйте ещё разок.");
            }
        }

        Console.WriteLine("Введите отдел сотрудника (IT, Logistics, Management, Supply):");
        string departmentInput = Console.ReadLine();

        if (!Enum.TryParse<Job.Department>(departmentInput, out Job.Department department))
        {
            Console.WriteLine("Неверный отдел. Пожалуйста, выберите из предложенных вариантов.");
            return;
        }

        DateTime hireDate = DateTime.Today;

        Employee newEmployee = new Employee
        {
            ID = id,
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = dateOfBirth
        };

        Job newJob = new Job
        {
            JobTitle = "Default Job Title",
            StartDate = hireDate,
            EndDate = DateTime.MaxValue,
            JobDepartment = department
        };

        newEmployee.Jobs.Add(newJob);

        employees.Add(newEmployee);

        Console.WriteLine($"Сотрудник {firstName} {lastName} успешно добавлен. Дата приема на работу: {hireDate:dd.MM.yyyy}");
    }






    private static void DeleteEmployee()
    {
        Console.WriteLine("Введите номер сотрудника для удаления его из системы:");
        string idToDelete = Console.ReadLine();

        Employee employeeToDelete = employees.FirstOrDefault(e => e.ID == idToDelete);

        if (employeeToDelete != null)
        {
            employees.Remove(employeeToDelete);
            Console.WriteLine($"Сотрудник - {employeeToDelete.FirstName} {employeeToDelete.LastName} успешно удалён из системы.");
        }
        else
        {
            Console.WriteLine("Сотрудник не найден.");
        }
    }

    private static void SearchEmployee()
    {
        Console.WriteLine("Введите фамилию сотрудника, чтобы начать поиск:");
        string lastName = Console.ReadLine();

        Employee foundEmployee = employees.FirstOrDefault(e => e.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase));

        Console.WriteLine("--------------------------------------------------");

        if (foundEmployee != null)
        {
            Console.WriteLine($"Сотрудник: {foundEmployee.FirstName} {foundEmployee.LastName}");

            var departments = foundEmployee.Jobs.Select(j => j.JobDepartment.ToString());
            string departmentList = string.Join(", ", departments);

            Console.WriteLine($"Отдел(ы): {departmentList}");
            Console.WriteLine($"Дата рождения: {foundEmployee.DateOfBirth:yyyy-MM-dd}");
        }
        else
        {
            Console.WriteLine("Сотрудники не найдены.");
        }

        Console.WriteLine("--------------------------------------------------");
    }




    private static void EmployeeWorkHistory()
    {
        Console.WriteLine("Введите номер сотрудника для отображения его трудовой истории:");
        string employeeId = Console.ReadLine();

        Employee employee = employees.FirstOrDefault(e => e.ID == employeeId);

        if (employee != null)
        {
            Console.WriteLine($"Работа - {employee.FirstName} {employee.LastName}:");

            foreach (var job in employee.Jobs)
            {
                string endDate = (job.EndDate == DateTime.MaxValue) ? "Present" : job.EndDate.ToString("dd.MM.yyyy");
                Console.WriteLine($"{job.JobDepartment} => {job.StartDate:dd.MM.yyyy}-{endDate}");
            }

            Console.WriteLine("Введите названиие отдела для изменения (или введите 'exit' для выхода):");
            string departmentName = Console.ReadLine();

            if (departmentName.ToLower() != "exit")
            {
                var selectedJob = employee.Jobs.FirstOrDefault(j => j.JobDepartment.ToString().ToLower(CultureInfo.InvariantCulture) == departmentName.ToLower(CultureInfo.InvariantCulture));

                if (selectedJob != null)
                {
                    Console.WriteLine($" Вы выбрали {selectedJob.JobDepartment} => {selectedJob.StartDate:dd.MM.yyyy}-{(selectedJob.EndDate == DateTime.MaxValue ? "Present" : selectedJob.EndDate.ToString("dd.MM.yyyy"))}");

                    Console.WriteLine("Выберите, что вы хотите изменить:");
                    Console.WriteLine("1. Изменить дату начала работы");
                    Console.WriteLine("2. Изменить дату окончания работы");
                    Console.WriteLine("3. Изменить и дату начала, и дату окончания работы");
                    Console.WriteLine("4. Выход без изменения");
                    int modifyChoice = int.Parse(Console.ReadLine());

                    switch (modifyChoice)
                    {
                        case 1:
                            Console.WriteLine("Введите новую дату начала (yyyy-MM-dd):");
                            DateTime newStartDate = DateTime.Parse(Console.ReadLine());
                            selectedJob.StartDate = newStartDate;
                            Console.WriteLine("История работы успешно сохранена.");
                            break;
                        case 2:
                            Console.WriteLine("Введите новую дату окончания (yyyy-MM-dd):");
                            DateTime newEndDate = DateTime.Parse(Console.ReadLine());
                            selectedJob.EndDate = newEndDate;
                            Console.WriteLine("История работы успешно сохранена.");
                            break;
                        case 3:
                            Console.WriteLine("Введите новую дату начала (yyyy-MM-dd):");
                            newStartDate = DateTime.Parse(Console.ReadLine());
                            Console.WriteLine("Введите новую дату окончания (yyyy-MM-dd):");
                            newEndDate = DateTime.Parse(Console.ReadLine());
                            selectedJob.StartDate = newStartDate;
                            selectedJob.EndDate = newEndDate;
                            Console.WriteLine("История работы успешно сохранена.");
                            break;
                        case 4:
                            Console.WriteLine("Выход без изменения.");
                            break;
                        default:
                            Console.WriteLine("Неверный выбор. Выход без изменений.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Неверное название отдела. У нас такого нет :) ");
                }
            }
            else
            {
                Console.WriteLine("Выход без изменения.");
            }
        }
        else
        {
            Console.WriteLine("Сотрудник не найден.");
        }
    }



    



    private static void UpdateSalaryHistory(Employee employee)
    {
        Console.WriteLine("Выберите:");
        Console.WriteLine("--------------------------");
        Console.WriteLine("1. Ввести новую зарплату");
        Console.WriteLine("--------------------------");
        Console.WriteLine("2. Удалить зарплату из истории");
        Console.WriteLine("--------------------------");
        Console.WriteLine("3. Выйти в главное меню");
        Console.WriteLine("--------------------------");

        int choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case 1:
                AddSalaryEntry(employee);
                break;
            case 2:
                RemoveSalaryEntry(employee);
                break;
            case 3:
                Console.WriteLine("Выход без изменений.");
                break;
            default:
                Console.WriteLine("Неправильный выбор. Выход без изменений .");
                break;
        }
    }

    private static void AddSalaryEntry(Employee employee)
    {
        Console.WriteLine("Введите год начала начисления:");
        int year = int.Parse(Console.ReadLine());

        Console.WriteLine("Введите месяц начала начисления:");
        int month = int.Parse(Console.ReadLine());

        Console.WriteLine("Введите сумму начисления:");
        decimal total = decimal.Parse(Console.ReadLine());

        Salary newSalary = new Salary
        {
            Year = year,
            Month = month,
            Total = total
        };

        employee.Salaries.Add(newSalary);

        Console.WriteLine($" Новая зарплата добавлена {year}-{month:00} => {total} рублей");
    }

    private static void RemoveSalaryEntry(Employee employee)
    {
        Console.WriteLine("Введите год начала начислений для удаления:");
        int yearToRemove = int.Parse(Console.ReadLine());

        Console.WriteLine("Введите месяц начала начислений для удаления:");
        int monthToRemove = int.Parse(Console.ReadLine());

        Salary salaryToRemove = employee.Salaries.FirstOrDefault(s => s.Year == yearToRemove && s.Month == monthToRemove);

        if (salaryToRemove != null)
        {
            employee.Salaries.Remove(salaryToRemove);
            Console.WriteLine($"Записи о начислениях с  {yearToRemove}-{monthToRemove:00} успешно удалены.");
        }
        else
        {
            Console.WriteLine("Начисления не найдены в эти даты.");
        }
    }


    private static void DisplaySalaryHistory()
    {
        Console.WriteLine("Введите номер сотрудника для отображения истории начислений:");
        string employeeId = Console.ReadLine();

        Employee employee = employees.FirstOrDefault(e => e.ID == employeeId);

        if (employee != null)
        {
            Console.WriteLine($"История начислений - {employee.FirstName} {employee.LastName}:");

            foreach (var salary in employee.Salaries.OrderBy(s => new DateTime(s.Year, s.Month, 1)))
            {
                Console.WriteLine($"{salary.Year}-{salary.Month:00} => {salary.Total} rubles");
            }

            Console.WriteLine("Хотите внести изменения в историю начислений? (yes/no)");
            string updateChoice = Console.ReadLine();

            if (updateChoice.ToLower() == "yes")
            {
                UpdateSalaryHistory(employee);
            }
            else
            {
                Console.WriteLine("Выход без изменений .");
            }
        }
        else
        {
            Console.WriteLine("Сотрудник не найден.");
        }
    }

    private static void EmployeePayroll()
    {
        Console.WriteLine("Введите номер сотрудника для расчёта общих начслений:");
        string employeeId = Console.ReadLine();

        Employee employee = employees.FirstOrDefault(e => e.ID == employeeId);

        if (employee != null)
        {
            Console.WriteLine($" Общие - {employee.FirstName} {employee.LastName}:");

            Console.WriteLine("Введите год начала:");
            int startYear = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите конечный год:");
            int endYear = int.Parse(Console.ReadLine());

            decimal totalPayroll = 0;

            foreach (var salary in employee.Salaries.Where(s => s.Year >= startYear && s.Year <= endYear))
            {
                totalPayroll += salary.Total;
            }

            Console.WriteLine($"Общая сумма за период {startYear}-{endYear}: {totalPayroll} рублей");

            Console.WriteLine("Хотите изменить историю начислений? (yes/no)");
            string updateChoice = Console.ReadLine();

            if (updateChoice.ToLower() == "yes")
            {
                UpdateSalaryHistory(employee);
            }
            else
            {
                Console.WriteLine("Выход без изменений.");
            }
        }
        else
        {
            Console.WriteLine("Сотрудник не найден.");
        }
    }

    private static void ExportToXml()
    {
        XElement xmlDepartments = new XElement("Отделы");

        foreach (Job.Department department in Enum.GetValues(typeof(Job.Department)))
        {
            XElement xmlDepartment = new XElement("Отдел",
                new XAttribute("Название", department),
                new XElement("Количество_работающих_сотрудников", GetEmployeeCountByDepartment(department.ToString())),
                new XElement("Количество_работающих_сотрудников_молодежь", GetYoungEmployeeCountByDepartment(department.ToString()))
            );

            xmlDepartments.Add(xmlDepartment);
        }

        Console.WriteLine("Enter the file path to save the XML file:");
        string filePath = Console.ReadLine();

        XDocument xmlDoc = new XDocument(xmlDepartments);

        try
        {
            xmlDoc.Save(filePath);
            Console.WriteLine($"Data exported to XML file: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving XML file: {ex.Message}");
        }
    }



    private static void ShowAllEmployees()
    {
        Console.WriteLine("Все сотрудники:");
        Console.WriteLine("--------------------------");

        foreach (var employee in employees)
        {
            string departmentList = string.Join(", ", employee.Jobs.Select(j => j.JobDepartment));
            Console.WriteLine($"{employee.FirstName} {employee.LastName} => {departmentList}");
        }

        Console.WriteLine("--------------------------");
    }


    private static void EmployeesInMultipleDepartments()
    {
        var employeesInMultipleDepartments = employees
            .Where(e => e.Jobs.GroupBy(j => j.JobDepartment).Count() > 1)
            .ToList();

        Console.WriteLine("Сотрудники работающие более чем в одном отделе:");

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

        Console.WriteLine($"Год с самым большим набором: {maxHiringYear}");
        Console.WriteLine($"Год с самым большим числом увольнений: {minDismissalYear}");
    }

    private static void EmployeesWithAnniversary()
    {
        Console.WriteLine("Введите текущий год:");
        int currentYear = int.Parse(Console.ReadLine());

        var employeesWithAnniversary = employees
            .Where(e => (currentYear - e.DateOfBirth.Year) % 5 == 0)
            .ToList();

        Console.WriteLine("Сотрудники-юбиляры:");

        foreach (var employee in employeesWithAnniversary)
        {
            int yearsSinceBirth = currentYear - employee.DateOfBirth.Year;
            Console.WriteLine($"{employee.FirstName} {employee.LastName} => {yearsSinceBirth} лет");
        }
    }
}
