document.addEventListener("DOMContentLoaded", function () {
    const passwordField = document.querySelector("#Password");
    const passwordError = document.createElement("span");
    passwordError.classList.add("text-danger");
    passwordField.parentNode.appendChild(passwordError);

    passwordField.addEventListener("input", function () {
        const password = passwordField.value;
        const regex = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,}$/;

        if (!regex.test(password)) {
            passwordError.textContent = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.";
        } else {
            passwordError.textContent = "";
        }
    });
});
