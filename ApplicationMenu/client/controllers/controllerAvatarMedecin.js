menu.controller('AvatarMedecinCtrl', function ($scope, $state, $rootScope, $http) {
	// Breadcrumb settings
	$rootScope.chemin = 'Accueil';
	$rootScope.chemin3 = 'Choix Médecin';
	$rootScope.suivant = false;

	// Previous function : go back to sexe avatar select AND send "stop" to server to stop the avatar menu
	$scope.previous = function () {
		$state.go('avatar');
		$http.get("stop");
	}

	// Next function : send "validerAvatar" to menu to validate the one choose by the patient AND the reduction factor choose by the doctor
	$scope.next = function () {
		$http.get("validerAvatar/" + $scope.facteurAvatar);
		$state.go('mainMenu');
	}
});
