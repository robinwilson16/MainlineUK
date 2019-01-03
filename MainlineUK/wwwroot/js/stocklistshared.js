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

function getPrice(price) {
    let moneyFormat = wNumb({
        decimals: 0,
        thousand: ',',
        prefix: '£'
    });

    return moneyFormat.to(price);
}

function getMonthlyPayment(JSON) {
    let payment = 0;

    try {
        payment = JSON.Payment;
    }
    catch {
        //payment = 0;
    }

    let moneyFormat = wNumb({
        decimals: 2,
        thousand: ',',
        prefix: '£'
    });

    return moneyFormat.to(payment);
}

function getMinMonthlyPayment(JSON) {
    let minPayment = null;

    try {
        for (var criteria of JSON.FinanceProductResults) {
            for (var payment of criteria.ProductResults) {
                if (minPayment === null) {
                    minPayment = payment.Payment;
                }
                else if (minPayment > payment.Payment) {
                    minPayment = payment.Payment;
                }
            }
        }
    }
    catch {
        minPayment = 0;
    }

    return minPayment;
}

function getAdvert(advert) {
    if (advert.length > 200) {
        advert = advert.substring(0, 197) + "...";
    }

    return advert;
}

function getPreviousOwners(prevOwners) {
    if (prevOwners === 1) {
        prevOwners += " Previous Owner";
    }
    else if (prevOwners > 1) {
        prevOwners += " Previous Owners";
    }
    else {
        prevOwners = "Unknown";
    }

    return prevOwners;
}

function pagesHtml(numPages, curPage, numPagesEitherSide) {
    let htmlData = "";
    let buttonStyle = "";
    let suffix = "";
    let startPage = +curPage - +numPagesEitherSide;
    let endPage = +curPage + +numPagesEitherSide;

    //Ensure start and end pages are not too small/large
    if (startPage < 1) {
        startPage = 1;
    }

    if (endPage > numPages) {
        endPage = numPages;
    }

    if (curPage !== 1) {
        htmlData += `
            <button class="btn btn-outline-secondary PageNav" aria-label="1">First</button>`;
    }

    for (let i = startPage; i <= endPage; i++) {
        //Highlight current page
        if (i === +curPage) {
            buttonStyle = "btn-secondary";
        }
        else {
            buttonStyle = "btn-outline-secondary";
        }

        //If last page then add ... to indicate more pages if there are
        if (i === +curPage + 3 && numPages > i) {
            suffix = "...";
        }
        else {
            suffix = "";
        }

        htmlData += `
            <button class="btn ${buttonStyle} PageNav" aria-label="${i}">${i}${suffix}</button>`;
    }

    if (curPage !== numPages) {
        htmlData += `
        <button class="btn btn-outline-secondary PageNav" aria-label="${numPages}">Last</button>`;
    }

    return htmlData;
}

function photosHtml(itemID, vehiclePhotos) {
    let htmlData = "";
    let navItems = "";
    let slideshowItems = "";
    let photoNum = 0;
    let classRef = "";
    let tagName = "";

    for (var photo of vehiclePhotos) {
        if (photoNum === 0) {
            classRef = "active";
            tagName = "src";
        }
        else {
            classRef = "";
            tagName = "data-src";
        }

        navItems +=
            `<li data-target="#PhotoCarousel${itemID}" data-slide-to="${photoNum}" class="${classRef}"></li>`;

        slideshowItems +=
            `<div class="carousel-item ${classRef}">
                <a class="OpenVehicle" data-toggle="modal" data-id="${itemID}" data-target="#vehicleModal" data-loading-text="">
                    <img ${tagName}="/stockdata/images/${itemID}/${photo.photoID}.jpg" alt="Images of" class="card-img-top" />
                </a>
            </div>`;

        photoNum += 1;
    }

    htmlData +=
        `<div class="carouselArea">
            <div class="carouselFrame">
                <div id="PhotoCarousel${itemID}" class="carousel slide lazy" data-ride="carousel" data-interval="0">
                    <ol class="carousel-indicators">
                        ${navItems}
                    </ol>
                    <div class="carousel-inner" role="listbox">
                        ${slideshowItems}
                    </div>
                    <a class="carousel-control-prev" href="#PhotoCarousel${itemID}" role="button" data-slide="prev">
                        <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                        <span class="sr-only">Previous</span>
                    </a>
                    <a class="carousel-control-next" href="#PhotoCarousel${itemID}" role="button" data-slide="next">
                        <span class="carousel-control-next-icon" aria-hidden="true"></span>
                        <span class="sr-only">Next</span>
                    </a>
                </div>
            </div>
        </div>`;

    return htmlData;
}

