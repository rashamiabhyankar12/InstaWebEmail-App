# InstaWebEmail-App

![App UI preview](https://github.com/user-attachments/assets/58c7f33a-4fcc-4072-8642-8e0e3a41f86c)

## Design

- Figma: [https://www.figma.com/community/file/1694294795649635279](https://www.figma.com/community/file/1694294795649635279)

A web application built with ASP.NET and SQL Server for managing grocery-related operations. This project is designed to run locally in Visual Studio and connects to a SQL Server database named `Groce[...] 

## Table of Contents
- [Prerequisites](#prerequisites)
- [Setup](#setup)
- [Database configuration](#database-configuration)
- [Import database](#import-database)
- [Run the application](#run-the-application)
- [Admin login](#admin-login)

## Prerequisites
Before starting, make sure you have the following installed:

- Visual Studio 2022 or later (Community edition is sufficient)
- SQL Server (Developer edition recommended)
- SQL Server Management Studio (SSMS)
- Git

## Setup
1. Clone the repository:

   ```bash
   git clone https://github.com/rashamiabhyankar12/InstaWebEmail-App.git
   ```

2. Open the solution in Visual Studio:
   - Navigate to the cloned repository folder
   - Open the `InstaWebEmail-App` directory
   - Double-click `GroceryStore.sln`

## Database configuration
Open `appsettings.json` and update the database connection string.

Example:

```json
"ConnectionStrings": {
  "conn": "data source=YOUR_SERVER_NAME;initial catalog=Grocery;integrated security=true;encrypt=false"
}
```

Replace `YOUR_SERVER_NAME` with your local SQL Server instance name, for example:

```text
RASHAMI\SQLEXPRESS
```

> The connection string should point to an instance where the `Grocery` database exists or where the `.bacpac` file will be imported.

## Import database
Use SSMS to import the database into SQL Server:

1. Open SQL Server Management Studio (SSMS)
2. Connect to your SQL Server instance
3. Right-click `Databases`
4. Select `Import Data-tier Application...`
5. Choose the `Grocery.bacpac` file
6. Complete the wizard to create the `Grocery` database

## Run the application
Once the solution is open and the database is configured:

1. Restore NuGet packages if prompted
2. Build the solution
3. Start the app in Visual Studio
4. Sign in using the admin credentials below

## Admin login
Use the following credentials to sign in after the app is running:

- Username: `admin@gmail.com`
- Password: `Admin@123`

## Summary
After completing the setup steps, the project should run locally, connect to the `Grocery` database, and allow access with the default admin account.
