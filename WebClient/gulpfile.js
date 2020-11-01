
var gulp = require("gulp");
var sass = require("gulp-sass");
var sourcemaps = require("gulp-sourcemaps");

var libPaths = {
    bootstrap: ["bower_components/lib/bootstrap-sass/**/*.js", "bower_components/libs/bootstrap-sass/**/*.scss", "bower_components/libs/bootstrap-sass/**/*.eot", "bower_components/lib/bootstrap-sass/**/*.svg", "bower_components/lib/bootstrap-sass/**/*.ttf", "bower_components/lib/bootstrap-sass/**/*.woff", "bower_components/lib/bootstrap-sass/**/*.woff2"],
    jquery: ["bower_components/lib/jquery/**/*.min.js"]
};

gulp.task("default", function() {
});

gulp.task("copyLibs", function() {
    gulp.src(libPaths.bootstrap).pipe(gulp.dest("assets/libs/bootstrap"));
    gulp.src(libPaths.jquery).pipe(gulp.dest("assets/libs/jquery"));
});

var styleInput = "assets/**/*.scss";
var styleOutput = "assets/**/*.css";

gulp.task("sass", function() {
    return gulp.src("assets/**/*.scss")
        .pipe(sourcemaps.init())
        .pipe(sass({ outputStyle: "compressed" }).on("error", sass.logError))
        .pipe(sourcemaps.write())
        .pipe(gulp.dest(styleOutput));
});

gulp.task("sass:watch", function() {
    gulp.watch(styleInput, ["sass"]);
});