document.getElementById("send").addEventListener("click", function()
{
    var fname = document.getElementById("fname").value;
    var lname = document.getElementById("lname").value;

    var data = { name: fname + " " + lname }

    fetch("https://registration-function-stagging-gcdmcdeydhdchvd4.swedencentral-01.azurewebsites.net/api/RegisterVisitor",
    {
        method: "POST",
        headers: {"content-Type": "application/json"},
        body: JSON.stringify(data)
    })

    .then(function(res) { return res.text(); })
    .then(function(data)
    {
        document.getElementById("result").textContent = data;
        document.getElementById("fname").value = "";
        document.getElementById("lname").value = "";
    })

    .catch(function(err)
    {
        document.getElementById("result").textContent = "Error";
        console.log(err);
    });

});
