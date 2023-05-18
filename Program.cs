using TeacherPractise.Service;
using TeacherPractise.Service.Token.RegistrationToken;
using TeacherPractise.Model;
using TeacherPractise.Service.Email;
using TeacherPractise.Mapper;
using TeacherPractise.Dto.Request;
using TeacherPractise.Dto.Response;
using TeacherPractise.Mapper;
using System.Data.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text;
using System;

using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;

            //var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";  
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
            SecurityService securityService = new(builder.Configuration);
            builder.Services.AddSingleton<SecurityService>(securityService);

            builder.Services.AddSingleton<AppUserService>();
            builder.Services.AddSingleton<TeacherService>();
            builder.Services.AddSingleton<RegistrationService>();
            builder.Services.AddSingleton<SchoolService>();
            builder.Services.AddSingleton<StudentService>();
            builder.Services.AddSingleton<ConfirmationTokenService>();
            builder.Services.AddSingleton<CoordinatorService>();
            builder.Services.AddSingleton<EmailService>();
            builder.Services.AddSingleton<CustomMapper>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Teacher practice API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            
            //builder.Services.AddCors();
            var  AllowSpecificOrigin = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: AllowSpecificOrigin, policy =>
                {
                    policy.WithOrigins("http://localhost:80", "http://localhost:5000", "http://localhost:5000/login", "http://localhost") 
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            

            builder.Services.AddAuthorization();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.Cookie.Name = "access_token";

                });

            builder.Services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    var key = new SymmetricSecurityKey(securityService.Key);
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("JwtPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                });
            });
            
            builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
            builder.Services.AddControllers();
            
            using (var ctx = new Context())
            {
                //private readonly RegistrationService regService;
                /*RegistrationService regService;
                SchoolService schService;
                AppUserService appService;*/

                //User appUser = new User("coordinator@student.osu.cz", "secret_passwd123", "Testing", "Tester", 1, "123456789", Roles.ROLE_COORDINATOR, false, true);
                //User user = ctx.Users.Where(q => q.Username == "coordinator@student.osu.cz").FirstOrDefault();
                /*var school1 = new School() { Id = 1, Name = "ZŠ Dobrá" };
                ctx.Schools.Add(school1);
                ctx.SaveChanges();*/

                //RegistrationDto request = new RegistrationDto("coordinator@student.osu.cz", "Testing", "Tester", 2, "123456789", "secret_passwd123", "coordinator");
                //UserLoginDto req = new UserLoginDto("r93452@student.osu.cz", "secret_passwd123");
                //appService.login(req, HttpContext);
                //regService.register(request);
                /*User appUser = new User("r31252@student.osu.cz", "secret_passwd123", "Testing", "Tester", 2, "123456789", Roles.ROLE_TEACHER, false, true);

                List<string> results = ctx.Database.SqlQuery<string>("SELECT name FROM sys.tables ORDER BY name").ToList();

                // Vypsání názvů tabulek
                /*foreach (var tableName in results)
                {
                    Console.WriteLine(tableName);
                }*/

                /*var userPractices = ctx.Set<User>()
                               .SelectMany(u => u.AttendedPractices)
                               .ToList();

                foreach (var up in userPractices)
                {
                    Console.WriteLine($"Student ID: {up.User.Id}, Practice ID: {up.PracticeId}");
                }*/

                /*var remPractices = ctx.Practices;
                ctx.Practices.RemoveRange(remPractices); 
                ctx.SaveChanges();*/

                var practices = ctx.Practices.ToList();
                foreach (var practice in practices)
                {
                    //Type typeDate = practice.Date.GetType();
                    //Console.WriteLine(typeDate.ToString());
                    Console.WriteLine($"Datum: {practice.Date}, Čas začátku: {practice.Start}, Poznamka: {practice.Note}, SubjectID: {practice.SubjectId}, TeacherID: {practice.TeacherId}");
                }

                /*string sqlQuery = "SELECT * FROM Practice";
                var results = ctx.Database.SqlQuery<Practice>(sqlQuery);

                foreach (var result in results)
                {
                    Console.WriteLine($"Datum: {practice.Date}, Čas začátku: {practice.Start}, Poznamka: {practice.Note}");
                }*/
            }
            
            var app = builder.Build();

            //app.UseHttpsRedirection();
            
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();
            app.UseCors(AllowSpecificOrigin);

            //app.UseHsts(); //------
            app.UseMvc();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();

            app.MapControllers();
            app.Run();
        
