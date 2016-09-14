menu.controller('AvatarCtrl', function ($scope, $state, $rootScope, $http) {
	// Breadcrumb settings
	$rootScope.chemin = 'Accueil';
	$rootScope.chemin1 = 'Avatar';
	$rootScope.stateChemin1 = $state.current.name;
	$rootScope.chemin3 = '';
	$rootScope.suivant = true;

	var sexe = "F";

	// Previous function : menu return
	$scope.previous = function () {
		$state.go('mainMenu');
	}

	// Next function : http get to server + next state go 
	$scope.next = function () {
		$http.get("/" + sexe + "_avatar");
		$state.go('avatar_choix');
	}

	// Radio button management
	$scope.sexe_selected = function(type){
        sexe=type;
	}
});


