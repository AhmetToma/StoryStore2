
$(function () {
    let pathUrl = "/Home/AddNewStory";

    if (window.location.pathname === pathUrl) {
        getAgeRange("#addNewStory-selectAgeRange");

    }
});



$("#addNewStory").click(e => {

    e.preventDefault();
    var formData = new FormData();

    let storyImage = document.getElementById("addNewStory-storyImage").files[0];
    let audioFile = document.getElementById("addNewStory-audioFile").files[0];
    let pdfFile = document.getElementById("addNewStory-pdfFile").files[0];

    let story = {

        storyName: $("#addNewStory-storyName").val(),
        AuthorName: $("#addNewStory-AuthorName").val(),
        description: $("#addNewStory-Description").val(),
        ageRangeId: parseInt($("#addNewStory-selectAgeRange").val())
    }
    console.log(storyImage);
    formData.append("storyImage", storyImage);
    formData.append("audioFile", audioFile);
    formData.append("pdfFile", pdfFile);
    formData.append("storyName", story.storyName);
    formData.append("AuthorName", story.AuthorName);
    formData.append("description", story.description);
    formData.append("ageRangeId", story.ageRangeId);


    if (story.ageRangeId === "" || story.storyName === "" || story.description === "" || story.author === "") {
        Swal.fire({
            type: 'error',
            title: "This Fields is Required : story name,Description,author,age Range",
            timer: 10000
        });
    }

    else if (storyImage === null || storyImage === undefined) {
        Swal.fire({
            type: 'error',
            title: "upload image Please",
            timer: 10000
        });
    }

    else {

        ShowLoader();
        $.ajax({
            type: "POST",
            contentType: false,
            processData: false,
            url: "/Story/AddNewStory",
            data: formData,
            success: (response) => {
                HideLoader();
                if (response === "done") {
                    $('.addNewStoryForm')[0].reset();
                    Swal.fire({
                        type: 'success',
                        title: "successfuly new story has been added",
                        timer: 10000
                    });
                }
                else {
                    Swal.fire({
                        type: 'error',
                        title: "some thing went wrong",
                        timer: 10000
                    });
                }
            },
            error: () => {
                Swal.fire({
                    type: 'error',
                    title: "some thing went wrong",
                    timer: 10000
                });
                HideLoader();
            }
        });
    }
})
