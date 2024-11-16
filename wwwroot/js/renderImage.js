function displayImages(input, selectorToRenderImage) {
    document.querySelector(selectorToRenderImage).innerHTML = '';
    if (input.files && input.files.length != 0) {
        document.querySelector(selectorToRenderImage).innerHTML = `<p class='fw-bold text-primary mt-2'>
        Hình ảnh vừa chọn \(${input.files.length}\) </p>`;

        for (var i = 0; i < input.files.length; i++) {
            var reader = new FileReader();

            reader.onload = function (e) {
                document.querySelector(selectorToRenderImage).innerHTML += `<img src="${e.target.result}" class="img-thumbnail me-2" />`;
            }
            reader.readAsDataURL(input.files[i]);
        }
    }
}