function window_onload() {

    if (typeof (eDescripcion) != "undefined") {
        if (eDescripcion != "") {
            window.showModalDialog('errormsgbox.htm', window, 'dialogHeight:160px;dialogWidth:500px;status:0;resizable:yes;scroll:0;help:0;unadorned:0');
            eDescripcion = "";
        }
    }

    if (typeof (statusBarText) != "undefined") {
        try { parent.frames("sidebar").statusText = statusBarText; } catch (e) { }
        try { parent.frames("header").userControl = userControl; } catch (e) { }
        try { parent.frames("header").fun = fun; } catch (e) { }
        try { parent.frames("header").modo = modo; } catch (e) { }
    }

} //function window_onload(){

function ValidarFecha(val) {
    var value = ValidatorGetValue(val.controltovalidate);
    var rv = val.RV.toUpperCase();
    var tf = val.TipoFecha.toUpperCase();
    if (value.length == 0 && rv.toUpperCase() == "FALSE") return true;
    var valor = FormatoFecha(value, tf);

    if (valor != false) {
        control = document.all[val.controltovalidate];
        control.value = valor;
        return true;
    }
    else return false;
} //function ValidarFecha(val){

function FormatoFecha(value, tf) {
    var cGuion = 0;
    var cSlash = 0;
    var cDptos = 0;
    var cSpace = 0;
    var sSep = "";
    var sConstSep = "/";

    switch (tf) {
        case "DDMMAAAA": //DDMMAAAA
            value = DelChar(value, " ");

            for (i = 0; i < value.length; i++) {
                var inner = value.substr(i, 1);
                if (inner == "-") cGuion++;
                if (inner == "/") cSlash++;
            }

            if (cSlash > 0 && cGuion > 0 || cSlash == 0 && cGuion == 0) return false;

            if (cSlash > 0) sSep = "/";
            if (cGuion > 0) sSep = "-";

            vector = value.split(sSep);

            if (parseInt(vector[0]) < 10) {
                vector[0] = "0" + Number(vector[0]);
            }
            if (parseInt(vector[1]) >= 0 && parseInt(vector[1]) < 10) {
                vector[1] = "0" + Number(vector[1]);
            }

            vector[2] = "" + Number(vector[2]);
            if (parseInt(vector[2]) > 79 && parseInt(vector[2]) < 100) {
                vector[2] = "19" + Number(vector[2]);
            }
            if (parseInt(vector[2]) > 0 && parseInt(vector[2]) < 80) {
                s = "";
                if (parseInt(vector[2]) < 10) s = "0";
                vector[2] = "20" + s + Number(vector[2]);
            }

            var yy = Number(vector[2]);
            var mm = Number(vector[1]);
            var dd = Number(vector[0]);
            mm--;
            var date = new Date(yy, mm, dd);
            var res = (typeof (date) == "object" && yy == date.getFullYear() && mm == date.getMonth() && dd == date.getDate()) ? true : false;

            if (res) var r = vector[0] + sConstSep + vector[1] + sConstSep + vector[2];
            else var r = false;
            return r;
            break;

        case "DDMMAAAAHHMMSS": //DDMMAAAAHHMMSS
            value = trim(value, " ");
            for (i = 0; i < value.length; i++) {
                var inner = value.substr(i, 1);
                if (inner == "-") cGuion++;
                if (inner == "/") cSlash++;
                if (inner == ":") cDptos++;
                if (inner == " ") cSpace++;
            }
            if (cSlash > 0 && cGuion > 0 || cSlash == 0 && cGuion == 0) return false;
            if (cDptos > 2) return false;
            if (cSpace > 1) return false;

            if (cSlash > 0) sSep = "/";
            if (cGuion > 0) sSep = "-";

            var tmpVec = value.split(" ");
            var fecha = tmpVec[0];
            var hora = tmpVec[1];
            if (hora == null) hora = "0:0:0"
            var vectorhor = hora.split(":");
            //hora
            i = vectorhor.length;
            if (i == 1) {
                ho = vectorhor[0];
                mi = 0;
                se = 0;
            }
            if (i == 2) {
                ho = vectorhor[0];
                mi = vectorhor[1];
                se = 0;
            }
            if (i == 3) {
                ho = vectorhor[0];
                mi = vectorhor[1];
                se = vectorhor[2];
            }
            if (isNaN(ho)) return false;
            if (isNaN(mi)) return false;
            if (isNaN(se)) return false;

            if (Number(ho) < 0 || Number(ho) > 23) return false;
            if (Number(mi) < 0 || Number(mi) > 59) return false;
            if (Number(se) < 0 || Number(se) > 59) return false;
            ho = parseInt(ho) < 10 ? "0" + Number(ho) : ho;
            mi = parseInt(mi) < 10 ? "0" + Number(mi) : mi;
            se = parseInt(se) < 10 ? "0" + Number(se) : se;

            //fecha
            var vectorfec = fecha.split(sSep);

            if (parseInt(vectorfec[0]) < 10) {
                vectorfec[0] = "0" + Number(vectorfec[0]);
            }
            if (parseInt(vectorfec[1]) >= 0 && parseInt(vectorfec[1]) < 10) {
                vectorfec[1] = "0" + Number(vectorfec[1]);
            }
            vectorfec[2] = "" + Number(vectorfec[2]);
            if (parseInt(vectorfec[2]) > 79 && parseInt(vectorfec[2]) < 100) {
                vectorfec[2] = "19" + Number(vectorfec[2]);
            }
            if (parseInt(vectorfec[2]) > 0 && parseInt(vectorfec[2]) < 80) {
                s = "";
                if (parseInt(vectorfec[2]) < 10) s = "0";
                vectorfec[2] = "20" + s + Number(vectorfec[2]);
            }

            var yy = Number(vectorfec[2]);
            var mm = Number(vectorfec[1]);
            var dd = Number(vectorfec[0]);
            mm--;
            tmphor = Number(ho);
            tmpmin = Number(mi);
            tmpseg = Number(se);
            var date = new Date(yy, mm, dd);
            var res = (typeof (date) == "object" && yy == date.getFullYear() && mm == date.getMonth() && dd == date.getDate()) ? true : false;

            if (res) var r = vectorfec[0] + sConstSep + vectorfec[1] + sConstSep + vectorfec[2] + " " + ho + ":" + mi + ":" + se;
            else var r = false;
            return r;
            break;

        case "MMAAAA": //MMAAAA
            value = DelChar(value, " ");
            for (i = 0; i < value.length; i++) {
                var inner = value.substr(i, 1);
                if (inner == "-") cGuion++;
                if (inner == "/") cSlash++;
            }
            if (cSlash > 0 && cGuion > 0 || cSlash == 0 && cGuion == 0) return false;

            if (cSlash > 0) sSep = "/";
            if (cGuion > 0) sSep = "-";

            vector = value.split(sSep);
            i = vector.length;
            if (i != 2) return false;

            if (parseInt(vector[0]) >= 0 && parseInt(vector[0]) < 10) {
                vector[0] = "0" + Number(vector[0]);
            }

            vector[1] = "" + Number(vector[1]);
            if (parseInt(vector[1]) > 79 && parseInt(vector[1]) < 100) {
                vector[1] = "19" + Number(vector[1]);
            }
            if (parseInt(vector[1]) > 0 && parseInt(vector[1]) < 80) {
                s = "";
                if (parseInt(vector[1]) < 10) s = "0";
                vector[1] = "20" + s + Number(vector[1]);
            }


            var yy = Number(vector[1]);
            var mm = Number(vector[0]);
            var dd = 01;
            mm--;
            var date = new Date(yy, mm, dd);
            var res = (typeof (date) == "object" && yy == date.getFullYear() && mm == date.getMonth() && dd == date.getDate()) ? true : false;

            if (res) var r = vector[0] + sConstSep + vector[1];
            else var r = false;
            return r;
            break;
    }
    return false;
} //function FormatoFecha(value,tf){
function fmtfec(control, tf) {
    if (control.value.length == 0) return;
    var valor = FormatoFecha(control.value, tf.toUpperCase());
    if (valor != false) control.value = valor;
}

