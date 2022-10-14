let ShowAlert = (isDeleted, i) => {
    if(isDeleted == true){
        $(`#split_${i}`).css("display", "none")
        alert("Split deleted successfully!")
    }
    else{
        alert("You cannot delete a split that is being used by other users!")
    }
}

let DeleteSplits = (count, url) => {
    for (let i = 0; i < count; i++)
    {
        $(`#deleteSplit_${i}`).click(e => {
            let id = window["split" + i]
            $.ajax({
                type: "post",
                url: url,
                success: (resp) => {
                    let isDeleted = JSON.parse(resp);
                    ShowAlert(isDeleted, i);
                },
                error: (err) => {
                    console.log(err);
                },
                contentType: "application/json",
                data: JSON.stringify(id)
            })
        })
    }
}
