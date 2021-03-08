$(function () {

    let url = "/Home/DeleteStory";
    if (window.location.pathname === url) {
        GetAllStories();
        GetStoriesCount();

    }
});
let requesQuery = {
    pageNumber: 1,
    pageSize: 6,
    storyName: "",
};

// #region Ajax Call And create  table
function GetAllStories() {

    if (requesQuery.pageNumber === 1) {
        disableButton("#btn-storyActions-previous");
        ActiveButton("#btn-storyActions-next");
    }
    else {
        ActiveButton("#btn-storyActions-previous");
        ActiveButton("#btn-storyActions-next");
    }

    $("#table-storyActions").empty();
    $(`#num-storyActions-pageNumber`).text(requesQuery.pageNumber);
    requesQuery.storyName = $("#inp-storyActions-storyName").val();
    ShowLoader();
    console.log(requesQuery);
    $.ajax({
        type: "POST",
        contentType: "application/json;charset=utf-8",
        url: "/Story/GetAllStories",
        data: JSON.stringify(requesQuery),
        success: (list) => {

            if (list.length !== 0) {
                $(`.recordNotFound`).css('display', 'none');
                CreateStoryTabel(list, "#table-storyActions");
            }
            else {
                disableButton("#btn-storyActions-next");
                $(`.recordNotFound`).css('display', 'block');
                HideLoader();
            }
        }
    });
}
function CreateStoryTabel(list, tableId) {
    $(tableId).empty();
    list.map((element, index) => {
        $(tableId).append(`
<tr id="t${index}" >
    <td>${element.storyId}</td>
    <td>${element.storyName ? element.storyName : ""} </td>
    <td>${element.author ? element.author : ""} </td>
   <td onclick="DeleteStory(${element.storyId})"><i  class="fa fa-2x fa-trash text-danger" aria-hidden="true"></i></td>
             </tr>
`);
    });
    HideLoader();
}


function GetStoriesCount() {
    $('#select-storyActions-selectRowCount').empty();
    $.getJSON(`/Story/GetStoriesCount`, function (num) {

        if (num < 10) {
            $('#select-storyActions-selectRowCount').append(`
               <option selected value="2">2</option>
               <option selected value="6">6</option>
         <option value="${num}">All Records (${num})</option>
`);
        }

        else {
            $('#select-storyActions-selectRowCount').append(`
               <option selected value="6">6</option>
                    <option value="10">10</option>
                    <option value="15">15</option>
                    <option value="20">20</option>
                    <option value="${num}">All Records (${num})</option>
`);
        }
        $('#span-storyActions-rowCount').text(num)
    }

    )

   
}

$('#select-storyActions-selectRowCount').on('change', () => {
    ShowLoader();
    requesQuery.pageSize = parseInt($('#select-storyActions-selectRowCount').val());
    requesQuery.pageNumber = 1;
    $('#num-storyActions-pageNumber').text(requesQuery.pageNumber);
    GetAllStories();
});
//#endregion
//#region Next-Previous Hanldler
$("#btn-storyActions-previous").on('click', (event) => {
    event.preventDefault();
    if (requesQuery.pageNumber > 1) requesQuery.pageNumber -= 1;
    $('#num-storyActions-pageNumber').text(requesQuery.pageNumber);
    GetAllStories();
});
$("#btn-storyActions-next").on('click', (event) => {
    event.preventDefault();
    requesQuery.pageNumber += 1;
    $('#num-storyActions-pageNumber').text(requesQuery.pageNumber);
    GetAllStories();
});
//#endregion
// #region search
let timer;
let typingInterval = 500;
$("#inp-storyActions-storyName").keyup(function () {
    clearTimeout(timer);
    requesQuery.pageNumber = 1;
    $('#num-storyActions-pageNumber').text(requesQuery.pageNumber);
    timer = setTimeout(GetAllStories, typingInterval);
});

// #endregion 
// #region reset 
$('#btn-storyActions-resetSearch').click((event) => {
    event.preventDefault();
    $("#inp-storyActions-storyName").val('');
    requesQuery.pageNumber = 1;
    requesQuery.pageSize = 6;
    $('#select-storyActions-selectRowCount').val('6');
    $('#num-storyActions-pageNumber').text(requesQuery.pageNumber);
    GetAllStories();
});

//#endregion




// #region Delete Story

// #region delete makineler
function DeleteStory(storyId) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    });
    swalWithBootstrapButtons.fire({
        title: `story id: ${storyId} will  be deleted`,
        text: `Are you sure?`,
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, Remove !',
        cancelButtonText: `No , Don't!`,
        reverseButtons: true
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "GET",
                contentType: "application/json;charset=utf-8",
                url: "/Story/DeleteStory?storyId=" + storyId,
                success: (message) => {
                    if (message === "done") {
                        GetAllStories();
                        GetStoriesCount();
                        Swal.fire({
                            title: 'done!',
                            text: 'story has been removed',
                            type: 'success',
                            timer: 5000
                        });
                    }
                    else {
                        Swal.fire({
                            type: 'error',
                            title: 'Oops...',
                            text: 'some thing went wrong',
                            timer: 5000
                        });
                    }
                }

            });
        }
    });
}
//#endregion

