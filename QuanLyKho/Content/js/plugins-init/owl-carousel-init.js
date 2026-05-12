$('.owl-carousel').owlCarousel({
    loop:true,
    margin:15,
    responsiveClass:true,
    navText : ["<i class='ti-shift-left-alt'></i>","<i class='ti-shift-right-alt'></i>"],
    dots:false,
    autoWidth:true,
    responsive:{
        0:{
            items:1,
            nav:true
        },
        600:{
            items:2,
            nav:false
        },
        1000:{
            items:5,
            nav:true,
            loop:false
        }
    }
})



