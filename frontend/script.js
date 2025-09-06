const button = document.getElementById("registerBtn");

button.addEventListener("click", async function () {
    const firstName = document.getElementById("fname").value;
    const lastName = document.getElementById("lname").value;

    const fullName = firstName + " " + lastName;

    if (fullName.trim() === "") {
        alert("First name and lastname");
        return;
    }

    try {
        const response = await fetch("https://registration-function-g3hpc7fybuggb0ev.swedencentral-01.azurewebsites.net/api/RegisterVisitor", {
            method: "POST",
            headers: {
                "Content-Type": "text/plain"
            },
            body: fullName
        });

        const result = await response.text();

        document.getElementById("result").innerText = result;

    } catch (error) {
        console.error("error", error);
        alert("error visitor");
    }
});






