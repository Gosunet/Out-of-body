menu.controller('OobCtrl', function ($scope, $state, $http, $rootScope) {
	// Breadcrumb settings
	$rootScope.chemin = 'Accueil';
	$rootScope.chemin1 = 'Sortie de corps';
	$rootScope.stateChemin1 = $state.current.name;
	$rootScope.chemin2 = '';
	$rootScope.chemin3 = '';
	$rootScope.suivant = true;


	$scope.morphing = false;
	$scope.baton = false;
	$scope.message = '';

	// Previous function : back to menu
	$scope.previous = function () {
		$state.go('mainMenu');
	}

	/*Next function : send "oob" to server to run Unity's exercice 
					AND "X_X_X" for configure the scene whith baton, morphing, ghost AND go to the next state*/

	$scope.next = function () {
		if ($scope.baton && $scope.morphing && $scope.ghost){
			$http.get('oob/1_1_1');
		}	
		else if ($scope.baton && $scope.morphing){
			$http.get('oob/1_1_0');
		}		
		else if ($scope.morphing && $scope.ghost){
			$http.get('oob/0_1_1');
		}			
		else if ($scope.morphing){
			$http.get('oob/0_1_0');
		}	
		else if ($scope.baton){
			$http.get('oob/1_0_0');
		}
		else {
			$http.get('oob/0_0_0');
		}
		$state.go('runOob');
	};

});
