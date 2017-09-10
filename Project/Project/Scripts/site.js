function donutChart(id, obj) {

    var board = document.getElementById(id);
    var ctx = board.getContext("2d");
    var colors = ["#DC143C", "#1E90FF", "#3CB371", "#DAA520", "#F079B9", "#808000", "#DA39A3", "#FF7F50", "#808080", "#20B2AA"];
    var i, sum = 0, pointer = 0;
    var xC = 100, yC = 100, r = 70;
    var xR = 240, yR = 15;

    for (i = 0; i < obj.length; i++) sum += obj[i].value;
    if (sum === 0 || obj[0].name === null) {
        return;
    }
    ctx.font = "14px Arial";
    for (i = 0; i < obj.length; i++) {
        ctx.lineWidth = 40;
        ctx.strokeStyle = colors[i];
        ctx.beginPath();
        ctx.arc(xC, yC, r, pointer, pointer + (obj[i].value * 2 * Math.PI / sum));
        ctx.stroke();
        pointer += obj[i].value * 2 * Math.PI / sum;
        if (obj[i].name !== null) {
            ctx.lineWidth = 1;
            ctx.fillStyle = colors[i];
            ctx.fillRect(xR, yR, 10, 10);
            ctx.stroke();
            ctx.fillStyle = "#000000";
            ctx.fillText(Math.round(obj[i].value * 100 / sum) + "%", xR - 40, yR + 10);
            ctx.fillText(obj[i].name, xR + 15, yR + 10);
            yR += 17;
        }
    }
}

function voteSuccess(data) {
    var msg;
    if (data.result) {
        msg = document.getElementById("successText");
        msg.innerHTML = data.msg;
        $('#successAlert').modal('toggle');
    } else {
        msg = document.getElementById("errorText");
        msg.innerHTML = data.msg;
        $('#errorAlert').modal('toggle');
    }
}
function commentSuccess(data) {
    if (data.result) {
        var board = document.getElementById("discussion" + data.publication);
        var discussion = document.createElement("DIV");
        discussion.setAttribute("class", "row container-fluid");
        var header = document.createElement("DIV");
        header.setAttribute("class", "col-sm-2 text-right");
        var hour = document.createElement("SPAN");
        hour.setAttribute("class", "label label-default");
        hour.innerHTML = data.hour;
        header.appendChild(hour);
        header.appendChild(document.createElement("BR"));
        var user = document.createTextNode(data.user);
        header.appendChild(user);
        discussion.appendChild(header);
        var text = document.createElement("DIV");
        text.setAttribute("class", "col-sm-9");
        var textArea = document.getElementById("commentTextArea");
        var content = document.createElement("H6");
        content.innerHTML = textArea.value;
        textArea.value = "";
        text.appendChild(content);
        discussion.appendChild(text);
        board.appendChild(discussion);
    } else {
        var msg = document.getElementById("errorText");
        msg.innerHTML = data.msg;
        $('#errorAlert').modal('toggle');
    }
}
function censorSuccess(data) {
    var msg;
    if (data.result) {
        msg = document.getElementById("successText");
        msg.innerHTML = data.msg;
        $('#successAlert').modal('toggle');

    } else {
        msg = document.getElementById("errorText");
        msg.innerHTML = data.msg;
        $('#errorAlert').modal('toggle');
    }
}

function closeSuccess(data) {
    var msg;
    if (data.result) {
        msg = document.getElementById("successText");
        msg.innerHTML = data.msg;
        $('#successAlert').modal('toggle');

    } else {
        msg = document.getElementById("errorText");
        msg.innerHTML = data.msg;
        $('#errorAlert').modal('toggle');
    }
}

var loadFile = function (event) {
    var output = document.getElementById('imgPreview');
    output.hidden = false;
    output.src = URL.createObjectURL(event.target.files[0]);
};

function addOption() {
    if (optionID < 10) {
        var options = document.getElementById("options");
        var label = document.createElement("LABEL");
        label.setAttribute("for", "OptionName_" + optionID + "_");
        label.setAttribute("class", "col-xs-2");
        label.innerHTML = 'Opção ' + (optionID + 1);
        options.appendChild(label);
        var div = document.createElement("DIV");
        div.setAttribute("class", "col-xs-10");
        var input = document.createElement("INPUT");
        input.setAttribute("type", "text");
        input.setAttribute("id", "OptionName_" + optionID + "_");
        input.setAttribute("name", "OptionName[" + optionID + "]");
        input.setAttribute("class", "form-control optionsToPreview");
        div.appendChild(input);
        options.appendChild(div);
        optionID++;
        if (optionID === 10) {
            var btn = document.getElementById("btnAddOption");
            btn.setAttribute("disabled", true);
        }
    }
}

function logSucess(data) {
    var msg;
    if (data.result) {
        var container = document.getElementById("log" + data.user);
        container.innerHTML = "";
        var text = "";
        for (var i = 0; i < data.logs.length; i++) {
            var parag = document.createElement("p");
            var bold = document.createElement("b");
            bold.innerText = data.logs[i].Hour;
            var label = document.createElement("label");
            label.innerText = " : " + data.logs[i].Description;
            parag.appendChild(bold);
            parag.appendChild(label);
            var div = document.createElement("div");
            div.appendChild(parag);
            container.appendChild(div);
        }
        if (data.hasmore) {
            document.getElementById("right" + data.user).innerHTML = '<a data-ajax="true" data-ajax-method="Get" data-ajax-success="logSucess" href="/Users/Log?userID=' + data.user + '&page=' + (data.page + 1) + '" class="btn btn-default"><span class="glyphicon glyphicon-chevron-right" aria-label="Right Align" aria-hidden="true"></span></a>';
        }
        if (data.page!=0) {
            document.getElementById("left" + data.user).innerHTML = '<a data-ajax="true" data-ajax-method="Get" data-ajax-success="logSucess" href="/Users/Log?userID=' + data.user + '&page=' + (data.page - 1) + '" class="btn btn-default"><span class="glyphicon glyphicon-chevron-left" aria-label="Right Align" aria-hidden="true"></span></a>';
        }
    } else {
        msg = document.getElementById("errorText");
        msg.innerHTML = data.msg;
        $('#errorAlert').modal('toggle');
    }
}
