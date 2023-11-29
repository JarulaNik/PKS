using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EmployeeManagement
{
    class Program
    {
        static string filePath = "employees.xml";

        static void Main(string[] args)
        {
            List<Employee> employees = LoadEmployees();

            while (true)
            {
                Console.WriteLine("Система управления сотрудниками");
                Console.WriteLine("==============================");
                Console.WriteLine("1. Добавить сотрудника");
                Console.WriteLine("2. Удалить сотрудника");
                Console.WriteLine("3. Поиск сотрудника по фамилии");
                Console.WriteLine("4. История трудовой деятельности сотрудника");
                Console.WriteLine("5. Начисления заработной платы сотрудника");
                Console.WriteLine("6. Экспорт в XML файл");
                Console.WriteLine("7. Показать всех сотрудников");
                Console.WriteLine("8. Обновление зарплаты и ФИО");
                Console.WriteLine("0. Выход");
                Console.WriteLine("==============================");
                Console.Write("Введите ваш выбор: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddEmployee(employees);
                        break;
                    case "2":
                        RemoveEmployee(employees);
                        break;
                    case "3":
                        SearchEmployeeByLastName(employees);
                        break;
                    case "4":
                        ShowEmployeeWorkHistory(employees);
                        break;
                    case "5":
                        ShowEmployeeSalaryDetails(employees);
                        break;
                    case "6":
                        ExportToXml(employees);
                        break;
                    case "7":
                        DisplayAllEmployees(employees);
                        break;
                    case "8":
                        UpdateSalaryAndName(employees);
                        break;
                    case "0":
                        SaveEmployees(employees);
                        return;
                    default:
                        Console.WriteLine("Ошибка. Пожалуйста, повторите выбор.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static List<Employee> LoadEmployees()
        {
            if (File.Exists(filePath))
            {
                XDocument document = XDocument.Load(filePath);

                return document.Root.Elements("Employee").Select(e => new Employee
                {
                    Id = int.Parse(e.Element("Id").Value),
                    Name = e.Element("Name").Value,
                    Salary = decimal.Parse(e.Element("Salary").Value),
                    Department = e.Element("Department").Value,
                    WorkHistory = e.Element("WorkHistory").Elements("Job").Select(j => new Job
                    {
                        Company = j.Element("Company").Value,
                        Position = j.Element("Position").Value,
                        StartYear = int.Parse(j.Element("StartYear").Value),
                        EndYear = int.Parse(j.Element("EndYear").Value)
                    }).ToList()
                }).ToList();
            }
            else
            {
                return new List<Employee>();
            }
        }

        static void SaveEmployees(List<Employee> employees)
        {
            XDocument document = new XDocument(
                new XElement("Employees",
                    employees.Select(e =>
                        new XElement("Employee",
                            new XElement("Id", e.Id),
                            new XElement("Name", e.Name),
                            new XElement("Salary", e.Salary),
                            new XElement("Department", e.Department),
                            new XElement("WorkHistory", e.WorkHistory.Select(j =>
                                new XElement("Job",
                                    new XElement("Company", j.Company),
                                    new XElement("Position", j.Position),
                                    new XElement("StartYear", j.StartYear),
                                    new XElement("EndYear", j.EndYear)
                                )
                            ))
                        )
                    )
                )
            );

            document.Save(filePath);
        }

        static void AddEmployee(List<Employee> employees)
        {
            Console.WriteLine("Добавление сотрудника");
            Console.WriteLine("=====================");

            Console.Write("Введите ID сотрудника: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Ошибка. Введите корректное число для ID сотрудника.");
                return;
            }

            Console.Write("Введите имя сотрудника: ");
            string name = Console.ReadLine();

            Console.Write("Введите зарплату сотрудника: ");

            decimal salary;
            if (!decimal.TryParse(Console.ReadLine(), out salary))
            {
                Console.WriteLine("Ошибка. Введите корректное число для зарплаты сотрудника.");
                return;
            }

            Console.Write("Введите отдел сотрудника: ");
            string department = Console.ReadLine();

            employees.Add(new Employee
            {
                Id = id,
                Name = name,
                Salary = salary,
                Department = department,
                WorkHistory = new List<Job>()
            });

            Console.WriteLine("Сотрудник успешно добавлен.");
        }

        static void RemoveEmployee(List<Employee> employees)
        {
            Console.WriteLine("Удаление сотрудника");
            Console.WriteLine("==================");

            Console.Write("Введите ID сотрудника для удаления: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Ошибка. Введите корректное число для ID сотрудника.");
                return;
            }

            Employee employee = employees.FirstOrDefault(e => e.Id == id);

            if (employee != null)
            {
                employees.Remove(employee);
                Console.WriteLine("Сотрудник успешно удален.");
            }
            else
            {
                Console.WriteLine("Сотрудник не найден.");
            }
        }

        static void SearchEmployeeByLastName(List<Employee> employees)
        {
            Console.WriteLine("Поиск сотрудника по фамилии");
            Console.WriteLine("============================");

            Console.Write("Введите фамилию сотрудника: ");
            string lastName = Console.ReadLine();

            var searchResult = employees.FirstOrDefault(e => e.Name.EndsWith(lastName, StringComparison.OrdinalIgnoreCase));

            if (searchResult != null)
            {
                Console.WriteLine($"ID: {searchResult.Id}");
                Console.WriteLine("Имя: searchResult.Name");
                Console.WriteLine("Зарплата: {searchResult.Salary}");
                Console.WriteLine($"Отдел: {searchResult.Department}");
            }
            else
            {
                Console.WriteLine("Сотрудник не найден.");
            }
        }
        static void ShowEmployeeWorkHistory(List<Employee> employees)
        {
            Console.WriteLine("История трудовой деятельности сотрудника");
            Console.WriteLine("========================================");

            Console.Write("Введите ID сотрудника: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Ошибка. Введите корректное число для ID сотрудника.");
                return;
            }

            Employee employee = employees.FirstOrDefault(e => e.Id == id);

            if (employee != null)
            {
                Console.WriteLine($"Имя: {employee.Name}");
                Console.WriteLine("История работы:");

                if (employee.WorkHistory.Count > 0)
                {
                    foreach (var job in employee.WorkHistory.OrderBy(j => j.StartYear))
                    {
                        Console.WriteLine($"Компания: {job.Company}");
                        Console.WriteLine($"Позиция: {job.Position}");
                        Console.WriteLine($"Год начала работы: {job.StartYear}");
                        Console.WriteLine($"Год окончания работы: {job.EndYear}");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("Нет данных о трудовой деятельности.");
                }
            }
            else
            {
                Console.WriteLine("Сотрудник не найден.");
            }
        }

        static void ShowEmployeeSalaryDetails(List<Employee> employees)
        {
            Console.WriteLine("Расчет заработной платы сотрудника");
            Console.WriteLine("=================================");

            Console.Write("Введите ID сотрудника: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Ошибка. Введите корректное число для ID сотрудника.");
                return;
            }

            Employee employee = employees.FirstOrDefault(e => e.Id == id);

            if (employee != null)
            {
                Console.WriteLine($"Имя: {employee.Name}");
                Console.WriteLine("Детали заработной платы:");

                Console.Write("Введите начальную дату периода (гггг-мм-дд): ");
                DateTime startDate;
                if (!DateTime.TryParse(Console.ReadLine(), out startDate))
                {
                    Console.WriteLine("Ошибка. Введите корректную дату в формате гггг-мм-дд.");
                    return;
                }

                Console.Write("Введите конечную дату периода (гггг-мм-дд): ");
                DateTime endDate;
                if (!DateTime.TryParse(Console.ReadLine(), out endDate))
                {
                    Console.WriteLine("Ошибка. Введите корректную дату в формате гггг-мм-дд.");
                    return;
                }

                var salaryDetails = employee.WorkHistory.Where(j => j.StartYear >= startDate.Year && j.EndYear <= endDate.Year)
                                            .Select(j => j.Salary)
                                            .ToList();

                if (salaryDetails.Count > 0)
                {
                    Console.WriteLine($"Максимальная заработная плата: {salaryDetails.Max()}");
                    Console.WriteLine($"Минимальная заработная плата: {salaryDetails.Min()}");
                    Console.WriteLine($"Средняя заработная плата: {salaryDetails.Average()}");
                }
                else
                {
                    Console.WriteLine("Нет данных о заработной плате в указанном периоде.");
                }
            }
            else
            {
                Console.WriteLine("Сотрудник не найден.");
            }
        }

        static void ExportToXml(List<Employee> employees)
        {
            Console.WriteLine("Экспорт в XML файл");
            Console.WriteLine("==================");

            Console.WriteLine("Введите путь и имя файла для экспорта (с расширением): ");
            string fullPath = Console.ReadLine();

            if (File.Exists(fullPath))
            {
                Console.WriteLine("Файл с таким именем уже существует. Пожалуйста, выберите другое имя файла.");
                return;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Employee>));

                using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    serializer.Serialize(fileStream, employees);
                }

                Console.WriteLine($"Данные успешно экспортированы в файл: {fullPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при экспорте XML-файла: " + ex.Message);
            }
        }
        static void DisplayAllEmployees(List<Employee> employees)
        {
            Console.WriteLine("Список всех сотрудников");
            Console.WriteLine("======================");

            if (employees.Count > 0)
            {
                foreach (var employee in employees)
                {
                    Console.WriteLine($"ID: {employee.Id}");
                    Console.WriteLine($"Имя: {employee.Name}");
                    Console.WriteLine($"Зарплата: {employee.Salary}");
                    Console.WriteLine($"Отдел: {employee.Department}");
                    Console.WriteLine("======================");
                }
            }
            else
            {
                Console.WriteLine("Нет данных о сотрудниках.");
            }
        }

        static void UpdateSalaryAndName(List<Employee> employees)
        {
            Console.WriteLine("Обновление зарплаты и ФИО сотрудника");
            Console.WriteLine("==================================");

            Console.Write("Введите ID сотрудника: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Ошибка. Введите корректное число для ID сотрудника.");
                return;
            }

            Employee employee = employees.FirstOrDefault(e => e.Id == id);

            if (employee != null)
            {
                Console.Write("Введите новую зарплату сотрудника: ");
                decimal newSalary;
                if (!decimal.TryParse(Console.ReadLine(), out newSalary))
                {
                    Console.WriteLine("Ошибка. Введите корректное число для новой зарплаты сотрудника.");
                    return;
                }

                Console.Write("Введите новое имя сотрудника: ");
                string newName = Console.ReadLine();

                employee.Salary = newSalary;
                employee.Name = newName;

                Console.WriteLine("Данные успешно обновлены.");
            }
            else
            {
                Console.WriteLine("Сотрудник не найден.");
            }
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public string Department { get; set; }
        public List<Job> WorkHistory { get; set; }
    }

    public class Job
    {
        public string Company { get; set; }
        public string Position { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public decimal Salary { get; set; }
    }
}
