function toogleMobileNavigation() {
    document.querySelector(".mobile-nav-menu").classList.toggle("active");
    var mobileToogleBtn = document.querySelector("#mobile-nav-toogle-button");
    mobileToogleBtn.classList.toggle("fa-bars");
    mobileToogleBtn.classList.toggle("fa-close");
}

function toStickyNavigation() {
    var header = document.querySelector('.header');
    var stickyPoint = header.offsetTop;
    // xử lý sự kiện cuộn chuột
    window.addEventListener("scroll", function (e) {
        if (window.scrollY >= stickyPoint) {
            header.classList.add("sticky");
        } else {
            header.classList.remove("sticky");
        }
    });
}

function countDown(dueDate, dSelector, hSelector, mSelector, sSelector) {
    var countDownDate = new Date(dueDate).getTime();
    var now = new Date().getTime();
    var distance = countDownDate - now;
    var days = Math.floor(distance / (1000 * 60 * 60 * 24));
    var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
    var seconds = Math.floor((distance % (1000 * 60)) / 1000);
    document.querySelector(dSelector).innerHTML = (days >= 10 ? days : '0' + days) + 'd';
    document.querySelector(hSelector).innerHTML = (hours >= 10 ? hours : '0' + hours) + 'h';
    document.querySelector(mSelector).innerHTML = (minutes >= 10 ? minutes : '0' + minutes) + 'm';
    document.querySelector(sSelector).innerHTML = (seconds >= 10 ? seconds : '0' + seconds) + 's';
}

function AddToCart(pid, button, input = null) {
    var count = 1;
    if (input != null) {
        count = document.querySelector(input).value;
    }
    const xhttp = new XMLHttpRequest();

    xhttp.onload = function () {
        const res = JSON.parse(this.responseText);

        if (res['status'] == true) {
            var currentValue = document.querySelector('.badge').innerHTML;

            if (!currentValue.trim()) { // không có phần tử này đang hiển thị trên trang
                currentValue = 0;
            }

            document.querySelector('.badge').style.padding = '2px 4px';
            document.querySelector('.badge').innerHTML = parseInt(currentValue) + res['count'];

            var content = res['count'] == 0 ? 'cập nhật' : 'thêm'

            showToast({
                type: "success",
                title: "Thêm vào giỏ hàng",
                content: `Đã ${content} ${res['quantity']} sản phẩm vào giỏ hàng`
            });

            button.parentNode.classList.add('addedToCart');
            if (input != null) {
                document.querySelector(input).value = 1;
            }
        }
        else {
            showToast({
                type: "error",
                title: "Lỗi đã xảy ra",
                content: `${res['errorMessage']}`
            });
        }
    }

    xhttp.open("GET", `/Cart/AddToCart?pid=${pid}&count=${count}`);
    xhttp.send();
}

function toggleModal(modalSelector, callback = function () { }) {
    callback();
    document.querySelector(modalSelector).classList.toggle('active');

    document.querySelector('.close-modal-btn').onclick = function () {
        document.querySelector(modalSelector).classList.toggle('active');
    }
    document.querySelector('.modal-overlay').onclick = function () {
        document.querySelector(modalSelector).classList.toggle('active');
    }
}