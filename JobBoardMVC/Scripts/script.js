// Jacob Janak 2017

// Customizable variables
var jobsPerPage = $("#jobs-per-page :selected").val(); // Amount of jobs to be displayed at once
var maxPagesDisplayed = 5; // Limits the number of pagination links displayed at once. Must be an odd number.

// Global variables
var rows = document.getElementsByTagName("tr"); // Live node-list of all rows. Note: this includes table header
var rowsFiltered = document.querySelectorAll("tr:not(.filtered)"); // Gets only rows matching search criteria. Note: not a live list
var pageLinks = document.getElementsByClassName("paginationLink");
var totalPages = Math.ceil((rowsFiltered.length - 1) / jobsPerPage); // How many pages are needed to fit all jobs
var pageLimit = Math.min(maxPagesDisplayed, totalPages);
var companyExists = false; // holds if search is valid company
var addedRows = document.querySelectorAll(".added"); // number of rows added
var tabletWidth = 938;
var mobileWidth = 600;
// Script begins


hideExtraRows(1); // Initialize the pagination to page 1
$("#job-listings-table").tablesorter(); // Enable tableSorter.js
$("#savedJobTable").tablesorter(); // Enable tableSorter.js
$("#savedCompanyTable").tablesorter(); // Enable tableSorter.js
$("#allCompaniesTable").tablesorter(); // Enable tableSorter.js

// Event Handlers
$("#jobs-per-page").change(function () { // When user changes the results per page dropdown menu
    jobsPerPage = $("#jobs-per-page :selected").val();
    hideExtraRows(1);
    updatePaginationBar(1);
});

$(".paginationLink").click(function () {
    var page = parseInt($(this).text()); // Find what number user clicked
    hideExtraRows(page);
    updatePaginationBar(page);
});

$(".fa").click(function () {
    if ($(this).hasClass("fa-angle-double-left")) { // If they click "go to first page" arrow
        hideExtraRows(1);
        updatePaginationBar(1);
    }
    else if ($(this).hasClass("fa-angle-double-right")) { // If they click "go to last page" arrow
        hideExtraRows(totalPages); 
        updatePaginationBar(totalPages);
    };
});

var filter;
var typedYet = false;
if (typedYet === false) {
    $("#TitleSearch").keydown(function () { // on any key pressed table will be displayed
        if (typedYet === false) {
            typedYet = true;
            $("#job-listings-table").toggle();
            $("#paginationContainer").toggle();
        }
    })
}

if (typedYet === false) {
    $("#TitleSearch").click(function (){
        if (typedYet === false) {
            if (screen.width < mobileWidth) {
                typedYet = true;
                $("#job-listings-table").toggle();
                $("#paginationContainer").toggle();
            }
        }
    })
}


$("#TitleSearch").bind('input', function () { //Will not trigger twice on control or alt commands such as copy paste
    filter = document.getElementById("TitleSearch").value.toUpperCase(); // Filter = whatever user has typed in
    $(".added").remove();
    // Loop through all table rows, and hide those that don't match the search query
    for (let i = 1; i < rows.length; i++) {
        company = rows[i].getElementsByTagName("td")[0]; // If we change table layout then you must also change the index here
        title = rows[i].getElementsByTagName("td")[2]; // If we change table layout then you must also change the index here
        // Check if company matches or job title matches
        if (company.innerHTML.toUpperCase().indexOf(filter) > -1 || title.innerHTML.toUpperCase().indexOf(filter) > -1) {
            $(rows[i]).css("display", "table-row");
            $(rows[i]).removeClass("filtered");
        } else { // Else add this "filtered" class, which exists so that the hideExtraRows() function doesn't interfere
            $(rows[i]).addClass("filtered");
        };
    };
    // adds link for company with no jobs
    if (rowsFiltered.length === 1) {//if no results are found
        var result = "/CompanyInfo/Details/" + filter; //url
        console.log(result);
        $.get(result).done(function () { // check if company page exists
            console.log("success!");

            var display = toTitleCase(filter);
            $('tbody').append("<tr class=\"added\"><td class=\"target\">" + display + "</td><td></td><td>no jobs available at this time</td><td></td><td></td><td></td></tr>");
            $('.target').wrapInner('<a class ="link">');
            $('.link').attr({ 'href': result });
            $('.added').css("display", "table-row");
        }).fail(function () {
            console.log("fail!");
        });
    }
    if (addedRows.length > 1) {
        $(addedRows[1]).remove();
    }
    hideExtraRows(1);
    updatePaginationBar(1);
})

