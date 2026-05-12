var toolbarOptions = [
    ['html'],

    ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
    ['blockquote', 'code-block'],

   /* [{ 'header': 1 }, { 'header': 2 }],   */            // custom button values
    [ 'link', 'image', 'video', 'formula' ],          // add's image support
    [{ 'list': 'ordered'}, { 'list': 'bullet' }],
    [{ 'script': 'sub'}, { 'script': 'super' }],      // superscript/subscript
    [{ 'indent': '-1'}, { 'indent': '+1' }],          // outdent/indent
    [{ 'direction': 'rtl' }],                         // text direction

    [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
    [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

    [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
    [{ 'font': [] }],
    [{ 'align': [] }],

    ['clean']                                         // remove formatting button
];
var quill = new Quill('#editor', {
modules: {
    toolbar: toolbarOptions,
},
placeholder: 'Compose an epic...',
theme: 'snow'
});




var htmlButton = document.querySelector('.ql-html');

htmlButton.addEventListener('click', function() {
	var htmlEditor = document.querySelector('.ql-html-editor');
  if (htmlEditor){ 
    console.log(htmlEditor.value.replace(/\n/g, ""));
  	quill.root.innerHTML = htmlEditor.value.replace(/\n/g, "");
    quill.container.removeChild(htmlEditor);
  } else {
    
    options = {
      "indent":"auto",
      "indent-spaces":2,
      "wrap":80,
      "markup":true,
      "output-xml":false,
      "numeric-entities":true,
      "quote-marks":true,
      "quote-nbsp":false,
      "show-body-only":true,
      "quote-ampersand":false,
      "break-before-br":true,
      "uppercase-tags":false,
      "uppercase-attributes":false,
      "drop-font-tags":true,
      "tidy-mark":false
    }
    htmlEditor = document.createElement("textarea");
    htmlEditor.className = 'ql-editor ql-html-editor'
    htmlEditor.innerHTML = tidy_html5(quill.root.innerHTML, options).replace(/\n\n/g, "\n");
    quill.container.appendChild(htmlEditor);
  }
});