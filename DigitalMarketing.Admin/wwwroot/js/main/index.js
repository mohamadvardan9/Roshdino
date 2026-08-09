console.log("INDEX JS LOADED");


// ===============================
// Language Dropdown
// ===============================

function toggleLanguageMenu() {

    console.log("LANGUAGE BUTTON CLICKED");

    const dropdown =
        document.querySelector(".language-dropdown");

    if (!dropdown) {
        console.log("language-dropdown NOT FOUND");
        return;
    }

    dropdown.classList.toggle("open");
}


// ===============================
// Change Language
// ===============================

function changeLanguage(language) {

    console.log("Changing language to:", language);

    const returnUrl =
        window.location.pathname + window.location.search;

    const url =
        `/Culture/SetLanguage?culture=${language}&returnUrl=${encodeURIComponent(returnUrl)}`;

    window.location.href = url;
}