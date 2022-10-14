let buildDropdown = (filteredExercises, i, selectedExercises = []) => {
    let div = document.getElementById(`muscleGroupSelector_${i}`);
    let deleteBtn = document.getElementById(`deleteBtn_${i}`)
    let container = document.createElement('div');
    container.id = `exercisesDropdown_${i}`;
    let label = document.createElement('label');
    label.innerText = "Exercises";
    container.appendChild(label);

    let dropdown = document.createElement('select');
    dropdown.classList.add('js-example-basic-multiple')
    dropdown.classList.add('form-control')
    dropdown.name = `Workouts[${i}].Exercises`
    dropdown.id = `exerciseList_${i}`
    dropdown.multiple = "multiple";
    filteredExercises = JSON.parse(filteredExercises);

    filteredExercises.forEach(ex => {
        let option = document.createElement('option');
        option.innerText = ex.exerciseName;
        option.value = ex.exerciseId;
        if (JSON.stringify(selectedExercises) != JSON.stringify([])) {
            if (selectedExercises.indexOf(option.value) != -1) {
                option.selected = true;
            }     
        }
        dropdown.appendChild(option);
    })

    container.appendChild(dropdown);

    let dropdownSpan = document.createElement('span');
    dropdownSpan.classList.add('text-danger');
    dropdownSpan.id = `exerciseSpan_${i}`
    container.appendChild(dropdownSpan);
    dropdown.onchange = e => {
        dropdownSpan.innerText = ""
    }
    div.insertBefore(container, deleteBtn);
}

let ClearSelect = (i) => {
    var div = document.getElementById(`muscleGroupSelector_${i}`);
    var dropdown = document.getElementById(`exercisesDropdown_${i}`);
    if (dropdown != null) {
        div.removeChild(dropdown)
    }
}

let CreateWorkout = (list, i, url) => {
    let parent = document.getElementById('workout');

    let div = document.createElement('div');
    let btn = document.getElementById('addWorkoutBtn')
    div.style.marginTop = '10px';
    div.classList.add('form-group');
    div.classList.add('card');
    div.classList.add('card-body');
    div.style.backgroundColor = "#d4f0a5"
    div.style.color = "#393E46"
    div.id = `muscleGroupSelector_${i}`
    parent.insertBefore(div, btn);

    let isDeleted = document.createElement('input')
    isDeleted.id = `isDeleted_${i}`
    isDeleted.style.display = 'none'
    isDeleted.name = `Workouts[${i}].IsDeleted`
    div.appendChild(isDeleted)

    let nameLabel = document.createElement('label')
    nameLabel.name = "WorkoutNames"
    nameLabel.classList.add("control-label")
    nameLabel.innerText = "Workout name"

    let nameInput = document.createElement('input')
    nameInput.name = `Workouts[${i}].WorkoutName`
    nameInput.id = `workoutName_${i}`;
    nameInput.classList.add("form-control")
    nameInput.onchange = e => {
        nameSpan.innerText = "";
    }

    let nameSpan = document.createElement('span')
    nameSpan.name = "WorkoutNames"
    nameSpan.id = `nameSpan_${i}`
    nameSpan.classList.add("text-danger")
    div.appendChild(nameLabel);
    div.appendChild(nameInput);
    div.appendChild(nameSpan);


    let label = document.createElement('label');
    label.innerText = "Muscle groups"
    label.classList.add('control-label')
    label.classList.add('select-label')
    label.htmlFor = 'mgSelect'
    div.appendChild(label)

    let select = document.createElement('select')
    select.classList.add("js-example-basic-multiple")
    select.classList.add("form-control")
    select.id = `mgSelect_${i}`
    select.name = `Workouts[${i}].SelectedMuscleGroups`
    select.multiple = "multiple";
    for (let muscle of list) {
        let option = document.createElement('option');
        option.value = muscle.value
        option.innerText = muscle.text
        select.append(option);
    }
    div.append(select);

    let muscleGroupsSpan = document.createElement('span');
    muscleGroupsSpan.classList.add('text-danger');
    muscleGroupsSpan.id = `mgSpan_${i}`
    div.append(muscleGroupsSpan);

    $(`#mgSelect_${i}`).select2()

    select.onchange = e => {
        muscleGroupsSpan.innerText = ""
        let queryString = "?";
        let indexOfMuscles = 0;

        for(let elem of $(`#mgSelect_${i}`).select2('data')){
            queryString = `${queryString}[${indexOfMuscles}]=${elem.id}&`
            indexOfMuscles++;
        }

        let filteredExercises = [];

        $.ajax({
            type: "get",
            url: `${url}${queryString}`,
            success: (resp) => {
                filteredExercises = JSON.stringify(resp);
                ClearSelect(i);
                debugger;
                buildDropdown(filteredExercises, i);
                $(`#exerciseList_${i}`).select2()
            },
            error: (err) => {
                console.log(err);
            }
        })
    }

    let deleteBtn = document.createElement('button');
    deleteBtn.type = 'button'
    deleteBtn.classList.add('btn');
    deleteBtn.classList.add('btn-danger');
    deleteBtn.innerText = "Delete workout";
    deleteBtn.style.marginTop = "10px";
    deleteBtn.id = `deleteBtn_${i}`
    div.appendChild(deleteBtn)
    deleteBtn.onclick = e => {
        div.style.display = 'none'
        isDeleted.value = true
    }
}


let FormIsValid = () => {
    let ok = true;
    for (let i in $("#workout")[0].children) {
        let el = $("#workout")[0].children[i]
        if (el.nodeType == 1 && el.id.indexOf("muscleGroupSelector") >= 0 && el.children[0].value != "true") {
            let index = parseInt(i) - 1;
            if($(`#workoutName_${index}`).val() == ""){
                let span = document.getElementById(`nameSpan_${index}`)
                span.innerText = "Required field";
                ok = false;
            }
            if($(`#mgSelect_${index}`).select2('data').length == 0){
                let span = document.getElementById(`mgSpan_${index}`)
                span.innerText = "Please select at least one muscle group";
                ok = false;
            }
            else{
                if($(`#exerciseList_${index}`).select2('data').length == 0){
                    let span = document.getElementById(`exerciseSpan_${index}`)
                    span.innerText = "Please select at least one exercise";
                    ok = false;
                }
            }
        }
    }
    
    return ok;
}