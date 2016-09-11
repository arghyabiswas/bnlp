var app = angular.module('BNLP', []);

app.directive('choiceTree', function() {
      return {
        template: '<ul><choice ng-repeat="choice in tree"></choice></ul>',
        replace: true,
        transclude: true,
        restrict: 'E',
        scope: {
          tree: '=ngModel'
        }
      };
});

app.directive('choice', function($compile) {
  return { 
    restrict: 'E',
    //In the template, we do the thing with the span so you can click the 
    //text or the checkbox itself to toggle the check
    template: '<li>' +
      '<span >' +
        '{{choice.Type}} : ' +
        '{{choice.English}} :' +
        '[{{choice.Lemma}}]' +
        '{{choice.Bengali}} :' +
        '</span>' +
        '<div style="word-wrap: break-word;">{{choice.GlobalTokenProperty}} </div>' +
    '</li>',
    link: function(scope, elm, attrs) {
      //Add children by $compiling and doing a new choice directive
      if (scope.choice.tokens != null) {
        var childChoice = $compile('<choice-tree ng-model="choice.tokens"></choice-tree>')(scope)
        elm.append(childChoice);
      }
    }
  };
});

