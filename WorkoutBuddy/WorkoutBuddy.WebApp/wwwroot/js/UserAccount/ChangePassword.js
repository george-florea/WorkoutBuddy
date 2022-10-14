$("document").ready(e => {
    $("#oldPassword").change(e => {
        $.ajax({
            type: "post",
            url: "/UserAccount/VerifyOldPassword",
            success: (resp) => {
                if (resp == false) {
                    $("#oldPasswordSpan").text("This does not match the old password!");
                }
                else {
                    $("#oldPasswordSpan").text("");
                }
            },
            error: (err) => {
                console.log(err);
            },
            contentType: "application/json",
            data: JSON.stringify(e.currentTarget.value)
        })
    })

    $("#oldPassword").change(e => {
        $("#newPasswordSpan").text("");
    })

    $("#newPassword").change(e => {
        $("#newPasswordSpan").text("");
    })
    
    $("#confirmPassword").change(e => {
        $("#newPasswordSpan").text("");
    })

    $("#changeBtn").click( e => {
        let isValid = true;
        if ($("#oldPassword").val() == "") {
            $("#oldPasswordSpan").text("Required field!");
            isValid = false;
        }
        if ($("#newPassword").val() == "") {
            $("#newPasswordSpan").text("Required field!");
            isValid = false;
        }
        if($('#confirmPassword').val() == ""){
            $("#newPasswordSpan").text("Required field!");
            isValid = false;
        }
        if($('#confirmPassword').val() != $("#newPassword").val()){
            $("#confirmPasswordSpan").text("The passwords do not match!");
            isValid = false;
        }

        if(!isValid){
            return false;
        }
        else {
            var model = {
                oldPassword: $("#oldPassword").val(),
                newPassword: $("#newPassword").val()
            }
            $.ajax({
                type: "post",
                url: "/UserAccount/ChangePassword",
                success: (resp) => {
                    if (resp == false) {
                        $("#newPasswordSpan").text("The password must contain minimum eight characters, at least one uppercase letter, one lowercase letter, one number");
                    }
                    else{
                        location.replace('/UserAccount/ProfilePage')
                    }
                },
                error: (err) => {
                    console.log(err);
                },
                contentType: "application/json",
                data: JSON.stringify(model)
            })
        }
    })
})