function ValidarRut(val) {
    var value = ValidatorGetValue(val.controltovalidate);
    value = FormatoRut(value); //FormatoRut
    var valorFinal = value;
    var rv = val.RV.toUpperCase();
    if (value.length == 0 && rv.toUpperCase() == "FALSE") return true;
    var tmpvec = value.split("-");
    if (tmpvec.length < 2) return false;
    var rut = DelChar(tmpvec[0], ".");
    var drut = tmpvec[1];
    var douSuma = 0.0000;
    var x = 0;
    var intMultiplo = 0;

    if (rut.length < 1 || drut.length < 1) return false;

    intMultiplo = 2;
    for (x = 0; x < rut.length; x++) {
        douSuma = douSuma + (Number(rut.substr(rut.length - 1 - x, 1)) * intMultiplo);
        intMultiplo++;
        if (intMultiplo == 8) intMultiplo = 2;
    }
    var res = 11 - (douSuma % 11);

    switch (res) {
        case 11:
            var dc = "0";
            break;
        case 10:
            var dc = "K";
            break;
        default:
            var dc = res.toString();
    }
    if (dc.toUpperCase() == drut.toUpperCase()) {
        control = document.all[val.controltovalidate];
        control.value = valorFinal;
        return true;
    }
    else {
        control = document.all[val.controltovalidate];
        control.value = valorFinal;
        return false;
    }
} //function ValidarRut(val){


