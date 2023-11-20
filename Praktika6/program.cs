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
                Console.WriteLine("3. Поиск сотрудника");
                Console.WriteLine("4. Статистика по отделам");
                Console.WriteLine("5. Сотрудники в нескольких отделах");
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
                        SearchEmployee(employees);
                        break;
                    case "4":
                        DepartmentStatistics(employees);
                        break;
                    case "5":
                        EmployeesInMultipleDepartments(employees);
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
                    Department = e.Element("Department").Value
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
                            new XElement("Department", e.Department)
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
                Department = department
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

        static void SearchEmployee(List<Employee> employees)
        {
            Console.WriteLine("Поиск сотрудника");
            Console.WriteLine("=================");

            Console.Write("Введите запрос: ");
            string query = Console.ReadLine();

            var searchResults = employees.Where(e =>
                e.Id.ToString() == query ||
                e.Name.Contains(query) ||
                e.Department.Contains(query)
            );

            if (searchResults.Any())
            {
                foreach (var employee in searchResults)
                {
                    Console.WriteLine($"ID: {employee.Id}");
                    Console.WriteLine($"Имя: {employee.Name}");
                    Console.WriteLine($"Зарплата: {employee.Salary}");
                    Console.WriteLine($"Отдел: {employee.Department}");
                    Console.WriteLine("=====================");
                }
            }
            else
            {
                Console.WriteLine("Результаты не найдены.");
            }
        }

        static void DepartmentStatistics(List<Employee> employees)
        {
            Console.WriteLine("Статистика по отделам");
            Console.WriteLine("=====================");

            var statistics = employees
                .GroupBy(e => e.Department)
                .Select(g => new
                {
                    Department = g.Key,
                    Count = g.Count(),
                    AverageSalary = g.Average(e => e.Salary),
                    TotalSalary = g.Sum(e => e.Salary)
                });

            foreach (var stat in statistics)
            {
                Console.WriteLine($"Отдел: {stat.Department}");
                Console.WriteLine($"Всего сотрудников: {stat.Count}");
                Console.WriteLine($"Средняя зарплата: {stat.AverageSalary}");
                Console.WriteLine($"Общая зарплата: {stat.TotalSalary}");
                Console.WriteLine("=====================");
            }
        }

        static void EmployeesInMultipleDepartments(List<Employee> employees)
        {
            Console.WriteLine("Сотрудники в нескольких отделах");
            Console.WriteLine("==============================");

            var multipleDepartments = employees
                .GroupBy(e => e.Name)
                .Where(g => g.Count() > 1)
                .Select(g => new
                {
                    EmployeeName = g.Key,
                    Departments = string.Join(", ", g.Select(e => e.Department))
                });

            foreach (var employee in multipleDepartments)
            {
                Console.WriteLine($"Имя сотрудника: {employee.EmployeeName}");
                Console.WriteLine($"Отделы: {employee.Departments}");
                Console.WriteLine("=====================");
            }
        }

        public static void ExportToXml(List<Employee> employees)
        {
            Console.WriteLine("Экспорт в XML файл");
            Console.WriteLine("===================");

            Console.Write("Введите имя выходного файла: ");
            string fileName = Console.ReadLine();

            // Получение пути к рабочему столу
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string filePath = Path.Combine(desktopPath, fileName);

            XDocument document = new XDocument(
                new XElement("Employees",
                    employees.Select(e =>
                        new XElement("Employee",
                            new XElement("Id", e.Id),
                            new XElement("Name", e.Name),
                            new XElement("Salary", e.Salary),
                            new XElement("Department", e.Department)
                        )
                    )
                )
            );

            document.Save(filePath);
            Console.WriteLine("XML файл успешно экспортирован.");

            // Чтение экспортированного XML файла
            Console.WriteLine("Чтение экспортированного XML файла:");
            XDocument exportedDocument = XDocument.Load(filePath);
            foreach (XElement employeeElement in exportedDocument.Descendants("Employee"))
            {
                string id = employeeElement.Element("Id")?.Value;
                string name = employeeElement.Element("Name")?.Value;
                string salary = employeeElement.Element("Salary")?.Value;
                string department = employeeElement.Element("Department")?.Value;

                Console.WriteLine($"Id: {id}, Name: {name}, Salary: {salary}, Department: {department}");
            }
        }

        static void DisplayAllEmployees(List<Employee> employees)
        {
            Console.WriteLine("Все сотрудники");
            Console.WriteLine("==============");

            foreach (var employee in employees)
            {
                Console.WriteLine($"ID: {employee.Id}");
                Console.WriteLine($"Имя: {employee.Name}");
                Console.WriteLine($"Зарплата: {employee.Salary}");
                Console.WriteLine($"Отдел: {employee.Department}");
                Console.WriteLine("=====================");
            }
        }

        static void UpdateSalaryAndName(List<Employee> employees)
        {
            Console.WriteLine("Обновление зарплаты и ФИО");
            Console.WriteLine("========================");

            Console.Write("Введите ID сотрудника: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Ошибка. Введите корретное число для ID сотрудника.");
                return;
            }
            Employee employee = employees.FirstOrDefault(e => e.Id == id);

            if (employee != null)
            {
                Console.Write("Введите новое имя сотрудника: ");
                string name = Console.ReadLine();
                employee.Name = name;

                Console.Write("Введите новую зарплату сотрудника: ");
                decimal salary;
                if (!decimal.TryParse(Console.ReadLine(), out salary))
                {
                    Console.WriteLine("Ошибка. Введите корректное число для зарплаты сотрудника.");
                    return;
                }
                employee.Salary = salary;

                Console.WriteLine("Данные сотрудника успешно обновлены.");
            }
            else
            {
                Console.WriteLine("Сотрудник не найден.");
            }
        }
    }
