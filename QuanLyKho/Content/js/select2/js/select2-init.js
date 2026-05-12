

(function($) {
    "use strict"

//All

$('.multi-crmnt').select2();

$(".single-select-crmnt").select2();


$(".single-select-search-crmnt").select2({
  minimumResultsForSearch: Infinity,
});

$(".single-select-ticket").select2({
  minimumResultsForSearch: Infinity,
  placeholder: "-- Lựa chọn --"
});


//hợp đồng
$("#single-month-hd").select2({
  minimumResultsForSearch: Infinity,
  placeholder: "-- Kết thúc trong tháng --"
});

//công nợ
$(".single-group-kh").select2({
  placeholder: "-- Chọn nhóm khách hàng --"
});

$(".single-group-pt").select2({
  placeholder: "-- Chọn người phụ trách --"
});

$('.multi-select-crmnt').select2(
  {
    placeholder: "-- Trạng thái --"
  }
);







// single select box form sản phẩm
$('.multi-select-sp').select2();

$("#single-select-sp, #single-select-sp-7").select2({
  minimumResultsForSearch: Infinity,
  placeholder: ""
});

$("#single-select-sp1").select2({
  minimumResultsForSearch: Infinity,
  placeholder: ""
});

$("#single-select-sp2, #single-select-sp5").select2({
  minimumResultsForSearch: Infinity,
  placeholder: ""
});

$("#single-select-sp3, #single-select-sp6").select2({
  minimumResultsForSearch: Infinity
});

$("#single-select-sp4").select2({
  minimumResultsForSearch: Infinity,
  placeholder: ""
});


    // dropdown báo giá
    $('.dropdown-groups-bg').select2(
      {       
        placeholder: "-- Lựa chọn --",
      }
    );

// single select box form Đơn Hàng
$("#single-select-dh").select2({
  minimumResultsForSearch: Infinity,
  placeholder: "-- Trạng thái --"
});

$('.multi-select-dh, .multi-select-dh-1').select2();

$('.dropdown-groups-dh, .dropdown-groups-dh-1').select2(
  {       
    placeholder: "-- Lựa chọn --",
  }
);

$("#single-select-dh-1, #single-select-dh-2").select2({
  minimumResultsForSearch: Infinity,
  placeholder: ""
});


    
    
    // single select box
    $("#single-select, #single-select-search, #single-select-search1").select2();
    $("#single-select1, #single-select5, #single-select6, #single-select7").select2();

    $("#single-select-npt, #single-select-npt1").select2();

    $("#single-select2, #single-select8, #single-select118, #single-select128, #single-select138, #single-select148").select2({
        minimumResultsForSearch: Infinity,
        placeholder: "-- Lựa chọn --"
      });

      $("#single-select-pt, #single-select-pt1, #single-select-pt2, #single-select-pt3, #single-select-pt5, #single-select-pt6").select2({
        minimumResultsForSearch: Infinity,
        placeholder: "-- Lựa chọn --"
      });

      $("#single-select3").select2({
        minimumResultsForSearch: Infinity,
        placeholder: "-- Lựa chọn --"
      });

      $("#single-select4").select2({
        minimumResultsForSearch: Infinity,
        placeholder: "-- Lựa chọn --"
      });

      //select 2 edit form kh

      $("#single-select10").select2({
        minimumResultsForSearch: Infinity,
        placeholder: "-- Lựa chọn --"
      });


      $("#single-job-1").select2({
        minimumResultsForSearch: Infinity,
        placeholder: "-- Lựa chọn --"
      });

      //Form Step khách hàng
      $("#single-step-kh, #single-step-bl, #single-step-cd, #single-step-search-kh, #single-step-qlnd, #single-step-qlnd2, #single-step-qlnd3").select2();
      $('.multi-select-step').select2();

//Form Step chiến dịch email
$("#single-step-kh-1, #single-step-bl-1, #single-step-cd-1, #single-step-search-kh-1").select2();
$('.multi-select-step-1').select2();

//Form Step email
$("#single-step-kh-2, #single-step-bl-2, #single-step-cd-2, #single-step-search-kh-2").select2();
$('.multi-select-step-2').select2();
$("#single-step-kh-3, #single-step-bl-3, #single-step-cd-3, #single-step-search-kh-3").select2();
$('.multi-select-step-3').select2();

//Form chi tiết khách hàng
$("#single-detail-kh, #single-select-nx").select2();
$("#single-kh-list").select2({
        placeholder: "Chọn lịch chăm sóc",
        allowClear: false
    });

    $("#single-kh-list88").select2({
      placeholder: "Lựa chọn nhân viên",
      allowClear: false
  });

  $("#single-kh-list99").select2({
    placeholder: "Chọn trạng thái",
    allowClear: false,
    minimumResultsForSearch: Infinity
});

    $("#single-kh-list1").select2({
      placeholder: "Chọn người phụ trách",
      allowClear: false
  });

  $("#single-kh-list2").select2({
    placeholder: "Chọn lịch chăm sóc",
    allowClear: false
});

$("#single-kh-list3").select2({
  placeholder: "Trạng thái",
  allowClear: false
});

$("#single-kh-list4").select2({
  placeholder: "Chọn người phụ trách",
  allowClear: false
});

$("#single-job-crmnt").select2({
  placeholder: "-- Lựa chọn --",
  allowClear: false,
  minimumResultsForSearch: Infinity
});


//Form Step chiến dịch SMS

$("#single-step-nhan-1, #single-step-nhan-2, #single-step-nhan-3").select2();

$("#single-step-bl-1-sms, #single-step-kh-1-sms, #single-step-cd-1-sms, #single-step-bl-1-sms1, #single-step-kh-1-sms2, #single-step-cd-1-sms3").select2();

      $("#single-select11, #single-select12, #single-select13, #single-select14, #single-select19, #single-select20").select2();

      //form search nâng cao

      $("#single-select81, #single-select51, #single-select61, #single-select71").select2();

      $("#single-chonmau, #single-chonmau1").select2();

   
    // multi select box
    $('.multi-select, .multi-select1, .multi-select-nhan, .single-step-qlnd1').select2();

    // dropdown option groups
    $('.dropdown-groups').select2(
      {       
        placeholder: "-- Lựa chọn --",
      }
    );

    //disabling options
    $('.disabling-options').select2();

    //disabling a select2 control
    $(".js-example-disabled").select2();
    $(".js-example-disabled-multi").select2();
    $("#js-programmatic-enable").on("click", function () {
        $(".js-example-disabled").prop("disabled", false);
        $(".js-example-disabled-multi").prop("disabled", false);
    });
    $("#js-programmatic-disable").on("click", function () {
        $(".js-example-disabled").prop("disabled", true);
        $(".js-example-disabled-multi").prop("disabled", true);
    });


    // select2 with labels
    $(".select2-with-label-single").select2();
    $(".select2-with-label-multiple").select2();


    //select2 container width
    $(".select2-width-50").select2();
    $(".select2-width-75").select2();


    //select2 themes
    $(".js-example-theme-single").select2({
        theme: "classic"
    });
    $(".js-example-theme-multiple").select2({
        theme: "classic"
    });


    //ajax remote data
    $(".js-data-example-ajax").select2({
        width: "100%",
        ajax: {
          url: "https://api.github.com/search/repositories",
          dataType: 'json',
          delay: 250,
          data: function (params) {
            return {
              q: params.term, // search term
              page: params.page
            };
          },
          processResults: function (data, params) {
            // parse the results into the format expected by Select2
            // since we are using custom formatting functions we do not need to
            // alter the remote JSON data, except to indicate that infinite
            // scrolling can be used
            params.page = params.page || 1;
      
            return {
              results: data.items,
              pagination: {
                more: (params.page * 30) < data.total_count
              }
            };
          },
          cache: true
        },
        placeholder: 'Search for a repository',
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        minimumInputLength: 1,
        templateResult: formatRepo,
        templateSelection: formatRepoSelection
      });

      function formatRepo (repo) {
        if (repo.loading) {
          return repo.text;
        }
      
        var markup = "<div class='select2-result-repository clearfix'>" +
          "<div class='select2-result-repository__avatar'><img src='" + repo.owner.avatar_url + "' /></div>" +
          "<div class='select2-result-repository__meta'>" +
            "<div class='select2-result-repository__title'>" + repo.full_name + "</div>";
      
        if (repo.description) {
          markup += "<div class='select2-result-repository__description'>" + repo.description + "</div>";
        }
      
        markup += "<div class='select2-result-repository__statistics'>" +
          "<div class='select2-result-repository__forks'><i class='fa fa-flash'></i> " + repo.forks_count + " Forks</div>" +
          "<div class='select2-result-repository__stargazers'><i class='fa fa-star'></i> " + repo.stargazers_count + " Stars</div>" +
          "<div class='select2-result-repository__watchers'><i class='fa fa-eye'></i> " + repo.watchers_count + " Watchers</div>" +
        "</div>" +
        "</div></div>";
      
        return markup;
      }
      
    function formatRepoSelection (repo) {
        return repo.full_name || repo.text;
    }




    // loading array data
    var data = [
        {
            id: 0,
            text: 'enhancement'
        },
        {
            id: 1,
            text: 'bug'
        },
        {
            id: 2,
            text: 'duplicate'
        },
        {
            id: 3,
            text: 'invalid'
        },
        {
            id: 4,
            text: 'wontfix'
        }
    ];
    $(".js-example-data-array").select2({
      data: data
    })


    //automatic selection
    $('#automatic-selection').select2({
        selectOnClose: true
    });
      

    //remain open after selection
    $('#remain-open').select2({
        closeOnSelect: false
    });


    //dropdown-placement
    $('#dropdown-placement').select2({
        dropdownParent: $('#select2-modal')
    });


    // limit the number of selection
    $('#limit-selection').select2({
        maximumSelectionLength: 2
    });


    // dynamic option
    $('#dynamic-option-creation').select2({
        tags: true
    });


    // tagging with multi value select box
    $('#multi-value-select').select2({
        tags: true
    });


    // single-select-placeholder
    $(".single-select-placeholder").select2({
        placeholder: "Select a state",
        allowClear: true
    });


    // multi select placeholder
    $(".multi-select-placeholder").select2({
        placeholder: "Select a state"
    });


    //default selection placeholder
    $(".default-placeholder").select2({
        placeholder: {
            id: '-1', // the value of the option
            text: 'Select an option'
          }
    });


    // customizing how results are matched
    function matchCustom(params, data) {
        // If there are no search terms, return all of the data
        if ($.trim(params.term) === '') {
          return data;
        }
    
        // Do not display the item if there is no 'text' property
        if (typeof data.text === 'undefined') {
          return null;
        }
    
        // `params.term` should be the term that is used for searching
        // `data.text` is the text that is displayed for the data object
        if (data.text.indexOf(params.term) > -1) {
          var modifiedData = $.extend({}, data, true);
          modifiedData.text += ' (matched)';
    
          // You can return modified objects from here
          // This includes matching the `children` how you want in nested data sets
          return modifiedData;
        }
    
        // Return `null` if the term should not be displayed
        return null;
    }
    $(".customize-result").select2({
        matcher: matchCustom
    });


    // matching grouped options 

    function matchStart(params, data) {
        // If there are no search terms, return all of the data
        if ($.trim(params.term) === '') {
          return data;
        }
      
        // Skip if there is no 'children' property
        if (typeof data.children === 'undefined') {
          return null;
        }
      
        // `data.children` contains the actual options that we are matching against
        var filteredChildren = [];
        $.each(data.children, function (idx, child) {
          if (child.text.toUpperCase().indexOf(params.term.toUpperCase()) == 0) {
            filteredChildren.push(child);
          }
        });
      
        // If we matched any of the timezone group's children, then set the matched children on the group
        // and return the group object
        if (filteredChildren.length) {
          var modifiedData = $.extend({}, data, true);
          modifiedData.children = filteredChildren;
      
          // You can return modified objects from here
          // This includes matching the `children` how you want in nested data sets
          return modifiedData;
        }
      
        // Return `null` if the term should not be displayed
        return null;
    }
    $(".match-grouped-options").select2({
        matcher: matchStart
    });


    //minimum search term length
    $(".minimum-search-length").select2({
        minimumInputLength: 3 // only start searching when the user has input 3 or more characters
    });

    //maximum search term length
    $(".maximum-search-length").select2({
        maximumInputLength: 20 // only allow terms up to 20 characters long
    });


    // programmatically add new option
    var data = {
        id: 1,
        text: 'New Item'
    };
    var newOption = new Option(data.text, data.id, false, false);
    $(".add-new-options").append(newOption).trigger('change').select2();


    // create if not exists

    // Set the value, creating a new option if necessary
    if ($('.create-if-not-exists').find("option[value='" + data.id + "']").length) {
        $('.create-if-not-exists').val(data.id).trigger('change');
    } else { 
        // Create a DOM Option and pre-select by default
        var newOption = new Option(data.text, data.id, true, true);
        // Append it to the select
        $('.create-if-not-exists').append(newOption).trigger('change').select2();
    } 

    

    // using jquery selector

    $('.jquery-selector').select2();
    $('.jquery-selector').on("change", function(){
        var selectData = $(this).find(':selected');
        var value = selectData.val();
        alert("you select " + value);
    });


    // select2 rtl support
    $(".rtl-select2").select2({
        dir: "rtl"
    });


    // destroy selector
    $(".destroy-selector").select2();

    $("#destroy-selector-trigger").click(function(){
        $(".destroy-selector").select2("destroy");
    });


    // opening options
    $(".opening-dropdown").select2();
    $("#dropdown-trigger-open").click(function(){
        $(".opening-dropdown").select2('open');
    });


    // open or close dropdown
    $(".open-close-dropdown").select2();
    $("#opening-dropdown-trigger").click(function(){
        $(".open-close-dropdown").select2('open');
    });
    $("#closing-dropdown-trigger").click(function(){
        $(".open-close-dropdown").select2('close');
    });


    // select2 methods
    var $singleUnbind = $(".single-event-unbind").select2();

    $(".js-programmatic-set-val").on("click", function () {
        $singleUnbind.val("CA").trigger("change");
    });
    
    $(".js-programmatic-open").on("click", function () {
        $singleUnbind.select2("open");
    });
    
    $(".js-programmatic-close").on("click", function () {
        $singleUnbind.select2("close");
    });
    
    $(".js-programmatic-init").on("click", function () {
        $singleUnbind.select2({
            width: "400px"
        });
    });
    
    $(".js-programmatic-destroy").on("click", function () {
        $singleUnbind.select2("destroy");
    });


    var $unbindMulti = $(".js-example-programmatic-multi").select2();
    $(".js-programmatic-multi-set-val").on("click", function () {
        $unbindMulti.val(["CA", "HA"]).trigger("change");
    });
    
    $(".js-programmatic-multi-clear").on("click", function () {
        $unbindMulti.val(null).trigger("change");
    });


})(jQuery);