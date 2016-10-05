menu.controller('HumanoidesConfigCtrl', function ($scope, $state, $http, $rootScope, $localStorage) {
    $rootScope.chemin = 'Accueil';
    $rootScope.chemin1 = 'Exercice des humanoïdes';
    $rootScope.stateChemin1 = $state.current.name;
    $rootScope.chemin2 = '';
    $rootScope.suivant = true;

    // Previous function : go back to menu
    $scope.previous = function () {
        $state.go('mainMenu');
    };

    // Initialistion of numbers : load last values or default values
    initHumanoide = function () {
        $scope.nbIntervalle = Number(window.localStorage["local_hum_nbIntervalle"]);
        $scope.diffIntervalle = Number(window.localStorage["local_hum_diffIntervalle"]);
        $scope.nbRepet = Number(window.localStorage["local_hum_nbRepet"]);
    };
    
     // Initialisation
    initHumanoide();
    
    // Function to send values to server AND go to running exercice state
	var sendMessage = function () {
		var type ="hu";
		var message = type + '/' + $scope.nbRepet + '_' + $scope.nbIntervalle + '_' + $scope.diffIntervalle;
		//$http.get(message);
		$state.go('runHumanoide');
	};

    // Save function : save all value in browser local storage
    saveAllValues = function (){
        window.localStorage["local_hum_nbRepet"] = $scope.nbRepet;
        window.localStorage["local_hum_nbIntervalle"] = $scope.nbIntervalle;
        window.localStorage["local_hum_diffIntervalle"] = $scope.diffIntervalle;
    };

    // Watch function to update result value and save all values
    $scope.$watch('nbRepet + nbIntervalle + diffIntervalle', function () {
        $scope.nbEssai = ($scope.nbIntervalle) * $scope.nbRepet;
        saveAllValues();
    });
    
    // Function exectute click on "run exercice" button click : check if all values are corrects
	$scope.executer_click = function(){
		$scope.informationsDonnees ="";
		if ($scope.nbEssai > 0 & $scope.nbIntervalle == 1){
				sendMessage();
		}
		else if ($scope.nbEssai > 0 & $scope.nbIntervalle > 1 & $scope.diffIntervalle != 0){
				sendMessage();
		}
		else
			$scope.informationsDonnees = "Tous les champs ne sont pas remplis";
	};
});