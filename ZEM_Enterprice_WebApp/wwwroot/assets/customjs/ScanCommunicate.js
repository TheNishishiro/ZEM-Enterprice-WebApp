const api_url = "https://localhost:44381/api/";


async function checkBIN() {
    var constructed_api_url = api_url + "scannerInfo/" + document.getElementById("code-field").value;

    const response = await fetch(constructed_api_url);
    const d = await response.json();

    console.log(d);

    if (d["statusCode"] == 0) {
    document.getElementById("bin-label").textContent = "Nie znaleziono";
        return;
    }


    document.getElementById("bin-label").textContent = d["bin"];
}

class SendObject {
    kodWiazki = document.getElementById("kod-kabla-field").value;
    isLookingBack = document.getElementById("AddBack-field").checked;
    forcedQuantity = 0;
    dostDate = document.getElementById("data-scanned-field").value;
    dokDostawy = document.getElementById("nr-dostawy-field").value;
    isForcedQuantity = false;
    isForcedOverLimit = false;
    isForcedBackAck = false;
    isForcedBack = false;
    isForcedInsert = false;
    isForcedUndeclared = false;
    User = document.getElementById("username-field").value;
}

function BuildScannerURL(sendObject) {
    return api_url + "scannerInfo/" + sendObject.kodWiazki + "," + sendObject.forcedQuantity + "," + 
        sendObject.isLookingBack + "," + sendObject.dostDate + "," + sendObject.dokDostawy + "," +
        sendObject.isForcedQuantity + "," + sendObject.isForcedOverLimit + "," +
        sendObject.isForcedBackAck + "," + sendObject.isForcedBack + "," + sendObject.isForcedInsert + "," + sendObject.isForcedUndeclared + "," +
        sendObject.User;
}

async function SendReceive(myScan) {
    var constructed_api_url = BuildScannerURL(myScan);
    console.log(constructed_api_url);
    const response = await fetch(constructed_api_url);
    return d = await response.json();
}

function parseISOString(s) {
    var b = s.split(/\D+/);
    return new Date(Date.UTC(b[0], --b[1], b[2], b[3], b[4], b[5], b[6]));
}

async function manualEntry(e) {
    if (document.getElementById("nr-dostawy-field").value == "" || document.getElementById("data-scanned-field").value == "")
        return;

    var enterKey = 13;
    if (e.which == enterKey) {
        if (document.getElementById("Manual-Input").checked == true) {
            await getCodeDetails();
        }
    }
}

async function automaticEntry() {
    if (document.getElementById("nr-dostawy-field").value == "" || document.getElementById("data-scanned-field").value == "")
        return;

    if (document.getElementById("Manual-Input").checked == false) {
        await getCodeDetails();
    }
}

