document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".toggle-password").forEach(function (button) {
        button.addEventListener("click", function () {
            const input = this.closest(".input-group").querySelector("input");
            const icon = this.querySelector("i");
            if (input.type === "password") {
                input.type = "text";
                icon.classList.remove("fa-eye");
                icon.classList.add("fa-eye-slash");
            } else {
                input.type = "password";
                icon.classList.remove("fa-eye-slash");
                icon.classList.add("fa-eye");
            }
        });
    });
});

async function buttonRegisterClick() {
    const userInfo = getUserInfo();
    const validationResult = await validateUserInfo(userInfo);
    if (!validationResult) {
        return;
    }
    await registerUser(userInfo);
}

function getUserInfo() {
    return {
        firstName: document.getElementById("firstName").value.trim(),
        lastName: document.getElementById("lastName").value.trim(),
        gender: parseInt(document.getElementById("gender").value),
        username: document.getElementById("username").value.trim(),
        password: document.getElementById("password").value,
        emailAddress: document.getElementById("emailAddress").value.trim(),
        userType: 1,
    };
}

async function validateUserInfo(userInfo) {
    const firstNameElement = document.getElementById("firstName");
    const lastNameElement = document.getElementById("lastName");
    const usernameElement = document.getElementById("username");
    const passwordElement = document.getElementById("password");
    const confirmPasswordElement = document.getElementById("confirmPassword");
    const emailElement = document.getElementById("emailAddress");

    const firstNameErrorElement = document.getElementById("firstNameError");
    const lastNameErrorElement = document.getElementById("lastNameError");
    const usernameErrorElement = document.getElementById("usernameError");
    const passwordErrorElement = document.getElementById("passwordError");
    const confirmPasswordErrorElement = document.getElementById("confirmPasswordError");
    const emailErrorElement = document.getElementById("emailError");

    const checkFirstName = validateTextInput(userInfo.firstName, firstNameElement, firstNameErrorElement, "tên");
    const checkLastName = validateTextInput(userInfo.lastName, lastNameElement, lastNameErrorElement, "họ");
    let checkUsername = validateTextInput(userInfo.username, usernameElement, usernameErrorElement, "tài khoản");
    if (checkUsername) {
        checkUsername = await isUniqueUsername(userInfo.username);
        if (checkUsername === true) {
            usernameElement.style.border = errorBorderStyle;
            usernameErrorElement.textContext = "Tên tài khoản đã được sử dụng. Vui lòng sử dụng tên tài khoản khác"
        }
    }

    const checkPassword = validateTextInput(userInfo.password, passwordElement, passwordErrorElement, "mật khẩu");
    const checkConfirmPassword = validateTextInput(
        confirmPasswordElement.value.trim(),
        confirmPasswordElement,
        confirmPasswordErrorElement,
        "xác nhận mật khẩu");

    const checkSamePassword = userInfo.password === confirmPasswordElement.value;
    if (!checkSamePassword) {
        confirmPasswordElement.style.border = errorBorderStyle;
        confirmPasswordErrorElement.textContent =
            "Xác nhận mật khẩu không trùng với mật khẩu";
    }

    let checkEmail = validateTextInput(userInfo.emailAddress, emailElement, emailErrorElement, "email");
    if (checkEmail) {
        checkEmail = validateEmail(userInfo.emailAddress);
        if (checkEmail === false) {
            emailElement.style.border = errorBorderStyle;
            emailErrorElement.textContent = "Vui lòng nhập đúng định dạng email";
        }
        checkEmail = await isUniqueEmail(userInfo.emailAddress);
        if (checkEmail === true) {
            emailElement.style.border = errorBorderStyle;
            emailErrorElement.textContent = "Email này đã được sử dụng. Vui lòng nhập 1 email khác";
        }
    }

    const result = (checkFirstName && checkLastName && !checkUsername && checkPassword && checkConfirmPassword && !checkEmail && checkSamePassword);
    return result;
}

async function registerUser(userInfo) {
    let response = await fetch(host + "/auth/register", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(userInfo),
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

async function isUniqueEmail(emailAddress) {
    const response = await fetch(host + "/is-unique-email?email=" + emailAddress, { method: "GET", });
    const result = await response.json();
    return result;
}

async function isUniqueUsername(username) {
    const response = await fetch(host + "/is-unique-username?username=" + username, { method: "GET", });
    const result = await response.json();
    return result;
}

document.getElementById("firstName").addEventListener("change", () => clearValidation("firstName", "firstNameError"));
document.getElementById("lastName").addEventListener("change", () => clearValidation("lastName", "lastNameError"));
document.getElementById("username").addEventListener("change", () => clearValidation("username", "usernameError"));
document.getElementById("password").addEventListener("change", () => clearValidation("password", "passwordError"));
document.getElementById("confirmPassword").addEventListener("change", () => clearValidation("confirmPassword", "confirmPasswordError"));
document.getElementById("emailAddress").addEventListener("change", () => clearValidation("emailAddress", "emailError"));