function ValidaUserName(ctrl) {
    var login = document.getElementById(ctrl.id);
    if (login.value != '') {
        if (ValidarRutAtento(login.value)) {
            login.value = FormatoRut(login.value);
        }

    }
}

function PreparaRut(ctrl) {
    var login = document.getElementById(ctrl.id);
    if (ValidarRutAtento(login.value)) {
        var str = new String(login.value);
        if (str != '') {
            var rexGuion = new RegExp('-', 'gi');
            var tmp = str.split(rexGuion);
            login.value = DelChar(tmp[0], ".");
            return true;
        }

    }
    else {
        alert('El RUT es incorrecto');
        return false;
    }
}

function ValidarRutAtento(value) {
    value = FormatoRut(value); //FormatoRut
    var valorFinal = value;
    var tmpvec = value.split("-");
    if (tmpvec.length < 2) return false;
    var rut = DelChar(tmpvec[0], ".");
    var drut = tmpvec[1];
    var douSuma = 0.0000;
    var x = 0;
    var intMultiplo = 0;

    if (rut.length < 1 || drut.length < 1) return false;

    intMultiplo = 2;
    for (x = 0; x < rut.length; x++) {
        douSuma = douSuma + (Number(rut.substr(rut.length - 1 - x, 1)) * intMultiplo);
        intMultiplo++;
        if (intMultiplo == 8) intMultiplo = 2;
    }
    var res = 11 - (douSuma % 11);

    switch (res) {
        case 11:
            var dc = "0";
            break;
        case 10:
            var dc = "K";
            break;
        default:
            var dc = res.toString();
    }
    if (dc.toUpperCase() == drut.toUpperCase()) {
        return true;
    }
    else {
        return false;
    }
}

