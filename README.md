# UptimeRobot
A monitoring application using .Net Core MVC, Entity Framework, Microsoft Identity and Bootstrap. This application enables users to create, modify and view different monitors. The application keeps track of the monitors' history and displays the up time information on the monitors' details page. A hosted service runs minutely to check all the monitors and alerts users if their monitor is down. An e-mail sender is used to notify the owners but the application can be extended to make use of different notification tools.

![Details](https://i.ibb.co/PD8vd9B/Monitor-Details.jpg)

# Technologies and Dependencies
The project is written in **C#** with **.Net Core.**
The database is managed with **Entity Framework Core**
The front-end is written with **razor** and implements **bootstrap**
The project makes use of several third party libraries such as:

 - **Automapper** is used to map entites and viewmodels
 - **Serilog** is used to log into text files. The logging file is located in the ***Logs*** folder and is created daily.
 - **SendGrid** used as a mail delivery service
 - **Microsoft Identity** is used to manage users

# Services
There are three main services in the project. *MonitorService* for the data management, *EmailSender* to manage e-mails and *UptimeService* to monitor URL's. These three services are all located under the *Services* folder. All services run asynchronously. 

## UptimeService
Uptime Service is a hosted service that works minutely. Each minute it iterates over the monitors according to their interval and the URL's that are in the system are pinged to monitor if they are up. Each time they are pinged, a log file is created to track the history of the monitor.

## MonitorService
Monitor service is the layer between the controller and the database. It makes use of the **AutoMapper** package to map entities and viewmodels. 

## EmailSender 
Email sender is a service that handles all the e-mails. Currently it uses SendGrid as a mail delivery service. All the e-mails can be monitored on the SendGrid panel. 
> A default API Key has been added to the system. You can use and modify your own key using *User Secrets* with the title *SendGridKey* 
> You can also modify or store keys by accessing the *secrets.json* file.

# Errors
This application is designed to continue to work even if an error is raised. The application handles the error and logs it depending on the level of the error. The application can be extended to display user-friendly e-mails after handling them or sending e-mails when a critical error is raised.



