menu.controller('HumanoidesConfigCtrl', function($scope, $state, $http, $rootScope, $localStorage){
    $rootScope.chemin = 'Accueil';
    $rootScope.chemin1 = 'Exercice des humanoïdes';
    $rootScope.stateChemin1 = $state.current.name;
});
