// API Configuration
const baseApiUrl = 'http://localhost:5000/api';

// DOM Elements
const showLoginBtn = document.getElementById('showLogin');
const showRegisterBtn = document.getElementById('showRegister');
const showTodoBtn = document.getElementById('showTodo');
const logoutBtn = document.getElementById('logout');

const loginSection = document.getElementById('loginSection');
const registerSection = document.getElementById('registerSection');
const todoSection = document.getElementById('todoSection');

const loginForm = document.getElementById('loginForm');
const registerForm = document.getElementById('registerForm');
const addTodoForm = document.getElementById('addTodoForm');

const loginMessage = document.getElementById('loginMessage');
const registerMessage = document.getElementById('registerMessage');
const todoList = document.getElementById('todoList');

// State
let isAuthenticated = false;
let todos = [];

// Event Listeners
showLoginBtn.addEventListener('click', () => showSection('login'));
showRegisterBtn.addEventListener('click', () => showSection('register'));
showTodoBtn.addEventListener('click', () => showSection('todo'));
logoutBtn.addEventListener('click', logout);

loginForm.addEventListener('submit', handleLogin);
registerForm.addEventListener('submit', handleRegister);
addTodoForm.addEventListener('submit', handleAddTodo);

// Initialize app
document.addEventListener('DOMContentLoaded', () => {
    // Start with login section visible
    showSection('login');
    // Then check auth status
    checkAuthStatus();
});

// Navigation
function showSection(section) {
    // Hide all sections
    loginSection.style.display = 'none';
    registerSection.style.display = 'none';
    todoSection.style.display = 'none';

    // Remove active class from all nav buttons
    document.querySelectorAll('.nav-btn').forEach(btn => btn.classList.remove('active'));

    // Show selected section
    switch (section) {
        case 'login':
            loginSection.style.display = 'block';
            showLoginBtn.classList.add('active');
            break;
        case 'register':
            registerSection.style.display = 'block';
            showRegisterBtn.classList.add('active');
            break;
        case 'todo':
            todoSection.style.display = 'block';
            showTodoBtn.classList.add('active');
            if (isAuthenticated) {
                loadTodos();
            }
            break;
    }
}

// Authentication functions
async function checkAuthStatus() {
    try {
        const response = await fetch(`${baseApiUrl}/todos`, {
            method: 'GET',
            credentials: 'include'
        });

        if (response.ok) {
            setAuthenticated(true);
        } else {
            setAuthenticated(false);
        }
    } catch (error) {
        console.error('Auth check failed:', error);
        setAuthenticated(false);
    }
}

function setAuthenticated(authenticated) {
    isAuthenticated = authenticated;
    
    if (authenticated) {
        showLoginBtn.style.display = 'none';
        showRegisterBtn.style.display = 'none';
        showTodoBtn.style.display = 'inline-block';
        logoutBtn.style.display = 'inline-block';
        // Only show todo section if we're not already on it
        if (todoSection.style.display === 'none') {
            showSection('todo');
        }
    } else {
        showLoginBtn.style.display = 'inline-block';
        showRegisterBtn.style.display = 'inline-block';
        showTodoBtn.style.display = 'none';
        logoutBtn.style.display = 'none';
        showSection('login');
    }
}

async function handleLogin(e) {
    e.preventDefault();
    
    const email = document.getElementById('loginEmail').value;
    const password = document.getElementById('loginPassword').value;

    try {
        const response = await fetch(`${baseApiUrl}/accounts/login`, {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ email, password })
        });

        if (response.ok) {
            showMessage(loginMessage, 'Login successful!', 'success');
            setAuthenticated(true);
            loginForm.reset();
        } else {
            const error = await response.text();
            showMessage(loginMessage, 'Login failed. Please check your credentials.', 'error');
        }
    } catch (error) {
        console.error('Login error:', error);
        showMessage(loginMessage, 'Login failed. Please try again.', 'error');
    }
}

