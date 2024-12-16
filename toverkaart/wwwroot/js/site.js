const mapContainer = document.getElementById('mapContainer');
const mapImage = document.getElementById('mapImage');

let scale = 1; // Initial zoom level
let isDragging = false;
let startX = 0, startY = 0;
let translateX = 0, translateY = 0;

// Handle mouse wheel zoom
mapContainer.addEventListener('wheel', (event) => {
    event.preventDefault();

    const zoomAmount = 0.1; // Zoom increment
    const maxZoom = 3;
    const minZoom = 0.5;

    // Adjust scale
    scale += event.deltaY < 0 ? zoomAmount : -zoomAmount;
    scale = Math.min(Math.max(scale, minZoom), maxZoom); // Clamp zoom levels

    // Apply scaling
    mapImage.style.transform = `scale(${scale}) translate(${translateX}px, ${translateY}px)`;
});

// Handle drag start
mapContainer.addEventListener('mousedown', (event) => {
    isDragging = true;
    startX = event.clientX;
    startY = event.clientY;
    mapContainer.style.cursor = 'grabbing'; // Change cursor
});

// Handle dragging
mapContainer.addEventListener('mousemove', (event) => {
    if (!isDragging) return;

    // Calculate the drag offset
    const dx = event.clientX - startX;
    const dy = event.clientY - startY;

    // Update the translation values
    translateX += dx / scale; // Adjust for current zoom level
    translateY += dy / scale;

    // Apply transformation
    mapImage.style.transform = `scale(${scale}) translate(${translateX}px, ${translateY}px)`;

    // Update start position
    startX = event.clientX;
    startY = event.clientY;
});

// Handle drag end
mapContainer.addEventListener('mouseup', () => {
    isDragging = false;
    mapContainer.style.cursor = 'grab';
});

mapContainer.addEventListener('mouseleave', () => {
    isDragging = false; // Stop dragging if the mouse leaves the container
    mapContainer.style.cursor = 'grab';
});