(function($) {
    "use strict"
    
    // Colorpicker
    $(".colorpicker").asColorPicker();
    $(".complex-colorpicker").asColorPicker({
        mode: 'palettes'
    });
    $(".gradient-colorpicker").asColorPicker({
        mode: 'palettes'
    });
})(jQuery);