// ------------------------------------------------------------------------------------
// JS Content same as HCSP, HCVU
// ------------------------------------------------------------------------------------

var Clock = {
    SystemDateTime: "",
    SystemTime: "",
    StartClock: function (lblClockID, formatter, formatterTime) {
        var lbl = document.getElementById(lblClockID);
        var str = lbl.innerHTML.split(" ");
        var SystemDate = str[0];
        var SystemTime = str[1];
        var timer;

        var Dates = SystemDate.split("/");
        var Times = SystemTime.split(":");
        if (parseInt(Times[2]) < 10)
        { Times[2] = Times[2].substring(1); }
        if (parseInt(Times[1]) < 10)
        { Times[1] = Times[1].substring(1); }
        if (parseInt(Times[0]) < 10)
        { Times[0] = Times[0].substring(1); }
        if (parseInt(Dates[0]) < 10)
        { Dates[0] = Dates[0].substring(1); }
        if (parseInt(Dates[1]) < 10)
        { Dates[1] = Dates[1].substring(1); }
        var d = new Date(parseInt(Dates[2]), parseInt(Dates[1]) - 1, parseInt(Dates[0]), parseInt(Times[0]), parseInt(Times[1]), parseInt(Times[2]), 0);

        d.setSeconds(d.getSeconds() + 1);
        //alert(d.getSeconds());
        var strFormatter = formatter;
        var strFormatterTime = formatterTime;
        strFormatter = strFormatter.replace("dd", ((d.getDate() < 10) ? "0" : "") + d.getDate());
        strFormatter = strFormatter.replace("MM", ((d.getMonth() + 1 < 10) ? "0" : "") + (d.getMonth() + 1));
        strFormatter = strFormatter.replace("yyyy", d.getFullYear());
        strFormatter = strFormatter.replace("HH", ((d.getHours() < 10) ? "0" : "") + d.getHours());
        strFormatter = strFormatter.replace("mm", ((d.getMinutes() < 10) ? "0" : "") + d.getMinutes());
        strFormatter = strFormatter.replace("ss", ((d.getSeconds() < 10) ? "0" : "") + d.getSeconds());
        
        strFormatterTime = strFormatterTime.replace("HH", ((d.getHours() < 10) ? "0" : "") + d.getHours());
        strFormatterTime = strFormatterTime.replace("mm", ((d.getMinutes() < 10) ? "0" : "") + d.getMinutes());
        strFormatterTime = strFormatterTime.replace("ss", ((d.getSeconds() < 10) ? "0" : "") + d.getSeconds());
        
        this.SystemDateTime = strFormatter;
        this.SystemTime = strFormatterTime;
        lbl.innerHTML = this.SystemDateTime;
        timer = setTimeout("Clock.StartClock('" + lblClockID + "', '" + formatter + "','" + formatterTime + "');", 1000);
    }
};
