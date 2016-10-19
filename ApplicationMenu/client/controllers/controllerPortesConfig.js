menu.controller('PortesConfigCtrl', function ($scope,$state,$http, $rootScope, $localStorage) {
	// Breadcrumb settings
	$rootScope.chemin = 'Accueil';
	$rootScope.chemin1 = 'Exercice des portes';
	$rootScope.stateChemin1 = 'portes';
	if($state.current.name == 'entiere')
		$rootScope.chemin2 = 'Portes entières'
	else if($state.current.name == 'demi_haut')
		$rootScope.chemin2 = 'Étagères';
	else	$rootScope.chemin2 = 'Portiques';
	$rootScope.stateChemin2 = $state.current.name;
	$rootScope.chemin3 = '';
	$rootScope.suivant = true;

	// Initialistion of numbers : load last values or default values
	initDemiePorte = function() {
		$scope.nbTailleLargeur =Number(window.localStorage["local_nbTailleLargeur"]);
		$scope.nbTailleHauteur = Number(1) ;
		$scope.largeurMin = Number(window.localStorage["local_largeurMin"]);
        $scope.largeurMax = Number(window.localStorage["local_largeurMax"]);
        $scope.hauteurMin = Number(window.localStorage["local_hauteurMin"]);
        $scope.hauteurMax = Number(window.localStorage["local_hauteurMax"]);
		$scope.nbRepet =Number(window.localStorage["local_nbRepet"]) | 0;
	};
	initHauteur = function() {
		$scope.nbTailleHauteur =Number(window.localStorage["local_nbTailleHauteur"]);
		$scope.hauteurMin = Number(window.localStorage["local_hauteurMin"]) | 0;
        $scope.hauteurMax = Number(window.localStorage["local_hauteurMax"]) | 0;
	};

	// Determine if we are with full doors or only half-doors AND call init function related
	if($state.current.name == "entiere"){
		$scope.hauteur = true;
		initDemiePorte();
		initHauteur();
	}
	else{
		$scope.hauteur = false;
		initDemiePorte();
	}

	// Watch function to update result value and save all values
	$scope.$watch('nbRepet + nbTailleLargeur + nbTailleHauteur + largeurMin + largeurMax + hauteurMin + hauteurMax', function() {
		$scope.nbEssai = ($scope.nbTailleLargeur * $scope.nbTailleHauteur) * $scope.nbRepet;
		saveAllValues();
	});

	// Function exectute click on "run exercice" button click : check if all values are corrects
	$scope.executer_click = function(){
		$scope.informationsDonnees ="";
		if ($scope.nbEssai > 0 & $scope.nbTailleLargeur == 1){
			if($scope.hauteur)
				testerH();
			else
				sendMessage();
		}
		else if ($scope.nbEssai > 0 & $scope.nbTailleLargeur > 1 & $scope.largeurMin >= 1 & $scope.largeurMax >= 1){
			if($scope.hauteur)
				testerH();
			else
				sendMessage();
		}
		else
			$scope.informationsDonnees = "Tous les champs ne sont pas remplis";
	}

	// Function to check if height values are corrects
	testerH = function(){
		if($scope.nbTailleHauteur == 1)
			sendMessage();
		else if ($scope.nbTailleHauteur > 1 & $scope.hauteurMin >= 1 & $scope.hauteurMax >= 1)
			sendMessage();
		else
			$scope.informationsDonnees = "Tous les champs ne sont pas remplis";
	}

	// Function to send values to server AND go to running exercice state
	var sendMessage = function () {
		var type ="";
		if($state.current.name == "entiere")
			type ="e";
		else if ($state.current.name == "demi_haut")
			type ="dh";
		else
			type = "db";

		var message = type + '/' + $scope.nbRepet + '_' + $scope.nbTailleLargeur + '_' + $scope.largeurMin + '_' + $scope.largeurMax + '_' + $scope.nbTailleHauteur + '_' + $scope.hauteurMin + '_' + $scope.hauteurMax;
		$http.get(message);
		$state.go('runPortes');
	};

	// Save function : save all value in browser local storage
	saveAllValues=function() {
		window.localStorage["local_nbRepet"] = $scope.nbRepet;
		window.localStorage["local_nbTailleLargeur"] = $scope.nbTailleLargeur;
		window.localStorage["local_largeurMin"] = $scope.largeurMin;
        window.localStorage["local_largeurMax"] = $scope.largeurMax;
		if($scope.hauteur){
			window.localStorage["local_nbTailleHauteur"] = $scope.nbTailleHauteur;
			window.localStorage["local_hauteurMin"] = $scope.hauteurMin;
            window.localStorage["local_hauteurMax"] = $scope.hauteurMax;
		}
	};

	// Previous function : go back to door type choice
	$scope.previous = function () {
		$state.go('portes');
	};
    
    $scope.largeurMinChanged = function () {
        if ($scope.largeurMin > $scope.largeurMax){
            $scope.largeurMax = $scope.largeurMin
        }
    };
    
    $scope.largeurMaxChanged = function () {
        if ($scope.largeurMax < $scope.largeurMin){
            $scope.largeurMin = $scope.largeurMax
        } 
    };
    
    $scope.hauteurMaxChanged = function () {
        if ($scope.hauteurMax < $scope.hauteurMin){
            $scope.hauteurMin = $scope.hauteurMax
        } 
    };
    
    $scope.hauteurMinChanged = function () {
        if ($scope.hauteurMin > $scope.hauteurMax){
            $scope.hauteurMax = $scope.hauteurMin
        } 
    };
    
    
});