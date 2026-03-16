# SafeVaultApp

SafeVault is a secure web API application designed to protect user data by applying modern security practices. It validates and sanitizes user input, uses parameterized database queries to prevent SQL injection, and includes authentication and role-based authorization with hashed passwords and JWT tokens. The application also protects administrative features so only authorized users can access them, and it includes tests that verify resistance to common attacks such as SQL injection and XSS.

## Prerequisites

- .NET 10 SDK
- MySQL
- Postman or another API testing tool

## Setup and Run

### 1. Restore dependencies

```bash
dotnet restore
```

### 2. Configure application settings

Copy SafeVault/appsettings.example.json to SafeVault/appsettings.json and replace the placeholder values with your local:

- MySQL connection string

- JWT secret key

### 3. Build the solution

```bash
dotnet build
```

### 4. Run the application

From the SafeVault project folder:

```bash
dotnet run
```

## Testing

To run the test project:

```bash
dotnet test
```

## Additional Notes

- The application uses BCrypt for password hashing.

- JWT is used for authentication and role-based authorization.

- Administrative routes are restricted to users with the admin role.

- Input validation and sanitization are used to reduce the risk of SQL injection and XSS.

