# Employee Service

Small WCF REST service for reading an employee hierarchy and updating an employee enabled state.

## Database Setup

Run the database script:

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -C -b -i EmployeeService\deployment\CreateDatabase.sql
```

The script creates:

- Database: `Test`
- Table: `[dbo].[Employee]`
- Seed data with two separate employee trees

The service connection string is configured in `EmployeeService\Web.config`:

```xml
Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Test;Integrated Security=True
```

## Service Endpoint

```text
http://localhost:64014/EmployeeService.svc
```

## API Requests

Get employee hierarchy by employee ID:

```http
GET http://localhost:64014/EmployeeService.svc/GetEmployeeById?id=1
```

Response:

```json
{
  "ID": 1,
  "Name": "Andrii",
  "ManagerID": null,
  "Enable": true,
  "Employees": [
    {
      "ID": 2,
      "Name": "Oleksii",
      "ManagerID": 1,
      "Enable": true,
      "Employees": [
        {
          "ID": 3,
          "Name": "Roman",
          "ManagerID": 2,
          "Enable": true,
          "Employees": [
            {
              "ID": 5,
              "Name": "Dmytro",
              "ManagerID": 3,
              "Enable": false,
              "Employees": []
            },
            {
              "ID": 6,
              "Name": "Vladyslav",
              "ManagerID": 3,
              "Enable": true,
              "Employees": []
            }
          ]
        },
        {
          "ID": 4,
          "Name": "Serhii",
          "ManagerID": 2,
          "Enable": true,
          "Employees": []
        }
      ]
    }
  ]
}
```

Update employee enabled state:

```http
PUT http://localhost:64014/EmployeeService.svc/EnableEmployee?id=1&enable=false
```

Successful response:

```json
"Employee with ID 1 was updated successfully."
```

If the employee does not exist, the service returns `404 Not Found`.
