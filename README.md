<img width="1440" height="720" alt="figmawebposter" src="https://github.com/user-attachments/assets/58c7f33a-4fcc-4072-8642-8e0e3a41f86c" />


🔧 Setup Steps
Install prerequisites

1.Install Visual Studio (Community edition works fine).

2.Install SQL Server (Developer edition recommended).

3.Clone the repository

4.Open Command Prompt.

5.Navigate to the folder where you want the project.

6.Run:

cmd
git clone https://github.com/rashamiabhyankar12/InstaWebEmail-App.git
7.Open the solution

8.Go to the cloned folder → InstaWebEmail-App.

9.Double‑click GroceryStore.sln to open in Visual Studio.

10.Configure database connection

Open appsettings.json.

Update the connection string:

json
"ConnectionStrings": {
  "conn": "data source=YOUR_SERVER_NAME;initial catalog=Grocery;integrated security=true;encrypt=false"
}
Replace YOUR_SERVER_NAME with your SQL Server instance (e.g., RASHAMI\\SQLEXPRESS).

11.Import database

In SQL Server Management Studio (SSMS):

Right‑click Databases → Import Data‑tier Application.

Select Grocery.bacpac file.

Complete the wizard to create the Grocery database.

12.👤 Admin Login
Once the app is running, use these credentials to log in:

Username: admin@gmail.com

Password: Admin@123

✅ After this, you should be able to run the app, connect to the Grocery database, and log in with the admin account.
   


