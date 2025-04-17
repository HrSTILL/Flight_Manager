
function getValueById(id) {
    const element = document.getElementById(id);
    return element ? element.value : '';
}

function submitReservation() {
    const flightId = document.getElementById("flightId").value;
    const leaderFirstName = document.getElementById("leaderFirstName").value;
    const leaderMiddleName = document.getElementById("leaderMiddleName").value;
    const leaderLastName = document.getElementById("leaderLastName").value;
    const leaderEGN = document.getElementById("leaderEGN").value;
    const leaderPhone = document.getElementById("leaderPhone").value;
    const leaderNationality = document.getElementById("leaderNationality").value;
    const leaderEmail = document.getElementById("leaderEmail").value;
    const ticketType = document.getElementById("seatClass").value;

    const numTickets = parseInt(document.getElementById("numTickets").value);
    const guests = [];

    console.log(`Number of tickets: ${numTickets}`);

    for (let i = 0; i < numTickets - 1; i++) {
        const guestFirstName = document.getElementById(`guestFirstName${i}`);
        const guestMiddleName = document.getElementById(`guestMiddleName${i}`);
        const guestLastName = document.getElementById(`guestLastName${i}`);
        const guestEGN = document.getElementById(`guestEGN${i}`);
        const guestPhone = document.getElementById(`guestPhone${i}`);
        const guestNationality = document.getElementById(`guestNationality${i}`);

        console.log(`Accessing guest form elements for index ${i}:`, {
            guestFirstName: guestFirstName,
            guestMiddleName: guestMiddleName,
            guestLastName: guestLastName,
            guestEGN: guestEGN,
            guestPhone: guestPhone,
            guestNationality: guestNationality
        });

        if (guestFirstName && guestMiddleName && guestLastName && guestEGN && guestPhone && guestNationality) {
            guests.push({
                FirstName: guestFirstName.value,
                MiddleName: guestMiddleName.value,
                LastName: guestLastName.value,
                EGN: guestEGN.value,
                PhoneNumber: guestPhone.value,
                Nationality: guestNationality.value
            });
        } else {
            console.warn(`Guest form elements for guest index ${i} are not found.`);
        }
    }

    const reservationData = {
        Flight_Number_id: flightId,
        LeaderEmail: leaderEmail,
        FirstName: leaderFirstName,
        MiddleName: leaderMiddleName,
        LastName: leaderLastName,
        EGN: leaderEGN,
        PhoneNumber: leaderPhone,
        Nationality: leaderNationality,
        TicketType: ticketType,
        Guests: guests
    };

    fetch('/Reservations/SubmitReservation', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(reservationData)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            alert(data.message);
            closeModal();
        })
        .catch(error => {
            console.error('Error:', error);
            alert('There was an error saving your reservation.');
        });
}


function openReservationPopup(flightId) {
    document.getElementById("flightId").value = flightId;
    document.getElementById("reservationModal").style.display = "block";
    document.getElementById("flightInfo").innerText = `Flight ID: ${flightId}`;
    generateGuestForms();
}

function closeModal() {
    document.getElementById("reservationModal").style.display = "none";
}

function generateGuestForms() {
    const numTickets = parseInt(document.getElementById("numTickets").value);
    const guestFormsContainer = document.getElementById("guestForms");
    guestFormsContainer.innerHTML = '';

    for (let i = 0; i < numTickets - 1; i++) {
        guestFormsContainer.innerHTML += `
            <div class="guest-form">
                <h4>Guest ${i + 1}</h4>
                <input type="text" placeholder="First Name" id="guestFirstName${i}" required />
                <input type="text" placeholder="Middle Name" id="guestMiddleName${i}" />
                <input type="text" placeholder="Last Name" id="guestLastName${i}" required />
                <input type="text" placeholder="EGN" id="guestEGN${i}" required />
                <input type="text" placeholder="Phone Number" id="guestPhone${i}" required />
                <input type="text" placeholder="Nationality" id="guestNationality${i}" required />
            </div>
        `;
    }

    console.log("Generated guest forms:", guestFormsContainer.innerHTML);
}

window.onclick = function (event) {
    const modal = document.getElementById("reservationModal");
    if (event.target === modal) {
        closeModal();
    }
};





var customModal = document.getElementById("customPassengerModal");
var customPassengerListContainer = document.getElementById("customPassengerListContainer");
var customCloseModal = document.getElementsByClassName("custom-close-btn")[0];

customCloseModal.onclick = function () {
    customModal.style.display = "none";
}

function togglePassengers(reservationGroup) {
    console.log(`Details button clicked for reservation group ${reservationGroup}`);

    customModal.style.display = "block";
    customPassengerListContainer.innerHTML = "Loading passengers...";

    fetch(`/Staff/GetPassengers2?reservationGroup=${reservationGroup}`)
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                var passengers = data.passengers;
                var passengerListHtml = '';

                passengers.forEach(function (passenger) {
                    passengerListHtml += `<div>Full Name: ${passenger.First_Name} ${passenger.Middle_Name || ''} ${passenger.Last_Name}</div>`;
                    passengerListHtml += `<div>EGN: ${passenger.EGN}</div>`;
                    passengerListHtml += `<div>Phone: ${passenger.Phone_Number}</div>`;
                    passengerListHtml += `<div>Nationality: ${passenger.Nationality}</div>`;
                    passengerListHtml += '<hr>';
                });

                customPassengerListContainer.innerHTML = passengerListHtml;
            } else {
                customPassengerListContainer.innerHTML = 'No passengers found for this group.';
            }
        })
        .catch(error => {
            customPassengerListContainer.innerHTML = 'Error loading passengers.';
            console.error('Error fetching passengers:', error);
        });
}

window.onclick = function (event) {
    if (event.target === customModal) {
        customModal.style.display = "none";
    }
}


function getValueById(id) {
    return document.getElementById(id).value;
}

function applyFilter() {
    const filter = document.getElementById("filter").value;
    const recordsPerPage = document.getElementById("recordsPerPage").value;
    window.location.href = `/Staff/Reservations_Staff?filterType=${filter}&recordsPerPage=${recordsPerPage}`;
}

function updateRecordsPerPage() {
    const recordsPerPage = document.getElementById("recordsPerPage").value;
    const filterType = document.getElementById("filter").value;

    window.location.href = `/Staff/Reservations_Staff?page=1&recordsPerPage=${recordsPerPage}&filterType=${filterType}`;
}


function prevPage() {
    const currentPage = parseInt(document.getElementById("pageNumber").textContent.split(' ')[0], 10);
    if (currentPage > 1) {
        goToPage(currentPage - 1);
    }
}

function nextPage() {
    const pageInfo = document.getElementById("pageNumber").textContent.split(' ');
    const currentPage = parseInt(pageInfo[0], 10);
    const totalPages = parseInt(pageInfo[2], 10);
    if (currentPage < totalPages) {
        goToPage(currentPage + 1);
    }
}

function goToPage(page) {
    const recordsPerPage = document.getElementById("recordsPerPage").value;
    const filterType = document.getElementById("filter").value;
    window.location.href = `/Staff/Reservations_Staff?page=${page}&recordsPerPage=${recordsPerPage}&filterType=${filterType}`;
}