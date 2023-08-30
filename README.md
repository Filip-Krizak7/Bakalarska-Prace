# Bakalarska-Prace
# Filip Křižák

Database model diagram
![image](https://github.com/Filip-Krizak7/Bakalarska-Prace/assets/121099068/2e4a2f72-ec2e-4393-989b-3f7895e27534)

Deployment steps:
- change REACT_APP_AXIOS_URL in .env.production
- change BASE_URL_PRODUCTION in AppConfig.cs to your frontend address
- a new email has been created, if you want to change it, you can do it in AppConfig.cs
- in Program.cs add new Cors (policy.WithOrigins("your_frontend_address"))
- to generate ASP.NET production build run command "dotnet publish -c Release -o ./publish"
- to generate ReactJS production build run command "npm run build"
- database migration tutorial - https://www.entityframeworktutorial.net/code-first/code-based-migration-in-code-first.aspx
- install URL Rewrite IIS module to ensure proper functioning of the frontend - https://www.iis.net/downloads/microsoft/url-rewrite
- install ASP.NET Hosting Bundle to run API on IIS - https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-7.0.10-windows-hosting-bundle-installer

- change User Files and Reports folder path in AppConfig.cs if you want (if path doesn't exist, the folders are created automatically)
- you can change where API runs in appsettings.json
- if you need you can change the connection string to MS SQL Server database in App.config
