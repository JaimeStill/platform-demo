@echo off

echo Updating dbseeder dependencies...
cd .\dbseeder
call dotnet add package Microsoft.EntityFrameworkCore.Relational
call dotnet add package Microsoft.EntityFrameworkCore.SqlServer

echo Updating PlatformDemo.Data dependencies...
cd ..\PlatformDemo.Data
call dotnet add package Microsoft.EntityFrameworkCore.SqlServer
call dotnet add package Microsoft.EntityFrameworkCore.Tools

echo Updating PlatformDemo.Web dependencies...
cd ..\PlatformDemo.Web
call dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson
call dotnet add package Microsoft.EntityFrameworkCore.Design

cd ..
echo Dependencies successfully updated!