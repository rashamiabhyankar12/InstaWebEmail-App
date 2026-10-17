<img width="1440" height="720" alt="figmawebposter" src="https://github.com/user-attachments/assets/58c7f33a-4fcc-4072-8642-8e0e3a41f86c" />

1.Manually setup every thing (Recommended for developers)
Make sure:
VisualStudio 2022 pre-installed in your machine
pre-installed Sql server 2025 

Now, you can follow these steps:
1.Open the command prompt. Go to a directory where you want to clone this project. Use this command to clone the project.
git clone https://github.com/rashamiabhyankar12/InstaWebEmail-App.git

2.Go to the directory where you have cloned this project, open the directory InstaWebEmail-App. You will find a file with name GroceryStore.sln. Double click on this file and this project will be opened in Visual Studio.

3.Open appsettings.json file and update connection string.
"ConnectionStrings": {
  "conn": "data source=your_server_name;initial catalog=Grocery; integrated security=true;encrypt=false"
}
4.Run the project.
5.How to logged-in with admin account??
username: admin@gmail.com
password: Admin@123
