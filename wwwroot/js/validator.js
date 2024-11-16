function Validator(options) {

    // function getParentElement(element, selector) { 
    //     while (element.parentNode) {
    //         if(element.parentNode.matches(selector)) {
    //             return element.parentNode;
    //         }
    //         element = element.parentNode;
    //     }
    // }

    var selectorRule = {}; // các phần tử đang invalid

    function validate(inputElement, rule) {
        let parent = inputElement.parentNode;
        let errorMessage;
        let rules = selectorRule[rule.inputSelector];
        for (let i = 0; i < rules.length; i++) {
            errorMessage = rules[i](inputElement.value);
            if (errorMessage) break;
        }

        if (errorMessage) {
            parent.classList.add(options.invalidClass);
            parent.querySelector(options.errorElement).innerHTML = errorMessage;
        } else {
            parent.classList.remove(options.invalidClass);
            parent.querySelector(options.errorElement).innerHTML = "";
        }
        return !errorMessage;
    }
    let form = document.querySelector(options.formSelector);
    if (form) {
        form.onsubmit = function (e) {
            e.preventDefault();
            var isValid = true;
            options.rules.forEach((rule) => {
                let inputElement = document.querySelector(rule.inputSelector);
                if (!validate(inputElement, rule)) {
                    isValid = false;
                }
            });
            if (isValid) {
                options.onSubmit();
            }
        }

        options.rules.forEach((rule) => {
            if (Array.isArray(selectorRule[rule.inputSelector])) {
                selectorRule[rule.inputSelector].push(rule.validate);
            } else {
                selectorRule[rule.inputSelector] = [rule.validate];
            }

            let inputElement = document.querySelector(rule.inputSelector);
            var parent = inputElement.parentNode;

            inputElement.onblur = function () {
                validate(inputElement, rule);
            };

            inputElement.oninput = function () {
                parent.classList.remove(options.invalidClass);
                parent.querySelector(options.errorElement).innerHTML = "";
            };
        });
    }
}

Validator.required = function (inputSelector, errorMessage) {
    return {
        inputSelector: inputSelector,
        validate: function (value) {
            return value.trim()
                ? undefined
                : errorMessage || "Hãy nhập vào trường này";
        },
    };
};

Validator.isEmail = function (inputSelector, errorMessage) {
    return {
        inputSelector: inputSelector,
        validate: function (value) {
            var emailRegEx = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
            return emailRegEx.test(value)
                ? undefined
                : errorMessage ||
                `${value} không phải là một địa chỉ email hợp lệ`;
        },
    };
};

Validator.minLength = function (inputSelector, length, errorMessage) {
    return {
        inputSelector: inputSelector,
        validate: function (value) {
            return value.length >= length
                ? undefined
                : errorMessage || `Nhập ít nhất ${length} ký tự`;
        },
    };
};
Validator.equalTo = function (inputSelector, bindingValue, errorMessage) {
    return {
        inputSelector: inputSelector,
        validate: function (value) {
            return value === bindingValue()
                ? undefined
                : errorMessage || "Giá trị nhập vào không trùng khớp";
        },
    };
};
