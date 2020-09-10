var app = angular.module('demoApp', ['ngRoute', 'ngResource', 'ui.grid', 'ui.bootstrap', 'ngAnimate', 'ngSanitize','ui.grid.pagination',]);
app.config([
    '$routeProvider', function ($routeProvider) {

        $routeProvider.when("/", {
            controller: "phoneBookController",
            templateUrl: "/app/pages/phoneBook/phoneBook.html",
            allowAnonymous: true,
        });

        $routeProvider.otherwise({ redirectTo: "/" });
    }
]);
