async function registerVisitor() {
    const name = document.getElementById("nameInput").value;

    try {
        const response = await fetch("https://registration-function-g3hpc7fybuggb0ev.swedencentral-01.azurewebsites.net/api/RegisterVisitor", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ name })
        });

        const result = await response.text();
        alert(result);
    } catch (err) {
        console.error("Fel vid registrering:", err);
        alert("Det gick inte att registrera besökare.");
    }
}

document.getElementById("registerBtn").addEventListener("click", registerVisitor);




