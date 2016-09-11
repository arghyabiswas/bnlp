
function TranslationService($http){
	var self = this,
		Input = '',
		Output = '',
		DebugOutput='';

		this.$get = function(){
			$http({
			  method: 'GET',
			  url: '/api/Translation',
			  params: {id: self.Input}
			}).then(function successCallback(response) {
			    self.Output = response.data;
			  }, function errorCallback(response) {
			    self.Output = response;
			  });
		};

		this.$getDebug = function(){
			$http({
			  method: 'GET',
			  url: '/api/TranslationDebug',
			  params: {id: self.Input}
			}).then(function successCallback(response) {
			    self.DebugOutput = response.data;
			  }, function errorCallback(response) {
			    self.Output = response;
			  });
		};
}

app.service('TranslationService',TranslationService);
TranslationService.$inject = ['$http'];

function TranslationController($scope,TranslationService){
	var self = this,
	Input = '',
	Output = '',
	DebugOutput='';

	self.Input = 'This is cat.';
	$scope.$watch(
			function() {return TranslationService.Output},
            function() {self.Output = TranslationService.Output}
    );

    $scope.$watch(
			function() {return TranslationService.DebugOutput},
            function() {self.DebugOutput = TranslationService.DebugOutput}
    );

	this.Translate=function(){
		TranslationService.Input = self.Input;
		TranslationService.$get();
	}

	this.TranslateDebug=function(){
		TranslationService.Input = self.Input;
		TranslationService.$getDebug();
	}
}


app.controller('TranslationController', TranslationController);
TranslationController.$inject = ['$scope','TranslationService']



