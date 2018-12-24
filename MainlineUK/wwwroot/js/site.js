//Load in functionality for customer list
loadStockList();

function loadStockList() {
    var currentSort = $("#CurrentSortID").val();

    loadList("CarListArea", "Stock", "StockList", currentSort, "", "");
}

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

//If from price selected then only show higher to prices
$("#FilterPriceFrom").change(function (event) {
    var minPrice = $(this).val();

    $("#FilterPriceTo option").each(function (event) {
        if (parseInt($(this).val()) <= minPrice) {
            $(this).hide();
        }
        else {
            $(this).show();
        }
    });
});

//If to price selected then only show lower from prices
$("#FilterPriceTo").change(function (event) {
    var maxPrice = $(this).val();

    $("#FilterPriceFrom option").each(function (event) {
        if (parseInt($(this).val()) >= maxPrice) {
            $(this).hide();
        }
        else {
            $(this).show();
        }
    });
});

//Submit search form when a change is made
$("#SearchForm select").change(function (event) {
    //Reset page back to 1
    $("#PageID").val("1");

    $("#SearchForm").submit();
});

$("#SearchForm").submit(function (event) {
    event.preventDefault();

    var currentSort = $("#CurrentSortID").val();

    var make = $("#FilterMake").val();
    var model = $("#FilterModel").val();
    var priceFrom = $("#FilterPriceFrom").val();
    var priceTo = $("#FilterPriceTo").val();
    var mileage = $("#FilterMileage").val();
    var transmission = $("#FilterTransmission").val();
    var fuelType = $("#FilterFuelType").val();
    var bodyType = $("#FilterBodyType").val();
    var searchString = "";
    var pageNum = $("#PageID").val();

    //Remove spaces from models as causes issues
    model = model.replace(" ", "_");
    bodyType = bodyType.replace(" ", "_");

    //Construct search string based on what was selected
    if (make.length > 1) {
        searchString = searchString + "&make=" + make;
    }
    if (model.length > 1) {
        searchString = searchString + "&model=" + model;
    }
    if (priceFrom.length > 1) {
        searchString = searchString + "&min_price=" + priceFrom;
    }
    if (priceTo.length > 1) {
        searchString = searchString + "&max_price=" + priceTo;
    }
    if (mileage.length > 1) {
        searchString = searchString + "&mileage=" + mileage;
    }
    if (transmission.length > 1) {
        searchString = searchString + "&transmission=" + transmission;
    }
    if (fuelType.length > 1) {
        searchString = searchString + "&fuel_Type=" + fuelType;
    }
    if (bodyType.length > 1) {
        searchString = searchString + "&body_Type=" + bodyType;
    }

    //Trim first & from search string if has values and replace with ?
    if (searchString.length > 1) {
        searchString = searchString.substring(1, searchString.length);
        searchString = "?" + searchString;
    }

    $("#CarListArea").html($("#LoadingHTML").html());
    loadList("CarListArea", "Stock", "StockList", currentSort, searchString, pageNum);
});

function loadList(
    loadIntoDivID,
    relativeURL,
    listToRefresh,
    currentSort,
    currentFilter,
    pageNum
) {
    var dataToLoad;

    if (currentSort.length > 0 && pageNum.length > 0) {
        dataToLoad = "/" + relativeURL + "/Index/" + currentSort + "/" + pageNum + "/" + currentFilter + " #" + listToRefresh;
    }
    else if (currentSort.length > 0) {
        dataToLoad = "/" + relativeURL + "/Index/" + currentSort + "/" + currentFilter + " #" + listToRefresh;
    }
    else {
        dataToLoad = "/" + relativeURL + "/Index/" + currentFilter + " #" + listToRefresh;
    }

    $("#" + loadIntoDivID).load(dataToLoad, function (responseText, textStatus, req) {
        if (textStatus === "error") {
            doErrorModal("Error Loading " + dataToLoad, "The list at " + dataToLoad + " returned a server error and could not be loaded");
        }
        else {
            console.log(dataToLoad + " Loaded");
            listLoadedFunctions();
        }
    });
}

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
        var pageNum = $(this).attr("aria-label");

        $("#PageID").val(pageNum);
        $("#SearchForm").submit();
    });

}

