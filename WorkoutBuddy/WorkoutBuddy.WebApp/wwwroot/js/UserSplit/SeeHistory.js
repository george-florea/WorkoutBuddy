let WorkoutHistory = () => {
  let infoBtn = document.getElementById("info");
  let infoContainer = document.getElementById("infoContainer");
  let chartBtn = document.getElementById("chart");
  let chartContainer = document.getElementById("chartContainer");

  infoBtn.onclick = (e) => {
    chartContainer.style.display = "none";
    infoContainer.style.display = "block";
    chartBtn.classList.remove("tabBtnActive");
    chartBtn.classList.add("tabBtn");
    infoBtn.classList.remove("tabBtn");
    infoBtn.classList.add("tabBtnActive");
  };
  chartBtn.onclick = (e) => {
    infoContainer.style.display = "none";
    chartContainer.style.display = "block";
    chartBtn.classList.add("tabBtnActive");
    chartBtn.classList.remove("tabBtn");
    infoBtn.classList.add("tabBtn");
    infoBtn.classList.remove("tabBtnActive");
  };
};

let ClearDiv = () => {
  let parent = document.getElementById("history-body");
  parent.innerHTML = "";
};

let CreateHistory = (resp) => {
  let parent = document.getElementById("history-body");
  for (let ex of resp.exercises) {
    let container = document.createElement("div");
    container.classList.add("card")
    container.classList.add("my-3")
    container.classList.add("d-flex")
    container.classList.add("flex-row")
    container.style.height = "100%";
    container.style.width =  "45rem";
    container.style.borderRadius = "1.5rem";
    parent.append(container);
    
    let nameContainer = document.createElement("div");
    nameContainer.classList.add("col-4")
    nameContainer.classList.add("d-flex")
    nameContainer.classList.add("align-items-center")
    nameContainer.classList.add("justify-content-center")
    nameContainer.classList.add("text-center")
    nameContainer.style.backgroundColor = "#393E46";
    nameContainer.style.color = "#efefef";
    nameContainer.style.borderRadius = "1.5rem 0 0 1.5rem";
    container.append(nameContainer);

    let name = document.createElement("h3");
    name.classList.add("card-title");
    name.innerText = `${ex.exerciseName} `;
    nameContainer.append(name);

    if (ex.isPr)
    {
      let icon = document.createElement('i');
      icon.classList.add("fa-solid")
      icon.classList.add("fa-trophy")
      icon.classList.add("text-warning")
      name.append(icon);
    }
    
    
    let setsContainer = document.createElement("div");
    setsContainer.classList.add("col-8")
    setsContainer.classList.add("d-flex")
    setsContainer.classList.add("flex-column")
    setsContainer.classList.add("justify-content-center")
    setsContainer.classList.add("p-3")
    setsContainer.style.backgroundColor = '#d4f0a5';
    setsContainer.style.borderRadius = "0 1.5rem 1.5rem 0";
    container.append(setsContainer);

    for (let set of ex.sets) {
      if (set.distance != null) {
        let p = document.createElement("p");
        p.innerText = ` ${set.distance} m x ${set.duration} mins`;
        p.classList.add("listItem")
        setsContainer.appendChild(p);
      }
      if (set.reps != null) {
        let p = document.createElement("p");
        p.innerText = ` ${set.reps} reps x ${set.weight} kgs`;
        p.classList.add("listItem")
        setsContainer.appendChild(p);
      }
    }
    
    
  }
};

let ClearDates = () => {
  let parent = document.getElementById("dateNav");
  for (let i in parent.children) {
    console.log(parent.children[i].nodeType);
    if (
      parent.children["1"].value != "" &&
      parent.children["1"].nodeType == 1
    ) {
      parent.removeChild(parent.children["1"]);
    }
  }
};

let CreateDates = (resp, index, datesNo, id) => {
  let parent = document.getElementById("dateNav");

  for (let i in resp) {
    let dates = new Date(resp[i]);
    let dateFormat = `${dates.getDate()}/${dates.getMonth()}/${dates.getFullYear()}`;

    let nextBtn = document.getElementById("next");
    let button = document.createElement("button");
    button.type = "button";
    button.id = `date_${index * datesNo + parseInt(i)}`;
    button.classList.add("dateBtn");
    button.value = `${resp[i]}`;
    button.innerText = dateFormat;
    parent.insertBefore(button, nextBtn);

    button.onclick = (e) => {
      for (let i = 1; i < parent.children.length - 1; i++) {
        if (parent.children[i].nodeType == 1) {
          parent.children[i].classList.remove("dateBtnActive");
          parent.children[i].classList.add("dateBtn");
        }
      }

      e.currentTarget.classList.remove("dateBtn");
      e.currentTarget.classList.add("dateBtnActive");

      let queryString = `?WorkoutId=${id}&Date=${e.currentTarget.value}`;
      $.ajax({
        type: "get",
        url: `/UserSplit/GetHistory${queryString}`,
        success: (resp) => {
          ClearDiv();
          CreateHistory(resp);
        },
        error: (err) => {
          console.log(err);
        },
      });
    };
  }
};

let ChartBuilder = (index, dates, coefList, needsColoring = true) => {
  const labels = [];
  for (let i in dates) {
    let date = new Date(dates[i]);
    let dateFormat = `${date.getDate()}/${date.getMonth()}/${date.getFullYear()}`;
    labels.push(dateFormat);
  }

  const data = {
    labels: labels,
    datasets: [
      {
        label: "Your workouts progress",
        backgroundColor: "rgb(255, 99, 132)",
        borderColor: "rgb(255, 99, 132)",
        data: coefList,
      },
    ],
  };

  let config = {}
  if(needsColoring){
    config = {
      type: "line",
      data: data,
      options: {
          legend: {
              labels: {
                  color: "#d4f0a5",
              }
          },
          scales: {
              y: {
                  ticks: {
                      color: "#d4f0a5",
                  }
              },
              x: {
                  ticks: {
                      color:"#d4f0a5",
                  }
              }
          }
        },
    };
  }
  else{
    config = {
      type: "line",
      data: data,
      options: {},
    };
  }
  
  const myChart = new Chart(
    document.getElementById(`myChart_${index}`),
    config
  );
};
