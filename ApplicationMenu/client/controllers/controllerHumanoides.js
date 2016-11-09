menu.controller('HumanoideCtrl', function ($scope,$state, $rootScope) {
	// Breadcrumb settings
	$rootScope.chemin = 'Accueil';
	$rootScope.chemin1 = 'Exercice des humanoïdes ';
	$rootScope.stateChemin1 = $state.current.name;
	$rootScope.chemin2 = '';
	$rootScope.chemin3 = '';
	$rootScope.suivant = true;

	// Next function : go to next state (doors configuration) with type of doors as paramaters
	$scope.suivant_click = function(type){
			$state.go(type);
	}

	// Previous function : go back to menu
	$scope.previous = function () {
		$state.go('mainMenu');
	}
});