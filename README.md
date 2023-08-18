# YFS.Backend
Your Financial Space

## About
Welcome to the [YFS] backend API repository! 
This project serves as the backbone of our application, providing the necessary functionality 
to power our front-end interfaces and deliver a seamless user experience.

### Features
2022 - August 2023\
User Authentication and Registration: Securely handle user authentication and registration processes, 
ensuring data privacy and controlled access.\
Account Group Creation: Easily create groups of accounts to help users categorize and manage their financial resources efficiently.\
Account Management: Provide comprehensive APIs for creating new accounts, 
complete with the ability to perform various financial operations such as adding funds, withdrawing, and transferring between accounts.\
Operations Management: Enable users to initiate financial transactions seamlessly, 
including adding funds to accounts, performing withdrawals, and facilitating transfers between accounts.\

### Technology Stack
ASP.NET Core 6.0 (with .NET 6.0)\
ASP.NET Identity Core\
JWT-Bearer authentication to protect API\
ASP.NET WebApi Core\
ASP.NET Core Swagger\
Entity Framework Core 7\
AutoMapper\
MSSQL DataBase\
xUnit\

### Getting Started
- download project or git clone/
- Switch to develop branch. /
	git checkout develop/
- Ensure you have MS SQL Express installed and running/
- Launch the solution(YFS.sln in backend folder) in visual studio/
- Change configuration for db connect. 
Edit file appsettings.json
 "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=YFS;Trusted_Connection=True;Encrypt=False;"
  },
Set your address server in the line Server(localhost)
- DB create automatically after run app
- Start Debuggin(F5 button)

You get the following screen: 
![image](https://github.com/tsaa1986/YFS/assets/26444246/d294738e-4fbe-4766-ab77-608414df1d8c)

