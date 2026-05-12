$('#ck_editor,#ck_editor3,#ck_editor4,#ck_editor1, #ck_editor-cauhinh').trumbowyg({
    lang: 'vi',
    btnsDef: {
// Create a new dropdown
image: {
    dropdown: ['insertImage', 'upload'],
    ico: 'insertImage'
}
},
    btns: [
    ['viewHTML'],
['formatting'],
['strong', 'em', 'del'],
['superscript', 'subscript'],
['link'],
['image'], // Our fresh created dropdown
['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
['unorderedList', 'orderedList'],
['horizontalRule'],
['removeformat'],
['fullscreen'],
['table'],
['foreColor', 'backColor'],
['fontsize'],
['fontfamily'],
],

plugins: {
// Add imagur parameters to upload plugin for demo purposes
upload: {
    serverPath: 'https://api.imgur.com/3/image',
    fileFieldName: 'image',
    headers: {
        'Authorization': 'Client-ID xxxxxxxxxxxx'
    },
    urlPropertyName: 'data.link'
}
}

  });