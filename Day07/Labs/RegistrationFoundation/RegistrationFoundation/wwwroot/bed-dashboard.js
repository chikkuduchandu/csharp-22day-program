// ===================================================
// Hospital Bed Availability Dashboard
// Demonstrates:
// - Arrays
// - Objects
// - Loops
// - Conditions
// - DOM manipulation
// ===================================================


// -----------------------------
// BED DATA (Mock backend data)
// -----------------------------
let beds = [
    { bedNumber: 1, isOccupied: false },
    { bedNumber: 2, isOccupied: true },
    { bedNumber: 3, isOccupied: false },
    { bedNumber: 4, isOccupied: true },
    { bedNumber: 5, isOccupied: false },
    { bedNumber: 6, isOccupied: false },
    { bedNumber: 7, isOccupied: true },
    { bedNumber: 8, isOccupied: false },
    { bedNumber: 9, isOccupied: true },
    { bedNumber: 10, isOccupied: false },
    { bedNumber: 11, isOccupied: false },
    { bedNumber: 12, isOccupied: false }
];


// -----------------------------
// FUNCTION: Render beds on screen
// -----------------------------
function renderBeds() {

    let container = document.getElementById("bedContainer");
    let counter_occ = document.getElementById("bed-counter-occupied");
    let counter_total = document.getElementById("bed-counter-total");

    // Clear existing beds
    container.innerHTML = "";
    
    let count_occupied_beds = 0;
    let total_count = 0;

    // Loop through all beds
    for (let i = 0; i < beds.length; i++) {
        total_count += 1;
        let bed = beds[i];

        // Create a div for each bed
        let bedDiv = document.createElement("div");

        // Assign common bed class
        bedDiv.classList.add("bed");

        // Condition to decide color
        if (bed.isOccupied) {
            count_occupied_beds+=1;
            bedDiv.classList.add("occupied");
            bedDiv.innerText = "Bed " + bed.bedNumber + "\nOccupied";
        } else {
            bedDiv.classList.add("available");
            bedDiv.innerText = "Bed " + bed.bedNumber + "\nAvailable";
        }

        // Click event to toggle bed status
        bedDiv.onclick = function () {
            if (!bed.isOccupied) {
                bed.isOccupied = !bed.isOccupied;
                count_occupied_beds+=1
            }
            renderBeds(); // Re-render UI
        };

        // Add bed to container
        
        container.appendChild(bedDiv);
        
        

    }
    counter_occ.innerHTML=`<p>Total ${count_occupied_beds}</p>`;
    counter_total.innerHTML=`<p>occupied ${total_count}</p>`;
    
    
}


// -----------------------------
// INITIAL LOAD
// -----------------------------
renderBeds();
