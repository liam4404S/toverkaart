const mapImage = document.getElementById('mapImage');
const container = document.getElementById('mapContainer');

let scale = 1;
let originX = 0;
let originY = 0;
let startX = 0;
let startY = 0;
let isDragging = false;

const imageWidth = 3554; // Set your image width here
const imageHeight = 2500; // Set your image height here

// Update transform and button positions
function updateTransform() {
    const containerRect = container.getBoundingClientRect();

    // Calculate max pan values based on zoom level
    const maxX = imageWidth * scale - containerRect.width;
    const maxY = imageHeight * scale - containerRect.height;

    // Constrain the panning within bounds
    originX = Math.max(Math.min(originX, 0), -maxX);
    originY = Math.max(Math.min(originY, 0), -maxY);

    // Apply the transformation to the map image
    mapImage.style.transform = `translate(${originX}px, ${originY}px) scale(${scale})`;

    // Update button positions
    document.querySelectorAll('.map-attraction').forEach((btn) => {
        const x = parseFloat(btn.getAttribute('data-x')); // Original position of the button
        const y = parseFloat(btn.getAttribute('data-y'));

        // Adjust button position based on pan (origin) and scale
        const btnX = (x * scale) + originX;
        const btnY = (y * scale) + originY;

        // Set the position for the buttons using transform
        btn.style.transform = `translate(${btnX}px, ${btnY}px)`;
    });
}

container.addEventListener('wheel', (e) => {
    e.preventDefault();
    const zoomIntensity = 0.1;
    const direction = e.deltaY > 0 ? -1 : 1;
    const scaleFactor = 1 + direction * zoomIntensity;

    const rect = mapImage.getBoundingClientRect();
    const mouseX = e.clientX - rect.left;
    const mouseY = e.clientY - rect.top;

    const dx = mouseX / scale - mouseX / (scale * scaleFactor);
    const dy = mouseY / scale - mouseY / (scale * scaleFactor);

    originX += dx;
    originY += dy;
    scale *= scaleFactor;

    // Ensure the scale is within bounds
    scale = Math.min(Math.max(scale, 0.5), 3); // You can adjust the scale range

    updateTransform();
});

container.addEventListener('mousedown', (e) => {
    isDragging = true;
    startX = e.clientX - originX;
    startY = e.clientY - originY;
    container.style.cursor = 'grabbing';
});

window.addEventListener('mousemove', (e) => {
    if (!isDragging) return;
    originX = e.clientX - startX;
    originY = e.clientY - startY;
    updateTransform();
});

window.addEventListener('mouseup', () => {
    isDragging = false;
    container.style.cursor = 'grab';
});

updateTransform();

//inlog pagina visibility
function toggleVisible() {
    event.preventDefault();
    const inlogVisible = document.getElementById("inlogVisible");
    const aanmaakVisible = document.getElementById("aanmaakVisible");
    if (inlogVisible.style.display === "none") {
        inlogVisible.style.display = "block";
        aanmaakVisible.style.display = "none";
    }
    else {
        inlogVisible.style.display = "none";
        aanmaakVisible.style.display = "block";
    }
}

//plattegrond button pop ups
function attractiePopup() {
    Swal.fire({
        title: "Attractienaam",
        text: "informatie over de attractie",
        imageUrl: "https://placehold.co/300x300",
        draggable: true
    });
}