//PLCCEG0663316
async function getCodeDetails() {
    var myScan = new SendObject();

    var shouldExit = false;
    var response = await SendReceive(myScan);
    console.log(response);
    while (response["header"] == 1) {
        // Not in tech
        if (response["flag"] == 100) {
            window.alert("Kod nie znaleziony w bazie technicznych");
            shouldExit = true;
        }
        // quantity incorrect
        else if (response["flag"] == 110) {
            var newQuantity = prompt("Deklarowana ilość (" + response["args"][0] + ") nie zgadza się ze zeskanowaną " + response["sztukiSkanowane"] + "(aktualnie zeskanowanych: " + response["args"][1] + "), zatwierdź, zmień lub anuluj skan.", response["sztukiSkanowane"]);

            if (newQuantity == null || newQuantity == "") {
                shouldExit = true;
            }
            else {
                myScan.forcedQuantity = newQuantity;
                myScan.isForcedQuantity = true;
                response = await SendReceive(myScan);
            }
        }
        // quantity overlimit
        else if (response["flag"] == 111) {
            if (confirm("Po dodaniu rekordu ilość przewodów przekroczy deklarowaną ilość (" + response["args"][1] + ") do (" + response["args"][0] +"), kontynuować?")) {
                myScan.isForcedOverLimit = true;
                response = await SendReceive(myScan);
            } else {
                shouldExit = true;
            }
        }
        // code exists
        else if (response["flag"] == 200) {
            if (confirm("Kod został dziś zeskanowany, upewnij się że zeskanowałeś poprawny kod, dodać do bazy?")) {
                myScan.isForcedInsert = true;
                response = await SendReceive(myScan);
            } else {
                shouldExit = true;
            }
        }
        // code exists back
        else if (response["flag"] == 201) {
            if (confirm("Kod został dziś zeskanowany i dodany wstecz, upewnij się że zeskanowałeś poprawny kod, dodać do bazy?")) {
                myScan.isForcedInsert = true;
                response = await SendReceive(myScan);
            } else {
                shouldExit = true;
            }
        }
        // code not in declared
        else if (response["flag"] == 101) {
            if (confirm("Kod nie znaleziony w dokumencie dostawy, dodać mimo to?")) {
                myScan.isForcedUndeclared = true;
                response = await SendReceive(myScan);
            } else {
                shouldExit = true;
            }
        }
        // code is kanban
        else if (response["flag"] == 102) {
            window.alert("Zeskanowany kod należy do przewodu KanBan.");
            shouldExit = true;
        }
        // code is deleted
        else if (response["flag"] == 103) {
            window.alert("Zeskanowany kod nie powinien być już skanowany, został on usunięty z bazy technicznych.");
            shouldExit = true;
        }

        console.log(response);

        if (shouldExit == true)
            break;
    }

    document.getElementById("kod-kabla-field").value = "";
    document.getElementById("kod-kabla-field").focus();

    document.getElementById("kod-ciety-field").innerHTML = response["przewodCiety"];
    document.getElementById("bin-field").innerHTML = response["bin"];
    document.getElementById("kod-wiazki-field").innerHTML = response["kodWiazki"];
    document.getElementById("data-dostawy-stara-field").innerHTML = response["dataDostawy"].split('T')[0];
    document.getElementById("data-dostawy-nowa-field").innerHTML = response["dataDostawyOld"].split('T')[0];
    document.getElementById("ilosc-field").innerHTML = response["sztukiSkanowane"] + "/" + response["sztukiDeklatowane"];
    document.getElementById("litera-field").innerHTML = response["literaRodziny"];
    document.getElementById("SetId-field").innerHTML = response["numerKompletu"];
    document.getElementById("zeskanowanych-field").innerHTML = response["numScannedToComplete"] + "/" + response["numToComplete"];

    document.getElementById("rodzina-print").innerHTML = response["rodzina"];
    document.getElementById("kod-literowy-print").innerHTML = response["literaRodziny"];
    document.getElementById("kod-cyfrowy-print").innerHTML = response["kodWiazki"];
    document.getElementById("kabli-wiazke-print").innerHTML = response["numToComplete"];
    document.getElementById("bin-print").innerHTML = response["bin"];
    document.getElementById("nr-kompletu-print").innerHTML = response["numerKompletu"];
    document.getElementById("data-dostawy-print").innerHTML = response["dataDostawy"].split('T')[0];
    document.getElementById("ilosc-w-dostawie-print").innerHTML = response["sztukiDeklatowane"];
    document.getElementById("przykladowy-kod-kabla-print").innerHTML = response["przewodCiety"];

    if (response["isComplete"] == true) {
        document.getElementById("komplet-field").innerHTML = "KOMPLET: TAK";
        document.getElementById("window-div").style.backgroundColor = "#8BFF89";
    }
    else {
        document.getElementById("komplet-field").innerHTML = "KOMPLET: NIE";
        document.getElementById("window-div").style.backgroundColor = "#FF6459";
    }
    var d1 = new Date(response["dataDostawyOld"]);
    d1.toLocaleDateString('en-GB'); // dd/mm/yyyy
    var d2 = new Date(response["dataDostawy"]);
    d2.toLocaleDateString('en-GB'); // dd/mm/yyyy

    if (d1 < d2) {
        document.getElementById("zalegle-field").innerHTML = "ZALEGŁE";
    }
    else {
        document.getElementById("zalegle-field").innerHTML = "";
    }

    if (response["numScanned"] == 1) {
        var divToPrint = document.getElementById('print-div');

        var newWin = window.open('', 'Print-Window');

        newWin.document.open();
        newWin.document.write('<html>'
            + '<head><meta charset="utf-8">'
            + '<meta name="viewport" content="width=device-width, initial-scale=1.0, shrink-to-fit=no" >'
            + '<link rel="stylesheet" href="/assets/customcss/StyleSheet.css">'
            + '<link rel="stylesheet" href="/assets/bootstrap/css/bootstrap.min.css?h=07954550059301f8617ecdd11f08efba">'
            + '<link rel="stylesheet" href="/assets/css/styles.css?h=d41d8cd98f00b204e9800998ecf8427e"></head>'
            + '<body onload="window.print()">'
            + '<div id="print-div">'
            + divToPrint.innerHTML
            + '</div>'
            + '</body></html > ');

        newWin.document.close();

        setTimeout(function () { newWin.close(); }, 10);
    }
}
