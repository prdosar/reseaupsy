/*Toaster*/
toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": false,
    "progressBar": false,
    "positionClass": "toast-bottom-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "0",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}

/*DatePicker*/
function setDatePickerLang(language) {
    if (language != "fr") {

    }
}



//Function to convert a date from c# to javascript date
function ToJavaScriptDate(value) {
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(value);
    var dt = new Date(parseFloat(results[1]));
    var month = ((dt.getMonth() + 1) < 10) ? "0" + (dt.getMonth() + 1) : (dt.getMonth() + 1)
    var date = (dt.getDate() < 10) ? "0" + dt.getDate() : dt.getDate()
    return dt.getFullYear() + "-" + month + "-" + date;
}

function UnixTimeToDayTimeString(unixValue) {
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(unixValue);
    var dt = new Date(parseFloat(results[1]));
    var hours = (dt.getHours() < 10) ? "0" + dt.getHours() : dt.getHours()
    var minutes = (dt.getMinutes() < 10) ? "0" + dt.getMinutes() : dt.getMinutes()
    return hours + ":" + minutes
}


//Date Manipulation
function FullDateToShortDateTime(date) {
    var year = date.getFullYear();
    var month = date.getMonth() + 1
    month = (month < 10) ? "0" + month : month
    var day = date.getDate();
    day = (day < 10) ? "0" + day : day
    var hour = date.getHours();
    hour = (hour < 10) ? "0" + hour : hour
    var minute = date.getMinutes()
    minute = (minute < 10) ? "0" + minute : minute

    return `${year}-${month}-${day} ${hour}:${minute}`
}
function UnixToLongDate(unixInput, lang) {
    return (unixInput == "") ? "" : moment(unixInput).locale(lang).format("DD MMM YYYY")
}
function toInternationnalDate(inputDate) {
    return (inputDate == "") ? "" : moment(inputDate, "DD-MM-YYYY").format("YYYY-MM-DD")
}
function toFrenchDate(inputDate) {
    return (inputDate == "") ? "" : moment(inputDate, "YYYY-MM-DD").format("DD-MM-YYYY")
}
function internationnalToShortDate(inputDate) {
    return (inputDate == "") ? "" : moment(inputDate, "YYYY-MM-DD").format("DD MMM YYYY")
}




function isDatePickerDateValid(dateInternationnalFormat) {
    var momentDate = moment(dateInternationnalFormat)

    if (momentDate.isValid()) {
        return true
    }

    return false
}

function isFrenchDateValidOrEmpty(frenchDate) {
    if (!isDatePickerDateValid(toInternationnalDate(frenchDate)) && frenchDate != "") {
        return false;
    }
    return true
}

//Verify if date greater or equal than datetimetoday
function isDateGreaterOrEqualThanToday(dateToCheck) {

    var todayDate = DateTimeToday();
    var todayDay = todayDate.split('-')[2]
    var todayMonth = todayDate.split('-')[1]
    var todayYear = todayDate.split('-')[0]

    var dateToCheckDay = dateToCheck.split('-')[2]
    var dateToCheckMonth = dateToCheck.split('-')[1]
    var dateToCheckYear = dateToCheck.split('-')[0]

    if (dateToCheckYear > todayYear) {
        return true
    }

    if (dateToCheckMonth > todayMonth) {
        return true
    }

    if (dateToCheckDay >= todayDay) {
        return true
    }

    return false

}

//Verify if date greater or equal than datetimenow
function isDateGreaterThanNow(momentObject) {
    return moment().isBefore(momentObject)
}


//DateTime.Today javascript
function DateTimeToday() {
    return moment().format('YYYY-MM-DD');
}




