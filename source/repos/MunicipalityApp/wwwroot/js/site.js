// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


document.addEventListener("DOMContentLoaded", function () {
    // Engagement / progress logic (same as before)
    const locationEl = document.querySelector("[name='Location']") || document.querySelector("[name='Location']");
    const categoryEl = document.querySelector("[name='Category']");
    const descEl = document.querySelector("[name='Description']");
    const attachmentsInput = document.getElementById("attachmentsInput");
    const progressBar = document.getElementById("progressBar");
    const encouragement = document.getElementById("encouragement");
    const attachmentsList = document.getElementById("attachmentsList");

    function compute() {
        let score = 0;
        const loc = document.querySelector("[name='Location']");
        const cat = document.querySelector("[name='Category']");
        const desc = document.querySelector("[name='Description']");
        if (loc && loc.value.trim().length > 0) score += 30;
        if (cat && cat.value.trim().length > 0) score += 25;
        if (desc && desc.value.trim().length > 20) score += 30;
        if (attachmentsInput && attachmentsInput.files.length > 0) score += 15;
        if (score > 100) score = 100;
        if (progressBar) {
            progressBar.style.width = score + "%";
            progressBar.textContent = score + "%";
        }
        if (encouragement) {
            if (score < 30) encouragement.textContent = "Start with a clear location and category.";
            else if (score < 60) encouragement.textContent = "Great — add a concise description.";
            else if (score < 90) encouragement.textContent = "Almost there — attach photos if available.";
            else encouragement.textContent = "Awesome — ready to submit!";
        }
    }

    ["input", "change"].forEach(evt => {
        document.addEventListener(evt, compute);
    });

    if (attachmentsInput) {
        attachmentsInput.addEventListener("change", function () {
            if (!attachmentsList) return;
            const names = Array.from(attachmentsInput.files).map(f => f.name).join(", ");
            attachmentsList.textContent = names;
            compute();
        });
    }

    compute();
});