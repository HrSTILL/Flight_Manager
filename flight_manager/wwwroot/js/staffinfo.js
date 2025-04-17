// Това е за филтъра.
    function applyFilter() {
    const filter = document.getElementById("filter").value;
    const recordsPerPage = document.getElementById("recordsPerPage").value; 
    window.location.href = `/Admin/StaffInformation?filterType=${filter}&recordsPerPage=${recordsPerPage}`; 
}

// Това е за копчето с детайли на стафа
function toggleDetails(button) {
    const detailsRow = button.closest('tr').nextElementSibling;
    detailsRow.style.display = detailsRow.style.display === 'none' ? 'table-row' : 'none';
}

// Записи на страница
function updateRecordsPerPage() {
    const recordsPerPage = document.getElementById("recordsPerPage").value;
    const filterType = document.getElementById("filter").value;

    window.location.href = `/Admin/StaffInformation?page=1&recordsPerPage=${recordsPerPage}&filterType=${filterType}`;
}

//--------------------------------Create-----------------------------------------------

// Отваря Create PopUp-Menu-to
//--------------------------------Create-----------------------------------------------

// Opens Create Modal and disables background scroll
function OpenCreateModal() {
    const modal = document.getElementById("createStaffModal");
    modal.style.display = "block";
    document.body.classList.add("modal-open"); // Disable scroll
}

// Closes Create Modal and enables background scroll
function CloseCreateModal() {
    const modal = document.getElementById("createStaffModal");
    modal.style.display = "none";
    document.body.classList.remove("modal-open"); // Enable scroll
}

// Submits Create form
function submitCreateForm(event) {
    event.preventDefault();
    createStaffMember();
}

function isValidEmail(email) {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return emailRegex.test(email);
}

function isValidPhoneNumber(phoneNumber) {
    const phoneRegex = /^[0-9]{10}$/;
    return phoneRegex.test(phoneNumber);
}

function isValidEGN(egn) {
    const egnRegex = /^[0-9]{10}$/;
    return egnRegex.test(egn);
}



async function createStaffMember() {

    const email = document.getElementById("email").value;
    const phoneNumber = document.getElementById("phoneNumber").value;
    const egn = document.getElementById("EGN").value;

    if (!isValidEmail(email)) {
        alert("Please enter a valid email address.");
        return;
    }
    if (!isValidPhoneNumber(phoneNumber)) {
        alert("Please enter a valid 10-digit phone number.");
        return;
    }
    if (!isValidEGN(egn)) {
        alert("Please enter a valid 10-digit EGN.");
        return;
    }


    const staffMember = {
        username: document.getElementById("username").value,
        password: document.getElementById("password").value,
        firstName: document.getElementById("firstName").value,
        lastName: document.getElementById("lastName").value,
        email: document.getElementById("email").value,
        address: document.getElementById("address").value,
        EGN: document.getElementById("EGN").value,
        phoneNumber: document.getElementById("phoneNumber").value,
        rank: document.getElementById("rank").value
    };

    try {
        const response = await fetch('/Admin/CreateStaffMember', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(staffMember)
        });

        if (response.ok) {
            const result = await response.json();
            if (result.success) {
                alert("Staff member created successfully!");
                CloseCreateModal();
                window.location.reload();
            } else {
                alert("Error: " + result.message);
            }
        } else {
            alert("Failed to create staff member.");
        }
    } catch (error) {
        console.error("Error:", error);
    }
}


//---------------------------------Edit--------------------------------------

// Opens Edit Modal and disables background scroll
function OpenEditModal(staffId) {
    console.log("Opening edit modal for staff ID:", staffId);

    fetch(`/Admin/GetStaffMember/${staffId}`)
        .then(response => response.json())
        .then(data => {
            document.getElementById("editStaffId").value = data.id;
            document.getElementById("editUsername").value = data.username;
            document.getElementById("editPassword").value = data.password;
            document.getElementById("editFirstName").value = data.firstName;
            document.getElementById("editLastName").value = data.lastName;
            document.getElementById("editEmail").value = data.email;
            document.getElementById("editEGN").value = data.egn;
            document.getElementById("editPhoneNumber").value = data.phoneNumber;
            document.getElementById("editAddress").value = data.address;
            document.getElementById("editRank").value = data.rank;

            // Display the modal and disable background scroll
            const modal = document.getElementById("editStaffModal");
            modal.style.display = "block";
            document.body.classList.add("modal-open"); // Disable scroll
        })
        .catch(error => console.error("Error fetching staff member:", error));
}

// Closes Edit Modal and enables background scroll
function CloseEditModal() {
    const modal = document.getElementById("editStaffModal");
    modal.style.display = "none";
    document.body.classList.remove("modal-open"); // Enable scroll
}

// Edits Staff Member
async function editStaffMember(event) {
    event.preventDefault();


    const email = document.getElementById("editEmail").value;
    const phoneNumber = document.getElementById("editPhoneNumber").value;
    const egn = document.getElementById("editEGN").value;

    if (!isValidEmail(email)) {
        alert("Please enter a valid email address.");
        return;
    }
    if (!isValidPhoneNumber(phoneNumber)) {
        alert("Please enter a valid 10-digit phone number.");
        return;
    }
    if (!isValidEGN(egn)) {
        alert("Please enter a valid 10-digit EGN.");
        return;
    }

    const staffId = document.getElementById("editStaffId").value;
    const staffMember = {
        id: staffId,
        username: document.getElementById("editUsername").value,
        password: document.getElementById("editPassword").value,
        firstName: document.getElementById("editFirstName").value,
        lastName: document.getElementById("editLastName").value,
        email: document.getElementById("editEmail").value,
        egn: document.getElementById("editEGN").value,
        phoneNumber: document.getElementById("editPhoneNumber").value,
        address: document.getElementById("editAddress").value,
        rank: document.getElementById("editRank").value
    };

    try {
        const response = await fetch(`/Admin/UpdateStaffMember/${staffId}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(staffMember)
        });

        if (response.ok) {
            const result = await response.json();
            if (result.success) {
                alert("Staff member updated successfully!");
                CloseEditModal();
                window.location.reload();
            } else {
                alert("Error: " + result.message);
            }
        } else {
            alert("Failed to update staff member.");
        }
    } catch (error) {
        console.error("Error updating staff member:", error);
    }
}

//-------------------------------Delete--------------------------------------


// Функция за изтриване на запис [Delete]
async function deleteStaffMember(staffId) {
    if (!confirm("Are you sure you want to delete this staff member?")) {
        return; 
    }

    try {
        const response = await fetch(`/Admin/DeleteStaffMember/${staffId}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
            }
        });

        if (response.ok) {
            const result = await response.json();
            if (result.success) {
                alert("Staff member deleted successfully!");
                window.location.reload(); 
            } else {
                alert("Error: " + result.message);
            }
        } else {
            alert("Failed to delete staff member.");
        }
    } catch (error) {
        console.error("Error:", error);
    }
}