/* removed triggers twice when two keys are pressed simultaneously
$("#TitleSearch").keyup(function () { // Whenever user types in search bar
    if (typedYet === false) { // If this is their first time typing, make the table appear
        typedYet = true;
        $("#job-listings-table").toggle();
        $("#paginationContainer").toggle();
    };
    filter = document.getElementById("TitleSearch").value.toUpperCase(); // Filter = whatever user has typed in
    $(".added").remove();
    // Loop through all table rows, and hide those that don't match the search query
    for (let i = 1; i < rows.length; i++) {
        company = rows[i].getElementsByTagName("td")[0]; // If we change table layout then you must also change the index here
        title = rows[i].getElementsByTagName("td")[2]; // If we change table layout then you must also change the index here
        // Check if company matches or job title matches
        if (company.innerHTML.toUpperCase().indexOf(filter) > -1 || title.innerHTML.toUpperCase().indexOf(filter) > -1) { 
            $(rows[i]).css("display", "table-row");
            $(rows[i]).removeClass("filtered");
        } else { // Else add this "filtered" class, which exists so that the hideExtraRows() function doesn't interfere
            $(rows[i]).addClass("filtered");
        };
    };
    /* Tried to make more modular but pauses after checkurl is run
    if (rowsFiltered.length === 1) {
        var result = "/CompanyInfo/Details/" + filter;
        checkUrl(result);
        if (companyExists === true) {
            $('tbody').append("<tr class=\"added\"><td class=\"target\">" + filter + "</td><td></td><td>no jobs available at this time</td><td></td><td></td><td></td></tr>");
            $('.target').wrapInner('<a class ="link">');
            $('.link').attr({ 'href': result });
            $('.added').css("display", "table-row");
        }
    }
    
    // adds link for company with no jobs
    if (rowsFiltered.length === 1) {//if no results are found
        var result = "/CompanyInfo/Details/" + filter; //url
        console.log(result);    
        $.get(result).done(function () { // check if company page exists
            console.log("success!");
            
            var display = toTitleCase(filter);
            $('tbody').append("<tr class=\"added\"><td class=\"target\">" + display + "</td><td></td><td>no jobs available at this time</td><td></td><td></td><td></td></tr>");
            $('.target').wrapInner('<a class ="link">');
            $('.link').attr({ 'href': result });
            $('.added').css("display", "table-row");
        }).fail(function () {
            console.log("fail!");
        });
    }
    if (addedRows.length > 1) {
        $(addedRows[1]).remove();
    }
    hideExtraRows(1);
    updatePaginationBar(1);
});
*/

var hasBeenFocusedYet = false;
if (getCurentFileName() === "Admin") {  // This condition allows the table to animate once page is loaded, no click/focus needed
    $("#search-wrapper").animate({ height: "35px" }, 750);
} else {
    $("#TitleSearch").keydown(function () { // Once they click/focus on search bar it will move up to top of page
        if (!hasBeenFocusedYet) {
            hasBeenFocusedYet = true;
            $("#search-wrapper").animate({ height: "35px" }, 750);
        };
    });
};


// Function declarations
function getCurentFileName() {
    var pagePathName = window.location.pathname;
    return pagePathName.substring(pagePathName.lastIndexOf("/") + 1);
}



