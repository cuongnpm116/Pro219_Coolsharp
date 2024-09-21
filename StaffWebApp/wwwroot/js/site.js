// lấy địa chỉ host phục vụ việc redirect
const host = "https://" + window.location.host;
const errorBorderStyle = "2px solid red";

/**
 * hiện thị thông báo bằng sweetalert2
 * @param {any} text - nội dung thông báo 
 * @param {any} icon - loại icon của thông báo - lên document của sweetalert để tìm hiểu thêm
 */
function showNotification(text, icon) {
    Swal.fire({
        title: "Thông báo",
        text: text,
        icon: icon === "" ? "error" : icon,
    });
}

// lấy returnUrl nếu login có chứa tham số này
function getReturnUrl() {
    // Phân tích URL để lấy query string
    const urlParams = new URLSearchParams(window.location.search);
    // Lấy giá trị của tham số 'returnUrl'
    // phải đúng là ReturnUrl vì nó không ignore case :/
    const returnUrl = urlParams.get('ReturnUrl');
    return returnUrl;
}


// giống string.IsNullOrWhiteSpace của c#
function isNullOrWhiteSpace(input) {
    return !input || input.trim().length === 0;
}

// xóa viền của input có lỗi và xóa message
function clearValidation(elementId, errorElementId) {
    const inputElement = document.getElementById(elementId);
    const errorElement = document.getElementById(errorElementId);
    inputElement.style.border = "";
    errorElement.textContent = "";
}


/**
 * dùng để validate input có type = text
 * @param {string} elementValue - giá trị của input
 * @param {HTMLElement} inputElement - thẻ input có giá trị cần validate
 * @param {HTMLElement} inputErrorElement - thẻ chứa message khi có lỗi sau khi validate 
 * @param {string} message - custom message cho từng input 
 * @returns bool - true nếu input có value - false nếu null hoặc whitespace
 */
function validateTextInput(elementValue, inputElement, inputErrorElement, message) {
    inputElement.style.border = "";
    inputErrorElement.textContext = "";
    const isValid = isNullOrWhiteSpace(elementValue);
    if (isValid) {
        inputElement.style.border = errorBorderStyle;
        inputErrorElement.textContent = "Vui lòng nhập " + message;
        return false;
    } else {
        return true
    }
}

/**
 * Validate if a string is a valid email address.
 * @param {string} email - The email address to validate.
 * @returns {boolean} True if the email is valid, false otherwise.
 */
function validateEmail(email) {
    // Regular expression for basic email validation
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

