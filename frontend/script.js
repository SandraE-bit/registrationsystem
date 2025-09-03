fetch("https://registration-function-g3hpc7fybuggb0ev.swedencentral-01.azurewebsites.net/api/RegisterVisitor",{
method: "POST",
headers: {"content-Type": "application/json"},
body: JSON.stringify(data)
})

.then(res => res.text())
.then(data => console.log("Success:", data))
.catch(err => console.error(err));