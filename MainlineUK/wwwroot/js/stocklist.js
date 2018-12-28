//Load in functionality for customer list
$(".Filters .Buttons").hide();
loadStockList();

var numItemsPerPage = 10;
var numPagesEitherSide = 3;
var numPages = 1;
var curPage = 1;
var numItems = 1;

//New method for loading stock list
function loadStockList() {
    var dataToLoad = "/Stock/?handler=Json";

    var loadFormData = $.get(dataToLoad, function (data) {
        console.log(dataToLoad + " Loaded");

        numItems = data.stock.length;
        numPages = Math.ceil(numItems / numItemsPerPage);

        localStorage.setItem("stocklist", JSON.stringify(data));

        filterStocklist();
    });

    loadFormData.fail(function () {
        doErrorModal("Error Loading Form " + formToLoad, "The form at " + dataToLoad + " returned a server error and could not be loaded");
    });
}

function filterStocklist() {
    //Reset page to 1
    curPage = 1;

    let stocklist = JSON.parse(localStorage.getItem("stocklist"));
    let filteredStocklist = stocklist.stock;

    let make = $("#FilterMake").val();
    let model = $("#FilterModel").val();
    let minPrice = $("#FilterMinPrice").val();
    let maxPrice = $("#FilterMaxPrice").val();
    let minBudget = $("#FilterMinBudget").val();
    let maxBudget = $("#FilterMaxBudget").val();

    if (make.length > 1) {
        filteredStocklist = filteredStocklist.filter(stock => stock.make === make);
    }

    if (model.length > 1) {
        filteredStocklist = filteredStocklist.filter(stock => stock.model === model);
    }

    if (minPrice.length > 1) {
        minPrice = minPrice.replace("£", "");
        minPrice = minPrice.replace(",", "");
        filteredStocklist = filteredStocklist.filter(stock => stock.price >= minPrice);
    }

    if (maxPrice.length > 1) {
        maxPrice = maxPrice.replace("£", "");
        maxPrice = maxPrice.replace(",", "");
        filteredStocklist = filteredStocklist.filter(stock => stock.price <= maxPrice);
    }

    numItems = filteredStocklist.length;
    numPages = Math.ceil(numItems / numItemsPerPage);

    localStorage.setItem("filteredStocklist", JSON.stringify(filteredStocklist));
    displayStockList();
}

function displayStockList() {
    let stock = JSON.parse(localStorage.getItem("filteredStocklist"));
    let startItem = numItemsPerPage * curPage - numItemsPerPage;
    let endItem = numItemsPerPage * curPage;
    let htmlData = "";

    for (let i = startItem; i <= endItem; i++) {
        if (i >= numItems) {
            //exit loop if max items per page reached
            break;
        }

        htmlData += `
            <div class="row">
                <div class="col">
                    <div class="row Vehicle">
                        <div class="col-md-4">
                            <div class="row">
                                <div class="col">
                                    ${photosHtml(stock[i].stocklistImportID, stock[i].photo)}
                                </div>
                            </div>
                            <div class="row">
                                <div class="col text-center MoreDetails">
                                    <a href="#">More Details &gt;</a>
                                </div>
                            </div>
                        </div>
                        <div class="col">
                            <div class="row">
                                <div class="col-lg-9">
                                    <h2>${stock[i].make} ${stock[i].model}</h2>
                                    <h3>${stock[i].derivative} (${stock[i].manufacturedYear})</h3>
                                </div>
                                <div class="col-lg-3 text-right Price">
                                    <span>${getPrice(stock[i].price)}</span>
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col">
                                    ${getAdvert(stock[i].advertDescription1)}
                                </div>
                            </div>
                            <div class="row Spacer">
                                <div class="col-md-3 col-sm-4 col-6">
                                    <i class="fas fa-cogs"></i> ${stock[i].transmission}
                                </div>
                                <div class="col-md-3 col-sm-4 col-6">
                                    <i class="fas fa-tachometer-alt"></i> ${stock[i].mileage}${stock[i].mileageUnit}
                                </div>
                                <div class="col-md-3 col-sm-4 col-6">
                                    <i class="fas fa-palette"></i> ${stock[i].colour}
                                </div>
                                <div class="col-md-3 col-sm-4 col-6">
                                    <i class="fas fa-gas-pump"></i> ${stock[i].fuelType}
                                </div>
                                <div class="col-md-3 col-sm-4 col-6">
                                    <i class="fas fa-tools"></i> ${stock[i].engineSize}${stock[i].engineSizeUnit}
                                </div>
                                <div class="col-md-3 col-sm-4 col-6">
                                    <i class="fas fa-door-closed"></i> ${stock[i].doors}
                                </div>
                                <div class="col-md-6 col-sm-8 col-12">
                                    <i class="fas fa-users"></i> ${getPreviousOwners(stock[i].previousOwners)}
                                </div>
                            </div>
                            <div class="row Spacer">
                                <div class="col text-right">
                                    <button type="button" class="btn btn-primary">Apply</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>`;
    }

    htmlData += `
        <div class="row Spacer">
            <div class="alert alert-secondary col" role="alert">
                ${pagesHtml(numPages, curPage, numPagesEitherSide)}
            </div>
        </div>`;

    $("#VehicleArea").html(htmlData);
    listLoadedFunctions();
}

