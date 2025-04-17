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
    window.location.href = `/Admin/StaffInformation?page=${page}&recordsPerPage=${recordsPerPage}&filterType=${filterType}`;
}

