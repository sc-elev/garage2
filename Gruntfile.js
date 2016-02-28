'use strict';

module.exports = function (grunt) {
    grunt.initConfig({
        nggettext_extract: {
            pot: {
                files: {
//                    'po/template.pot': ['MittGarage/Views/Items/Admin.cshtml']
                      'po/template.pot': ['kalle.html']
                }
            },
        },

        nggettext_compile: {
            all: {
                files: {
                    'MittGarage/Scripts/translations.js': ['po/*.po']
                }
            },
        },
    });

    grunt.loadNpmTasks('grunt-angular-gettext');

    grunt.registerTask('default', ['nggettext_extract', 'nggettext_compile']);
//    grunt.registerTask('default', ['nggettext_extract']);
};
