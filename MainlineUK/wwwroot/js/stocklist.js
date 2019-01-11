//iVendi parameters
var iVendi = {
    terms: "60",
    mileages: "10000",
    deposits: "0, 500"
};

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
    //var dataToLoad = "/Stock/?handler=Json&min_price=9999&max_price=9999";
    var dataToLoad = "/Stock/?handler=Json";

    var loadFormData = $.get(dataToLoad, function (data) {
        console.log(dataToLoad + " Loaded");

        numItems = data.stock.length;
        numPages = Math.ceil(numItems / numItemsPerPage);

        localStorage.setItem("stocklist", JSON.stringify(data));

        //Get HPI details and update stocklist
        getVehicleHPIDetails()
            .then(newResult => filterStocklist(newResult))
            .catch(error => filterStocklist(error)); //If HPI data fails to be loaded then proceed anyway with HPI details unavailable

        //filterStocklist();
    });

    loadFormData.fail(function () {
        doErrorModal("Error Loading Form " + formToLoad, "The form at " + dataToLoad + " returned a server error and could not be loaded");
    });
}

function filterStocklist() {
    return new Promise(function (fulfill, reject) {
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

        if (minBudget.length > 1) {
            minBudget = minBudget.replace("£", "");
            minBudget = minBudget.replace(",", "");
            filteredStocklist = filteredStocklist.filter(stock => getMinMonthlyPayment(stock) >= minBudget);
        }

        if (maxBudget.length > 1) {
            maxBudget = maxBudget.replace("£", "");
            maxBudget = maxBudget.replace(",", "");
            filteredStocklist = filteredStocklist.filter(stock => getMinMonthlyPayment(stock) <= maxBudget);
        }

        numItems = filteredStocklist.length;
        numPages = Math.ceil(numItems / numItemsPerPage);

        localStorage.setItem("filteredStocklist", JSON.stringify(filteredStocklist));
        displayStockList();

        fulfill("Filter applied");
    });
}

function displayStockList() {
    return new Promise(function (fulfill, reject) {
        let stock = JSON.parse(localStorage.getItem("filteredStocklist"));
        let startItem = numItemsPerPage * curPage - numItemsPerPage;
        let endItem = numItemsPerPage * curPage - 1;
        let htmlData = "";

        var dateToday = new Date();
        var date2WeeksAgo = new Date();
        date2WeeksAgo.setDate(dateToday.getDate() - 28);

        for (let i = startItem; i <= endItem; i++) {
            if (i >= numItems) {
                //exit loop if max items per page reached
                break;
            }

            let createdDate = new Date(stock[i].createdDate);
            let dateOfRegistration = new Date(stock[i].dateOfRegistration);

            let createdDateStr = ("0" + createdDate.getDate()).slice(-2) + "/" + ("0" + createdDate.getMonth()).slice(-2) + "/" + createdDate.getFullYear();
            let dateOfRegistrationStr = ("0" + dateOfRegistration.getDate()).slice(-2) + "/" + ("0" + dateOfRegistration.getMonth()).slice(-2) + "/" + dateOfRegistration.getFullYear();

            let newStockHtml = "";

            if (createdDate >= date2WeeksAgo) {
                newStockHtml = `<div class="NewStock"><i class="fas fa-star"></i> New</div>`;
            }
            else {
                newStockHtml = ``;
            }

            let ivendiUsername = "www.ivendimotors.com";
            let ivendiQuoteeUID = "268E8202-338E-4B26-A6FE-74BCDAB0A357";

            let vehicleClass = "";

            if (stock[i].category === "CARS") {
                vehicleClass = "car";
            }
            else if (stock[i].category === "COMM") {
                vehicleClass = "lcv";
            }
            else {
                vehicleClass = "car";
            }

            let vehicleCondition = "used";

            if (i === 4) {
                htmlData += `
                <div class="row">
                    <div class="col">
                        <img src="/images/MotoNovo60Seconds.jpg" class="img-fluid mx-auto d-block" alt="MotoNovo Finance: Finance Decisions in 60 Seconds">
                    </div>
                </div>`;
            }

            if (i === 8) {
                htmlData += `
                <div class="row">
                    <div class="col">
                        <img src="/images/MotoNovoYesCar.jpg" class="img-fluid mx-auto d-block" alt="MotoNovo Finance: We Like to Say Yes">
                    </div>
                </div>`;
            }

            htmlData += `
                <div class="row">
                    <div class="col Vehicle">
                        <div class="row">
                            <div class="col-md-4 align-self-center">
                                <div class="row">
                                    <div class="col">
                                        ${newStockHtml}
                                        ${photosHtml(stock[i].stocklistImportID, stock[i].photo)}
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col text-center MoreDetails">
                                        <a href="#" data-toggle="modal" data-id="${stock[i].stocklistImportID}" data-target="#vehicleModal" data-loading-text="${stock[i].make} ${stock[i].model} ${stock[i].derivative} (${stock[i].manufacturedYear})">
                                            More Details <i class="fas fa-arrow-circle-right"></i>
                                        </a>
                                    </div>
                                </div>
                            </div>
                            <div class="col">
                                <div class="row">
                                    <div class="col-lg-9">
                                        <a class="OpenVehicle" href="#" data-toggle="modal" data-id="${stock[i].stocklistImportID}" data-target="#vehicleModal" data-loading-text="${stock[i].make} ${stock[i].model} ${stock[i].derivative} (${stock[i].manufacturedYear})"><h2>${stock[i].make} ${stock[i].model}</h2></a>
                                        <a class="OpenVehicle" href="#" data-toggle="modal" data-id="${stock[i].stocklistImportID}" data-target="#vehicleModal" data-loading-text="${stock[i].make} ${stock[i].model} ${stock[i].derivative} (${stock[i].manufacturedYear})"><h3>${stock[i].derivative} (${stock[i].manufacturedYear})</h3></a>
                                    </div>
                                    <div class="col-lg-3 text-right Price">
                                        <span>${getPrice(stock[i].price)}</span>
                                    </div>
                                </div>
                                <hr class="Divider" />
                                <div class="row">
                                    <div class="col">
                                        ${getAdvert(stock[i].advertDescription1)}
                                    </div>
                                </div>
                                <div class="row Spacer">
                                    <div class="col-md">
                                        <div class="alert alert-secondary" role="alert">
                                            <div class="row">
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
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row Spacer">
                            <div class="col">
                                ${monthlyPaymentsHtml(stock[i].stocklistImportID, stock[i].FinanceProductResults)}
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

        fulfill("Stock loaded");
    });
}

pricesSlider.on('change', function (values, handle) {
    filterStocklist();
});

budgetSlider.on('change', function (values, handle) {
    filterStocklist();
});

//Removed as switched to sliders instead
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
    filterStocklist();
});

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
        curPage = $(this).attr("aria-label");
        displayStockList();
    });

    $(".OpenVehicle").click(function (event) {
        let vehicleID = $(this).attr("data-id");
        let modalTitle = $(this).attr("data-loading-text");

        $("#vehicleID").val(vehicleID);
        $("#modalTitleID").val(modalTitle);
    });
}