function FormatoRut(value) {
    if (value.length < 2) return value;
    value = DelChar(value, " ");
    if (value.indexOf("-") != -1) {
        if (value.indexOf("-") < value.length - 1) {
            value = value.substr(0, value.indexOf("-") + 2);
        }
        else {
            if (value.indexOf("-") == value.length - 1) {
                value = value.substr(0, value.indexOf("-"));
            }
        }
    }
    value = DelChar(value, ".");
    value = DelChar(value, "-");
    for (i = 0; i < value.length - 1; i++) {
        var a = value.substr(i, 1);
        if ("0123456789".indexOf(a) == -1) return value;
    }
    var a = value.substr(value.length - 1, 1);
    if ("0123456789Kk".indexOf(a) == -1) return value;
    for (i = 0; value.substr(i, 1) == "0"; i = i * 1) value = value.substr(1, value.length - 1);
    if (value.length - 1 > 3) {
        var tmpnum = (value.length - 1) % 3;
        var desfaz = 0;
        var newvalue = "";
        if (tmpnum != 0) {
            for (i = 0; i < tmpnum; i++) newvalue += value.substr(i, 1);
            newvalue += ".";
            desfaz = i;
        }
        var cnt = 0;
        for (i = desfaz; i < value.length - 1; i++) {
            newvalue += value.substr(i, 1);
            cnt++;
            if (cnt % 3 == 0 && value.length - cnt > 3) newvalue += ".";
        }
    }
    else {
        newvalue = value.substr(0, value.length - 1);
    }
    newvalue += "-";
    newvalue += value.substr(value.length - 1, 1);
    return newvalue;
} //function FormatoRut(value){
function desfmtrut(control) {
    var value = control.value;
    if (value.indexOf(".") == -1) return; // && value.indexOf("-")==-1)
    value = DelChar(value, "."); //value=DelChar(value,"-");
    control.value = value;
    var rangeRef = control.createTextRange();
    rangeRef.move("character", value.length);
    rangeRef.select();
} //function desfmtrut(control){

function fmtrut(control) {
    if (control.value.length == 0) return;
    control.value = FormatoRut(control.value);
} //function fmtrut(control){

function DelChar(value, car) {
    if (value.length == 0) return value;
    while (value.indexOf(car) > -1) value = value.indexOf(car) == 0 ? value.substr(1) : (value.indexOf(car) == value.length - 1 ? value.substr(0, value.length - 1) : value.substr(0, value.indexOf(car)) + value.substr(value.indexOf(car) + 1));
    return value;
} //function DelChar(value,car){
















function trim(value, car) {
    if (value.length == 0) return value;
    while (value.substr(0, 1) == car) value = value.substr(1);
    while (value.substr(value.length - 1, 1) == car) value = value.substr(0, value.length - 1);
    return value;





} //function trim(value,car){


var DefaultButtonColor;

function disbut(button) {

    if (!window.document.body.submitted) {
        window.document.body.submitted = true;

        var i;
        var a;
        a = window.document.body.getElementsByTagName("INPUT");
        for (i = 0; i < a.length; i++) {
            if (a[i].type) {
                if (a[i].type.toLowerCase() == "submit") {
                    DefaultButtonColor = a[i].style.color;
                    a[i].style.color = "Gray";
                }
            }
        }
        return true;
    }
    else {
        return false;
    }

}

function undisbut(button) {

    window.document.body.submitted = false;

    var i;
    var a;
    a = window.document.body.getElementsByTagName("INPUT");
    for (i = 0; i < a.length; i++) {
        if (a[i].type) {
            if (a[i].type.toLowerCase() == "submit") {
                a[i].style.color = DefaultButtonColor;
            }
        }
    }

}


function rutValidar(val) {//para RutSnd
    var valor = ValidatorGetValue(val.controltovalidate);
    valor = DelChar(valor, ".");
    valor = DelChar(valor, "-");
    if (valor.length < 2) {
        alert("Ingrese Rut Valido"); //0 "Caracteres Invalidos para Fecha."
        return false;
    }
    var s = "0123456789kK";
    for (i = 0; i < valor.length; i++) {
        if (s.indexOf(valor.substr(i, 1)) == -1) {
            alert("Caracteres Invalidos para Rut");
            return false;
        }
    }
    var rut = valor.substr(0, valor.length - 1);
    var drut = valor.substr(valor.length - 1);
    var douSuma = 0.0000;
    var x = 0;
    var intMultiplo = 0;
    intMultiplo = 2;
    for (x = 0; x < rut.length; x++) {
        douSuma = douSuma + (Number(rut.substr(rut.length - 1 - x, 1)) * intMultiplo);
        intMultiplo++;
        if (intMultiplo == 8) intMultiplo = 2;
    }
    var res = 11 - (douSuma % 11);
    switch (res) {
        case 11:
            var dc = "0";
            break;
        case 10:
            var dc = "K";
            break;
        default:
            var dc = res.toString();
    }
    if (dc.toUpperCase() == drut.toUpperCase()) {
        control = document.all[val.controltovalidate];
        control.value = addmilsep(rut, ".", 3) + "-" + drut.toUpperCase();
        return true;
    }
    else {
        alert("Rut invalido");
        return false;
    }
} //function rutValidar(val){


