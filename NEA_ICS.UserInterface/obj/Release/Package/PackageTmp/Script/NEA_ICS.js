
// compute the unitcost and truncate to 4 decimal place for display
function computeUnit(qty, total, unit) {
    var iQty, iTotal;
    iQty = parseFloat(document.getElementById(qty).innerText.replace(/,/g, "").replace(/_/g, ""));
    iTotal = parseFloat(document.getElementById(total).innerText.replace(/,/g, "").replace(/_/g, ""));
    if (!isNaN(iQty) && !isNaN(iTotal) && iQty != "0") {
        document.getElementById(unit).innerText = (Math.floor(iTotal / iQty * Math.pow(10, 4)) / Math.pow(10, 4)).toFixed(4);
    }
    else {
        document.getElementById(unit).innerText = "0.0000";
    }
}

// compute the unitcost and truncate to 4 decimal place for display [For ITEM]
function computeItemUnit(qty, total, unit) {

    //To ensure control does not encounter any NULL value
    if (document.getElementById(qty) != null && document.getElementById(total) != null && document.getElementById(unit) != null) {

    var iQty, iTotal;
    iQty = parseFloat(document.getElementById(qty).value.replace(/,/g, "").replace(/_/g, ""));
    iTotal = parseFloat(document.getElementById(total).value.replace(/,/g, "").replace(/_/g, ""));
        
        if (!isNaN(iQty) && !isNaN(iTotal) && iQty != "0") {
        document.getElementById(unit).innerText = (Math.floor(iTotal / iQty * Math.pow(10, 4)) / Math.pow(10, 4)).toFixed(4);
        }
        else {
        document.getElementById(unit).innerText = "0.0000";
        }
    }
   
}

// compute the totalcost and truncate to 4 decimal place for display
function computeTotal(qty, unit, total) {
    var iQty, iUnitcost;
    iQty = parseFloat(document.getElementById(qty).innerText.replace(/,/g, "").replace(/_/g, ""));
    iUnitcost = parseFloat(document.getElementById(unit).innerText.replace(/,/g, "").replace(/_/g, ""));
    if (!isNaN(iQty) && !isNaN(iUnitcost) && iQty != "0") {
        document.getElementById(total).innerText = (Math.floor(iUnitcost * iQty * Math.pow(10, 4)) / Math.pow(10, 4)).toFixed(4);
    }
    else {
        document.getElementById(total).innerText = "0.0000";
    }
}

// compute the totalcost and truncate to 4 decimal place for display, Qty is a textbox
function computeItemTotal(qty, unit, total) {
    var iQty, iUnitcost;
    iQty = parseFloat(document.getElementById(qty).value.replace(/,/g, "").replace(/_/g, ""));
    iUnitcost = parseFloat(document.getElementById(unit).value.replace(/,/g, "").replace(/_/g, ""));
    if (!isNaN(iQty) && !isNaN(iUnitcost) && iQty != "0") {
        document.getElementById(total).innerText = (Math.floor(iUnitcost * iQty * Math.pow(10, 4)) / Math.pow(10, 4)).toFixed(4);
    }
    else {
        document.getElementById(total).innerText = "0.0000";
    }
}

// compute the totalcost and truncate to 2 decimal place for display
function computeTotal2(qty, unit, total) {
    var iQty, iUnitcost;
    iQty = parseFloat(document.getElementById(qty).value.replace(/,/g, "").replace(/_/g, ""));
    iUnitcost = parseFloat(document.getElementById(unit).innerText.replace(/,/g, "").replace(/_/g, ""));
    if (!isNaN(iQty) && !isNaN(iUnitcost) && iQty != "0") {
        document.getElementById(total).innerText = (Math.floor(iUnitcost * iQty * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2);
    }
    else {
        document.getElementById(total).innerText = "0.00";
    }
}

// invoke button control "btnExit"
function postbackFromJS(sender, e) {
    __doPostBack(sender, e);
}

//display worksheet status message
function ShowWorksheetSuccessMessage(value, workSheetID){
            
    alert(value);
    window.location.href = "frmPrintVerificationWorksheet.aspx?WorkSheetID=" + workSheetID;
            
}

//display status message
function ShowSuccessMessage(value){
        
    alert(value);
}

//display alert mesage
function ShowAlertMessage(value){
            
    alert(value);

}

function GetConfirmation(ctrl, message) {

    if (document.getElementById(ctrl) != null) {

        if (document.getElementById(ctrl).disabled == false) {
            return confirm(message);
        }
    }
    
}

