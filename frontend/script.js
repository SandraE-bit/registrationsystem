fetch("http://localhost:7071/api/RegisterVisitor",{
method: "POST",
headers: {"content-Type": "application/json"},
body: JSON.stringify(data)
})

.then(res => res.text())
.then(data => console.log("Success:", data))
.catch(err => console.error(err));