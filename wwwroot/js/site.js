// ═══════════════════════════════════════════
// ClinicQueue — Site JavaScript
// ═══════════════════════════════════════════

// Sidebar toggle for mobile
function toggleSidebar() {
    document.getElementById('sidebar').classList.toggle('show');
}

// Close sidebar on outside click (mobile)
document.addEventListener('click', function (e) {
    const sidebar = document.getElementById('sidebar');
    const toggle = document.querySelector('.sidebar-toggle');
    if (sidebar && window.innerWidth <= 992) {
        if (!sidebar.contains(e.target) && !toggle.contains(e.target)) {
            sidebar.classList.remove('show');
        }
    }
});

// Live clock in navbar
function updateClock() {
    const el = document.getElementById('currentTime');
    if (el) {
        const now = new Date();
        el.textContent = now.toLocaleTimeString('en-US', {
            hour: '2-digit', minute: '2-digit', second: '2-digit'
        });
    }
}
setInterval(updateClock, 1000);
updateClock();

// Auto-dismiss alerts after 5s
document.addEventListener('DOMContentLoaded', function () {
    const alerts = document.querySelectorAll('.alert-dismissible');
    alerts.forEach(function (alert) {
        setTimeout(function () {
            const bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
            bsAlert.close();
        }, 5000);
    });
});

// Confirmation dialog
function confirmAction(message) {
    return confirm(message || 'Are you sure you want to perform this action?');
}

// Loading spinner
function showLoading() {
    const overlay = document.querySelector('.loading-overlay');
    if (overlay) overlay.classList.add('show');
}

function hideLoading() {
    const overlay = document.querySelector('.loading-overlay');
    if (overlay) overlay.classList.remove('show');
}

// Client-side search for tables
function filterTable(inputId, tableId) {
    const input = document.getElementById(inputId);
    const filter = input.value.toLowerCase();
    const table = document.getElementById(tableId);
    const rows = table.querySelectorAll('tbody tr');

    rows.forEach(function (row) {
        const text = row.textContent.toLowerCase();
        row.style.display = text.includes(filter) ? '' : 'none';
    });
}
