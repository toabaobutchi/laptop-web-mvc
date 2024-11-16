// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function displayImages(input, selectorToRenderImage) {
    document.querySelector(selectorToRenderImage).innerHTML = '';
    if (input.files && input.files.length != 0) {
        document.querySelector(selectorToRenderImage).innerHTML = "<p>Hình ảnh vừa chọn</p>";
        
        for (var i = 0; i < input.files.length; i++) {
            var reader = new FileReader();

            reader.onload = function (e) {
                document.querySelector(selectorToRenderImage).innerHTML += `<img src="${e.target.result}" />`;
            }
            reader.readAsDataURL(input.files[i]);
        }
    }
}