function doModal(title, text) {
    console.log(text);

    html = '<div id="dynamicModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="confirm-modal" aria-hidden="true">';
    html += '<div class="modal-dialog">';
    html += '<div class="modal-content">';
    html += '<div class="modal-header">';
    html += '<h5 class="modal-title" id="customerModalLabel"><i class="fas fa-info-circle"></i> ' + title + '</h5>';
    html += '<button type="button" class="close" data-dismiss="modal" aria-label="Close">';
    html += '<span aria-hidden="true">&times;</span>';
    html += '</button>';
    html += '</div>';
    html += '<div class="modal-body">';
    html += '<p>' + text + '</p>';
    html += '</div>';
    html += '<div class="modal-footer">';
    html += '<span class="btn btn-primary" data-dismiss="modal">Close</span>';
    html += '</div>';  // content
    html += '</div>';  // dialog
    html += '</div>';  // footer
    html += '</div>';  // modalWindow
    $('body').append(html);
    $("#dynamicModal").modal();
    $("#dynamicModal").modal('show');

    $('#dynamicModal').on('hidden.bs.modal', function (e) {
        $(this).remove();
    });
}

function doErrorModal(title, text) {
    console.log(text);

    html = '<div id="dynamicModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="confirm-modal" aria-hidden="true">';
    html += '<div class="modal-dialog">';
    html += '<div class="modal-content">';
    html += '<div class="modal-header">';
    html += '<h5 class="modal-title" id="customerModalLabel"><i class="fas fa-exclamation-triangle"></i> ' + title + '</h5>';
    html += '<button type="button" class="close" data-dismiss="modal" aria-label="Close">';
    html += '<span aria-hidden="true">&times;</span>';
    html += '</button>';
    html += '</div>';
    html += '<div class="modal-body">';
    html += '<p>An unexpected error has occurred which could indicate a defect with the system</p>';
    html += '<div class="alert alert-danger" role="alert">';
    html += '<i class="fas fa-bug"></i> ' + text;
    html += '</div>';
    html += '</div>';
    html += '<div class="modal-footer">';
    html += '<span class="btn btn-primary" data-dismiss="modal">Close</span>';
    html += '</div>';  // content
    html += '</div>';  // dialog
    html += '</div>';  // footer
    html += '</div>';  // modalWindow
    $('body').append(html);
    $("#dynamicModal").modal();
    $("#dynamicModal").modal('show');

    $('#dynamicModal').on('hidden.bs.modal', function (e) {
        $(this).remove();
    });

    var audio = new Audio("/sounds/error.wav");
    audio.play();
}

function doCrashModal(error) {
    var stackError = $(error.responseText).find(".stackerror").html() || "Unknown error";
    var stackTrace = $(error.responseText).find(".rawExceptionStackTrace").html() || "";
    console.log(stackTrace);

    html = '<div id="dynamicModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="confirm-modal" aria-hidden="true">';
    html += '<div class="modal-dialog">';
    html += '<div class="modal-content">';
    html += '<div class="modal-header">';
    html += '<h5 class="modal-title" id="customerModalLabel"><i class="fas fa-exclamation-triangle"></i> An application error has occurred</h5>';
    html += '<button type="button" class="close" data-dismiss="modal" aria-label="Close">';
    html += '<span aria-hidden="true">&times;</span>';
    html += '</button>';
    html += '</div>';
    html += '<div class="modal-body">';
    html += '<p>An unexpected error has occurred which could indicate a defect with the system</p>';
    html += '<div class="alert alert-danger" role="alert">';
    html += '<i class="fas fa-bug"></i> ' + stackError;
    html += '</div>';
    html += '<div class="pre-scrollable small">';
    html += '<p><code>' + stackTrace + '</code></p>';
    html += '</div>';
    html += '</div>';
    html += '<div class="modal-footer">';
    html += '<span class="btn btn-primary" data-dismiss="modal">Close</span>';
    html += '</div>';  // content
    html += '</div>';  // dialog
    html += '</div>';  // footer
    html += '</div>';  // modalWindow
    $('body').append(html);
    $("#dynamicModal").modal();
    $("#dynamicModal").modal('show');

    $('#dynamicModal').on('hidden.bs.modal', function (e) {
        $(this).remove();
    });

    var audio = new Audio("/sounds/error.wav");
    audio.play();
}