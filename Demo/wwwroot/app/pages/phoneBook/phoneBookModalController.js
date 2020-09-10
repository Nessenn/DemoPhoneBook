'use strict';
app.controller('phoneBookModalController', function ($uibModalInstance, data) {
    var pc = this;
    pc.data = data;
    pc.submitted = false;

    pc.ok = function (phoneBookForm) {
        pc.submitted = true;

        if (!phoneBookForm.$valid) {
            return;
        }

        $uibModalInstance.close(pc.data);
    };

    pc.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});