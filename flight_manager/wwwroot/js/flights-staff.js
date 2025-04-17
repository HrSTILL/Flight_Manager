function prevPage() {
    const currentPage = parseInt(document.getElementById("pageNumber").innerText.split(' ')[0], 10);
    if (currentPage > 1) {
        goToPage(currentPage - 1);
    }
}

function nextPage() {
    const totalPages = parseInt(document.getElementById("pageNumber").innerText.split(' ')[2], 10);
    const currentPage = parseInt(document.getElementById("pageNumber").innerText.split(' ')[0], 10);
    if (currentPage < totalPages) {
        goToPage(currentPage + 1);
    }
}

function goToPage(page) {
    const recordsPerPage = document.getElementById("recordsPerPage").value;
    const filterType = document.getElementById("filter").value;
    window.location.href = `/Staff/Flights_Staff?page=${page}&recordsPerPage=${recordsPerPage}&filterType=${filterType}`;
}

// Това е за филтъра.
function applyFilter() {
    const filter = document.getElementById("filter").value;
    const recordsPerPage = document.getElementById("recordsPerPage").value;
    window.location.href = `/Staff/Flights_Staff?filterType=${filter}&recordsPerPage=${recordsPerPage}`;
}

// Това е за копчето с детайли на стафа
function toggleDetails(button) {
    const detailsRow = button.closest('tr').nextElementSibling;
    detailsRow.style.display = detailsRow.style.display === 'none' ? 'table-row' : 'none';
}

function togglePassangers(button) {
    const detailsRow = button.closest('tr').nextElementSibling;
    detailsRow.style.display = detailsRow.style.display === 'none' ? 'table-row' : 'none';
}

// Записи на страница
function updateRecordsPerPage() {
    const recordsPerPage = document.getElementById("recordsPerPage").value;
    const filterType = document.getElementById("filter").value;

    window.location.href = `/Staff/Flights_Staff?page=1&recordsPerPage=${recordsPerPage}&filterType=${filterType}`;
}



var customModal = document.getElementById("customPassengerModal");
var customPassengerListContainer = document.getElementById("customPassengerListContainer");
var customCloseModal = document.getElementsByClassName("custom-close-btn")[0];

function togglePassengers(flightId) {
    console.log(`Passengers button clicked for flight ${flightId}`);

    customModal.style.display = "block";
    document.body.classList.add("modal-open");
    customPassengerListContainer.innerHTML = "Loading passengers..."; 

    fetch(`/Staff/GetPassengers?flightId=${flightId}`)
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                var passengers = data.passengers;
                var passengerListHtml = '';

                passengers.forEach(function (passenger) {
                    passengerListHtml += `<div>Full Name: ${passenger.First_Name} ${passenger.Middle_Name} ${passenger.Last_Name}</div>`;
                    passengerListHtml += `<div>EGN: ${passenger.EGN}</div>`;
                    passengerListHtml += `<div>Phone: ${passenger.Phone_Number}</div>`;
                    passengerListHtml += '<hr>';
                });

                customPassengerListContainer.innerHTML = passengerListHtml;
            } else {
                customPassengerListContainer.innerHTML = 'No passengers found for this flight.';
            }
        })
        .catch(error => {
            customPassengerListContainer.innerHTML = 'Error loading passengers.';
            console.error('Error fetching passengers:', error);
        });
}

customCloseModal.onclick = function () {
    customModal.style.display = "none";
    document.body.classList.remove("modal-open");

}

window.onclick = function (event) {
    if (event.target == customModal) {
        customModal.style.display = "none";
        document.body.classList.remove("modal-open");   
    }
}









