menu.controller('mainMenuCtrl', function ($scope, $state, $rootScope, $http) {
    // Breadcrumb settings
    $rootScope.chemin = 'Accueil';
    $rootScope.chemin1 = '';
    $rootScope.chemin2 = '';
    $rootScope.chemin3 = '';
    $rootScope.chemin4 = '';
    $rootScope.suivant = true;

    // Exit function when button "quit app" clicked : send "/exit" to server to close all programs
    $scope.exit = function () {
        $http.get('/exit');
    }

    // Exit function on breadcrumb click when user is on running app : send "stop" to server to finish running exercice
    $rootScope.exit = function (state) {
        $http.get('stop');
        if (state)
            $state.go(state);
        else
            $state.go('mainMenu');
    }
});