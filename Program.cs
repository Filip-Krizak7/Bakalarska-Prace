using TeacherPractise.Service;
using TeacherPractise.Dto.Request;
using TeacherPractise.Service.Token.RegistrationToken;
using TeacherPractise.Service.Email;
using TeacherPractise.Mapper;
using TeacherPractise.Service.FileService;
using TeacherPractise.Service.CsvReport;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using TeacherPractise.Config;
using TeacherPractise.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
SecurityService securityService = new(builder.Configuration);
builder.Services.AddSingleton<SecurityService>(securityService);

builder.Services.AddSingleton<AppUserService>();
builder.Services.AddSingleton<FileService>();
builder.Services.AddSingleton<TeacherService>();
builder.Services.AddSingleton<RegistrationService>();
builder.Services.AddSingleton<SchoolService>();
builder.Services.AddSingleton<StudentService>();
builder.Services.AddSingleton<ConfirmationTokenService>();
builder.Services.AddSingleton<CsvReport>();
builder.Services.AddSingleton<CoordinatorService>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddSingleton<CustomMapper>();
builder.Services.AddSingleton<ForgotPasswordService>();

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

var app = builder.Build();

var timerTokens = new System.Timers.Timer
{
    Interval = TimeSpan.FromHours(24).TotalMilliseconds, 
    AutoReset = true
};

var timerUser = new System.Timers.Timer
{
    Interval = TimeSpan.FromMinutes(10).TotalMilliseconds,
    AutoReset = true
};

timerTokens.Elapsed += (sender, args) =>
{
    var now = DateTime.Now;
    if (now.Hour == 3 && now.Minute == 0)
    {
        var confirmationTokenService = app.Services.GetRequiredService<ConfirmationTokenService>();

        Console.WriteLine("Deleting expired tokens: " + DateTime.Now);

        confirmationTokenService.deleteExpiredConfirmationTokens();
        confirmationTokenService.deleteExpiredPasswordResetTokens();
    }
};

timerUser.Elapsed += (sender, args) =>
{
    var now = DateTime.Now;
    if (now.Minute % 10 == 0)
    {
        var appUserService = app.Services.GetRequiredService<AppUserService>();

        Console.WriteLine("Deleting user with expired tokens: " + DateTime.Now);

        appUserService.deleteUserByExpiredConfirmationToken();
    }
};

timerTokens.Start();
timerUser.Start();

using (var ctx = new Context())
{
    if(!ctx.Users.ToList().Any()) 
    {
        var registrationService = app.Services.GetRequiredService<RegistrationService>();

        School school1 = new School(1, "Ostravská univerzita"); 
        ctx.Schools.Add(school1);
        ctx.SaveChanges();  

        RegistrationDto request = new RegistrationDto(AppConfig.CONFIRMATION_EMAIL_ADDRESS, "Koordinátor", "Default", 1, "123456789", AppConfig.COORDINATOR_EMAIL_PASSWORD, "coordinator");
        registrationService.register(request);
    }  
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();
app.UseCors(AllowSpecificOrigin);

//app.UseHsts();
app.UseMvc();
app.UseStaticFiles();
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();
app.UseDeveloperExceptionPage();

app.MapControllers();
app.Run();