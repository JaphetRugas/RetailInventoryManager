# RetailInventoryManager

A simple inventory management system for a retail store built using a C# console application with Entity Framework Core and SQL Server.

## Prerequisites
- .NET SDK (7.0 or later)
- SQL Server (LocalDB)
- NuGet Package Manager (for package installation)

## Technologies Used
- C# (.NET 7 or later)
- Entity Framework Core
- Microsoft SQL Server (LocalDB)

## Installation
1. Clone this repository:
   ```sh
   git clone <repository-url>
   cd RetailInventoryManager
   ```

2. Install the required NuGet packages in Visual Studio:
   - `Microsoft.EntityFrameworkCore.SqlServer`
   - `Microsoft.EntityFrameworkCore.Design`
   - `Microsoft.EntityFrameworkCore.Tools`
   - `Microsoft.Extensions.Configuration.Json`

3. Ensure that `appsettings.json` is set to **Copy Always** in properties.

4. Apply database migrations in the **Package Manager Console**:
   ```sh
   Add-Migration InitialCreate
   Update-Database
   ```

## Features
- **AddProduct(Product product)**: Adds a new product to the inventory.
- **RemoveProduct(int productId)**: Removes a product from the inventory based on its ID.
- **UpdateProduct(int productId, int newQuantity)**: Updates the quantity of a product.
- **ListProducts()**: Displays all items in the inventory.
- **GetTotalValue()**: Calculates and returns the total value of the inventory.

## Database Configuration
- The application uses **SQL Server LocalDB** as the database.
- To check installed LocalDB versions, run:
   ```sh
   sqllocaldb versions
   ```

## Notes
- Product IDs are positive integers.
- Prices must be non-negative real numbers.
- Quantities must be non-negative integers.

