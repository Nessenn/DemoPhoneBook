'use strict';
app.controller('phoneBookController', [
    '$scope', 'phoneBookService', 'confirmModalService', 'uiGridConstants', '$uibModal', '$timeout',
    function ($scope, phoneBookService, confirmModalService, uiGridConstants, $uibModal, $timeout) {

        $scope.isBusy = false;
        $scope.modal = {
            data: {}
        };
        $scope.paginationOptions = {
            pageSize: 10, pageNumber: 1, sort: [], filters: []
        };

        $scope.loadData = function () {
            $scope.isBusy = true;
            let currentPage = $scope.paginationOptions.pageNumber;
            let data = {
                take: $scope.paginationOptions.pageSize,
                skip: (currentPage - 1) * $scope.paginationOptions.pageSize,
                sort: $scope.paginationOptions.sort,
                filter: { filters: $scope.paginationOptions.filters }
            }
            phoneBookService.getEntities(data).then(function (response) {
                $scope.isBusy = false;

                $scope.gridOptions.totalItems = response.data.total;
                $scope.gridOptions.data = response.data.data;

            });
        };

        $scope.gridOptions = {
            data: [],
            useExternalPagination: true,
            paginationCurrentPage: $scope.paginationOptions.pageNumber,
            pageSize: 10,
            enableFiltering: true,
            paginationPageSizes: [10, 25, 50, 75],
            paginationPageSize: $scope.paginationOptions.pageSize,
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;

                $scope, gridApi.core.on.filterChanged($scope,
                    function (rowEntity, colDef, newValue, oldValue) {

                        $scope.paginationOptions.filters = [];
                        for (var i = 0; i < rowEntity.grid.columns.length; i++) {
                            var column = rowEntity.grid.columns[i];
                            if (column.filter.term) {
                                $scope.paginationOptions.filters.push(
                                    {
                                        field: column.field,
                                        value: column.filter.term,
                                        logic: 'and',
                                        operator: 'contains'
                                    })
                            }
                        }

                        $scope.loadData();
                    }
                );

                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        $scope.paginationOptions.sort = [];
                    } else {
                        $scope.paginationOptions.sort = [];
                        for (var i = 0; i < sortColumns.length; i++) {
                            $scope.paginationOptions.sort.push({ dir: sortColumns[i].sort.direction, field: sortColumns[i].field });
                        }
                    }

                    $scope.loadData();
                });
                $scope.gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    $scope.paginationOptions.pageNumber = newPage;
                    $scope.paginationOptions.pageSize = pageSize;
                    $scope.loadData();
                });
            },
            columnDefs: [
                {
                    name: 'Phone Number',
                    field: 'phoneNumber',
                    width: '150',
                    filter: {
                        condition: true
                    }
                },
                {
                    name: 'Name',
                    field: 'name',
                    width: '25%',
                    filter: {
                        condition: true
                    }
                },
                {
                    name: 'Address',
                    field: 'address',
                    filter: {
                        condition: true
                    }
                },
                {
                    field: 'id',
                    name: '',
                    width: '120',
                    cellTemplate: '<button class="btn btn-sm btn-danger" style="margin-left: 5px;margin-right: 5px;margin-top: 1px;margin-bottom: 1px;" ng-click="grid.appScope.deleteRow(row)">Delete</button>' +
                        '<button class="btn btn-sm btn-primary" ng-click="grid.appScope.editRow(row)">Edit</button>',
                    enableFiltering: false,
                    enableSorting: false
                }
            ]
        };

        $scope.deleteRow = function (row) {
            let modalOptions = {
                closeButtonText: 'Cancel',
                actionButtonText: 'Yes',
                headerText: 'Confirmation',
                bodyText: 'Are you sure you want to delete?'
            };
            confirmModalService.showModal(modalOptions).result.then(function (res) {
                phoneBookService.deleteEntity(row.entity.id).then(function (response) {
                    $scope.loadData();
                });
            }, function (res) {

            });
        }

        $scope.editRow = function (row) {
            $scope.modal.data = {
                phoneNumber: row.entity.phoneNumber,
                name: row.entity.name,
                address: row.entity.address,
                id: row.entity.id
            };

            $scope.modal.data.headerText = "Edit";
            $scope.openModal();
        }

        $scope.new = function () {
            $scope.modal.data = {};
            $scope.modal.data.headerText = "New";
            $scope.openModal();
        }

        $scope.openModal = function () {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/pages/phoneBook/phoneBookModal.html',
                controller: 'phoneBookModalController',
                controllerAs: 'pc',
                backdrop: 'static',
                size: 'sm',
                resolve: {
                    data: function () {
                        return $scope.modal.data;
                    }
                }
            }).result.then(function (res) {
                if (res.id) {
                    phoneBookService.updateEntity(res).then(function (response) {
                        $scope.loadData();
                        if (response.data.status == "ok") {

                        }
                    });
                }
                else {
                    phoneBookService.createEntity(res).then(function (response) {
                        $scope.loadData();
                        if (response.data.status == "ok") {

                        }
                    });
                }
            }, function (res) {

            });
        }

        $scope.loadData();
    }
]
);

