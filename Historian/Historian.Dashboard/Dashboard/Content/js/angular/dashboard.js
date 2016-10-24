angular.module('dashboardApp', ['components'])
       .controller('SidebarController', ['$scope', function ($scope) {
           
           
           $scope.channels = [{name: 'hello', selected: false}]


       }])
       .controller('SummaryController', function () {

       });