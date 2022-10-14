let DeleteComm = (isDeleted, isReply, i) => {
    if (isReply) {
        if(isDeleted == true){
            i.style.display = "none"
            alert("Reply deleted successfully!")
        }
        else{
            alert("The reply couldnt be deleted")
        }
    }
    else{
        if(isDeleted == true){
            $(`#comment_${i}`).css("display", "none")
            alert("Comment deleted successfully!")
        }
        else{
            alert("The comment couldnt be deleted")
        }
    }
    
}


let Delete = (url, e, isReply) => {
    let currentTarget = e.currentTarget;
    $.ajax({
        type: "post",
        url: url,
        success: (resp) => {
            if (isReply) {
                DeleteComm(resp, isReply, currentTarget.parentElement.parentElement.parentElement);
            }
            else {
                let i = currentTarget.parentNode.parentNode.parentNode.id;
                i = i.split('_')[1];
                DeleteComm(resp, isReply, i);
            }
            
            
        },
        error: (err) => {
            console.log(err);
        },
        contentType: "application/json",
        data: JSON.stringify(e.currentTarget.dataset['value'])
    })
}



let AddComment = (url, text, splitId, parentId = null) => {
    let model = {
        "CommentText": text,
        "ParentSplitID": splitId,
        "ParentCommentId": parentId
    }
    $.ajax({
        type: "post",
        url: url,
        success: (resp) => {
            
            location.reload();
        },
        error: (err) => {
            console.log(err);
        },
        contentType: "application/json",
        data: JSON.stringify(model)
    })
}