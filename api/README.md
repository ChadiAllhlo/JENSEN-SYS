# API - Backend Application

This is the backend API for the JENSEN-SYS application, built with ASP.NET Core 9.0 and Entity Framework Core.

## 📁 Structure

```
api/
├── Controllers/        # API endpoints
│   ├── AccountsController.cs
│   └── TodosController.cs
├── Data/              # Database context and seeding
│   ├── AppDbContext.cs
│   └── InitializeDb.cs
├── Models/            # Entity models
│   └── User.cs
├── ViewModels/        # Data transfer objects
│   ├── UserRegisterViewModel.cs
│   ├── LoginRequest.cs
│   └── TodoRequestModels.cs
├── Migrations/        # Entity Framework migrations
├── Properties/        # Project properties
├── Program.cs         # Application entry point
├── api.csproj         # Project file
├── appsettings.json   # Configuration
└── e-shop.sln         # Solution file
```

## 🚀 Features

### Authentication & Authorization
- **User Registration**: Admin-only account creation
- **User Login/Logout**: Secure authentication with cookies
- **Role-based Authorization**: Admin and User roles
- **Profile Management**: User profile retrieval

### To-Do Management
- **CRUD Operations**: Create, Read, Update, Delete todos
- **User Isolation**: Each user sees only their own todos
- **Data Validation**: Comprehensive input validation
- **Real-time Updates**: Immediate database updates

### Security Features
- **Input Sanitization**: HTML sanitization for XSS prevention
- **Rate Limiting**: Global API protection
- **Account Lockout**: Brute force protection
- **Security Headers**: Modern security standards
- **Request Size Limits**: DoS protection

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 9.0
- **Database**: SQLite with Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **Security**: Ganss.Xss for HTML sanitization
- **Validation**: Data Annotations and Fluent Validation

## 🔐 Security Implementation

### Input Validation
```csharp
[StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")]
```

### HTML Sanitization
```csharp
private readonly HtmlSanitizer _htmlSanitizer = new();
model.Email = _htmlSanitizer.Sanitize(model.Email);
```

### Rate Limiting
```csharp
options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    RateLimitPartition.GetFixedWindowLimiter("GlobalLimiter", ...));
```

### Security Headers
- Content Security Policy (CSP)
- X-Content-Type-Options
- X-Frame-Options
- X-XSS-Protection
- Strict-Transport-Security

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

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=eshop.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### Development Settings
- HTTPS development certificate
- Detailed error pages
- SQLite database

## 📊 Database

### Entity Framework Core
- **Code-First Approach**: Models define database schema
- **Migrations**: Version-controlled database changes
- **SQLite**: Lightweight, file-based database
- **Seeding**: Initial data population

### Models
- **User**: Identity user with custom properties
- **Todo**: Task management entity
- **IdentityRole**: Role-based authorization

## 🚀 Running the Application

### Prerequisites
- .NET 9.0 SDK
- SQLite (included with .NET)

### Development
1. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

2. **Run migrations:**
   ```bash
   dotnet ef database update
   ```

3. **Start the application:**
   ```bash
   dotnet run
   ```

4. **Access the API:**
   - API: `https://localhost:5001/api`
   - Frontend: `https://localhost:5001`

### Production
1. **Build the application:**
   ```bash
   dotnet publish -c Release
   ```

2. **Deploy to your hosting environment**

## 🔍 Development Workflow

### Adding New Features
1. Create/update models in `Models/`
2. Add ViewModels in `ViewModels/`
3. Implement controllers in `Controllers/`
4. Update database with migrations
5. Test API endpoints

### Database Changes
1. Modify models
2. Create migration: `dotnet ef migrations add MigrationName`
3. Update database: `dotnet ef database update`

### Security Updates
1. Update validation attributes
2. Add security headers as needed
3. Test with security tools
4. Update documentation

## 🧪 Testing

### API Testing
- Use tools like Postman or curl
- Test all endpoints with valid/invalid data
- Verify authentication and authorization
- Check rate limiting behavior

### Security Testing
- Test input validation
- Verify XSS protection
- Check CSRF protection
- Test rate limiting

## 📈 Performance

### Optimizations
- **Entity Framework**: Efficient querying
- **Rate Limiting**: API protection
- **Static Files**: Efficient serving
- **Caching**: Browser caching headers

### Monitoring
- Application logging
- Performance metrics
- Error tracking
- Security monitoring

## 🔧 Troubleshooting

### Common Issues
1. **Database Connection**: Check connection string
2. **Migration Errors**: Ensure migrations are up to date
3. **Authentication Issues**: Verify cookie settings
4. **CORS Errors**: Check CORS configuration

### Logs
- Check application logs for errors
- Use `dotnet ef database update --verbose` for migration issues
- Monitor security headers in browser dev tools