function FechaValidar(val) {
    var valor = ValidatorGetValue(val.controltovalidate);
    var formato = val.formato.toUpperCase();
    var valor = trim(valor, " ");
    var sep = "/";
    if (valor.length == 0) return true;
    var s = "0123456789: " + sep;
    for (i = 0; i < valor.length; i++) {
        if (s.indexOf(valor.substr(i, 1)) == -1) {
            alert("Caracteres Invalidos para Fecha."); //0 "Caracteres Invalidos para Fecha."
            return false;
        }
    }
    if (valor.indexOf(sep) == -1) {
        alert("No Posee Separador de Fecha Valido."); //1 "No Posee Separador de Fecha Valido."
        return false;
    }

    formato = formato.toUpperCase();

    if (formato == "MMAAAA") formato = "MM/AAAA";
    if (formato == "DDMMAAAA") formato = "DD/MM/AAAA";
    if (formato == "DDMMAAAAHHMMSS") formato = "DD/MM/AAAA/hh:mm:ss";

    var f = valor.split(sep);
    if (typeof (f[0]) != "undefined") f[0] = trim(f[0], " ");
    if (typeof (f[1]) != "undefined") f[1] = trim(f[1], " ");
    if (typeof (f[2]) != "undefined") f[2] = trim(f[2], " ");

    var ff = formato.split(sep);
    var cntfmt = ff.length;

    switch (cntfmt) {
        case 2:
            var dd = 1;
            var mm = Number(f[0]);
            var yy = Number(f[1]);
            if (dd < 1 || mm < 1 || yy < 1) {
                alert("Ingreso Invalido para Fecha.");
                return false;
            }
            break;
        case 3:
            var dd = Number(f[0]);
            var mm = Number(f[1]);
            var yy = Number(f[2]);
            if (dd < 1 || mm < 1 || yy < 1) {
                alert("Ingreso Invalido para Fecha.");
                return false;
            }
            break;
        case 4:
            var dd = Number(f[0]);
            var mm = Number(f[1]);
            var yy = Number(f[2]);

            if (isNaN(f[0]) || isNaN(f[1])) {//no numericos
                alert("Ingreso Invalido para Fecha.");
                return false;
            }

            if (isNaN(yy)) {//no es numerico
                if (typeof (f[2]) == 'string') {
                    if (f[2].indexOf(" ") > -1) {
                        if (f[2].indexOf(" ") != f[2].lastIndexOf(" ")) {
                            var tmpstr = f[2].substr(0, f[2].indexOf(" ") + 1);
                            f[2] = f[2].substr(f[2].indexOf(" ") + 1);
                            var i = 0;
                            for (i = 0; i < f[2].length; i++) if (f[2].substr(i, 1) != " ") tmpstr += f[2].substr(i, 1);
                            var s = tmpstr.split(" ");
                        }
                        else
                            var s = f[2].split(" ");
                    }
                    else {
                        alert("Ingreso Invalido para Fecha.");
                        return false;
                    }
                    if (!isNaN(s[0])) {
                        yy = Number(s[0]);
                        if (!isNaN(s[1]))//es numerico
                            f[3] = Number(s[1]);
                        else
                            f[3] = s[1];
                    }
                    else {
                        alert("Ingreso Invalido para Fecha.");
                        return false;
                    }

                }
                else {
                    alert("Ingreso Invalido para Fecha.");
                    return false;
                }
            }

            var hrs = f[3];
            if (typeof (f[3]) != "undefined") {
                if (!isNaN(hrs)) {//es numerico
                    hrs = "" + hrs + "";
                    if (hrs.length == 1) {
                        hrs = "0" + hrs + ":00:00";
                    }
                    else {
                        if (Number(hrs.substr(0, 1)) > 2) hrs = hrs + "0";
                        var a = hrs + "00000";
                        a = a.substr(0, 6);
                        hrs = "";
                        var j = 0;
                        for (j = 0; j < 3; j++) {
                            if (hrs != "") hrs = hrs + ":"
                            hrs = hrs + a.substr(0, 2);
                            a = a.substr(2);
                        }
                    }
                }
                if (hrs.indexOf(":") > -1)
                    var h = hrs.split(":");
                else {
                    if (!isNaN(f[3])) {//es numerico
                        var a = Number(f[3]);
                        if (a < 0 || a > 23) var s = a + ":0:0";
                        else var s = "0:0:0";
                    }
                    else {
                        var s = "0:0:0";
                    }
                    var h = hrs.split(":");
                }
                switch (h.length) {
                    case 1:
                        h[1] = "00";
                        h[2] = "00";
                        var ss = h[0] + ":" + h[1] + h[2]
                        break;
                    case 2:
                        h[2] = "00";
                        var ss = h[0] + ":" + h[1] + h[2]
                        break;
                    case 3:
                        var ss = h[0] + ":" + h[1] + h[2]
                        break;
                    default:
                        var ss = h[0] + ":" + h[1] + h[2]
                }
                var hh = Number(h[0]);
                var mi = Number(h[1]);
                var ss = Number(h[2]);
                if ((hh < 0 || hh > 23) || (mi < 0 || mi > 59) || (ss < 0 || ss > 59)) {
                    alert("Ingreso Invalido para hora.");
                    return false;
                }
            }
            else {
                var hh = "00";
                var mi = "00";
                var ss = "00";
            }
            break;
        default:
            alert("Ingreso Invalido para Fecha.");
            return false;
    }

    mm--;
    if (Number(yy) < 10) yy = "0" + Number(yy);
    if (Number(yy) > 79 && yy < 100) yy = "19" + yy;
    if (Number(yy) < 80) yy = "20" + yy;

    var date = new Date(yy, mm, dd);
    var r = (typeof (date) == "object" && yy == date.getFullYear() && mm == date.getMonth() && dd == date.getDate()) ? true : false;
    if (!r) {
        alert("Fecha no Valida."); //4 "Fecha no Valida."
        return false;
    }
    mm++;

    var d = (Number(dd) < 10) ? "0" + Number(dd) : Number(dd);
    var m = (Number(mm) < 10) ? "0" + Number(mm) : Number(mm);
    var a = yy;
    switch (cntfmt) {
        case 2:
            control = document.all[val.controltovalidate];
            control.value = m + sep + a;
            break;
        case 3:
            control = document.all[val.controltovalidate];
            control.value = d + sep + m + sep + a;
            break;
        case 4:
            var hhh = (Number(hh) < 10) ? "0" + Number(hh) : Number(hh);
            var min = (Number(mi) < 10) ? "0" + Number(mi) : Number(mi);
            var seg = (Number(ss) < 10) ? "0" + Number(ss) : Number(ss);
            control = document.all[val.controltovalidate];
            control.value = d + sep + m + sep + a + " " + hhh + ":" + min + ":" + seg;
            break;
    }
    return true;
} //function FechaValidar(ctrl,formato){

