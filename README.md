# Bakalarska-Prace
# Filip Křižák

Database model diagram
![image](https://github.com/Filip-Krizak7/Bakalarska-Prace/assets/121099068/2e4a2f72-ec2e-4393-989b-3f7895e27534)

Deployment steps:
- change REACT_APP_AXIOS_URL in .env.production
- change BASE_URL_PRODUCTION in AppConfig.cs
- a new email has been created, if you want to change it, you can do it in AppConfig.cs
- to generate ASP.NET production build run command "dotnet publish -c Release -o ./publish"
- to generate ReactJS production build run command "npm run build"
- database migration tutorial - https://www.entityframeworktutorial.net/code-first/code-based-migration-in-code-first.aspx

- change User Files and Reports folder path in AppConfig.cs if you want (if path doesn't exist, the folders are created automatically)
