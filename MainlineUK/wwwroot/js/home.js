loadStockList();
animateStockList();

var curItem = 0;
var numItems = 0;
var numItemsToShow = 3;
var latestCarControlsHover = false;

//New method for loading stock list
function loadStockList() {
    var dataToLoad = "/Stock/?handler=Json&sortOrder=Latest";

    var loadFormData = $.get(dataToLoad, function (data) {
        console.log(dataToLoad + " Loaded");

        numItems = data.stock.length;
        localStorage.setItem("stocklist", JSON.stringify(data));

        showStockList();
    });

    loadFormData.fail(function () {
        doErrorModal("Error Loading Form " + formToLoad, "The form at " + dataToLoad + " returned a server error and could not be loaded");
    });
}

function showStockList() {
    let htmlData = "";

    let stocklist = JSON.parse(localStorage.getItem("stocklist"));
    let stock = stocklist.stock;
    let startItem = curItem;
    let endItem = curItem + numItemsToShow;

    for (let i = startItem; i < endItem; i++) {
        let j = i;

        //Ensure list resets to begining of list if has reached the end
        if (j > numItems - 1) {
            j = i - numItems;
        }

        //Ensure list loops back to end and does not go negative
        if (j < 0) {
            j = 0;
        }

        htmlData += `
            <div class="col-md-4">
                <div class="card LatestVehicle">
                    <div class="card-img-top">
                        ${photosHtml(stock[j].stocklistImportID, stock[j].photo)}
                    </div>
                    <div class="container Details">
                        <div class="row">
                            <div class="col-xl-8 col-6">
                                <h2>${stock[j].make} ${stock[j].model}</h2>
                            </div>
                            <div class="col-xl-4 col-6 text-right Price">
                                <span>${getPrice(stock[j].price)}</span>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <h3>${stock[j].derivative} (${stock[j].manufacturedYear})</h3>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-6">
                                <i class="fas fa-cogs"></i> ${stock[j].transmission}
                            </div>
                            <div class="col-6">
                                <i class="fas fa-tachometer-alt"></i> ${stock[j].mileage}${stock[j].mileageUnit}
                            </div>
                            <div class="col-6">
                                <i class="fas fa-palette"></i> ${stock[j].colour}
                            </div>
                            <div class="col-6">
                                <i class="fas fa-gas-pump"></i> ${stock[j].fuelType}
                            </div>
                            <div class="col-6">
                                <i class="fas fa-tools"></i> ${stock[j].engineSize}${stock[j].engineSizeUnit}
                            </div>
                            <div class="col-6">
                                <i class="fas fa-door-closed"></i> ${stock[j].doors}
                            </div>
                            <div class="col-12">
                                <i class="fas fa-users"></i> ${getPreviousOwners(stock[j].previousOwners)}
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col text-center MoreDetails">
                            <a href="#">Finance from xxx p/m</a>
                        </div>
                    </div>
                </div>
            </div>`;
    }

    $("#LatestVehicleArea").html(htmlData);
    listLoadedFunctions();
}

function advanceStockList(direction) {
    if (direction === "R") {
        curItem -= 1;

        //If at begining then restart at end
        if (curItem < 0) {
            curItem = numItems - 1;
        }
    }
    else {
        curItem += 1;

        //If at end then restart at begining
        if (curItem > numItems - 1) {
            curItem = 0;
        }
    }
    showStockList();
}

function animateStockList() {
    setTimeout(function () {
        //Pause if focus is on buttons
        if (latestCarControlsHover === false) {
            advanceStockList("F");
        }
        animateStockList();
    }, 5000);
    //Every 5 secs
}

//Used to pause animation if mouse is over navigation controls
$(".LatestCarControls").mouseenter(function (event) {
    latestCarControlsHover = true;
});

$(".LatestCarControls").mouseleave(function (event) {
    latestCarControlsHover = false;
});

//Used to pause animation if mouse is over latest car area
$("#LatestVehicleArea").mouseenter(function (event) {
    latestCarControlsHover = true;
});

$("#LatestVehicleArea").mouseleave(function (event) {
    latestCarControlsHover = false;
});

$(".LatestCarControls").click(function (event) {
    let direction = $(this).attr("aria-label");
    advanceStockList(direction);
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
}