function kptxt(ctrl, evt, tc) {
    var charCode = (window.Event) ? evt.which : evt.keyCode;
    charCode = (tc == 'M') ? (charCode >= 97 && charCode <= 122) ? charCode = charCode - 32 : charCode : (tc == 'm') ? (charCode >= 65 && charCode <= 90) ? charCode = charCode + 32 : charCode : charCode;
    if (window.Event) evt.which = charCode;
    else evt.keyCode = charCode;
    return true;
} //kptxt(ctrl,evt,tc,efrom)

function kpfec(ctrl, evt, efrom) {
    var valor = ctrl.value;
    var charCode = (window.Event) ? evt.which : evt.keyCode;
    switch (efrom) {
        case "ku":
            if (charCode != 8) return;
            break;
        case "kp":
            if (charCode > 31 && (!(charCode >= 47 && charCode <= 57))) {
                charCode = 0;
                return false;
            }
            break;
    } //switch
    return;
} //function kpfec(ctrl,evt,efrom){

function kpnum(ctrl, evt, milsep, decsep, numdec, efrom) {
    var valor = ctrl.value;
    var charCode = (window.Event) ? evt.which : evt.keyCode;
    switch (efrom) {
        case "ku":
            if (charCode != 8) return;
            var strdec = "";
            var strent = "";
            valor = DelChar(valor, milsep);
            valor = DelChar(valor, decsep);
            if (numdec > 0) {
                while (valor.length < numdec) valor = "0" + valor;
                strdec = valor.substr(valor.length - numdec);
                strent = valor.substr(0, valor.length - numdec);
                strent = addmilsep(strent, milsep, 3);
                strdec = decsep + strdec;
            }
            else {
                strent = addmilsep(valor, milsep, 3);
                //strdec =decsep + replichar(numdec,"0") ;
            }
            ctrl.value = strent + strdec;
            break;
        case "kp":
            if (charCode > 31 && (!(charCode >= 48 && charCode <= 57))) {
                charCode = 0;
                return false;
            }

            ctrl.value = "" + formato(valor, milsep, decsep, numdec);

            break;
    } //switch
    return;
} //function kpnum(ctrl,evt,milsep,decsep,numdec,efrom)


