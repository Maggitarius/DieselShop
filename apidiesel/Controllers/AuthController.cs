    using apidiesel.Models;
    using BCrypt.Net;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly dieselContext _context;

        public AuthController(dieselContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            if (_context.Employees.Any(u => u.Login == dto.Email) || _context.Customers.Any(c => c.Email == dto.Email))
                return BadRequest("Пользователь с таким логином уже существует.");

            if (dto.UserType == "employee")
            {
                var employee = new Employee
                {
                    FullName = dto.FullName,
                    Position = dto.Position,
                    Login = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
                };

                _context.Employees.Add(employee);
            }
            else if (dto.UserType == "customer")
            {
                var customer = new Customer
                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),


                };

                _context.Customers.Add(customer);
            }
            else
            {
                return BadRequest("Неправильный тип пользователя.");
            }

            _context.SaveChanges();
            return Ok("Успешная регистрация.");
        }


        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var employee = _context.Employees.FirstOrDefault(u => u.Login == dto.Email);
            if (employee != null && BCrypt.Net.BCrypt.Verify(dto.Password, employee.PasswordHash))
            {
                return Ok(new
                {
                    userType = "employee",
                    id = employee.EmployeeId,
                    fullName = employee.FullName,
                    position = employee.Position,
                    email = employee.Login
                });
            }

            var customer = _context.Customers.FirstOrDefault(c => c.Email == dto.Email);
            if (customer != null && BCrypt.Net.BCrypt.Verify(dto.Password, customer.PasswordHash))
            {
                return Ok(new
                {
                    userType = "customer",
                    fullName = customer.FullName,
                    email = customer.Email,
                    customerId = customer.CustomerId
                });
            }

            return Unauthorized("Неправильный логин или пароль.");
        }

        public class RegisterDto
        {
            public string FullName { get; set; }
            public string? Position { get; set; } 
            public string Email { get; set; }     
            public string Password { get; set; }
            public string UserType { get; set; }  
        }

        public class LoginDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }