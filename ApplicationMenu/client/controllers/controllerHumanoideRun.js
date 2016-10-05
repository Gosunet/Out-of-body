menu.controller('HumanoideRunCtrl', function ($scope, $state, $http, $rootScope, $document, $timeout) {
    // Breadcrumb settings
    $rootScope.chemin = 'Accueil';
    $rootScope.chemin2 = 'Application en cours';
    $rootScope.stateChemin2 = 'runHumanoide';
    $rootScope.suivant = false;

    //Pool function : ask to server if exercice is done, in this case, go to menu automatically
    var pool = function () {
        if ($state.current.name !== 'runHumanoide')
            return;
        $http.get('/humanoide').then(function (resp) {
            $state.go('mainMenu');
            return;
        });
        // ask every 200 ms!
        $timeout(pool, 200);
    }

    // Stop backspace key event
    $document.on('keydown', function (e) {
        if (e.which === 8) {
            e.preventDefault();
        }
    });

    // Call function to start communication with server
    pool();

    // End function on "go to menu" button click : send "stop" to finish Unity's exercice AND go to menu state
    $scope.end_click = function () {
        $http.get('stop');
        $state.go('mainMenu');
    };
});