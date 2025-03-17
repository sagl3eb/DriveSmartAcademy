document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('contactForm');
    const inputs = document.querySelectorAll('.glass-input');

    // Add floating label animation
    inputs.forEach(input => {
        input.addEventListener('focus', () => {
            input.parentElement.classList.add('focused');
        });

        input.addEventListener('blur', () => {
            if (!input.value) {
                input.parentElement.classList.remove('focused');
            }
        });
    });

    // Form submission with animation
    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        const submitBtn = form.querySelector('.submit-btn');
        const originalText = submitBtn.innerHTML;

        // Start loading animation
        submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Sending...';
        submitBtn.disabled = true;

        try {
            // Simulate form submission
            await new Promise(resolve => setTimeout(resolve, 2000));

            // Success animation
            submitBtn.innerHTML = '<i class="fas fa-check"></i> Sent Successfully!';
            submitBtn.style.background = 'var(--gradient-2)';

            // Reset form after delay
            setTimeout(() => {
                form.reset();
                submitBtn.innerHTML = originalText;
                submitBtn.style.background = 'var(--gradient-1)';
                submitBtn.disabled = false;
            }, 3000);

        } catch (error) {
            // Error animation
            submitBtn.innerHTML = '<i class="fas fa-times"></i> Error. Try Again';
            submitBtn.style.background = '#dc3545';

            setTimeout(() => {
                submitBtn.innerHTML = originalText;
                submitBtn.style.background = 'var(--gradient-1)';
                submitBtn.disabled = false;
            }, 3000);
        }
    });
});