'use strict';
app.controller('confirmModalController', function ($uibModalInstance, data) {
    var pc = this;
    pc.data = data;
    pc.submitted = false;

    pc.ok = function () {
        pc.submitted = true;
        $uibModalInstance.close(pc.data);
    };

    pc.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});