function formato(valor, milsep, decsep, numdec) {
    var strdec = "";
    var strent = "";

    if (valor == "") {
        if (numdec > 0) {
            return "0" + decsep + replichar(numdec - 1, "0");
        }
    }
    else {
        valor = DelChar(valor, milsep);
        valor = DelChar(valor, decsep);
        if (numdec > 0) {
            valor += '0';
            strdec = valor.substr(valor.length - numdec);
            strent = valor.substr(0, valor.length - numdec);
            strent = addmilsep(strent, milsep, 3);
            strdec = decsep + strdec.substr(0, strdec.length - 1);
        }
        else {
            if (valor != "") valor += '0';
            strent = addmilsep(valor, milsep, 3);
            strent = strent.substr(0, strent.length - 1);
            strdec = "";
        }
        return strent + strdec;
    }
    return ""
} //function formato(valor,milsep,decsep,numdec){


function addmilsep(valor, milsep, d) {
    var valorf = "";
    var j = 0;
    if (valor == "" || Number(valor) == 0) return "0";
    valor = "" + Number(valor);
    for (i = valor.length - 1; i >= 0; i--) {
        valorf = valor.substr(i, 1) + valorf;
        j++;
        if (j == d) {
            valorf = milsep + valorf;
            d = 3;
            j = 0;
        }
    }
    if (valorf.indexOf(milsep) == 0) valorf = valorf.substr(1, valorf.length);
    return valorf;
} //function addmilsep(valor,milsep,d){


function replichar(num, car) {
    var i = 0;
    var d = "";
    for (i = 0; i < num; i++) {
        d += car;
    }
    return d;
} //function replichar(num,car){


function esperabusqueda() {
    document.getElementById("bs1").style.visibility = "hidden";
    document.getElementById("bs3").style.visibility = "hidden";
    document.getElementById("bs4").style.visibility = "hidden";
    document.getElementById("bs5").style.visibility = "hidden";
    document.getElementById("cpcuerpo").innerHTML = "<br><br><table id=resultadocab><tr><td valign=bottom><table id=vinetarsl><tr><td width=49><img src=images/cab/registroizq.jpg width=49 height=30></td><td width=175 background=../App_Themes/TemaCentral/images/rsl/titCentro.jpg class=tituloResultado>" + "Procesando...</td><td width=12><img src=images/rsl/titDerecha.jpg width=12 height=30 /></td></tr></table></td></tr></table><table id=rs-nr><tr><td><center><br><br><br><object classid=clsid:D27CDB6E-AE6D-11cf-96B8-444553540000 codebase=http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0 width=100 height=100><param name=movie value=flash/logo-girando.swf><param name=quality value=high><embed src=flash/logo-girando.swf quality=high pluginspage=http://www.macromedia.com/go/getflashplayer type=application/x-shockwave-flash width=100 height=100></embed></object><br/>Espere unos momentos, estamos procesando la informaci&#243;n<br><br><br><br><br><br></td></tr></table>";
}

function checkMail(x) {
    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (filter.test(x)) return true; else return false;
}