/** 
 * `delay` is the duration until the toast is faded out
 * `removeAfterDelay` is the duration to wait for the toast is removed completely from DOM
*/
function showToast({
    title = '',
    content = '',
    type = 'info',
    delay = 3000,
    removeAfterDelay = 500,
    closeIcon = '<ion-icon name="close-outline"></ion-icon>',
    floatDuration = 500,
    fadeOutDuration = 500,
    toastContainerId = 'toast-container',
    icon = ''
}) {
    const toastContainer = document.getElementById(toastContainerId);
    if (toastContainer) {
        const toast = document.createElement('div');

        // sau 1 khoảng thời gian thì ẩn toast đi
        const removeTimeout = setTimeout(function () {
            toast.remove();
        }, delay + removeAfterDelay);

        // khi người dùng ấn nút tắt
        toast.onclick = function (e) {
            if (e.target.closest('.toast-close')) {
                this.style.opacity = '0';
                this.style.transform = 'translateX(100%)';
                clearTimeout(removeTimeout);
                setTimeout(() => { toastContainer.removeChild(toast); }, removeAfterDelay);
            }
        }

        const icons = {
            "success": "checkmark-circle",
            "info": "information-circle",
            "warning": "alert-circle",
            "error": "bug"
        };

        icon = icon === '' ? icons[type] : icon;

        toast.classList.add('toast', `toast-${type}`); // thêm lớp cơ sở cho toast
        toast.style.animation = `floatInto ${floatDuration / 1000}s cubic-bezier(0.175, 0.885, 0.32, 1.275), fadeOut ${fadeOutDuration / 1000}s ${delay / 1000}s ease-in-out forwards`;
        toast.innerHTML = `<div class="toast-icon">
                                <ion-icon name="${icons[type]}"></ion-icon>
                            </div>
                            <div class="toast-body">
                                <h3 class="toast-body-header">${title}</h3>
                                <p class="toast-body-content">${content}</p>
                            </div>
                            <div class="toast-close">
                                ${closeIcon}
                            </div>`;
        toastContainer.appendChild(toast);
    }
}

// HOW TO USE

// Animations
/*
@keyframes floatInto {
    from {
        opacity: 0;
        transform: translateX(100%);
    }

    to {
        opacity: 1;
        transform: translateX(0);
    }
}

@keyframes fadeOut {
    to {
        opacity: 0;
        transform: translateX(100%);
    }
}
*/

// Recommended css
/*
:root {
    --success-color: #00c000;
    --warning-color: #ffc302;
    --info-color: #00a8eb;
    --error-color: #f02c67;
}
#toast-container {
    position: fixed;
    transition: all 0.5s linear;
    top: 30px;
    right: 30px;
    width: 500px;
    max-width: 100%;
}
@media (max-width: 530px) {
    #toast-container {
        right: 0;
    }
}
.toast {
    padding: 1.2rem 0.5rem;
    background-color: #fff;
    display: flex;
    align-items: center;
    border-left: 7px solid;
    gap: 1rem;
    transition: all 0.5s linear;
}

.toast + .toast {
    margin-top: 0.865rem;
}

.toast-success {
    border-left-color: var(--success-color);
}

.toast-success .toast-icon,
.toast-success .toast-body-header {
    color: var(--success-color);
}

.toast-warning {
    border-left-color: var(--warning-color);
}

.toast-warning .toast-icon,
.toast-warning .toast-body-header {
    color: var(--warning-color);
}

.toast-info {
    border-left-color: var(--info-color);
}

.toast-info .toast-icon,
.toast-info .toast-body-header {
    color: var(--info-color);
}

.toast-error {
    border-left-color: var(--error-color);
}

.toast-error .toast-icon,
.toast-error .toast-body-header {
    color: var(--error-color);
}

.toast-icon {
    font-size: 1.5rem;
}

.toast-body {
    flex: 1;
}

.toast-body-header {
    font-size: 1.2rem;
    font-weight: 600;
}

.toast-body-content {
    font-size: 1rem;
    color: #888;
    margin-top: 0.5rem;
}
.toast-close {
    font-size: 1.5rem;
    cursor: pointer;
}
*/