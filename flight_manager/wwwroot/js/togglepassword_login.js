document.addEventListener("DOMContentLoaded", function () {
    const toggleIcon = document.getElementById("toggleIcon");
    if (toggleIcon) {
        toggleIcon.addEventListener("click", togglePassword);
    }
});

function togglePassword() {
    const passwordField = document.getElementById("password");
    const toggleIcon = document.getElementById("toggleIcon");

    if (passwordField && toggleIcon) {
        if (passwordField.type === "password") {
            passwordField.type = "text";
            toggleIcon.src = "/images/closed-eye-login-icon.png";
        } else {
            passwordField.type = "password";
            toggleIcon.src = "/images/opened-eye-login-icon.png";
        }
    }
}
