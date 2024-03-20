document.addEventListener("DOMContentLoaded", function() {
    const toggleButton = document.getElementById("toggleElementsButton");
    const toggleableElements = document.getElementById("toggleableElements");

    toggleButton.addEventListener("click", function() {
        if (toggleableElements.style.display === "none") {
            toggleableElements.style.display = "flex";
        } else {
            toggleableElements.style.display = "none";
        }
    });
});

document.addEventListener("DOMContentLoaded", function() {
    const showFormButton = document.getElementById("showFormButton");
    const calculationForm = document.getElementById("calculationForm");
    const calculateButton = document.getElementById("calculateButton");
    const input1 = document.getElementById("input1");
    const input2 = document.getElementById("input2");
    const resultDisplay = document.getElementById("result");

    showFormButton.addEventListener("click", function() {
        if (calculationForm.style.display === "none") {
            calculationForm.style.display = "block";
        } else {
            calculationForm.style.display = "none";
            resultDisplay.style.display = "none"; // Hide result if form is hidden
        }
    });

    calculateButton.addEventListener("click", function() {
        const value1 = parseFloat(input1.value) || 0;
        const value2 = parseFloat(input2.value) || 0;
        const sum = value1 + value2;
        resultDisplay.textContent = "Sum: " + sum;
        resultDisplay.style.display = "block";
    });
});