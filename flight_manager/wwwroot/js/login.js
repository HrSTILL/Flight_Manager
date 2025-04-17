document.getElementById('loginForm').addEventListener('submit', async function (event) {
    event.preventDefault();

    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    try {
        const response = await fetch('/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ username, password })
        });

        const messageDiv = document.getElementById('message');

        if (response.ok) {
            const user = await response.json();

            console.log('Login successful, user:', user);

            if (user && user.rank) {
                console.log('User Rank:', user.rank);

                if (user.rank === 'admin') {
                    window.location.href = '/AdminDashboard';
                } else if (user.rank === 'staff') {
                    window.location.href = '/StaffDashboard';
                } else {
                    console.error('Unexpected rank:', user.rank);
                    messageDiv.textContent = 'Unexpected user rank.';
                }
            } else {
                console.error('No user rank returned from the server');
                messageDiv.textContent = 'Login successful, but no user rank was returned.';
            }
        } else {
                alert('Invalid username or password.');
        }
    } catch (error) {
        console.error('Error during login:', error);
        const messageDiv = document.getElementById('message');
        messageDiv.textContent = 'An error occurred while logging in. Please try again later.';
    }
});
