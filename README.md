This is EmployeeMAnagement API project designed in .Net Core 8.0 with minimal API
The Authentication technique used is JWT token authentication
To avoid use of database, data is maintained in Repository as static list.
UserRepository has list of Users with their roles
EmployeeRepository has list of Employess
Maintaing username and password in static list is not a good practice but to show JWT authentication without any external tools I have used it.
There are 2 Users created,
harbinger_administrator : is the admin user who will have access to all endpoints
harbinger_member: is the member user who will not have access to create, update and delete.
I have used swagger UI to test the endpoints. Also, added the security definition to add bearer token.
Implemented CORS policy.
