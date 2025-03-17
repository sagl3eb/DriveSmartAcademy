document.addEventListener('DOMContentLoaded', function () {
    const cards = document.querySelectorAll('.course-card');

    cards.forEach(card => {
        const detailsBtn = card.querySelector('.details-btn');

        detailsBtn.addEventListener('click', () => {
            card.classList.add('flipped');
        });

        card.querySelector('.course-card-back').addEventListener('click', () => {
            card.classList.remove('flipped');
        });
    });
});