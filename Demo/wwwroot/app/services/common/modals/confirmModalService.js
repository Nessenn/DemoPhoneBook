app.service('confirmModalService', ['$uibModal',
    function ($uibModal) {

        let modalOptionsDefault = {
            closeButtonText: 'Close',
            actionButtonText: 'OK',
            headerText: 'Confirmation',
            bodyText: 'Perform this action?'
        };

        this.showModal = function (modalOptions) {
            modalOptionsDefault = modalOptions;
            return $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/pages/common/modals/confirmModal.html',
                controller: 'confirmModalController',
                controllerAs: 'pc',
                backdrop: 'static',
                size: 'sm',
                resolve: {
                    data: function () {
                        return modalOptionsDefault;
                    }
                }
            })
        };
    }]);