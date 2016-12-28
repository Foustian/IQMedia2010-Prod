function GetDateStringFromUTCFormat(dt) {

    var d = new Date(dt);
    return (d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + ("0" + d.getDate()).slice(-2) + " " + ("0" + d.getHours()).slice(-2) + ":" + ("0" + d.getMinutes()).slice(-2) + ":" + ("0" + d.getSeconds()).slice(-2));
}