function hideExtraRows(page) {
    rowsFiltered = document.querySelectorAll("tr:not(.filtered)");
    for (let i = 1; i <= rowsFiltered.length; i++) {
        if (i > (page - 1) * jobsPerPage && i <= page * jobsPerPage) { // Checks if row is on the current page or not
            $(rowsFiltered[i]).css("display", "table-row");
        } else {
            $(rowsFiltered[i]).css("display", "none");
        };
    };
};
function toTitleCase(title) {
    var test = title.split(" ");
    var words = test.length;
    var display = "";
    for (var i = 0; i < words; i++) {
        var part = test[i].charAt(0).toUpperCase() + test[i].substr(1).toLowerCase();
        display = display + part + " ";
    }
    return display;
}
/* User story 148, does not work, working solution above 
function checkUrl(link) {
    console.log(link);
    $.get(link).done(function () {
        console.log("success!");
        companyExists = true;
        return;
    }).fail(function () {
        console.log("fail!");
        companyExists = false;
        return;
    });
};
*/
function updatePaginationBar(page) {
    /* Update Pagination Bar Function:
    * Bootstrap makes the last pagination link have rounded edges and makes it difficult to get the same style on other links 
    * That's why the last pagination link must always be visible if the pagination bar is visible
    * You'd have to change the bootstrap files to overwrite this behavior
    * Pagination links that aren't the first or last one can be hidden if they're not necessary
    * Ideally, the middle pagination link gets the "active" class. This is the default because it looks best/makes sense
    * However, if the "active" link is left or right of the middle it goes through a different code block
    * To-the-left, dead center, and to-the-right are all handled in seperate code blocks (only when all links are visible)
    * Code blocks for displaying none, some, and all pagination links are also seperate */

    totalPages = Math.ceil((rowsFiltered.length - 1) / jobsPerPage);


    // Clear the pagination bar
    var currentActivePage = document.getElementsByClassName("active");
    $(".active").removeClass("active");

    if (totalPages <= 1) { // Hide the page links if there's no need for them
        $(pageLinks).css("display", "none");
        $(".fa-2x").css("display", "none");
        $(pageLinks[0]).addClass("active"); // This is mainly here for tableSorter.js so it can find the current page
    }
    else if (totalPages < pageLimit) { // Hide some page links but don't hide the last one because it has a nice border-radius
        $(".fa-2x").css("display", "inline-block");

        for (let i = 1; i < pageLimit; i++) {
            if (i < totalPages) {
                $(pageLinks[i - 1]).html("<a>" + i + "</a>");
                $(pageLinks[i - 1]).css("display", "inline");
            } else {
                $(pageLinks[i - 1]).css("display", "none");
            };
        };
        var lastPageLink = $(pageLinks[pageLimit - 1]);
        $(lastPageLink).html("<a>" + totalPages + "</a>");
        $(lastPageLink).css("display", "inline");
        if (page == totalPages) {
            $(lastPageLink).addClass("active");
        } else {
            $(pageLinks[page - 1]).addClass("active");
        };
    } else { // If all page links are visible do this
        $(pageLinks).css("display", "inline"); // Show all
        $(".fa-2x").css("display", "inline-block");

        // Reassign "active" class
        if (page <= Math.floor(pageLimit / 2)) { // If page is left of middle pagiantion link
            for (let i = 1; i <= pageLimit; i++) {
                $(pageLinks[i - 1]).html("<a>" + i + "</a>");
            };
            $(pageLinks[page - 1]).addClass("active");
        }
        else if (page > totalPages - Math.floor(pageLimit / 2)) { // Else if page is right of middle pagiantion link
            var iterationCount = 0; // Iteration count will be different than var i so it's necessary to keep track of this
            for (let i = totalPages - pageLimit + 1; i <= totalPages; i++) {
                $(pageLinks[iterationCount]).html("<a>" + i + "</a>");
                if (page === i) {
                    $(pageLinks[iterationCount]).addClass("active");
                };
                iterationCount++;
            };
        } else { // Else if this is the middle pagination link
            for (let i = 0; i <= pageLimit; i++) {
                $(pageLinks[i]).html("<a>" + (page + Math.floor(i + 1 - pageLimit / 2)) + "</a>");
            };
            $(pageLinks[Math.floor(pageLimit/ 2)]).addClass("active"); // Assign "active" class to middle page link
        };
    };
};

//buttons turn grey after being clicked
$(document).ready(function () {
    $(".btn-grey").click(function () { // add btn-grey class to target
        $(this).css("background-color", "lightgrey");
        $(this).css("border-color", "black");
    });
});

//at certain scroll position change CSS to keep search bar visible
$(document).ready(function () {
    var elem = $('#search-wrapper') //var assignment
    var nav = $('nav')
    var title = $('#TitleSearch')
    $(window).scroll(function () { 
        elem.toggleClass('fixed-search-div', $(window).scrollTop() > 30); //when scoll position = 30 CSS change occurs
        nav.toggleClass('fixed-nav', $(window).scrollTop() > 30); // lower number = trigger happens sooner
        title.toggleClass('fixed-search', $(window).scrollTop() > 30);
    }).scroll();
});

    