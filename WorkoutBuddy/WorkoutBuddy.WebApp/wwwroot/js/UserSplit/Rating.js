let RateSplit = (splitId) => {
    let btn = document.getElementById('rateBtn');
    btn.onclick = e => {
        let widget = document.getElementsByClassName('star-widget')[0];
        for (let el of widget.children) {
            if(el.checked == true){
                let rating = el.id.split('-')[1];

                let model = {
                    "Rating": rating,
                    "SplitId": splitId
                }
                let span = document.getElementById('rateSpan')
                $.ajax({
                    type: "post",
                    url: "/Rating/PostRating",
                    success: (resp) => {
                        console.log(resp);
                        span.classList.add(`${resp.itemClass}`)
                        span.innerText = resp.message
                    },
                    error: (err) => {
                        console.log(err);
                    },
                    contentType: "application/json",
                    data: JSON.stringify(model)
                })
            }
        }
    }
};
