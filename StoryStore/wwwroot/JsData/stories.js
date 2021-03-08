$(function () {

    let url = "/Home/Stories";
    if (window.location.pathname === url) {
      
        GetStoriesByAgeRange();
        $(".left_col").css("height", "1500px");
       // GetStoriesCount();

    }
});
let requesQueryBoard = {
    pageNumber: 1,
    pageSize: 3,
    storyName: "",
   
};

// #region Ajax Call And create  table
function GetStoriesByAgeRange() {

    if (requesQueryBoard.pageNumber === 1) {
        disableButton("#btn-storyBoard-previous");
        ActiveButton("#btn-storyBoard-next");
    }
    else {
        ActiveButton("#btn-storyBoard-previous");
        ActiveButton("#btn-storyBoard-next");
    }

    $("#stories-storiesBoard").empty();
    $(`#num-storyBoard-pageNumber`).text(requesQueryBoard.pageNumber);
    requesQueryBoard.storyName = $("#inp-storyBoard-storyName").val();
    ShowLoader();
    console.log(requesQueryBoard);
    $.ajax({
        type: "POST",
        contentType: "application/json;charset=utf-8",
        url: "/Story/GetStoriesByAgeRange",
        data: JSON.stringify(requesQueryBoard),
        success: (list) => {

            if (list.length !== 0) {
                $(`.recordNotFound`).css('display', 'none');
                CreateStoryBoard(list, "#stories-storiesBoard");
            }
            else {
                window.scrollTo(0, 0);
                disableButton("#btn-storyBoard-next");
                $(`.recordNotFound`).css('display', 'block');
                HideLoader();
            }
        }
    });
}
function CreateStoryBoard(list, id) {
    
    $(id).empty();
    console.log(list);
    list.map((element, index) => {
        $(id).append(`
    <div onclick="GetStoryDetails(${element.storyId})" class="col-md-4">
        <div class="movie-card">
            <div class="movie-header" id="background${index+1}">
            </div>
            <!--movie-header-->
            <div class="movie-content">
                <div class="movie-content-header">
                    <a href="#">
                        <h3 class="movie-title">${element.storyName}</h3>
                    </a>
                </div>
                <div class="movie-info">
                  
                    <!--date,time-->
                   
                    <!--screen-->
                    <div class="info-section">
                        <label>Author</label>
                        <span>${element.author}</span>
                    </div>
                    <!--row-->
                    <div class="info-section">
                        <label>Range</label>
                        <span>${element.ageRange.range}</span>
                    </div>
                    <!--seat-->
                </div>
            </div>
            <!--movie-content-->
        </div>
    </div>

`);
        $(`#background${index + 1}`).css('background-image', `url('..${element.imageUrl}')`);
    });
    HideLoader();
}





//#endregion
//#region Next-Previous Hanldler
$("#btn-storyBoard-previous").on('click', (event) => {
    event.preventDefault();
    if (requesQueryBoard.pageNumber > 1) requesQueryBoard.pageNumber -= 1;
    $('#num-storyBoard-pageNumber').text(requesQueryBoard.pageNumber);
    GetStoriesByAgeRange();
});
$("#btn-storyBoard-next").on('click', (event) => {
    event.preventDefault();
    requesQueryBoard.pageNumber += 1;
    $('#num-storyBoard-pageNumber').text(requesQueryBoard.pageNumber);
    GetStoriesByAgeRange();
});
//#endregion
// #region search

$("#inp-storyBoard-storyName").keyup(function () {
    clearTimeout(timer);
    requesQueryBoard.pageNumber = 1;
    $('#num-storyBoard-pageNumber').text(requesQueryBoard.pageNumber);
    timer = setTimeout(GetStoriesByAgeRange, typingInterval);
});

// #endregion 


// #region story details
function GetStoryDetails(storyId) {
    window.open(`/Story/GetStoryDetails?storyId=${storyId}`, "_self");
}

//#endregion


