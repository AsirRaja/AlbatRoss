function ValidateTable(tablename, cellsLength, message) {     
    var rows = tablename.getElementsByTagName("tr");
    var header = tablename.getElementsByTagName("th");
    for (var i = 1; i < rows.length; i++) {
        var cells = rows[i].getElementsByTagName("td");
        for (var j = 0; j < cellsLength; j++) {
            cells[j].classList.remove("error");              // reset
            if (cells[j].innerText.trim() === "") {
                cells[j].classList.add("error");
                cells[j].setAttribute("tabindex", "0");      // focus enable
                cells[j].focus();
                rows[i].scrollIntoView({ behavior: "smooth", block: "center" });        //Validation Ana row ku Scroll Panni katum
                return false;
            }
        }
    }
}
function ValidateTableRow(tablename, value, previousRow) {
    var rows = tablename.getElementsByTagName("tr");
    for (var i = 0; i < rows.length; i++) {
        if (previousRow != undefined) { 
            rows[previousRow].style.background = "MistyRose";
        }
        var cells = rows[i].getElementsByTagName("td");
        for (var j = 0; j < cells.length; j++) {
            if (cells[1].innerText.trim() === value) {
                rows[i].scrollIntoView({ behavior: "smooth", block: "center" });
                rows[i].style.background = "green";
                return i;
            }
        }
    }
}
function ValidateEmpty(values) {
    if (values != null || values != undefined || value != "") {
        for (var i = 0; i < values.length; i++) {
            if (document.getElementById(values[i]).value == "" || document.getElementById(values[i]).value == null || document.getElementById(values[i]).value == undefined) {
                document.getElementById(values[i]).style.backgroundColor = 'red';
                alert(values[i] + " is Empty");
                return false;
            }
            else {
                document.getElementById(values[i]).style.backgroundColor = '';
            }
        }
    }
    return true;
}