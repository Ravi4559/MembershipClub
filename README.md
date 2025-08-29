## Membership Club - Customer Loyalty Rewards API
This repository contains the source code for the Membership Club application, a simple customer loyalty and rewards program. It features a secure ASP.NET Core Web API backend that handles user registration, authentication, and points management, along with a clean, responsive frontend built with vanilla HTML, CSS, and JavaScript.

### API Testing with Postman
[![Run in Postman](https://run.pstmn.io/button.svg)](https://raviravindra-4559-6716984.postman.co/workspace/8f9aea06-aafc-4bfb-8c66-21b7645234cf/collection/48026263-e72ef8d9-7d82-430c-b0f9-a40308418141?action=share&source=copy-link&creator=48026263)


### Core Workflow
The application follows a simple and secure workflow for customer interaction:

- Registration: A new user registers using their mobile number. The backend creates a new member record in the database.

- Verification & Login: The user verifies their mobile number using a static OTP (1234 for this demo). Upon successful verification, the backend generates a JSON Web Token (JWT) and sends it to the frontend.

- Authenticated Session: The frontend stores the JWT in localStorage and uses it to authorize subsequent requests. The UI switches to a "logged-in" view.

- Add Points: The logged-in user can add loyalty points by submitting a purchase amount. This action requires a valid JWT to be sent in the Authorization header.

- Check Balance: The user can view their total points balance at any time. This is also a protected action requiring the JWT.

- Logout: The user can log out, which clears the JWT from localStorage and returns them to the registration view.

### Tech Stack
This project is built with a modern and robust technology stack.

#### Backend
- Framework: ASP.NET Core 8 Web API

- Language: C#

- Database: SQL Server (via LocalDB)

- ORM: Entity Framework Core 8

- Authentication: JSON Web Tokens (JWT)

- API Documentation: Swagger 

#### Frontend
- UI: HTML5

- Styling: CSS3 (no frameworks)

- Logic: Vanilla JavaScript (ES6+)

- API Communication: fetch API

### Setup and Installation
Follow these steps to get the application running on your local machine.

#### Prerequisites
- .NET 8 SDK

- Visual Studio 2022 (with the ASP.NET and web development workload)

- SQL Server LocalDB (Typically installed with Visual Studio)

#### 1. Backend Setup
- Clone the repository:

git clone <your-repository-url>
cd MembershipClub

- Open the project in Visual Studio:

Double-click the MembershipClub.sln file.

- Configure appsettings.json:

 Open the appsettings.json file.

- Crucially, update the Jwt section. The Issuer and Audience values must be a single, valid URL where your application will run. You can find this URL in Properties/launchSettings.json.

Example Correction:

"Jwt": {
  "Key": "A_SUPER_SECRET_KEY_THAT_IS_LONG_ENOUGH_TO_BE_SECURE",
  "Issuer": "https://localhost:7263",
  "Audience": "https://localhost:7263"
}

- Create the Database:
-- Open the Package Manager Console in Visual Studio (View -> Other Windows -> Package Manager Console).
-- Run the following commands one by one to create the database and its tables based on the Entity Framework models:

- Add-Migration InitialCreate
-- Update-Database

### 2. Frontend Setup
The frontend files (intex.html, style.css, app.js) are located in the wwwroot folder and are served automatically by the backend.

- Configure the API URL:

In the Solution Explorer, open wwwroot/app.js.

- Find the API_BASE_URL constant.

Update its value to match the https URL you configured in appsettings.json and found in launchSettings.json.

// In wwwroot/app.js
const API_BASE_URL = 'https://localhost:7263'; // <-- Make sure this URL is correct!

### 3. Run the Application
Press F5 or the green "Play" button in Visual Studio to build and run the project.

Your browser will open, likely to the Swagger UI page (e.g., https://localhost:7263/swagger). -> This is expected.

- Navigate to the root URL of your application to see the frontend:
https://localhost:7263

### You can now test the full registration, login, and points management workflow!
