menu.controller('OobRunCtrl', function ($scope, $state, $http, $rootScope, $document) {
	// Breadcrumb settings
	$rootScope.chemin = 'Accueil';
	$rootScope.chemin1 = 'Sortie de corps';
	$rootScope.stateChemin1 = 'oob';
	$rootScope.chemin2 = '';
	$rootScope.chemin3 = 'Application en cours';
	$rootScope.stateChemin3 = $state.current.name;
	$rootScope.suivant = false;

	// Stop backspace key event
	$document.on('keydown', function(e){
    	if(e.which === 8){
        	e.preventDefault();
      	}
 	 });

	// Exit function on "go to menu" button click : send "stop" to finish Unity's exercice AND go to menu state
	$scope.exit = function () {
		$http.get('stop');
		$state.go('mainMenu');
	}
});
