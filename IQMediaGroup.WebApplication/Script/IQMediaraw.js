function setSeekPoint(C, B) {
    var Flash = document.getElementById('HUY');
    if (Flash!=null) {
        Flash.setSeekPoint(C);
    }
    if (B) { $(B).addClass("selected"); $("#timeBar").children(":not(.selected)").fadeTo(400, 0.4); $(B).removeClass("selected");$(B).fadeTo(400, 1); } 
}
