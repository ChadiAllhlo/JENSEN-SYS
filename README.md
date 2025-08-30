# JENSEN-SYS - User Registration & To-Do List Application

A secure, role-based user management system with to-do list functionality built with ASP.NET Core and vanilla JavaScript.

## 📁 Project Structure

```
JENSEN-SYS/
├── api/                    # Backend API (ASP.NET Core)
│   ├── Controllers/        # API endpoints
│   ├── Data/              # Database context and seeding
│   ├── Models/            # Entity models
│   ├── ViewModels/        # Data transfer objects
│   ├── Migrations/        # Entity Framework migrations
│   ├── Properties/        # Project properties
│   ├── Program.cs         # Application entry point
│   ├── api.csproj         # Project file
│   ├── appsettings.json   # Configuration
│   └── e-shop.sln         # Solution file
├── js-client/             # Frontend (Vanilla JavaScript)
│   ├── index.html         # Main application page
│   ├── app.js            # Client-side logic
│   └── styles.css        # Application styling
└── README.md             # This file
```

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK
- SQLite (included with .NET)

### Running the Application

1. **Navigate to the API directory:**
   ```bash
   cd api
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Run the application:**
   ```bash
   dotnet run
   ```

4. **Access the application:**
   - Open your browser and go to `https://localhost:5001`
   - The API will serve both the backend endpoints and the frontend files

## 🔐 Security Features

- **Role-based Authorization**: Admin and User roles
- **Input Validation**: Comprehensive data validation
- **XSS Protection**: HTML sanitization
- **SQL Injection Prevention**: Entity Framework Core
- **Rate Limiting**: Global API protection
- **Account Lockout**: Brute force protection
- **Security Headers**: Modern security standards

## 👥 User Roles

### Admin
- Create new user accounts
- Assign Admin or User roles
- Access to-do list management
- View user profiles

### User
- Access to-do list management
- Cannot create new accounts
- Cannot assign roles

## 🛠️ Development

### Backend (API)
- **Framework**: ASP.NET Core 9.0
- **Database**: SQLite with Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **Security**: Comprehensive security implementation

### Frontend (JS Client)
- **Technology**: Vanilla JavaScript
- **Styling**: CSS3
- **Architecture**: Single Page Application (SPA)

## 📝 API Endpoints

### Authentication
- `POST /api/accounts/register` - Register new user (Admin only)
- `POST /api/accounts/login` - User login
- `POST /api/accounts/logout` - User logout
- `GET /api/accounts/profile` - Get user profile

### To-Do Management
- `GET /api/todos` - Get user's todos
- `POST /api/todos` - Create new todo
- `PUT /api/todos/{id}` - Update todo
- `DELETE /api/todos/{id}` - Delete todo

## 🔧 Configuration

The application uses the following configuration files:
- `api/appsettings.json` - General configuration
- `api/appsettings.Development.json` - Development-specific settings

## 📊 Database

The application uses SQLite with automatic migrations. The database file (`eshop.db`) will be created automatically on first run.

## 🚀 Deployment

The application is ready for production deployment with:
- Enhanced security measures
- Rate limiting
- Input validation
- Secure headers
- Role-based access control
