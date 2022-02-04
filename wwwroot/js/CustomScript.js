function confirmDelete(uniqueId, IsDeletClicked) {
    var deleteSpan = "deleteSpan_" + uniqueId;
    var confirmDeleteSpan = "confirmDeleteSpan_" + uniqueId;

    //if isdeleteclicked is true that means yes has been clicked
    //hid delete button and display confirmdelete
    if (IsDeletClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();  
    }
    else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();   
    }
}