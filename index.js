const productList = document.querySelector('#productList');

document.querySelector('#displayProducts').addEventListener('click', listProducts);
document.querySelector('#login').addEventListener('click', login);
document.querySelector('#logout').addEventListener('click', logout);
document.querySelector('#badlogin').addEventListener('click', badlogin);

const baseApiUrl = 'https://localhost:5001/api';

async function listProducts() {
  console.log('List products');
  const cookie = await cookieStore.get('.AspNetCore.Identity.Application');
  console.log('Cookie', cookie);

  const response = await fetch(`${baseApiUrl}/products`, {
    method: 'GET',
    mode: 'cors',
    credentials: 'include',
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
    credentials: 'include',
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

  const response = await fetch(`${baseApiUrl}/login?useCookies=true`, {
    method: 'POST',
    credentials: 'include',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ email: 'michael@gmail.com', password: 'Pa$$w0rd' }),
  });

  console.log(response);

  productList.innerHTML = '';
}

async function logout() {
  console.log('Log out');

  const response = await fetch(`${baseApiUrl}/accounts/logout`, {
    method: 'POST',
    credentials: 'include',
  });

  console.log(response);
  productList.innerHTML = '';
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
