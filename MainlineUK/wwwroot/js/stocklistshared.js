function getPrice(price) {
    let moneyFormat = wNumb({
        decimals: 0,
        thousand: ',',
        prefix: '£'
    });

    return moneyFormat.to(price);
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

    for (let photo of vehiclePhotos) {
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
                <a class="OpenCar" data-toggle="modal" data-id="${itemID}" data-target="#CarModal" data-loading-text="">
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

var pricesSliderElem = document.getElementById('pricesSlider');
var budgetSliderElem = document.getElementById('budgetSlider');

var pricesSlider = noUiSlider.create(pricesSliderElem, {
    start: [0, 25000],
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
    start: [0, 2000],
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