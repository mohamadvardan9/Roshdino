document.addEventListener("DOMContentLoaded", function () {

    const searchButton =
        document.getElementById("searchButton");

    const searchOverlay =
        document.getElementById("searchOverlay");

    const searchClose =
        document.getElementById("searchClose");

    const searchInput =
        document.getElementById("globalSearchInput");

    const searchContent =
        document.getElementById("searchContent");


    if (!searchButton ||
        !searchOverlay ||
        !searchClose ||
        !searchInput ||
        !searchContent) {
        return;
    }


    let debounceTimer;


    // =========================
    // Open Search
    // =========================

    function openSearch() {

        searchOverlay.classList.add("show");

        setTimeout(() => {
            searchInput.focus();
        }, 100);
    }


    // =========================
    // Close Search
    // =========================

    function closeSearch() {

        searchOverlay.classList.remove("show");

        searchInput.value = "";

        clearTimeout(debounceTimer);

        showPlaceholder();
    }


    // =========================
    // Placeholder
    // =========================

    function showPlaceholder() {

        searchContent.innerHTML = `
            <div class="search-placeholder">

                <i class="bi bi-search"></i>

                <p>
                    برای جستجو شروع به تایپ کنید
                </p>

                <span>
                    مقالات، محصولات، دسته بندی و فرستندگان پیام
                </span>

            </div>
        `;
    }


    // =========================
    // Loading
    // =========================

    function showLoading() {

        searchContent.innerHTML = `
            <div class="search-loading">

                <div class="spinner-border spinner-border-sm"
                     role="status">
                </div>

                <span>
                    در حال جستجو...
                </span>

            </div>
        `;
    }


    // =========================
    // Empty
    // =========================

    function showEmpty(query) {

        searchContent.innerHTML = `
            <div class="search-empty">

                <i class="bi bi-search"></i>

                <span>
                    نتیجه‌ای برای «${escapeHtml(query)}» پیدا نشد
                </span>

            </div>
        `;
    }


    // =========================
    // Search
    // =========================

    async function performSearch(query) {

        showLoading();

        try {

            const response = await fetch(
                `/Search/Index?query=${encodeURIComponent(query)}`
            );


            if (!response.ok) {
                throw new Error("Search request failed.");
            }


            const results = await response.json();


            if (!results || results.length === 0) {

                showEmpty(query);

                return;
            }


            renderResults(results);

        }
        catch (error) {

            console.error(error);

            searchContent.innerHTML = `
                <div class="search-empty">

                    <i class="bi bi-exclamation-circle"></i>

                    <span>
                        خطایی هنگام جستجو رخ داد
                    </span>

                </div>
            `;
        }
    }


    function highlightText(text, query) {

        if (!text || !query) {
            return escapeHtml(text);
        }

        const safeText = escapeHtml(text);
        const safeQuery = escapeHtml(query);

        const regex = new RegExp(
            `(${safeQuery.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')})`,
            "gi"
        );

        return safeText.replace(
            regex,
            '<mark class="search-highlight">$1</mark>'
        );
    }

    // =========================
    // Render Results
    // =========================

    function renderResults(results) {

        searchContent.innerHTML = "";

        const query = searchInput.value.trim();


        results.forEach(result => {

            const item = document.createElement("a");

            item.className = "search-result";

            item.href = result.url;


            item.innerHTML = `

            <div class="search-result-icon">

                <i class="bi ${escapeHtml(result.icon)}"></i>

            </div>


            <div class="search-result-info">

                <span class="search-result-title">

                    ${highlightText(result.title, query)}

                </span>


                <span class="search-result-type">

                    ${escapeHtml(result.type)}

                </span>

            </div>


            <i class="bi bi-chevron-left"></i>

        `;


            searchContent.appendChild(item);

        });
    }


    // =========================
    // Escape HTML
    // =========================

    function escapeHtml(value) {

        const div = document.createElement("div");

        div.textContent = value ?? "";

        return div.innerHTML;
    }


    // =========================
    // Input
    // =========================

    searchInput.addEventListener(
        "input",
        function () {

            const query =
                searchInput.value.trim();


            clearTimeout(debounceTimer);


            if (!query) {

                showPlaceholder();

                return;
            }


            if (query.length < 2) {

                searchContent.innerHTML = `
                    <div class="search-empty">

                        <i class="bi bi-keyboard"></i>

                        <span>
                            حداقل ۲ حرف وارد کنید
                        </span>

                    </div>
                `;

                return;
            }


            debounceTimer = setTimeout(() => {

                performSearch(query);

            }, 300);

        }
    );


    // =========================
    // Button
    // =========================

    searchButton.addEventListener(
        "click",
        openSearch
    );


    searchClose.addEventListener(
        "click",
        closeSearch
    );


    // =========================
    // Click Outside
    // =========================

    searchOverlay.addEventListener(
        "click",
        function (event) {

            if (event.target === searchOverlay) {

                closeSearch();

            }

        }
    );


    // =========================
    // ESC
    // =========================

    document.addEventListener(
        "keydown",
        function (event) {

            if (event.key === "Escape" &&
                searchOverlay.classList.contains("show")) {

                closeSearch();

            }

        }
    );


    // =========================
    // CTRL + K
    // =========================

    document.addEventListener(
        "keydown",
        function (event) {

            if (
                (event.ctrlKey || event.metaKey) &&
                event.key.toLowerCase() === "k"
            ) {

                event.preventDefault();

                openSearch();

            }

        }
    );

});