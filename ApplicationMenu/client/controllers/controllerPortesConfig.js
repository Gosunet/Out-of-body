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
		$scope.nbTailleLargeur =Number(window.localStorage["local_nbTailleLargeur"]) | 1;
		$scope.diffTailleLargeur =Number(window.localStorage["local_diffTailleLargeur"]) | 0;
		$scope.nbTailleHauteur = Number(1) ;
		$scope.diffTailleHauteur = Number(0);
		$scope.nbRepet =Number(window.localStorage["local_nbRepet"]) | 0;
	};
	initHauteur = function() {
		$scope.nbTailleHauteur =Number(window.localStorage["local_nbTailleHauteur"]) | 1;
		$scope.diffTailleHauteur =Number(window.localStorage["local_diffTailleHauteur"]) | 0;
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
	$scope.$watch('nbRepet + nbTailleLargeur + nbTailleHauteur + diffTailleLargeur + diffTailleHauteur', function() {
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
		else if ($scope.nbEssai > 0 & $scope.nbTailleLargeur > 1 & $scope.diffTailleLargeur != 0){
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
		else if ($scope.nbTailleHauteur > 1 & $scope.diffTailleHauteur != 0)
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

		var message = type + '/' + $scope.nbRepet + '_' + $scope.nbTailleLargeur + '_' + $scope.diffTailleLargeur + '_' + $scope.nbTailleHauteur + '_' + $scope.diffTailleHauteur;
		$http.get(message);
		$state.go('runPortes');
	};

	// Save function : save all value in browser local storage
	saveAllValues=function() {
		window.localStorage["local_nbRepet"] = $scope.nbRepet;
		window.localStorage["local_nbTailleLargeur"] = $scope.nbTailleLargeur;
		window.localStorage["local_diffTailleLargeur"] = $scope.diffTailleLargeur;
		if($scope.hauteur){
			window.localStorage["local_nbTailleHauteur"] = $scope.nbTailleHauteur;
			window.localStorage["local_diffTailleHauteur"] = $scope.diffTailleHauteur;
		}
	};

	// Previous function : go back to door type choice
	$scope.previous = function () {
		$state.go('portes');
	}
});