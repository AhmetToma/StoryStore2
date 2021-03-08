

$(function () {
    let pathUrl = "/Home/SignUp";

    if (window.location.pathname === pathUrl) {
        getAgeRange("#singup-ageRanges");

    }
});

function getAgeRange(id) {
    $.ajax({
        type: "GET",
        contentType: "application/json;charset=utf-8",
        url: "/Home/GetAgeRanges",
        success: (list) => {
            $(`${id}`).empty();

            console.log(list);
            if (list.length !== 0) {
                list.map((element) => {
                    $(`${id}`).append("<option value='" + element.id + "'>" + element.range + "</option>");
                });

            }
        }
    });
}


$('#btn-signup-submit').click((e) => {
    e.preventDefault();

    let user = {
        email: $('#inp-singup-email').val(),
        password: $('#inp-singup-password').val(),
        fullName: $('#inp-signup-fullName').val(),
        ageRangeId: parseInt($('#singup-ageRanges').val())
    }
    console.log(user);
    if (user.email === '' || user.password === '', user.fullName === '' || user.ageRangeId==="") {
        Swal.fire({
            type: 'error',
            title: "you should enter all fields",
            timer: 3000
        });
    }
        else if (validateEmail(user.email) === false) {
            Swal.fire({
                type: 'error',
                title: "email not valid",
                timer: 1000
            });
        }
   
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json;charset=utf-8",
            url: "/Home/AddNewUser",
            data: JSON.stringify(user),
            success: (response) => {

                if (response === "done") {

                    Swal.fire({
                        type: 'success',
                        title: "you singup successful",
                        timer: 3000
                    });

                    setTimeout(() => window.open(`/Home/Dashboard`, "_self"),3000)
                    window.open(`/Home/Dashboard`, "_self")
                }
                else {

                    console.log(response);
                    Swal.fire({
                        type: 'error',
                        title: "Passwords must be at least 5 characters,and not dublicate email",
                        timer: 10000
                    });
                    HideLoader()
                }
            },
            error: () => {
                Swal.fire({
                    type: 'error',
                    title: "some thing went wrong",
                    timer: 10000
                });
            }
        });
    }
});

function validateEmail(email) {
    let reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    if (reg.test(email) == false) {
        return false;
    }
    return true;
}