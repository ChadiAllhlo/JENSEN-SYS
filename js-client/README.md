# JS Client - Frontend Application

This is the frontend client for the JENSEN-SYS application, built with vanilla JavaScript, HTML, and CSS.

## 📁 Structure

```
js-client/
├── index.html         # Main application page
├── app.js            # Client-side JavaScript logic
├── styles.css        # Application styling
├── package.json      # Project metadata
└── README.md         # This file
```

## 🚀 Features

### User Management
- **Login/Logout**: Secure authentication with cookies
- **Role-based UI**: Different interfaces for Admin and User roles
- **Profile Management**: View user profile and roles
- **User Registration**: Admin-only account creation with role assignment

### To-Do List Management
- **Create Todos**: Add new tasks with title and description
- **View Todos**: Display all user's tasks
- **Update Todos**: Mark tasks as complete/incomplete
- **Delete Todos**: Remove tasks from the list
- **Real-time Updates**: Immediate UI updates after operations

## 🛠️ Technology Stack

- **HTML5**: Semantic markup
- **CSS3**: Modern styling with responsive design
- **Vanilla JavaScript**: No frameworks, pure ES6+ JavaScript
- **Fetch API**: Modern HTTP client for API communication

## 🔐 Security Features

- **Input Validation**: Client-side validation with server-side verification
- **XSS Prevention**: Proper input sanitization
- **CSRF Protection**: Secure cookie handling
- **Role-based Access**: UI elements hidden based on user roles

## 📱 User Interface

### Login Section
- Email and password fields
- Validation feedback
- Error handling

### Registration Section (Admin Only)
- User registration form
- Role selection dropdown
- Comprehensive validation

### To-Do Section
- Task list display
- Add new task form
- Task management controls
- Completion status indicators

### Profile Section
- User information display
- Role information
- Session management

## 🎨 Styling

The application uses modern CSS features:
- **Flexbox**: Layout management
- **CSS Grid**: Complex layouts
- **CSS Variables**: Theme consistency
- **Responsive Design**: Mobile-friendly interface
- **Modern UI**: Clean, professional appearance

## 🔧 Development

### Running the Application
The frontend is served by the ASP.NET Core API. To run the complete application:

1. Navigate to the API directory:
   ```bash
   cd ../api
   ```

2. Start the API server:
   ```bash
   dotnet run
   ```

3. Access the application at `https://localhost:5001`

### Development Workflow
- Edit HTML, CSS, or JavaScript files
- The API serves static files from this directory
- Changes are reflected immediately on page refresh

## 📝 Code Organization

### app.js
- **Authentication Logic**: Login, logout, profile management
- **To-Do Management**: CRUD operations for tasks
- **UI Management**: Section switching and state management
- **API Communication**: Fetch API calls to backend
- **Error Handling**: User-friendly error messages

### index.html
- **Semantic Structure**: Proper HTML5 elements
- **Form Validation**: HTML5 validation attributes
- **Accessibility**: ARIA labels and semantic markup

### styles.css
- **Component Styles**: Modular CSS organization
- **Responsive Design**: Mobile-first approach
- **Theme System**: Consistent color and spacing

## 🔄 API Integration

The frontend communicates with the following API endpoints:

- `POST /api/accounts/login` - User authentication
- `POST /api/accounts/logout` - User logout
- `GET /api/accounts/profile` - User profile data
- `POST /api/accounts/register` - User registration (Admin only)
- `GET /api/todos` - Retrieve user's todos
- `POST /api/todos` - Create new todo
- `PUT /api/todos/{id}` - Update todo
- `DELETE /api/todos/{id}` - Delete todo

## 🚀 Performance

- **Minimal Dependencies**: No external libraries
- **Efficient DOM Manipulation**: Optimized JavaScript
- **Fast Loading**: Lightweight assets
- **Caching**: Browser caching for static assets

## 🔧 Browser Support

- **Modern Browsers**: Chrome, Firefox, Safari, Edge
- **ES6+ Features**: Arrow functions, template literals, destructuring
- **Fetch API**: Modern HTTP client
- **CSS Grid/Flexbox**: Modern layout features
