document.addEventListener('DOMContentLoaded', function () {
    // Initialize dashboard elements
    initDashboard();

    // Mobile sidebar toggle
    const sidebarToggles = document.querySelectorAll('#sidebarCollapse, #sidebarCollapseBtn');
    const sidebar = document.getElementById('sidebar');

    sidebarToggles.forEach(toggle => {
        toggle.addEventListener('click', function () {
            sidebar.classList.toggle('active');
        });
    });

    // Close sidebar when clicking outside on mobile
    document.addEventListener('click', function (event) {
        const isClickInsideSidebar = sidebar.contains(event.target);
        const isClickOnToggle = Array.from(sidebarToggles).some(toggle => toggle.contains(event.target));

        if (!isClickInsideSidebar && !isClickOnToggle && window.innerWidth < 768 && sidebar.classList.contains('active')) {
            sidebar.classList.remove('active');
        }
    });
});

function initDashboard() {
    // Add any dashboard initialization code here
    console.log('Student dashboard initialized');

    // Example: Animate progress numbers
    const progressElements = document.querySelectorAll('[data-progress]');
    progressElements.forEach(element => {
        const target = parseInt(element.getAttribute('data-progress'));
        let count = 0;
        const duration = 1000; // ms
        const interval = 10; // ms
        const increment = target / (duration / interval);

        const counter = setInterval(() => {
            count += increment;
            if (count >= target) {
                element.textContent = target + '%';
                clearInterval(counter);
            } else {
                element.textContent = Math.floor(count) + '%';
            }
        }, interval);
    });
}