async function handleRegister(e) {
    e.preventDefault();
    
    const firstName = document.getElementById('registerFirstName').value;
    const lastName = document.getElementById('registerLastName').value;
    const email = document.getElementById('registerEmail').value;
    const password = document.getElementById('registerPassword').value;
    const confirmPassword = document.getElementById('confirmPassword').value;

    if (password !== confirmPassword) {
        showMessage(registerMessage, 'Passwords do not match.', 'error');
        return;
    }

    try {
        const response = await fetch(`${baseApiUrl}/accounts/register`, {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                firstName,
                lastName,
                email,
                password,
                confirmPassword
            })
        });

        if (response.ok) {
            showMessage(registerMessage, 'Account created successfully! You can now login.', 'success');
            registerForm.reset();
            showSection('login');
        } else {
            const error = await response.text();
            showMessage(registerMessage, 'Registration failed. Please try again.', 'error');
        }
    } catch (error) {
        console.error('Registration error:', error);
        showMessage(registerMessage, 'Registration failed. Please try again.', 'error');
    }
}

async function logout() {
    try {
        const response = await fetch(`${baseApiUrl}/accounts/logout`, {
            method: 'POST',
            credentials: 'include'
        });

        if (response.ok) {
            setAuthenticated(false);
            todos = [];
            showMessage(loginMessage, 'Logged out successfully.', 'success');
        }
    } catch (error) {
        console.error('Logout error:', error);
    }
}

// Todo functions
async function loadTodos() {
    try {
        const response = await fetch(`${baseApiUrl}/todos`, {
            method: 'GET',
            credentials: 'include'
        });
        
        if (response.ok) {
            todos = await response.json();
            renderTodos();
        } else if (response.status === 401) {
            setAuthenticated(false);
        }
    } catch (error) {
        console.error('Load todos error:', error);
    }
}

async function handleAddTodo(e) {
    e.preventDefault();
    
    const title = document.getElementById('todoTitle').value;
    const description = document.getElementById('todoDescription').value;

    try {
        const response = await fetch(`${baseApiUrl}/todos`, {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ title, description })
        });

        if (response.ok) {
            const newTodo = await response.json();
            todos.unshift(newTodo);
            renderTodos();
            addTodoForm.reset();
        }
    } catch (error) {
        console.error('Add todo error:', error);
    }
}

async function toggleTodo(id, isCompleted) {
    try {
        const response = await fetch(`${baseApiUrl}/todos/${id}`, {
            method: 'PUT',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ isCompleted: !isCompleted })
        });

        if (response.ok) {
            const todo = todos.find(t => t.id === id);
            if (todo) {
                todo.isCompleted = !isCompleted;
                todo.completedAt = todo.isCompleted ? new Date().toISOString() : null;
                renderTodos();
            }
        }
    } catch (error) {
        console.error('Toggle todo error:', error);
    }
}

async function deleteTodo(id) {
    if (!confirm('Are you sure you want to delete this task?')) {
        return;
    }

    try {
        const response = await fetch(`${baseApiUrl}/todos/${id}`, {
            method: 'DELETE',
            credentials: 'include'
        });

        if (response.ok) {
            todos = todos.filter(t => t.id !== id);
            renderTodos();
        }
    } catch (error) {
        console.error('Delete todo error:', error);
    }
}

function renderTodos() {
    todoList.innerHTML = '';

    if (todos.length === 0) {
        todoList.innerHTML = '<p style="text-align: center; color: #718096; font-style: italic;">No tasks yet. Add your first task above!</p>';
        return;
    }

    todos.forEach(todo => {
        const todoElement = createTodoElement(todo);
        todoList.appendChild(todoElement);
    });
}

function createTodoElement(todo) {
    const div = document.createElement('div');
    div.className = `todo-item ${todo.isCompleted ? 'completed' : ''}`;

    const createdAt = new Date(todo.createdAt).toLocaleDateString();
    const status = todo.isCompleted ? 'completed' : 'pending';

    div.innerHTML = `
        <div class="todo-header">
            <div class="todo-checkbox">
                <input type="checkbox" ${todo.isCompleted ? 'checked' : ''} 
                       onchange="toggleTodo(${todo.id}, ${todo.isCompleted})">
                <span class="todo-title">${escapeHtml(todo.title)}</span>
            </div>
            <div class="todo-actions">
                <button class="btn btn-danger" onclick="deleteTodo(${todo.id})">Delete</button>
            </div>
        </div>
        ${todo.description ? `<div class="todo-description">${escapeHtml(todo.description)}</div>` : ''}
        <div class="todo-meta">
            <span class="todo-date">Created: ${createdAt}</span>
            <span class="todo-status ${status}">${status}</span>
        </div>
    `;

    return div;
}

// Utility functions
function showMessage(element, message, type) {
    element.textContent = message;
    element.className = `message ${type}`;
    
    setTimeout(() => {
        element.textContent = '';
        element.className = 'message';
    }, 5000);
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}
