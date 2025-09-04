document.getElementById("send").addEventListener("click", function()
{
    fetch("https://registration-function-g3hpc7fybuggb0ev.swedencentral-01.azurewebsites.net/api/RegisterVisitor",
    {
        method: "POST",
        headers: {"content-Type": "application/json"},
        body: JSON.stringify( {name: name} )
    })

    .then(function(res) { return res.text(); })
    .then(function(data)
    {
        document.getElementById("result").textContent = data;
        document.getElementById("fnamne").value = "";
        document.getElementById("lnamne").value = "";
    })

    .catch(function(err)
    {
        document.getElementById("result").textContent = "Error";
        console.log(err);
    });

});
