using EmployeeJwtAuthentication.Models;
using EmployeeJwtAuthentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using EmployeeJwtAuthentication.Middleware;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
   {
       //Added Swagger security definition and requirement to allow adding bearer token while testing APIs
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
//Added JWT Authentication and validation criteria
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

//Injected the required services to achieve Dependency Injection
builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();
// Use the custom error handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

//Middlewares
app.UseSwagger();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.UseAuthentication();

app.MapPost("/gettokenbylogin",
    (UserLogin user, IUserService userService) => Login(user, userService));

//User's can login with their username and password.
//After successful login the bearer token will be shared in response.
IResult Login(UserLogin user, IUserService userService)
{
    if(user == null)
    {
        throw new ArgumentNullException(nameof(user));
    }
    if (!string.IsNullOrEmpty(user.Username) &&
        !string.IsNullOrEmpty(user.Password))
    {
        //chec if User exist with provided username and password
        var loggedInUser = userService.GetUser(user);
        if (loggedInUser == null)
        {
            //Return not found if user does not exist
            return Results.NotFound("User does not exist");
        }
        else
        {
            //Claims for JWT token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, loggedInUser.Username),
                new Claim(ClaimTypes.Email, loggedInUser.Email),
                new Claim(ClaimTypes.GivenName, loggedInUser.FirstName),
                new Claim(ClaimTypes.Surname, loggedInUser.LastName),
                new Claim(ClaimTypes.Role, loggedInUser.Role)
            };
            //Token parameters
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
            //Write the JWT token
            var bearerToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Results.Ok(bearerToken);
        }
    }
    return Results.BadRequest("Username or Password is incorrect");
}


app.UseSwaggerUI();

app.Run();


// Make the implicit Program class public so test projects can access it
public partial class Program { }
