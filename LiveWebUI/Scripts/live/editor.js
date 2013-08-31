//function GetNewEditorHeight() {
//    var navbarHeight = document.querySelector('.navbar').clientHeight;
//    var footHeight = document.querySelector('.foot').clientHeight;
//    return window.innerHeight - navbarHeight - footHeight - 65;
//}

//function ResizeEditor() {
//    var editorHeight = GetNewEditorHeight();
//    editor.setSize("100%", editorHeight);
//}

var options = {
    height: "400px",
    lineNumbers: true,
    autofocus: true
};
var editor = CodeMirror.fromTextArea(document.querySelector('#taa-content'), options);
editor.setSize("100%", "400px");

document.querySelector('#taa-source').style.height = "600px";
var sourcePrepared = false;
var source = null;
function PrepareSource() {
    if (!sourcePrepared) {
        source = CodeMirror.fromTextArea(document.querySelector('#taa-source'), options);
        source.setSize("100%", "600px");
        sourcePrepared = true;
    }
}

//window.addEventListener('resize', ResizeEditor);
//ResizeEditor();