import { JsonDiffPage } from './index.js';

const visibleClass = 'visible';
const hiddenClass = 'hidden';

const logout = () => {
    const logoutButton = document.getElementById('logout-link');
    logoutButton.addEventListener('click', (event) => {
        localStorage.removeItem('userLogin');
        JsonDiffPage.unshowJsonDiffPage();
        showPromoPage();
    });
}

const showPromoPage = () => {
    const promoBlock = document.querySelector('.promo');
    if (promoBlock.classList.contains(hiddenClass)) {
        promoBlock.classList.remove(hiddenClass);
        promoBlock.classList.add(visibleClass);
    }

    const loginButton = document.getElementById('login-link');
    const startButton = document.querySelector('.start-link');
    const logoutBlock = document.querySelector('.logout');

    if (localStorage.getItem('userLogin')) {
        logoutBlock.classList.add(visibleClass);
        logoutBlock.classList.remove(hiddenClass);

        const usernameBlock = document.querySelector('.username');
        usernameBlock.innerHTML = localStorage.getItem('userLogin');

        loginButton.classList.add(hiddenClass);
        loginButton.classList.remove(visibleClass);

        startButton.classList.add(visibleClass);
        startButton.classList.remove(hiddenClass);
        startButton.addEventListener('click', (event) => {
            event.preventDefault();

            promoBlock.classList.remove(visibleClass);
            promoBlock.classList.add(hiddenClass);
            JsonDiffPage.showJsonDiffPage();
        });
    }
    else {
        logoutBlock.classList.add(hiddenClass);
        logoutBlock.classList.remove(visibleClass);

        loginButton.classList.add(visibleClass);
        loginButton.classList.remove(hiddenClass);

        startButton.classList.add(hiddenClass);
        startButton.classList.remove(visibleClass);

        loginButton.addEventListener('click', (event) => {
            event.preventDefault();
            showLoginPage();
        });
    }
}

const login = () => {
    const userLogin = document.getElementById('login-input').value;
    console.log(userLogin);
    if (userLogin === '') {
        const errorMessageClassList = loginForm.querySelector('.login-error').classList;
        if (!errorMessageClassList.contains(visibleClass)) {
            errorMessageClassList.remove(hiddenClass);
            errorMessageClassList.add(visibleClass);
        }
    }
    else {
        localStorage.setItem('userLogin', userLogin);

        const loginBlock = document.querySelector('.login-content');
        loginBlock.classList.remove(visibleClass);
        loginBlock.classList.add(hiddenClass);

        showPromoPage();
        console.log('promo showed');
    }
}

const showLoginPage = () => {
    const loginButton = document.getElementById('login-link');
    loginButton.classList.add(hiddenClass);
    const promoBlock = document.querySelector('.promo');
    promoBlock.classList.add(hiddenClass);

    const loginBlock = document.querySelector('.login-content');
    loginBlock.classList.remove(hiddenClass);
    loginBlock.classList.add(visibleClass);

    const loginForm = document.querySelector('.login-form');
    loginForm.addEventListener('submit', (event) => {
        event.preventDefault();
        login();
    })
};

showPromoPage();
logout();