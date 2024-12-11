document.addEventListener('DOMContentLoaded', function () {

    var counterGameEl = document.getElementById("karus_countergame");
    if (!counterGameEl) {
        return;
    }

    let maxCount = null;
    let countType = null;
    let currentCount = 0;
    let selectedColor = null;
    let gameTypes = {};

    // Dynamically Load Options from JSON
    fetch('/karus/data/crisisbox/countergame-config.json')
        .then(response => response.json())
        .then(data => {
            // Populate max count
            const maxCountGroup = document.getElementById('maxCountGroup');
            data.maxCounts.forEach(count => {
                const input = document.createElement('input');
                const label = document.createElement('label');
                const id = `max${count}`;
                input.type = 'radio';
                input.id = id;
                input.name = 'maxCount';
                input.value = count;
                label.setAttribute('for', id);
                label.textContent = count;
                maxCountGroup.appendChild(input);
                maxCountGroup.appendChild(label);
            });

            // Populate game types
            const countTypeGroup = document.getElementById('countTypeGroup');
            gameTypes = {};
            data.gameTypes.forEach(type => {
                const input = document.createElement('input');
                const label = document.createElement('label');
                const id = type.type;
                input.type = 'radio';
                input.id = id;
                input.name = 'countType';
                input.value = type.type;
                label.setAttribute('for', id);
                label.textContent = type.label;
                countTypeGroup.appendChild(input);
                countTypeGroup.appendChild(label);

                // Save SVG and color info in gameTypes object
                gameTypes[type.type] = { svg: type.svg, colors: type.colors };
            });
        });

    // Add event listeners to radio groups
    document.getElementById('maxCountGroup').addEventListener('change', (event) => {
        maxCount = parseInt(event.target.value);
        checkStartGameEnabled();
    });

    document.getElementById('countTypeGroup').addEventListener('change', (event) => {
        countType = event.target.value;
        const colorSection = document.getElementById('colorSection');
        const colorGroup = document.getElementById('colorGroup');
        selectedColor = null;

        if (!colorSection || !colorGroup) {
            console.error('Color section or group not found in the DOM.');
            return;
        }

        // Clear previous colors
        colorGroup.innerHTML = '';

        // Populate colors if available
        const colors = gameTypes[countType]?.colors || [];

        if (colors.length > 0) {
            colorSection.style.display = 'block';
            colors.forEach(color => {
                const circle = document.createElement('div');
                circle.className = 'color-circle';
                circle.style.backgroundColor = color.hex;
                circle.title = color.name;
                circle.addEventListener('click', () => {
                    // Deselect all colors
                    document.querySelectorAll('.color-circle').forEach(c => c.classList.remove('selected'));
                    // Select current color
                    circle.classList.add('selected');
                    selectedColor = color.hex;
                    checkStartGameEnabled();
                });
                colorGroup.appendChild(circle);
            });
        } else {
            colorSection.style.display = 'none';
        }
        checkStartGameEnabled();
    });

    // Enable Start Game Button
    function checkStartGameEnabled() {
        const startGameBtn = document.getElementById('startGame');
        const requiresColor = gameTypes[countType]?.colors?.length > 0;
        if (maxCount && countType && (!requiresColor || selectedColor)) {
            startGameBtn.disabled = false;
        } else {
            startGameBtn.disabled = true;
        }
    }

    // Start Game
    document.getElementById('startGame').addEventListener('click', () => {
        const svgContainer = document.querySelector('.svg-container');
        const countDisplay = document.getElementById('countDisplay');
        currentCount = 0;

        // Clear previous content
        svgContainer.innerHTML = '';
        countDisplay.textContent = currentCount;

        // Add SVG icons with color applied if selected
        for (let i = 0; i < maxCount; i++) {
            const img = document.createElement('img');
            img.src = `./icons/${gameTypes[countType].svg}`;
            img.dataset.index = i;
            img.classList.add('svg-icon');
            if (selectedColor) {
                img.style.filter = `drop-shadow(0 0 5px ${selectedColor})`;
            }
            svgContainer.appendChild(img);
        }

        // Show the modal
        const modal = new bootstrap.Modal(document.getElementById('counterModal'));
        modal.show();
    });

    // Reset Game
    document.getElementById('resetGame').addEventListener('click', () => {
        maxCount = null;
        countType = null;
        currentCount = 0;
        selectedColor = null;

        // Reset radio buttons
        document.querySelectorAll('input[type="radio"]').forEach((radio) => {
            radio.checked = false;
        });

        // Reset buttons
        document.getElementById('startGame').disabled = true;

        // Clear modal content
        document.querySelector('.svg-container').innerHTML = '';
        document.getElementById('countDisplay').textContent = '0';

        // Hide color section
        document.getElementById('colorSection').style.display = 'none';
    });
});