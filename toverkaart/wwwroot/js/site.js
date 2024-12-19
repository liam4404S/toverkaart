const mapContainer = document.getElementById('mapContainer');
const mapImage = document.getElementById('mapImage');
const buttonContainer = document.getElementById('buttonContainer');

let scale = 1; // Initial zoom level
let isDragging = false;
let startX = 0, startY = 0;
let translateX = 0, translateY = 0;

// Zoom functionality
mapContainer.addEventListener('wheel', (event) => {
    event.preventDefault();

    const zoomSpeed = 0.1; // Speed of zoom
    const maxZoom = 4; // Maximum zoom
    const minZoom = 0.5; // Minimum zoom

    // Adjust scale
    scale += event.deltaY < 0 ? zoomSpeed : -zoomSpeed;
    scale = Math.min(Math.max(scale, minZoom), maxZoom);

    applyTransform();
});

// Drag functionality
mapContainer.addEventListener('mousedown', (event) => {
    isDragging = true;
    startX = event.clientX;
    startY = event.clientY;
    mapContainer.style.cursor = 'grabbing';
});

mapContainer.addEventListener('mousemove', (event) => {
    if (!isDragging) return;

    // Calculate movement
    const dx = event.clientX - startX;
    const dy = event.clientY - startY;

    translateX += dx;
    translateY += dy;

    startX = event.clientX;
    startY = event.clientY;

    applyTransform();
});

mapContainer.addEventListener('mouseup', () => {
    isDragging = false;
    mapContainer.style.cursor = 'grab';
});

mapContainer.addEventListener('mouseleave', () => {
    isDragging = false;
    mapContainer.style.cursor = 'grab';
});

// Apply zoom and pan transformations
function applyTransform() {
    const rect = mapContainer.getBoundingClientRect();
    const imageRect = mapImage.getBoundingClientRect();

    // Ensure the image stays within the container bounds
    const maxTranslateX = Math.max(0, (rect.width - imageRect.width * scale));
    const maxTranslateY = Math.max(0, (rect.height - imageRect.height * scale));

    translateX = Math.min(translateX, maxTranslateX);
    translateY = Math.min(translateY, maxTranslateY);
    translateX = Math.max(translateX, rect.width - imageRect.width * scale);
    translateY = Math.max(translateY, rect.height - imageRect.height * scale);

    // Apply transformations
    const transform = `translate(${translateX}px, ${translateY}px) scale(${scale})`;
    mapImage.style.transform = transform;
    buttonContainer.style.transform = `translate(${translateX}px, ${translateY}px)`;

    buttonContainer.style.scale = `${1 / scale}`;
}