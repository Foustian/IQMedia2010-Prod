// JavaScript Document
$(document).ready(function() {
	initresize();
});
$(window).resize(function() {
     resize();
});
function initresize(){
	
	imgH = 1280;
	winW = $(window).width();
	winH = $(window).height();
	
	if(imgH > winH){
		var topM = (imgH - winH)*1.7/3;
		var logM = winH * 5/100;
		var powM = (winW - 263)/2;
	}
	
	$('#bg').css('top',-topM+'px');
	$('#logo').css('margin-bottom',logM+'px');
	$('.poweredby').css('margin-left',powM+'px');
}
function resize(){
	
	imgH = $('#bg img').height();
	winW = $(window).width();
	winH = $(window).height();
	
	if(imgH > winH){
		var topM = (imgH - winH)*1.7/3;
		var logM = winH * 5/100;
		var powM = (winW - 263)/2;
	}
	
	$('#bg').css('top',-topM+'px');
	$('#logo').css('margin-bottom',logM+'px');
	$('.poweredby').css('margin-left',powM+'px');
}