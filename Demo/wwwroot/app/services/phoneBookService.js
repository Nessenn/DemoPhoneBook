'use strict';
app.factory('phoneBookService', ['$http', function ($http) {

    var serviceBase = "http://localhost:65345/";

    var phoneBookServiceFactory = {};

    var _getEntities = function (data) {
        return $http.post(serviceBase + 'api/phonebooks/getPhoneBooks' , data);
    };

    var _getEntity = function (id) {
        return $http.get(serviceBase + 'api/phonebooks/' + id );
    };

    var _updateEntity = function (data) {
        return $http.put(serviceBase + 'api/phonebooks/', data);
    };

    var _createEntity = function (data) {
        return $http.post(serviceBase + 'api/phonebooks/', data);
    };

    var _deleteEntity = function (id) {
        return $http.delete(serviceBase + 'api/phonebooks/' + id );
    };

    phoneBookServiceFactory.getEntities = _getEntities;
    phoneBookServiceFactory.getEntity = _getEntity;
    phoneBookServiceFactory.updateEntity = _updateEntity;
    phoneBookServiceFactory.createEntity = _createEntity;
    phoneBookServiceFactory.deleteEntity = _deleteEntity;

    return phoneBookServiceFactory;

}]);