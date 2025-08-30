const productList = document.querySelector('#productList');
const todoList = document.querySelector('#todoList');

document.querySelector('#displayProducts').addEventListener('click', listProducts);
document.querySelector('#login').addEventListener('click', login);
document.querySelector('#logout').addEventListener('click', logout);
document.querySelector('#badlogin').addEventListener('click', badlogin);
document.querySelector('#addTodo').addEventListener('click', addTodo);

const baseApiUrl = 'https://localhost:5001/api';

let authToken = localStorage.getItem('authToken') || null;
let csrfToken = null;

async function listProducts() {
  console.log('List products');
  const response = await fetch(`${baseApiUrl}/products`, {
    method: 'GET',
    mode: 'cors',
    headers: authHeaders(),
  });

  if (response.ok) {
    const result = await response.json();
    console.log(result);
    displayProducts(result.data);
  } else {
    if (response.status === 401) displayError();
  }
}

async function badlogin() {
  console.log('Bad Log In');

  const response = await fetch(`${baseApiUrl}/accounts/badlogin`, {
    method: 'POST',
    headers: authHeaders(),
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ email: 'michael@gmail.com', password: 'Pa$$w0rd' }),
  });

  console.log(response);

  productList.innerHTML = '';
}

async function login() {
  console.log('Log In');

  const response = await fetch(`${baseApiUrl}/auth/login`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ email: 'michael@gmail.com', password: 'Pa$$w0rd' }),
  });

  if (response.ok) {
    const result = await response.json();
    authToken = result.token;
    localStorage.setItem('authToken', authToken);

    // decode token to read csrf claim (simple, non-validating decode)
    const payload = JSON.parse(atob(authToken.split('.')[1]));
    csrfToken = payload['csrf'];
    productList.innerHTML = '';
    await listTodos();
  } else {
    console.log('Login failed');
  }
}

async function logout() {
  console.log('Log out');
  authToken = null;
  csrfToken = null;
  localStorage.removeItem('authToken');
  productList.innerHTML = '';
  todoList.innerHTML = '';
}

function displayProducts(products) {
  productList.innerHTML = '';

  for (let product of products) {
    const div = document.createElement('div');
    div.textContent = product.name;

    productList.appendChild(div);
  }
}

function displayError() {
  productList.innerHTML = '<h2 style="color:red;">UNAUTHORIZED</h2>';
}

function authHeaders() {
  const headers = {};
  if (authToken) {
    headers['Authorization'] = `Bearer ${authToken}`;
  }
  if (csrfToken) {
    headers['X-CSRF-Token'] = csrfToken;
  }
  return headers;
}

async function listTodos() {
  const response = await fetch(`${baseApiUrl}/todos`, {
    method: 'GET',
    headers: authHeaders(),
  });
  if (response.ok) {
    const result = await response.json();
    renderTodos(result.data);
  } else if (response.status === 401) {
    displayError();
  }
}

async function addTodo() {
  const title = document.querySelector('#todoTitle').value.trim();
  const description = document.querySelector('#todoDesc').value.trim();
  if (!title) return;
  const response = await fetch(`${baseApiUrl}/todos`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json', ...authHeaders() },
    body: JSON.stringify({ title, description })
  });
  if (response.ok) {
    await listTodos();
  }
}

async function toggleTodo(id, todo) {
  const response = await fetch(`${baseApiUrl}/todos/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json', ...authHeaders() },
    body: JSON.stringify({ title: todo.title, description: todo.description, isCompleted: !todo.isCompleted })
  });
  if (response.ok) await listTodos();
}

async function deleteTodo(id) {
  const response = await fetch(`${baseApiUrl}/todos/${id}`, {
    method: 'DELETE',
    headers: authHeaders()
  });
  if (response.ok) await listTodos();
}

function renderTodos(todos) {
  todoList.innerHTML = '';
  for (const t of todos) {
    const div = document.createElement('div');
    div.textContent = `${t.title} ${t.isCompleted ? '(klar)' : ''}`;
    const toggleBtn = document.createElement('button');
    toggleBtn.textContent = t.isCompleted ? 'Återställ' : 'Klar';
    toggleBtn.addEventListener('click', () => toggleTodo(t.id, t));
    const delBtn = document.createElement('button');
    delBtn.textContent = 'Ta bort';
    delBtn.addEventListener('click', () => deleteTodo(t.id));
    div.appendChild(toggleBtn);
    div.appendChild(delBtn);
    todoList.appendChild(div);
  }
}
