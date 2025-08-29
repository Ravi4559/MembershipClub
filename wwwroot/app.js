// --- CONFIGURATION ---
const API_BASE_URL = 'https://localhost:7263'; 

// --- DOM ELEMENTS ---
const loggedOutView = document.getElementById('loggedOutView');
const loggedInView = document.getElementById('loggedInView');
const messageArea = document.getElementById('messageArea');

// Forms
const registerForm = document.getElementById('registerForm');
const verifyForm = document.getElementById('verifyForm');
const pointsForm = document.getElementById('pointsForm');

// Inputs
const registerMobileInput = document.getElementById('registerMobile');
const verifyMobileInput = document.getElementById('verifyMobile');
const otpInput = document.getElementById('otp');
const purchaseAmountInput = document.getElementById('purchaseAmount');

// Buttons
const checkPointsButton = document.getElementById('checkPointsButton');
const logoutButton = document.getElementById('logoutButton');

// Display
const welcomeMessage = document.getElementById('welcomeMessage');

// --- EVENT LISTENERS ---
registerForm.addEventListener('submit', handleRegister);
verifyForm.addEventListener('submit', handleVerify);
pointsForm.addEventListener('submit', handleAddPoints);
checkPointsButton.addEventListener('click', handleCheckPoints);
logoutButton.addEventListener('click', handleLogout);

// --- STATE MANAGEMENT ---
function updateUI() {
    const token = localStorage.getItem('jwtToken');
    if (token) {
        loggedInView.classList.remove('hidden');
        loggedOutView.classList.add('hidden');
        welcomeMessage.textContent = 'Welcome! You are logged in.';
    } else {
        loggedInView.classList.add('hidden');
        loggedOutView.classList.remove('hidden');
    }
}

function showMessage(message, isError = false) {
    messageArea.textContent = message;
    messageArea.style.color = isError ? '#e74c3c' : '#27ae60';
}

// --- API HANDLERS ---
async function handleRegister(e) {
    e.preventDefault();
    showMessage('Registering...');
    try {
        const response = await fetch(`${API_BASE_URL}/api/member/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ mobileNumber: registerMobileInput.value })
        });

        if (!response.ok) throw new Error('Registration failed.');
        
        const data = await response.json();
        showMessage(`Registration successful for Member ID: ${data.memberId}. Please verify with OTP '1234'.`);
        verifyMobileInput.value = registerMobileInput.value;
    } catch (error) {
        showMessage(error.message, true);
    }
}

async function handleVerify(e) {
    e.preventDefault();
    showMessage('Verifying...');
    try {
        // Frontend sends data to the backend API [cite: 104]
        const response = await fetch(`${API_BASE_URL}/api/member/verify`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                mobileNumber: verifyMobileInput.value,
                otp: otpInput.value,
            })
        });

        if (response.status === 401) throw new Error('Invalid OTP or mobile number.');
        if (!response.ok) throw new Error('Verification failed.');

        // Frontend receives the JWT [cite: 106]
        const data = await response.json();
        
        // Save JWT in localStorage [cite: 107]
        localStorage.setItem('jwtToken', data.token);
        
        showMessage('Login successful!');
        updateUI();
    } catch (error) {
        showMessage(error.message, true);
    }
}

async function handleAddPoints(e) {
    e.preventDefault();
    showMessage('Adding points...');
    
    // For protected actions, attach JWT in the Authorization header [cite: 109]
    const token = localStorage.getItem('jwtToken');
    if (!token) {
        showMessage('You are not logged in.', true);
        return;
    }
    
    try {
        const response = await fetch(`${API_BASE_URL}/api/points/add`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({ purchaseAmount: parseFloat(purchaseAmountInput.value) })
        });
        
        // Backend middleware checks the header; if invalid, it returns 401 
        if (response.status === 401) {
            handleLogout(); // Token is invalid/expired, so log out
            showMessage('Session expired. Please log in again.', true);
            return;
        }

        if (!response.ok) throw new Error('Failed to add points.');
        
        showMessage('Points added successfully!');
        pointsForm.reset();
        await handleCheckPoints(); // Refresh points display
    } catch (error) {
        showMessage(error.message, true);
    }
}

async function handleCheckPoints() {
    showMessage('Fetching points...');
    const token = localStorage.getItem('jwtToken');
    if (!token) return;

    try {
        const response = await fetch(`${API_BASE_URL}/api/points`, {
            method: 'GET',
            headers: { 'Authorization': `Bearer ${token}` }
        });
        
        if (response.status === 401) {
            handleLogout();
            showMessage('Session expired. Please log in again.', true);
            return;
        }

        if (!response.ok) throw new Error('Could not fetch points.');
        
        const data = await response.json();
        showMessage(`You have ${data.totalPoints} points.`);
    } catch (error) {
        showMessage(error.message, true);
    }
}

function handleLogout() {
    // On logout, remove the token from storage [cite: 114]
    localStorage.removeItem('jwtToken');
    showMessage('You have been logged out.');
    updateUI();
}

// --- INITIALIZATION ---
// Check login status on page load
document.addEventListener('DOMContentLoaded', updateUI);