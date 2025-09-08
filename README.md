Registrationsystem
This project is a system to register visitors.

How it works:

On a web page GitHub Pages there is a form where you type first name and last name.
Click on the Send button and the data is sent to an Azure Function App.
The function receives the data and saves it into a Cosmos DB database.
The user gets a response back and it shows on the web page.
All logs are sent to Application Insights so I can see what is happening.

Frameworks and Tools:

Frontend: HTML + JavaScript hosted on GitHub Pages
Backend: Azure Functions C# .NET
Database: Azure Cosmos DB NoSQL
Logging: Azure Application Insights
Version control: GitHub

File structure:

frontend/
index.html
script.js
backend/
RegisterVisitor.cs
Visitor.cs
Program.cs

How to test it:

Open the GitHub Pages site.
Type a name in the form.
Click Send.
If everything works you will see:  
Registered visitor: Your Name.
Check Cosmos DB in Azure and you will see the saved visitor.

Registered visitor: Your Name
Check Cosmos DB in Azure and you will see the saved visitor.
