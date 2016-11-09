menu.controller('HumanoidesConfigCtrl', function ($scope, $state, $http, $rootScope, $localStorage) {
    console.log($state.current.name);
    $rootScope.chemin = 'Accueil';
    $rootScope.chemin1 = 'Exercice des humanoïdes';
    $rootScope.stateChemin1 = 'humanoides';
    if($state.current.name == 'humanoides')
		$rootScope.chemin2 = 'Humanoides';
	else $rootScope.chemin2 = 'Batons';
    $rootScope.stateChemin2 = $state.current.name;
    $rootScope.chemin3 = '';
    $rootScope.suivant = true;

    // Previous function : go back to menu
    $scope.previous = function () {
        $state.go('humanoidesConfig');
    };

    // Initialistion of numbers : load last values or default values
    initHumanoide = function () {
        $scope.nbIntervalle = Number(window.localStorage["local_hum_nbIntervalle"]);
        $scope.nbRepet = Number(window.localStorage["local_hum_nbRepet"]);
        $scope.intervalleMin = Number(window.localStorage["local_hum_intervalleMin"]);
        $scope.intervalleMax = Number(window.localStorage["local_hum_intervalleMax"]);
    };

     // Initialisation
    initHumanoide();

    // Function to send values to server AND go to running exercice state
	var sendMessage = function () {
		var type ="hu";
        var type2 = ""
        if($state.current.name == "humanoides")
			type2 ="hu";
		else
			type2 = "ba";
		var message = type + '/' + $scope.nbRepet + '_' + $scope.nbIntervalle + '_' + $scope.intervalleMin + '_' + $scope.intervalleMax + '_' + type2;
		$http.get(message);
		$state.go('runHumanoide');
	};

    // Save function : save all value in browser local storage
    saveAllValues = function (){
        window.localStorage["local_hum_nbRepet"] = $scope.nbRepet;
        window.localStorage["local_hum_nbIntervalle"] = $scope.nbIntervalle;
        window.localStorage["local_hum_intervalleMin"] = $scope.intervalleMin;
        window.localStorage["local_hum_intervalleMax"] = $scope.intervalleMax;
    };

    // Watch function to update result value and save all values
    $scope.$watch('nbRepet + nbIntervalle + intervalleMin + intervalleMax', function () {
        $scope.nbEssai = ($scope.nbIntervalle) * $scope.nbRepet;
        saveAllValues();
    });

    // Function exectute click on "run exercice" button click : check if all values are corrects
	$scope.executer_click = function(){
		$scope.informationsDonnees ="";
		if ($scope.nbEssai > 0 & $scope.nbIntervalle == 1){
				sendMessage();
		}
		else if ($scope.nbEssai > 0 & $scope.nbIntervalle > 1 & $scope.intervalleMax >= 1 & $scope.intervalleMin >= 1) {
				sendMessage()
		}
		else
			$scope.informationsDonnees = "Tous les champs ne sont pas remplis";
	};

    $scope.intervalleMinChanged = function(){
        if ($scope.intervalleMin > $scope.intervalleMax){
            $scope.intervalleMax = $scope.intervalleMin
        }
    };

    $scope.intervalleMaxChanged = function(){
        if ($scope.intervalleMax < $scope.intervalleMin){
            $scope.intervalleMin = $scope.intervalleMax
        }
    };
});
