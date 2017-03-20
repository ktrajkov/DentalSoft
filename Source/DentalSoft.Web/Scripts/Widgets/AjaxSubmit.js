(function ($) {
    $.fn.ajaxSubmit = function (options) {
        return this.submit(function () {
            if ($(this).valid()) {
                $.ajax({
                    url: options.url,
                    type: "POST",
                    data: $(this).serialize(),
                    success: function (result) {
                        if (result === "") {
                            dentalSoft.showNotification("Успешен запис", "success");
                            options.success();
                        }
                        else {
                            for (var i = 0; i < result.length; i++) {
                                dentalSoft.showNotification(result[i].ErrorMessage, "error");
                            }
                        }
                    }
                });
            }
            return false;
        });
    };

}(jQuery));