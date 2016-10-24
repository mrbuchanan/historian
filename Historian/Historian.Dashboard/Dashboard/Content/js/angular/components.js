angular.module('components', [])
       // setup sidebar component
       .directive('sidebar', function () {
           return {
               // restrict to element name only
               restrict: 'E',
               transclude: true,
               scope: {},
               controller: function ($scope, $element) {
                   var channels = $scope.channels = $scope.$parent.channels;

                   $scope.select = function (channel) {
                       angular.forEach(channels, function (c) {
                           c.selected = false;
                       });

                       channel.selected = true;
                   };
               },
               template:
                   '<ul class="nav nav-sidebar">' +
                        '<li ng-repeat="channel in channels">' +
                            '<a href="" ng-click="select(channel)" ng-class="{active:channel.selected}">{{channel.title}}</a>' +
                        '</li>' +
                   '</ul>',
               replace: true,
           };
       })
       // setup tabs component
       .directive('tabs', function () {
           return {
               // restrict to element name only
               restrict: 'E',
               transclude: true,
               // define scope
               scope: {},
               // set controller for tabs
               controller: function ($scope, $element) {
                   // setup list of panes
                   var panes = $scope.panes = []

                   // select function, when selecting a tab
                   $scope.select = function (pane) {
                       // iterate through each tab
                       angular.forEach(panes, function (pane) {
                           // deselect pane
                           pane.selected = false;
                       });

                       // make tab selected
                       pane.selected = true;

                       // load pane contents
                       pane.load(pane);
                   };

                   // add pane to tab list
                   this.addPane = function (pane) {
                       // if no panes, make this one selected
                       if (panes.length == 0) $scope.select(pane);

                       // add to list
                       panes.push(pane);
                   };
               },
               template:
                   '<div>' +
                       '<ul class="nav nav-tabs">' +
                           '<li ng-repeat="pane in panes" ng-class="{active:pane.selected}">' +
                               '<a href="" ng-click="select(pane)">{{pane.title}}</a>' +
                           '</li>' +
                       '</ul>' +
                       '<div class="tab-content" ng-transclude></div>' +
                   '</div>',
               replace: true
           };
       })
       .directive('pane', function () {
           return {
               require: '^tabs',
               // restrict to element name only
               restrict: 'E',
               transclude: true,
               scope: { title: '@' },
               link: function (scope, element, attrs, tabsController) {
                   tabsController.addPane(scope);
               },
               controller: function($scope, $element) {
                   $scope.load = function (self) {
                       var x = self.modelUrl;
                   };
               },
               template:
                 '<div class="tab-pane" ng-class="{active: selected}" ng-transclude>' +
                 '</div>',
               replace: true
           };
       });