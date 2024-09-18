async function buttonLoginClick() {
    const usernameElement = document.getElementById("username");
    const usernameErrorElement = document.getElementById("usernameError");

    const passwordElement = document.getElementById("password");
    const passwordErrorElement = document.getElementById("passwordError");

    let cred = {
        username: usernameElement.value,
        password: passwordElement.value,
    };

    const checkUsername = validateTextInput(
        cred.username,
        usernameElement,
        usernameErrorElement,
        "tài khoản");

    const checkPassword = validateTextInput(
        cred.password,
        passwordElement,
        passwordErrorElement,
        "mật khẩu");

    if (!checkUsername || !checkPassword) {
        return;
    }

    let response = await fetch(host + "/auth/login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(cred),
    });

    let result = await response.text();

    if (response.status === 200) {
        let returnUrl = getReturnUrl();
        if (returnUrl === null) {
            window.location.assign(host + result);
        } else {
            window.location.assign(host + returnUrl);
        }
    } else {
        showNotification(result, "error");
    }
}

document.getElementById('toggle-password')
    .addEventListener('click', togglePasswordInput);

function togglePasswordInput() {
    let passwordInput = document.getElementById('password');
    let passwordIcon = document.getElementById('toggle-password-icon');
    if (passwordInput.type === 'password') {
        passwordInput.type = 'text';
        passwordIcon.classList.remove('fa-eye');
        passwordIcon.classList.add('fa-eye-slash');
    } else {
        passwordInput.type = 'password';
        passwordIcon.classList.remove('fa-eye-slash');
        passwordIcon.classList.add('fa-eye');
    }
}

document.getElementById("username").addEventListener("change", () => clearValidation("username", "usernameError"));
document.getElementById("password").addEventListener("change", () => clearValidation("password", "passwordError"));
