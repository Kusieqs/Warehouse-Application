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
				if (value.Length > 0)
					name = value;
				else
					throw new FormatException("Name is empty");
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
				if (value.Length > 0)
					lastName = value;
				else
					throw new FormatException("Last name is empty");
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
				if (value.Length > 9)
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
	}
    public enum PositionName
    {
        Admin, Employee, Deliver, Manager
    }

}

