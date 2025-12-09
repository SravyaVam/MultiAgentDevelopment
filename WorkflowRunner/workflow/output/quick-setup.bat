@echo off
echo 🚀 Setting up E-commerce API...
echo.
REM Create project directory
if exist MyEcommerceAPI rmdir /s /q MyEcommerceAPI
mkdir MyEcommerceAPI
cd MyEcommerceAPI
echo 📦 Creating .NET Web API project...
dotnet new webapi --force
echo 📄 Copying generated files...
xcopy "..\Controllers\" Controllers\ /E /I /Y
xcopy "..\Models\" Models\ /E /I /Y
xcopy "..\Services\" Services\ /E /I /Y
copy "..\Program.cs" Program.cs /Y
echo 🔧 Project configured for controllers and Swagger...
echo 🔧 Building project...
dotnet build
echo.
echo.
echo 🚀 Starting API server...
echo 🌐 Once started, check the console for the actual port
echo 🌐 Then open: http://localhost:[PORT]/swagger
echo.
dotnet run
pause