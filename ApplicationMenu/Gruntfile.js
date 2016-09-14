module.exports = function(grunt) {

  // Configuration de Grunt
  grunt.initConfig({
    concat: {
      options: {
        separator: ';', // permet d'ajouter un point-virgule entre chaque fichier concaténé.
      },
      compile: {
        src: ['client/controllers/controllerMenuWeb.js','client/controllers/controllerMainMenu.js','client/controllers/*.js'], // la source
        dest: 'build/build.min.js' // la destination finale
      }
    },
    uglify: {
      options: {
        separator: ';',
        mangle: false, // si false : enleve les changements de nom de variable
      },
      dist: {
        src: ['client/controllers/controllerMenuWeb.js','client/controllers/controllerMainMenu.js','client/controllers/*.js'],
        dest: 'build/build.min.js'
      }
    },
    watch: {
      scripts: {
        files: 'client/controllers/*.js', //'client/styles/*.css', // tous les fichiers JavaScript du dossier src
        tasks: ['uglify:dist,cssmin']
      }
    },
    cssmin: {
      options: {
        shorthandCompacting: false,
        roundingPrecision: -1
      },
      target: {
        files: {
          'build/build.min.css': ['client/styles/*.css']
        }
      }
    }
})

  // Définition des tâches Grunt
  grunt.loadNpmTasks('grunt-contrib-concat')
  grunt.loadNpmTasks('grunt-contrib-uglify')
  grunt.loadNpmTasks('grunt-contrib-watch')
  grunt.loadNpmTasks('grunt-contrib-cssmin');

  grunt.registerTask('default', ['dist','cssmin','watch'])
  grunt.registerTask('dev', ['concat:compile'])
  grunt.registerTask('dist', ['uglify:dist'])
  grunt.registerTask('style', ['cssmin'])
}