# Car Rental System

A car rental web application developed using ASP.NET Core MVC and Entity Framework Core.

## Technologies Used

* **Backend:** C#, ASP.NET Core MVC
* **Database:** Microsoft SQL Server, Entity Framework Core (Configured via Database-First approach)
* **Frontend:** HTML5, CSS3, JavaScript, Bootstrap 5, Razor Syntax

## System Features

The application operates under two primary user roles: Administrator (Admin) and Customer.

### Administrator (Admin) Module
* **Dashboard:** Visual representation of statistics including total revenue, active rentals count, available cars count, and total registered customers. Track recent 5 kiralama operations.
* **Car Management:** Add new vehicles to the system (with file/image upload support), edit existing vehicle details, and update vehicle statuses (Available, Rented, In Maintenance).
* **Soft Delete:** Vehicles are not physically deleted from the database to preserve data integrity and historical constraints; instead, they are hidden from the UI using an `IsDeleted` flag.
* **Reservation Management:** List all reservations and process vehicle returns via the "Collect Vehicle" function, resetting the car status back to "Available".

### Customer Module
* **Car Listing & Filtering:** Displays only available and non-deleted vehicles. Filter cars dynamically by category and gear type.
* **Reservation System:** Date range selection with client-side real-time cost calculation using JavaScript.
* **Overlap Control:** A backend validation algorithm that prevents double-booking by checking if the vehicle has an active reservation during the selected dates.
* **Payment Module:** A simulated (mock) payment step that records transaction data into the `Payments` table before finalizing the reservation status.
* **Profile & History:** A user account screen where customers can view their personal details and monitor active or past rental records.

## Database Schema

The system utilizes a relational database structure with the following key tables:
* `Users`: System users mapped to authentication roles (Admin/Customer).
* `Cars`: Vehicle information including Plate, Brand, Model, Price, Image URL, Status, and the Soft Delete flag.
* `Categories`, `GearTypes`, `FuelTypes`: Relational lookup tables for vehicle filtering.
* `Reservations`: Stores start/end dates, total amount, and operation status (Pending Payment, Active, Completed).
* `Payments`: Stores transaction date, amount, and the foreign key link to the respective reservation.

## Installation and Setup

1. Create a database named `CarRental` on your local SQL Server instance and configure the tables.
2. Open the project inside Visual Studio.
3. Update the Connection String inside `CarRentalContext` or `appsettings.json` with your SQL Server credentials.
4. Run the following command in the **Package Manager Console** to synchronize Entity Framework models with the database:
   ```powershell
   Scaffold-DbContext "Server=YOUR_SERVER_NAME;Database=CarRental;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force
   ```
5. Build and run the application.