pricesSlider.on('change', function (values, handle) {
    filterStocklist();
});

budgetSlider.on('change', function (values, handle) {
    filterStocklist();
});

//If make selected then only show models for this make and clear selection
$("#FilterMake").change(function (event) {
    var make = $(this).val();

    var modelDropDown = $("#FilterModel");
    if (make === "") {
        $("option, optgroup", modelDropDown).show();
    }
    else {
        modelDropDown.val("");
        $("optgroup, optgroup > option", modelDropDown).hide();
        $("optgroup[label='" + make + "'], optgroup[label='" + make + "'] > option", modelDropDown).show();
    }
});

////If from price selected then only show higher to prices
//$("#FilterPriceFrom").change(function (event) {
//    var minPrice = $(this).val();

//    $("#FilterPriceTo option").each(function (event) {
//        if (parseInt($(this).val()) <= minPrice) {
//            $(this).hide();
//        }
//        else {
//            $(this).show();
//        }
//    });
//});

////If to price selected then only show lower from prices
//$("#FilterPriceTo").change(function (event) {
//    var maxPrice = $(this).val();

//    $("#FilterPriceFrom option").each(function (event) {
//        if (parseInt($(this).val()) >= maxPrice) {
//            $(this).hide();
//        }
//        else {
//            $(this).show();
//        }
//    });
//});

//Submit search form when a change is made
$("#SearchForm select").change(function (event) {
    //Reset page back to 1
    //$("#PageID").val("1");

    //$("#SearchForm").submit();
    filterStocklist();
});

//$("#SearchForm").submit(function (event) {
//    event.preventDefault();

//    var currentSort = $("#CurrentSortID").val();

//    var make = $("#FilterMake").val();
//    var model = $("#FilterModel").val();
//    var minPrice = $("#FilterMinPrice").val();
//    var maxPrice = $("#FilterMaxPrice").val();
//    var minBudget = $("#FilterMinBudget").val();
//    var maxBudget = $("#FilterMaxBudget").val();
//    var searchString = "";
//    var pageNum = $("#PageID").val();


//    //Construct search string based on what was selected
//    if (make.length > 1) {
//        searchString = searchString + "&make=" + make;
//    }
//    if (model.length > 1) {
//        //Remove spaces from models as causes issues
//        model = model.replace(" ", "_");
//        searchString = searchString + "&model=" + model;
//    }
//    if (minPrice.length > 1) {
//        minPrice = minPrice.replace("£", "");
//        minPrice = minPrice.replace(",", "");
//        searchString = searchString + "&min_price=" + minPrice;
//    }
//    if (maxPrice.length > 1) {
//        maxPrice = maxPrice.replace("£", "");
//        maxPrice = maxPrice.replace(",", "");
//        searchString = searchString + "&max_price=" + maxPrice;
//    }
//    if (minBudget.length > 1) {
//        minBudget = minBudget.replace("£", "");
//        minBudget = minBudget.replace(",", "");
//        searchString = searchString + "&min_budget=" + minBudget;
//    }
//    if (maxBudget.length > 1) {
//        maxBudget = maxBudget.replace("£", "");
//        maxBudget = maxBudget.replace(",", "");
//        searchString = searchString + "&max_budget=" + maxBudget;
//    }

//    //Trim first & from search string if has values and replace with ?
//    if (searchString.length > 1) {
//        searchString = searchString.substring(1, searchString.length);
//        searchString = "?" + searchString;
//    }

//    $("#VehicleArea").html($("#LoadingHTML").html());
//    loadList("CarListArea", "Stock", "StockList", currentSort, searchString, pageNum);
//});

//function loadList(
//    loadIntoDivID,
//    relativeURL,
//    listToRefresh,
//    currentSort,
//    currentFilter,
//    pageNum
//) {
//    var dataToLoad;

//    if (currentSort.length > 0 && pageNum.length > 0) {
//        dataToLoad = "/" + relativeURL + "/Index/" + currentSort + "/" + pageNum + "/" + currentFilter + " #" + listToRefresh;
//    }
//    else if (currentSort.length > 0) {
//        dataToLoad = "/" + relativeURL + "/Index/" + currentSort + "/" + currentFilter + " #" + listToRefresh;
//    }
//    else {
//        dataToLoad = "/" + relativeURL + "/Index/" + currentFilter + " #" + listToRefresh;
//    }

//    $("#" + loadIntoDivID).load(dataToLoad, function (responseText, textStatus, req) {
//        if (textStatus === "error") {
//            doErrorModal("Error Loading " + dataToLoad, "The list at " + dataToLoad + " returned a server error and could not be loaded");
//        }
//        else {
//            console.log(dataToLoad + " Loaded");
//            listLoadedFunctions();
//        }
//    });
//}

function listLoadedFunctions() {
    $(function () {
        return $(".carousel.lazy").on("slide.bs.carousel", function (ev) {
            var lazy;
            lazy = $(ev.relatedTarget).find("img[data-src]");
            lazy.attr("src", lazy.data('src'));
            lazy.removeAttr("data-src");
        });
    });

    $(".PageNav").click(function (event) {
        //var pageNum = $(this).attr("aria-label");

        //$("#PageID").val(pageNum);
        //$("#SearchForm").submit();

        curPage = $(this).attr("aria-label");
        displayStockList();
    });

}