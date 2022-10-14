function BuildSet(setValue, index, type, exerciseTypes){
    var parent = document.getElementById(`exercise_${index}`);
    var sets = document.createElement('div');
    sets.id = `sets_${index}`
    sets.classList.add('form-group')
    parent.appendChild(sets);

    let val = parseInt(setValue);
    for(let i = 0; i < val; i++){
        let setLabel = document.createElement('label');
        setLabel.classList.add('form-label');
        setLabel.innerText = `Set ${i+1}:`
        sets.appendChild(setLabel)

        let container = document.createElement('div');
        container.classList.add('card');
        container.classList.add('card-body');
        container.style.backgroundColor = '#efefef'

        let label1 = document.createElement('label');
        let input1 = document.createElement('input');
        let label2 = document.createElement('label');
        let input2 = document.createElement('input');
        let span1 = document.createElement('span');
        let span2 = document.createElement('span');

        if(type == exerciseTypes['Cardio']){
            label1.innerText = "Distance";
            label1.classList.add('form-label');
            label1.name = `Exercises[${index}].Sets[${i}].Distance`;
            input1.name = `Exercises[${index}].Sets[${i}].Distance`;
            input1.classList.add('form-control')
            input1.type = 'number';
            input1.id = `input1_${i}_${index}`

            span1.classList.add('text-danger')
            span1.id = `span1_${i}_${index}`

            label2.innerText = "Duration";
            label2.classList.add('form-label')
            label2.name = `Exercises[${index}].Sets[${i}].Duration`;
            input2.name = `Exercises[${index}].Sets[${i}].Duration`;
            input2.classList.add('form-control')
            input2.type = 'number';
            input2.id = `input2_${i}_${index}`

            span2.classList.add('text-danger')
            span2.id = `span2_${i}_${index}`

            container.appendChild(label1);
            container.appendChild(input1);
            container.appendChild(span1);

            container.appendChild(label2);
            container.appendChild(input2);
            container.appendChild(span2);
        }
        else{
            label1.innerText = "Number of reps:";
            label1.classList.add('form-label');
            label1.name = `Exercises[${index}].Sets[${i}].Reps`;
            input1.name = `Exercises[${index}].Sets[${i}].Reps`;
            input1.classList.add('form-control')
            input1.type = 'number';
            input1.id = `input1_${i}_${index}`

            span1.classList.add('text-danger')
            span1.id = `span1_${i}_${index}`
            
            container.appendChild(label1);
            container.appendChild(input1);
            container.appendChild(span1);

            if(type == exerciseTypes['WeightLifting']){
                label2.innerText = "Weight";
                label2.classList.add('form-label')
                label2.name = `Exercises[${index}].Sets[${i}].Weight`;
                input2.name = `Exercises[${index}].Sets[${i}].Weight`;
                input2.classList.add('form-control')
                input2.type = 'number';
                input2.id = `input2_${i}_${index}`

                span2.classList.add('text-danger')
                span2.id = `span2_${i}_${index}`

                container.appendChild(label2);
                container.appendChild(input2);
                container.appendChild(span2);
            }
        }

        sets.appendChild(container);

        input1.onchange = e => {
            span1.innerText = "";
        }
        input2.onchange = e => {
            span2.innerText = "";
        }
    }
}

let ClearSet = (i) => {
    var div = document.getElementById(`exercise_${i}`);
    var sets = document.getElementById(`sets_${i}`);
    if (sets != null) {
        div.removeChild(sets)
    }
}

let VerifyInputs = (index, value) => {
    let ok = true;

    setsInput = document.getElementById(`noOfSets_${index}`);
    span = document.getElementById(`validateSetsNo_${index}`);

    if (setsInput.value == "") {
        span.innerText = "This is a required field!";
        ok = false;
    }

    for(let i = 0; i < value; i++){
        let input1 = document.getElementById(`input1_${i}_${index}`);
        let span1 = document.getElementById(`span1_${i}_${index}`);
    
        let input2 = document.getElementById(`input2_${i}_${index}`);
        let span2 = document.getElementById(`span2_${i}_${index}`);

        if (input1 != null && input1.value == "") {
            span1.innerText = "This is a required field!";
            ok = false;
        }

        if (input2 != null && input2.value == "") {
            span2.innerText = "This is a required field!";
            ok = false;
        }
    }

    return ok;
    
}

let VerifyDate = () => {
    if($("#date").val() == ''){
        $("#dateSpan").text("Required field!");
        return false;
    }
    val = Date.parse($("#date").val())
    date = new Date(val);
    currDate = new Date()
    currDate.setDate(currDate.getDate() - 7)
    if(currDate > date){
        $("#dateSpan").text("You cannot add a progress older than 7 days!");
        return false;
    }
    else{
        return true;
    }
}