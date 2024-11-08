using EmployeeJwtAuthentication.Models;
using EmployeeJwtAuthentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
   {
       options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
       {
           Type = SecuritySchemeType.Http,
           Scheme = "Bearer",
           Name = "Authorization",
           In = ParameterLocation.Header,
           BearerFormat = "JWT"
       });
       options.AddSecurityRequirement(new OpenApiSecurityRequirement
       {
           {
               new OpenApiSecurityScheme
               {
                   Reference = new OpenApiReference
                   {
                       Id = "Bearer",
                       Type = ReferenceType.SecurityScheme
                   }
               },
               new List<string>()
           }
       });
   });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
options.AddPolicy("CorsPolicy",
    builder => builder.SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});


builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
builder.Services.AddSingleton<IUserService, UserService>();

var app = builder.Build();

app.UseSwagger();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.UseAuthentication();

app.MapGet("/", () => "Hello World!").ExcludeFromDescription();

app.MapPost("/gettokenbylogin",
    (UserLogin user, IUserService userService) => Login(user, userService));

app.MapPost("/addemployee",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(Employee employee, IEmployeeService employeeService) => AddEmployee(employee, employeeService));

app.MapGet("/getemployeebyid",
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Member")]
(int id, IEmployeeService employeeService) => GetEmployeeById(id, employeeService));

app.MapGet("/getemployeelist",
(IEmployeeService employeeService) => GetEmployees(employeeService));

app.MapPut("/updateemployee",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(Employee employee, IEmployeeService employeeService) => UpdateEmployee(employee, employeeService));

app.MapDelete("/deleteemployee",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(int id, IEmployeeService employeeService) => DeleteEmployee(id, employeeService));

IResult Login(UserLogin user, IUserService userService)
{
    if (!string.IsNullOrEmpty(user.Username) &&
        !string.IsNullOrEmpty(user.Password))
    {
        var loggedInUser = userService.GetUser(user);
        if (loggedInUser == null)
        {
            return Results.NotFound("User does not exist");
        }
        else
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, loggedInUser.Username),
                new Claim(ClaimTypes.Email, loggedInUser.Email),
                new Claim(ClaimTypes.GivenName, loggedInUser.FirstName),
                new Claim(ClaimTypes.Surname, loggedInUser.LastName),
                new Claim(ClaimTypes.Role, loggedInUser.Role)
            };
            var token = new JwtSecurityToken(
                issuer: builder.Configuration["Jwt:Issuer"],
                audience: builder.Configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(30),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256)
                );

            var bearerToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Results.Ok(bearerToken);
        }
    }
    return Results.BadRequest("Username or Password is incorrect");
}

IResult AddEmployee(Employee employee, IEmployeeService employeeService)
{
    var result = employeeService.AddEmployee(employee);
    return Results.Ok(result);
}

IResult GetEmployeeById(int id, IEmployeeService employeeService)
{
    var employee = employeeService.GetEmployeeById(id);
    if (employee == null)
    {
        return Results.NotFound("Employee does not exist");
    }
    return Results.Ok(employee);
}

IResult GetEmployees(IEmployeeService employeeService)
{
    var employees = employeeService.GetEmployees();
    return Results.Ok(employees);
}

IResult UpdateEmployee(Employee employee, IEmployeeService employeeService)
{
    var updatedEmployee = employeeService.UpdateEmployee(employee);
    if (updatedEmployee == null)
    {
        return Results.NotFound("Employee does not exist");
    }
    return Results.Ok(updatedEmployee);
}

IResult DeleteEmployee(int id, IEmployeeService employeeService)
{
    var result = employeeService.DeleteEmployee(id);
    if (!result)
    {
        return Results.BadRequest("Failed to delete employee");
    }
    return Results.Ok(result);
}

app.UseSwaggerUI();

app.Run();
