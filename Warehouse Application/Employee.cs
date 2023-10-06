using System;
using System.Text.RegularExpressions;
namespace Warehouse_Application
{
	public class Employee
	{
		private string name;
		private string lastName;
		private string id;
		private int age;
		private PositionName position;
		private string password;
		private string login;
		public bool mainAccount { get; set; }


		public Employee(string name, string lastName, string id, int age, PositionName position)
		{
			if (name.Length > 0 && lastName.Length > 0 && Regex.IsMatch(id, @"^[a-zA-Z0-9]{3}$") && age >= 18)
			{
				this.name = name;
				this.lastName = lastName;
				this.id = id;
				this.age = age;
				this.position = position;
			}
			else
			{
				throw new FormatException("Input informations are not correct with guidelines");
			}
		}
		public Employee(string name, string lastName, PositionName position,int age, string id, string password, string login, bool mainAccount)
		{
			this.name = name;
			this.lastName = lastName;
			this.position = position;
			this.age = age;
			this.password = password;
			this.login = login;
			this.mainAccount = mainAccount;
			this.id = id;
		}
		public Employee()
		{ }
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				if (value.Length >= 3)
					name = value;
				else
					throw new FormatException("Name is to short");
			}
		}
		public string LastName
		{
			get
			{
				return lastName;
			}
			set
			{
				if (value.Length > 2)
					lastName = value;
				else
					throw new FormatException("Last name is to short");
			}
		}
		public string Id
		{
			get
			{
				return id;
			}
			set
			{
				if (Regex.IsMatch(value, @"^[a-zA-Z0-9]{3}$"))
					id = value;
				else
					throw new FormatException("Id of employee is uncorrect");
			}
		}
		public int Age
		{
			get
			{
				return age;
			}
			set
			{
				if (value >= 18)
					age = value;
				else
					throw new FormatException("Age is uncorrect");
			}
		}
		public PositionName Position
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
			}
		}
		public string Password
		{
			get
			{
				return password;
			}
			set
			{
				if (value.Length > 5)
					password = value;
				else
					throw new FormatException("Password is to short");
			}
		}
		public string Login
		{
			get
			{
				return login;
			}
			set
			{
				login = value;
			}
		}
		public void GraphicEmployee()
		{
            Console.WriteLine($"Name:         {Name}");
            Console.WriteLine($"Last name:    {LastName}");
            Console.WriteLine($"Id:           {Id}");
            Console.WriteLine($"Age:          {Age}");
            Console.WriteLine($"Position:     {Position}");
            Console.WriteLine($"Login:        {Login}");
            Console.WriteLine($"Password      {Password}");
            Console.WriteLine($"Main account: {mainAccount}");
        }
	}
    public enum PositionName
    {
        Admin, Employee, Supplier, Manager
    }

}

