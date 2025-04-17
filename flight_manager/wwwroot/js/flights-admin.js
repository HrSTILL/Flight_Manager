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
    window.location.href = `/Admin/Flights_Admin?page=${page}&recordsPerPage=${recordsPerPage}&filterType=${filterType}`;
}

// Това е за филтъра.
function applyFilter() {
    const filter = document.getElementById("filter").value;
    const recordsPerPage = document.getElementById("recordsPerPage").value;
    window.location.href = `/Admin/Flights_Admin?filterType=${filter}&recordsPerPage=${recordsPerPage}`;
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

    window.location.href = `/Admin/Flights_Admin?page=1&recordsPerPage=${recordsPerPage}&filterType=${filterType}`;
}

function isTakeoffBeforeLanding(takeoff, landing) {
    const takeoffDate = new Date(takeoff);
    const landingDate = new Date(landing);
    return takeoffDate < landingDate;
}


// Пътници Дисплей
var customModal = document.getElementById("customPassengerModal");
var customPassengerListContainer = document.getElementById("customPassengerListContainer");
var customCloseModal = document.getElementsByClassName("custom-close-btn")[0]; 

function togglePassengers(flightId) {
    console.log(`Passengers button clicked for flight ${flightId}`);

    customModal.style.display = "block";
    document.body.classList.add("modal-open");
    customPassengerListContainer.innerHTML = "Loading passengers..."; 

    fetch(`/Admin/GetPassengers?flightId=${flightId}`)
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

//--------------------------------Create-----------------------------------------------

// Отваря Create PopUp-Menu-to
function OpenFlightsModal() {
    const modal = document.getElementById("createFlightModal");
    modal.style.display = "block";
}

// Затваря Create PopUp-Menu-to
function CloseFlightsModal() {
    const modal = document.getElementById("createFlightModal");
    modal.style.display = "none";
}

// Записва го [Create] 
function submitFlightsCreateForm(event) {
    event.preventDefault();
    CreateFlight();
}

// Създаване на Запис [Create]
async function CreateFlight() {
    const Location_From = document.getElementById("location_from").value;
    const Location_To = document.getElementById("location_to").value;
    const Date_Hour_Takeoff = document.getElementById("date_hour_takeoff").value;
    const Date_Hour_Landing = document.getElementById("date_hour_landing").value;
    const Plane_Type = document.getElementById("plane_type").value;
    const Plane_Number = document.getElementById("plane_number").value;
    const Pilot_Name = document.getElementById("pilot_name").value;
    const Capacity_Normal = document.getElementById("capacity_normal").value;
    const Capacity_Buissness = document.getElementById("capacity_buissness").value;
    const Capacity_First_Class = document.getElementById("capacity_first_class").value;


    if (!isTakeoffBeforeLanding(Date_Hour_Takeoff, Date_Hour_Landing)) {
        alert("Takeoff time must be before the landing time.");
        return;
    }   

    const Flights = {
        Location_From: Location_From,
        Location_To: Location_To,
        Date_Hour_Takeoff: Date_Hour_Takeoff,
        Date_Hour_Landing: Date_Hour_Landing,
        Plane_Type: Plane_Type,
        Plane_Number: Plane_Number,
        Pilot_Name: Pilot_Name,
        Capacity_Normal: Capacity_Normal,
        Capacity_Buissness: Capacity_Buissness,
        Capacity_First_Class: Capacity_First_Class
    };


    try {
        const response = await fetch('/Admin/CreateFlight', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(Flights)
        });

        if (response.ok) {
            const result = await response.json();
            if (result.success) {
                alert("Flight was created successfully!");
                CloseFlightsModal();
                window.location.reload();
            } else {
                alert("Error: " + result.message);
            }
        } else {
            alert("Failed to create a flight.");
        }
    } catch (error) {
        console.error("Error:", error);
    }
}

//--------------------------------Edit-----------------------------------------------

function OpenEditFlightsModal(flightId) {
    fetch(`/Admin/GetFlight/${flightId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            document.getElementById("edit_flight_id").value = flightId;
            document.getElementById("edit_location_from").value = data.location_from;
            document.getElementById("edit_location_to").value = data.location_to;
            document.getElementById("edit_date_hour_takeoff").value = data.date_hour_takeoff;
            document.getElementById("edit_date_hour_landing").value = data.date_hour_landing;
            document.getElementById("edit_plane_type").value = data.plane_type;
            document.getElementById("edit_plane_number").value = data.plane_number;
            document.getElementById("edit_pilot_name").value = data.pilot_name;
            document.getElementById("edit_capacity_normal").value = data.capacity_normal;
            document.getElementById("edit_capacity_buissness").value = data.capacity_buissness;
            document.getElementById("edit_capacity_first_class").value = data.capacity_first_class;
            document.getElementById("editFlightModal").style.display = "block";
        })
        .catch(error => console.error("Error fetching flight data:", error));
}

function CloseEditFlightsModal() {
    document.getElementById("editFlightModal").style.display = "none";
}

async function editFlight(event) {
    event.preventDefault();
    const flightId = document.getElementById("edit_flight_id").value;

    const Date_Hour_Takeoff = document.getElementById("edit_date_hour_takeoff").value;
    const Date_Hour_Landing = document.getElementById("edit_date_hour_landing").value;

    if (!isTakeoffBeforeLanding(Date_Hour_Takeoff, Date_Hour_Landing)) {
        alert("Takeoff time must be before the landing time.");
        return;
    }


    const flightData = {
        Flight_Number_id: flightId,
        Location_From: document.getElementById("edit_location_from").value,
        Location_To: document.getElementById("edit_location_to").value,
        Date_Hour_Takeoff: document.getElementById("edit_date_hour_takeoff").value,
        Date_Hour_Landing: document.getElementById("edit_date_hour_landing").value,
        Plane_Type: document.getElementById("edit_plane_type").value,
        Plane_Number: document.getElementById("edit_plane_number").value,
        Pilot_Name: document.getElementById("edit_pilot_name").value,
        Capacity_Normal: document.getElementById("edit_capacity_normal").value,
        Capacity_Buissness: document.getElementById("edit_capacity_buissness").value,
        Capacity_First_Class: document.getElementById("edit_capacity_first_class").value
    };

    try {
        const response = await fetch(`/Admin/UpdateFlight/${flightId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(flightData)
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const result = await response.json();
        if (result.success) {
            alert("Flight updated successfully!");
            CloseEditFlightsModal();
            window.location.reload();
        } else {
            alert("Error: " + result.message);
        }
    } catch (error) {
        console.error("Error updating flight:", error);
    }
}


//--------------------------------Delete-----------------------------------------------
// Функция за изтриване на запис [Delete]
async function DeleteFlight(flightid) {
    if (!confirm("Are you sure you want to delete this flight?")) {
        return;
    }

    try {
        const response = await fetch(`/Admin/DeleteFlight/${flightid}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
            }
        });

        if (response.ok) {
            const result = await response.json();
            if (result.success) {
                alert("Flight deleted successfully!");
                window.location.reload();
            } else {
                alert("Error: " + result.message);
            }
        } else {
            alert("Failed to delete flight.");
        }
    } catch (error) {
        console.error("Error:", error);
    }
}