function monthlyPaymentsHtml(itemID, vehicleHPI) {
    let htmlData = "";
    let htmlItems = "";

    for (var criteria of vehicleHPI) {
        for (var payment of criteria.ProductResults) {

            htmlItems +=
                `<tr>
                    <td>&pound;${payment.Payment}</th>
                    <td>${criteria.Term} months</td>
                    <td>&pound;${criteria.Deposits}</td>
                    <td>${criteria.AnnualMileage} miles</td>
                </tr>`;
        }
    }

    if (htmlItems !== "") {
        htmlData +=
            `<table class="table table-striped text-center">
                <thead>
                    <tr>
                        <th scope="col">Monthly Payment</th>
                        <th scope="col">Term</th>
                        <th scope="col">Deposit</th>
                        <th scope="col">Annual Mileage</th>
                    </tr>
                </thead>
                <tbody>
                    ${htmlItems}
                </tbody>
            </table>`;
    }
    else {
        htmlData +=
            `<div class="alert alert-secondary text-center" role="alert">
                No financial products available for this vehicle
            </div>`;
    }

    return htmlData;
}

function financeFromHtml(itemID, vehicleHPI) {
    let htmlData = "";
    let minPayment = null;

    for (var criteria of vehicleHPI) {
        for (var payment of criteria.ProductResults) {
            if (minPayment === null) {
                minPayment = payment.Payment;
            }
            else if (minPayment > payment.Payment) {
                minPayment = payment.Payment;
            }
        }
    }

    if (minPayment !== null) {
        htmlData +=
            `Finance from ${getPrice(payment.Payment)} p/m <i class="fas fa-arrow-circle-right"></i>`;
    }
    else {
        htmlData +=
            `More details <i class="fas fa-arrow-circle-right"></i>`;
    }

    return htmlData;
}

var pricesSliderElem = document.getElementById('pricesSlider');
var budgetSliderElem = document.getElementById('budgetSlider');

//Get current values for prices and budgets
var minPrice = $("#FilterMinPrice").val();
var maxPrice = $("#FilterMaxPrice").val();
var minBudget = $("#FilterMinBudget").val();
var maxBudget = $("#FilterMaxBudget").val();

//If no values then set defaults
if (minPrice < 0) {
    minPrice = 0;
}
if (maxPrice <= 0 || maxPrice < minPrice) {
    maxPrice = 25000;
}
if (minBudget < 0) {
    minBudget = 0;
}
if (maxBudget <= 0 || maxBudget < minBudget) {
    maxBudget = 2000;
}

var pricesSlider = noUiSlider.create(pricesSliderElem, {
    start: [minPrice, maxPrice],
    connect: true,
    tooltips: false,
    step: 1000,
    range: {
        'min': 0,
        'max': 25000
    },
    ariaFormat: wNumb({
        decimals: 0
    }),
    format: wNumb({
        decimals: 0,
        thousand: ',',
        prefix: '£'
    })
});

var budgetSlider = noUiSlider.create(budgetSliderElem, {
    start: [minBudget, maxBudget],
    connect: true,
    tooltips: false,
    step: 10,
    range: {
        'min': 0,
        'max': 2000
    },
    ariaFormat: wNumb({
        decimals: 0
    }),
    format: wNumb({
        decimals: 0,
        thousand: ',',
        prefix: '£'
    })
});

pricesSlider.on('update', function (values, handle) {
    var FilterMinPrice = $("#FilterMinPrice");
    var FilterMaxPrice = $("#FilterMaxPrice");
    var FilterTextMinPrice = $("#FilterTextMinPrice");
    var FilterTextMaxPrice = $("#FilterTextMaxPrice");

    var value = values[handle];

    if (handle === 0) {
        FilterMinPrice.val(value);
        FilterTextMinPrice.html(value);
    }
    else if (handle === 1) {
        FilterMaxPrice.val(value);
        FilterTextMaxPrice.html(value);
    }
});

