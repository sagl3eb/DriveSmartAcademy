document.addEventListener('DOMContentLoaded', function () {
    const track = document.querySelector('.slider-track');
    const slides = document.querySelectorAll('.slide');
    const nextButton = document.querySelector('.next-btn');
    const prevButton = document.querySelector('.prev-btn');

    let currentIndex = 0;
    const slidesToShow = 3;
    const slideCount = slides.length;

    // Initialize slider
    function initSlider() {
        updateButtonsState();
    }

    // Update buttons state
    function updateButtonsState() {
        prevButton.disabled = currentIndex === 0;
        nextButton.disabled = currentIndex >= slideCount - slidesToShow;

        prevButton.style.opacity = prevButton.disabled ? '0.5' : '1';
        nextButton.style.opacity = nextButton.disabled ? '0.5' : '1';
    }

    // Move to next/previous slide
    function moveSlides(direction) {
        if (direction === 'next' && currentIndex < slideCount - slidesToShow) {
            currentIndex++;
        } else if (direction === 'prev' && currentIndex > 0) {
            currentIndex--;
        }

        const slideWidth = slides[0].offsetWidth;
        track.style.transform = `translateX(-${currentIndex * slideWidth}px)`;
        updateButtonsState();
    }

    // Event listeners
    nextButton.addEventListener('click', () => moveSlides('next'));
    prevButton.addEventListener('click', () => moveSlides('prev'));

    // Initialize slider
    initSlider();

    // Update on window resize
    window.addEventListener('resize', initSlider);
});