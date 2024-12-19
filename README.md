NorthwindRestApi Back-End Application
NorthwindRestApi is a robust ASP.NET Core back-end application designed to serve as the API layer for the nwReactVite front-end. This project demonstrates the implementation of RESTful services, secure authentication, and database interactions using modern back-end development practices.

# Key Features
# RESTful API
Provides a well-structured and secure API for front-end integration. 
Endpoints for CRUD operations on Customers, Products, and Users tables.
Designed to handle complex queries and efficient data retrieval.
Authentication and Authorization
Implements token-based authentication using JWT (JSON Web Tokens).
Protects sensitive endpoints by requiring valid tokens for access.
Includes role-based access control for secure operation.
Database Integration
Connects to a SQL Server database for storing and managing application data.
Uses Entity Framework Core for seamless database interaction and LINQ-based queries.
Supports asynchronous operations to improve application performance.
Error Handling and Logging
Built-in error handling to provide meaningful feedback for API consumers.
Logs errors for debugging and maintenance purposes.

# Technologies Used

**Languages**	C#
**Frameworks**	ASP.NET Core, Entity Framework Core
**Database**	SQL Server
**Security**	JWT Authentication, Role-Based Authorization
**Testing Frameworks**	xUnit, MSTest
**Environment**	.NET 6
**Version Control**	Git and GitHub

# Integration with Front-End
The NorthwindRestApi application is fully integrated with the nwReactVite front-end application, providing:

API endpoints for managing customers, products, and users.
Secure authentication workflows for login and session management.
Real-time synchronization of front-end state with back-end data.
Purpose
This project is part of the coursework for the second-year Software Developer program. It focuses on:

Building secure and scalable back-end applications.
Integrating back-end services with a React-based front-end.
Demonstrating knowledge of modern web development and API design.
