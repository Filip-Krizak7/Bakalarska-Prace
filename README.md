# Bakalarska-Prace
# Filip Křižák

Database model diagram
https://dbdiagram.io/d/639b990799cb1f3b55a1b98c

Deployment steps:
- change REACT_APP_AXIOS_URL in .env.production
- change BASE_URL_PRODUCTION in AppConfig.cs
- a new email has been created, if you want to change it, you can do it in AppConfig.cs
- to generate ASP.NET production build run command "dotnet publish -c Release -o ./publish"
- to generate ReactJS production build run command "npm run build"
- database migration tutorial - https://www.entityframeworktutorial.net/code-first/code-based-migration-in-code-first.aspx

- change User Files and Reports folder path in AppConfig.cs if you want (if path doesn't exist, the folders are created automatically)
