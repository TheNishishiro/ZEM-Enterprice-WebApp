async function GetMissing() {
    var constructed_api_url = api_url + "missingScan/" + document.getElementById("code-field").value.replace("PLC", "")
        + ',' + document.getElementById("date-picker").value;

    const response = await fetch(constructed_api_url);
    const d = await response.json();

    console.log(d);

    document.getElementById("missing-label").innerHTML = "";
    d.forEach(printElements);
}

function printElements(element, index, array) {
    document.getElementById("missing-label").innerHTML += "<br>" + element;
}