budgetSlider.on('update', function (values, handle) {
    var FilterMinBudget = $("#FilterMinBudget");
    var FilterMaxBudget = $("#FilterMaxBudget");
    var FilterTextMinBudget = $("#FilterTextMinBudget");
    var FilterTextMaxBudget = $("#FilterTextMaxBudget");

    var value = values[handle];

    if (handle === 0) {
        FilterMinBudget.val(value);
        FilterTextMinBudget.html(value);
    }
    else if (handle === 1) {
        FilterMaxBudget.val(value);
        FilterTextMaxBudget.html(value);
    }
});

//Load data in when model is displayed
$("#vehicleModal").on("shown.bs.modal", function () {
    var vehicleID = $("#vehicleID").val();
    var formTitle = $("#modalTitleID").val();

    $("#vehicleModalLabel").find(".title").html(formTitle);

    loadVehicleDetails(vehicleID);
});

$("#vehicleModal").on("hidden.bs.modal", function () {
    var loadingAnim = $("#LoadingAnimation").html();

    //Set back to loading text
    $("#VehicleDetails").html(loadingAnim);
});

function loadVehicleDetails(
    vehicleID
) {
    var dataToLoad = "/Stock/Details/" + vehicleID;

    var loadFormData = $.get(dataToLoad, function (data) {
        var formData = $(data).find("#VehicleInformation");
        $("#VehicleDetails").html(formData);

        console.log(dataToLoad + " Loaded");
    });

    loadFormData.fail(function () {
        doErrorModal("Error Loading Form " + formToLoad, "The form at " + dataToLoad + " returned a server error and could not be loaded");
    });
}

//iVendi
function getVehicleHPIDetails() {
    return new Promise(function (fulfill, reject) {
        //Get stocklist
        let stocklist = JSON.parse(localStorage.getItem("stocklist"));
        let fulfillvar = null;
        let rejectvar = null;

        //Get Json for vehicles to submit to iVendi
        let vehicleJson = "";
        for (let vehicle of stocklist.stock) {
            let vehicleClass = "Car";

            if (vehicle.category === "COMM") {
                vehicleClass = "LCV";
            }

            let dateOfRegistration = new Date(vehicle.dateOfRegistration);
            let dateOfRegistrationStr = ("0" + dateOfRegistration.getDate()).slice(-2) + "-" + ("0" + dateOfRegistration.getMonth()).slice(-2) + "-" + dateOfRegistration.getFullYear();

            if (vehicleJson !== "") {
                vehicleJson += `,`;
            }

            vehicleJson +=
                `{
                        "Id": ${vehicle.stocklistImportID},
                        "Dealer": "268E8202-338E-4B26-A6FE-74BCDAB0A357",
                        "Vehicle": {
                            "CashPrice": ${vehicle.price},
                            "IsNew": false,
                            "Identifier": "",
                            "IdentifierType": "RVC",
                            "Type": "${vehicleClass}",
                            "StockId": null,
                            "RegistrationNumber": "${vehicle.registration}",
                            "CurrentMileage": ${vehicle.mileage},
                            "RegistrationDate": "${dateOfRegistrationStr}",
                            "ProductUid": null
                        }
                    }`;
        }

        const PaymentSearchEndpoint = "https://quoteware3.ivendi.com/paymentsearch/";

        let PaymentSearchObject = `{
                "Debug": false,
                "Credentials": {
                    "Username": "www.ivendimotors.com",
                    "Mode": 0
                },
                "Parameters": {
                    "Terms": [${iVendi.terms}],
                    "AnnualMileages": [${iVendi.mileages}],
                    "Deposits": [${iVendi.deposits}]
                },
                "VehicleRequests": [
                    ${vehicleJson}
                ]
            }`;

        var dataToSend = PaymentSearchObject;

        var sentData = $.ajax({
            type: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            url: PaymentSearchEndpoint,
            data: dataToSend,
            success: (data) => {
                console.log("Vehicle HPI Data Retrieved");

                var mergedList = _.map(stocklist.stock, function (item) {
                    return _.extend(item, _.find(data.VehicleResults, { Id: item.Id }));
                });

                var stockWithHPI = { "stock": mergedList };
                localStorage.setItem("stocklist", JSON.stringify(stockWithHPI));

                //console.log(stockWithHPI);

                //filterStocklist();

                fulfillvar = true;
            }
        });

        sentData.done(function () {
            fulfill("HPI data retrieved");
        });

        sentData.fail(function () {
            doErrorModal("Error retrieving HPI data");

            reject(Error("Error retrieving HPI data"));
        });
    });
}