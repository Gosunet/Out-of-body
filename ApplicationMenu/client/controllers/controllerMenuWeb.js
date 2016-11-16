var modules = ['ui.router', 'ngStorage', 'ngMaterial'];
var menu = angular.module('menu', modules);

menu.controller('menuWebCtrl', function ($scope, $state, $rootScope, $http) {
	$state.go('mainMenu');
});

// States configurations
menu.config(function($stateProvider, $urlRouterProvider){
	$stateProvider
	// Menu part
	.state('mainMenu',{
		url: "",
		templateUrl: "client/templates/mainMenu.html"
	})
	// Out of body part
	.state('oob',{
		url: "/sortie_de_corps",
		templateUrl: "client/templates/oob.html"
	})
	.state('runOob',{
			url: "/sortie_de_corps/application_en_cours",
			templateUrl: "client/templates/runOob.html"
	})
	// Avatar part
	.state('avatar',{
		url: "/avatar",
		templateUrl: "client/templates/avatar.html"
	})
	.state('avatar_choix',{
		url: "/avatar/choix",
		templateUrl: "client/templates/avatarMedecin.html"
	})
	// Doors part
	.state('portes',{
		url: "/portes",
		templateUrl: "client/templates/portes.html"
	})
	.state('entiere',{
		url: "/portes/entiere",
		templateUrl: "client/templates/portesConfig.html"
	})
	.state('demi_haut',{
		url: "/portes/demi_haut",
		templateUrl: "client/templates/portesConfig.html"
	})
	.state('demi_bas',{
		url: "/portes/demi_bas",
		templateUrl: "client/templates/portesConfig.html"
	})
	.state('runPortes',{
		url: "/portes/en_cours",
		templateUrl: "client/templates/runPortes.html"
	})
    // Humanoides part
    .state('humanoidesConfig', {
        url : "/humanoides",
        templateUrl: "client/templates/humanoides.html"
    })
    .state('runHumanoide', {
        url : "/humanoides/en_cours",
        templateUrl: "client/templates/runHumanoide.html"
    })
    .state('humanoides', {
        url : "/humanoides/humanoides",
        templateUrl: "client/templates/humanoidesConfig.html"
    })
    .state('batons', {
        url : "/humanoides/batons",
        templateUrl: "client/templates/humanoidesConfig.html"
    })
    
});