//Escape text html entities
function EscapeHtmlEntities(text) {

    return text
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&apos;")
        .replace(/`/g, "&#96;")
}

//Verify filter date
function isStartDateValid(startDateInput) {
    var startDate = startDateInput.val();

    if (startDate == "") {
        return true;
    }
    var momentStartDate = moment(startDate, "DD-MM-YYYY")


    if (momentStartDate.isValid()) {
        return true
    }

    return false
}
function isEndDateValid(startDateInput, endDateInput) {
    var endDate = endDateInput.val();

    if (endDate == "") {
        return true;
    }
    var momentEndDate = moment(endDate, "DD-MM-YYYY")


    if (!momentEndDate.isValid()) {
        return false
    }

    if (!isEndDateGreaterThanStartDate(startDateInput, endDateInput)) {
        return false
    }

    return true
}
function isEndDateGreaterThanStartDate(startDateInput, endDateInput) {

    var startDate = startDateInput.val();
    var endDate = endDateInput.val();

    if (endDate == "" || startDate == "") {
        return true
    }

    var momentStartDate = moment(startDate, "DD-MM-YYYY")
    var momentEndDate = moment(endDate, "DD-MM-YYYY")

    console.log("momentStartDate: " + momentStartDate)
    console.log("momentEndDate: " + momentEndDate)

    if (momentStartDate <= momentEndDate) {
        return true
    }
    return false
}


//Pager
function pager(pagerUl, totalPage, pageNumber, nbPageShown, isShowFirstPage, isShowLastPage) {
    $(pagerUl).empty();

    var jump = Math.floor(nbPageShown / 2)
    var startPage = (pageNumber - jump <= 0) ? 1 : pageNumber - jump;
    var lastPage = (pageNumber + jump >= totalPage) ? totalPage : pageNumber + jump;

    //if (totalPage == 1)
    //    lastPage = 1;

    //Show extra page when current page is the first page
    if (startPage == pageNumber && lastPage != totalPage)
        lastPage++

    //Show extra page when current page is the last one
    if (lastPage == pageNumber && startPage != 1)
        startPage--

    //Show the button to go to the fist page
    if (isShowFirstPage && startPage != 1) {
        markup =
            `<li>
                <i class="fas fa-angle-double-left js-association-pager" data-page-number="1"></i>
            </li>`
        $(pagerUl).append(markup)
    }

    //Show the button to go to page - 1
    if (pageNumber != 1 && totalPage != 0) {
        markup =
            `<li>
                <i class="fas fa-angle-left js-association-pager" data-page-number="${pageNumber - 1}"></i>
            </li>`
        $(pagerUl).append(markup)
    }

    //Add the page button
    for (var i = startPage; i <= lastPage; i++) {
        markup =
            `<li>
                <span class="js-association-pager `
        if (i == pageNumber)
            markup += `active`

        markup +=
            `" data-page-number="${i}">
                    ${i}
                </span>
            </li>`

        $(pagerUl).append(markup)
    }

    //Show the button to go to page + 1
    if (pageNumber != totalPage && totalPage != 0) {
        markup =
            `<li>
                <i class="fas fa-angle-right js-association-pager" data-page-number="${pageNumber + 1}"></i>
            </li>`
        $(pagerUl).append(markup)
    }

    //Add the go to last page button
    if (isShowLastPage && lastPage != totalPage) {
        markup =
            `<li>
                <i class="fas fa-angle-double-right js-association-pager" data-page-number="${totalPage}"></i>
            </li>`
        $(pagerUl).append(markup)
    }
}
function rebuildTableWithStyle(tbodyElement) {

    var cellStyle = [];

    $(tbodyElement).find("tr:first-child td").each(function () {

        var markup = "";
        if ($(this).attr("id") != undefined) {
            markup += ` id="${$(this).attr("id")}"`
        }

        if ($(this).attr("class") != undefined) {
            markup += ` class="${$(this).attr("class")}"`
        }

        if ($(this).attr("style") != undefined) {
            markup += ` style="${$(this).attr("style")}"`
        }

        cellStyle.push(markup)
    })

    return cellStyle
}

//Function to add error class to element if the current value if the default value
function GenericCheckDefaultFieldError(element, currentValue, errorValue) {
    //Start by removing the error calss
    $(element).removeClass("input-validation-error")

    if (currentValue == errorValue) {
        $(element).addClass("input-validation-error")

        return false;
    }

    return true;
}

//Verify regex expression
function VerifyFieldWithRegex(element, value, regexExpression) {

    //Remove the error class by default
    element.removeClass("input-validation-error")

    //Verify if match the regex
    if (!regexExpression.test(value)) {
        //Doesnt match the regex, add error class
        element.addClass("input-validation-error")

        return false;
    }

    return true;
}

//Function to close a modal
function closeModal(Modal) {
    $(Modal).modal("hide");
};

//Default function to open a modal
function OpenModal(ModalElement) {
    var myModal = new bootstrap.Modal(ModalElement,
        {
            backdrop: 'static',
            keyboard: false
        }
    )
    myModal.show()
}

//Input mask
function initInputMask() {
    $(".js-mask-card-exp-date").mask("00/00");
    $(".js-mask-credit-card-number").mask("0000 0000 0000 0000");
    $(".js-mask-cvv").mask("0000");
    $(".js-mask-postal-code").mask("S0S 0S0");
    $(".js-mask-phone").mask("000-000-0000");
}

//Date Picker
$(".js-date-picker").datepicker({
    changeYear: true,
    changeMonth: true,
    dateFormat: "dd-mm-yy",
});

//Mask
$(".js-date-picker").mask("00-00-0000", { placeholder: 'DD-MM-YYYY' })

