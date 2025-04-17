function toggleDetails(button) {
    const detailsRow = button.closest('tr').nextElementSibling;
    detailsRow.style.display = detailsRow.style.display === 'none' ? 'table-row' : 'none';
}

function getValueById(id) {
    const element = document.getElementById(id);
    return element ? element.value : '';
}

function validateEmail(email) {
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailPattern.test(email);
}

function validateEGN(egn) {
    return egn.length === 10 && !isNaN(egn);
}

function validatePhoneNumber(phone) {
    const phonePattern = /^[0-9]{10}$/; 
    return phonePattern.test(phone);
}

function showError(inputElement, message) {
    inputElement.classList.add("error");
    let errorSpan = inputElement.nextElementSibling;
    if (!errorSpan || !errorSpan.classList.contains("error-message")) {
        errorSpan = document.createElement("span");
        errorSpan.classList.add("error-message");
        inputElement.after(errorSpan);
    }
    errorSpan.innerText = message;
}

function clearError(inputElement) {
    inputElement.classList.remove("error"); 
    const errorSpan = inputElement.nextElementSibling;
    if (errorSpan && errorSpan.classList.contains("error-message")) {
        errorSpan.remove();
    }
}

function validateForm() {
    let isValid = true;

    const leaderEGN = document.getElementById("leaderEGN");
    const leaderPhone = document.getElementById("leaderPhone");
    const leaderEmail = document.getElementById("leaderEmail");

    if (!validateEGN(leaderEGN.value)) {
        showError(leaderEGN, "EGN must be exactly 10 numnbers.");
        isValid = false;
    } else {
        clearError(leaderEGN);
    }

    if (!validatePhoneNumber(leaderPhone.value)) {
        showError(leaderPhone, "The Phone Number must be exactly 10 numbers.");
        isValid = false;
    } else {
        clearError(leaderPhone);
    }

    if (!validateEmail(leaderEmail.value)) {
        showError(leaderEmail, "Enter valid email adress.");
        isValid = false;
    } else {
        clearError(leaderEmail);
    }

    return isValid;
}

function submitReservation() {
    if (!validateForm()) {
        alert("Please correct the mistakes in your request.");
        return;
    }

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

    for (let i = 0; i < numTickets - 1; i++) {
        const guestFirstName = document.getElementById(`guestFirstName${i}`).value;
        const guestMiddleName = document.getElementById(`guestMiddleName${i}`).value;
        const guestLastName = document.getElementById(`guestLastName${i}`).value;
        const guestEGN = document.getElementById(`guestEGN${i}`).value;
        const guestPhone = document.getElementById(`guestPhone${i}`).value;
        const guestNationality = document.getElementById(`guestNationality${i}`).value;

        guests.push({
            FirstName: guestFirstName,
            MiddleName: guestMiddleName,
            LastName: guestLastName,
            EGN: guestEGN,
            PhoneNumber: guestPhone,
            Nationality: guestNationality
        });
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
            alert('There was an error while trying to make reservation.'); 
        });
}



function openReservationPopup(flightId) {
    document.getElementById("flightId").value = flightId;
    const modal = document.getElementById("reservationModal");
    modal.style.display = "block";
    document.body.classList.add("modal-open");
    document.getElementById("flightInfo").innerText = `Flight ID: ${flightId}`;
    generateGuestForms();
}

function closeModal() {
    document.getElementById("reservationModal").style.display = "none";
    document.body.classList.remove("modal-open");
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


function applyFilter() {
    const filter = document.getElementById("filter").value;
    const recordsPerPage = document.getElementById("recordsPerPage").value;
    window.location.href = `/Home/Flights?filterType=${filter}&recordsPerPage=${recordsPerPage}`;
}

function updateRecordsPerPage() {
    const recordsPerPage = document.getElementById("recordsPerPage").value;
    const filterType = document.getElementById("filter").value;

    window.location.href = `/Home/Flights?page=1&recordsPerPage=${recordsPerPage}&filterType=${filterType}`;
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
    window.location.href = `/Home/Flights?page=${page}&recordsPerPage=${recordsPerPage}&filterType=${filterType}`;
}


