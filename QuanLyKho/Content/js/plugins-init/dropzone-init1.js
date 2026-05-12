
  Dropzone.options.myDropzone = {
    // Prevents Dropzone from uploading dropped files immediately
    autoProcessQueue: false,
    dictDefaultMessage: "Upload ảnh sản phẩm",
    acceptedFiles: ".png,.jpg,.gif,.bmp,.jpeg",
    maxFiles: 4,
    maxFilesize: 1,
    parallelUploads: 10,
    addRemoveLinks: true,
    dictRemoveFile:"Xóa file",
    init: function() {
      var submitButton = document.querySelector("#submit-all")
      myDropzone = this; // closure
  
      submitButton.addEventListener("click", function() {
        myDropzone.processQueue(); 
        // autoProcessQueue: true// Tell Dropzone to process all queued files.
      });
  
      // You might want to show the submit button only when 
      // files are dropped here:
      this.on("addedfile", function() {
        // Show submit button here and/or inform user to click it.
      });
  
    }
  };

  