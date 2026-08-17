document.addEventListener("DOMContentLoaded", function () {

    const notificationButton =
        document.getElementById("notificationButton");

    const notificationDropdown =
        document.getElementById("notificationDropdown");


    if (!notificationButton || !notificationDropdown)
        return;


    notificationButton.addEventListener("click", function (event) {

        event.stopPropagation();

        notificationDropdown.classList.toggle("show");

    });


    document.addEventListener("click", function (event) {

        if (!event.target.closest(".notification-wrapper")) {

            notificationDropdown.classList.remove("show");

        }

    });

});