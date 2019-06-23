
function callAtTimeout() {
    console.log("Timeout occurred");
}

var quizCtrl = function ($scope, $http, helper, $interval, $window) {

    $scope.step = 1;

    $scope.createNew = function () {
        localStorage.removeItem("quiz");
        $scope.isNew = true;
        $scope.QuizEnded = false;
        $scope.elapsed = 0;
        $scope.time = 0;
    };

    if (localStorage.quiz) {
        $scope.isNew = false;
    } else {
        localStorage.setItem("quiz", "true");
        $scope.isNew = true;
    }


    $scope.loading = true;
    $scope.QuizEnded = false;


    /* $scope.quizName = '/api/quiz/get?id=1';*/// 'data/csharp.js';

    //Note: Only those configs are functional which is documented at: http://www.codeproject.com/Articles/860024/Quiz-Application-in-AngularJs
    // Others are work in progress.
    $scope.defaultConfig = {
        'allowBack': true,
        'allowReview': true,
        'autoMove': false,  // if true, it will move to next question automatically when answered.
        'duration': 0,  // indicates the time in which quiz needs to be completed. post that, quiz will be automatically submitted. 0 means unlimited.
        'pageSize': 1,
        'requiredAll': false,  // indicates if you must answer all the questions before submitting.
        'richText': false,
        'shuffleQuestions': false,
        'shuffleOptions': false,
        'showClock': false,
        'showPager': true,
        'theme': 'none'
    }

    $scope.goTo = function (index) {
        if (index > 0 && index <= $scope.totalItems) {
            $scope.currentPage = index;
            $scope.mode = 'quiz';
        }
    }

    $scope.onSelect = function (question, option) {
        if (question.QuestionTypeId == 1) {
            question.Options.forEach(function (element, index, array) {
                if (element.Id != option.Id) {
                    element.Selected = false;
                    //question.Answered = element.Id;
                }
            });
        }

        if ($scope.config.autoMove == true && $scope.currentPage < $scope.totalItems)
            $scope.currentPage++;
    }

    $scope.onSubmit = function () {
        $scope.focuscount = 4;
        var answers = [];
        var correctAnswers = 0;
        $scope.questions.forEach(function (q, index) {

            answers.push({ 'QuizId': $scope.quiz.Id, 'QuestionId': q.Id, 'Answered': q.Answered });
            if ($scope.isCorrect(q) === 'correct') {
                correctAnswers += 1;
            }
        });
        // alert(JSON.stringify(answers));
        // Post your data to the server here. answers contains the questionId and the users' answer.
        //$http.post('api/Quiz/Submit', answers).success(function (data, status) {
        //    alert(data);
        //});
        console.log($scope.questions);
        $scope.mode = 'result';

        //alert();

        // here we calc ScoredMarks : question lenght * correct answer ? 
        // we must here again calc
        $scope.ScoredMarks = correctAnswers * 10;

        $scope.PostQuiz();
    };

    $scope.pageCount = function () {
        return Math.ceil($scope.questions.length / $scope.itemsPerPage);
    };



    $scope.CountDown = {
        days: 0,
        hours: 0,
        minutes: 0,
        seconds: 0,
        getTimeRemaining: function (endtime) {
            var t = Date.parse(endtime) - Date.parse(new Date());
            var seconds = Math.floor((t / 1000) % 60);
            var minutes = Math.floor((t / 1000 / 60) % 60);
            var hours = Math.floor((t / (1000 * 60 * 60)) % 24);
            var days = Math.floor(t / (1000 * 60 * 60 * 24));
            return {
                'total': t,
                'days': days,
                'hours': hours,
                'minutes': minutes,
                'seconds': seconds
            };
        },

        initializeClock: function (endtime) {
            function updateClock() {
                var t = $scope.CountDown.getTimeRemaining(endtime);

                $scope.CountDown.days = t.days < 10 ? '0' + t.days : t.days;
                $scope.CountDown.hours = ('0' + t.hours).slice(-2);
                $scope.CountDown.minutes = ('0' + t.minutes).slice(-2);
                $scope.CountDown.seconds = ('0' + t.seconds).slice(-2);

                if (t.total <= 0) {
                    $interval.cancel(timeinterval);

                    $scope.onSubmit();

                }
            }

            updateClock();
            var timeinterval = $interval(updateClock, 1000);
        }
    }

    //var deadline = new Date(Date.parse(new Date()) + $scope.Mins * 60 * 1000);
    //$scope.CountDown.initializeClock(deadline);



    //If you wish, you may create a separate factory or service to call loadQuiz. To keep things simple, i have kept it within controller.
    $scope.loadQuiz = function (file) {
        // alert(file);
        $scope.loading = true;
        $http.get('/api/quiz/get?id=' + file)
            .then(function (res) {
                $scope.quiz = res.data.quiz;
                // alert(JSON.stringify(res.data));
                $scope.config = helper.extend({}, $scope.defaultConfig, res.data.config);
                $scope.questions = $scope.config.shuffleQuestions ? helper.shuffle(res.data.questions) : res.data.questions;
                $scope.totalItems = $scope.questions ? $scope.questions.length : 0;
                $scope.itemsPerPage = $scope.config.pageSize;
                $scope.currentPage = 1;
                $scope.mode = 'quiz';

                if ($scope.config.shuffleOptions)
                    $scope.shuffleOptions();

                $scope.$watch('currentPage + itemsPerPage', function () {
                    var begin = (($scope.currentPage - 1) * $scope.itemsPerPage),
                        end = begin + $scope.itemsPerPage;

                    $scope.filteredQuestions = $scope.questions.slice(begin, end);
                });


                $scope.Mins = 10;// Set your time here

                var deadline = new Date(Date.parse(new Date()) + $scope.Mins * 60 * 1000);

                $scope.CountDown.initializeClock(deadline);

                $scope.loading = false;

                $scope.focuscount = 0;

                $scope.step = 3;

            });
    };

    $scope.GetCourse = function () {
        $http.get('/api/quiz/GetCourse')
            .then(function (res) {
                $scope.courses = res.data;
            });
    }

    $scope.GetSemester = function () {
        $http.get('/api/quiz/GetSemester')
            .then(function (res) {
                $scope.semester = res.data;
            });
    }


    $scope.GetSubject = function () {
        $http.get('/api/quiz/GetSubject')
            .then(function (res) {
                $scope.subjects = res.data;

            });
    }

    $scope.GetUnit = function () {
        $http.get('/api/quiz/GetUnit')
            .then(function (res) {
                $scope.units = res.data;

            });
    }
    $scope.unitid = 0;
    $scope.subjectid = 0;
    $scope.ScoredMarks = 0;
    $scope.TotalMarks = 0;
    $scope.quizname = 0;
    $scope.GetQuiz = function () {
        // alert($scope.unitid);
        if ($scope.unitid > 0 && $scope.subjectid > 0) {

            $http.get('/api/quiz/Gets?subject=' + $scope.subjectid + '&unit=' + $scope.unitid)
                .then(function (res) {
                    $scope.quizs = res.data;
                });
        }
    }

    $scope.DrpChange = function (id) {

        $scope.Drp = id;
    };


    $scope.ChangeStep = function (id) {
        $scope.step = id;
        if ($scope.step === 2)
            $scope.GetQuiz();
    };


    $scope.PostQuiz = function () {
        $scope.score = {
            "SubjectID": $scope.subjectid,
            "CourseID": 0,
            "UnitID": $scope.unitid,
            "ScoredMarks": $scope.ScoredMarks,
            "TotalMarks": $scope.TotalMarks,
            "No_Attempts": 0
        };
        var model = $.param($scope.score);
        $http.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
        $http({
            method: 'POST',
            url: '/api/quiz/SaveScore',
            data: model,
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).then(function (data, status, headers, config) {
            // handle success things
            // $scope.notifies = JSON.parse(data.data.Json);
            alert("Your Test Submitted Sucessfully");
        }).catch(function (data, status, headers, config) {
            // handle error things
            console.log(data);
        });

    };

    $scope.shuffleOptions = function () {
        $scope.questions.forEach(function (question) {
            question.Options = helper.shuffle(question.Options);
        });
    };

    // $scope.loadQuiz($scope.quizName);

    $scope.isAnswered = function (index) {
        var answered = 'Not Answered';
        $scope.questions[index].Options.forEach(function (element, index, array) {
            if (element.Selected == true) {
                answered = 'Answered';
                return false;
            }
        });
        return answered;
    };

    $scope.isCorrect = function (question) {
        var result = 'correct';
        question.Options.forEach(function (option, index, array) {
            if (helper.toBool(option.Selected) != option.IsAnswer) {
                result = 'wrong';
                return false;
            } else {
                $scope.ScoredMarks += 1;
            }
            $scope.TotalMarks += 1;
        });
        return result;
    };

    $scope.CloseSession = function () {
        $scope.mode = 'rating';
    }


    /////////////////////////////////


    $scope.rating = 0;
    $scope.ratings = [{
        current: 3,
        max: 5
    }];

    $scope.getSelectedRating = function (rating) {
        console.log(rating);
    }

    $scope.setMinrate = function () {
        $scope.ratings = [{
            current: 1,
            max: 5
        }];
    }


    $scope.setMaxrate = function () {
        $scope.ratings = [{
            current: 5,
            max: 5
        }];
    }

    $scope.sendRate = function () {
        //alert("Thanks for your valuable rating")
        $scope.ratingmodel = {
            "Rating": $scope.ratings[0].current,
            "CourseID": $scope.CourseID
        }

        var model1 = $.param($scope.ratingmodel);
        $http.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';

        $http({
            method: 'Post',
            url: '/api/quiz/SaveRating',
            data: model1,
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).then(function (data, status, headers, config) {
            alert("Rating submitted Sucessfully \n Thanks for your valuable rating!!!");
            window.location.href = ("http://ireadytoday.shlrtechnosoft.net/Student");
        }).catch(function (data, status, headers, config) {
            console.log(data);
        });

    }

   

    window.onblur = function (e) {
        $scope.focuscount += 1;

        if ($scope.focuscount == 1) {
            alert("You tried to change tab. If you change tab one more time your session will be closed");
        }
        else if ($scope.focuscount == 2) {
            alert("Last Warning");
        }
        else if ($scope.focuscount == 3) {
            $scope.onSubmit();            
            e.preventDefault();
        }
    }

};

quizCtrl.$inject = ['$scope', '$http', 'helperService', '$interval', '$window'];
app.controller('quizCtrl', quizCtrl);



app.directive('starRating', function () {
    return {
        restrict: 'A',
        template: '<ul class="rating">' +
            '<li ng-repeat="star in stars" ng-class="star" ng-click="toggle($index)">' +
            '\u2605' +
            '</li>' +
            '</ul>',
        scope: {
            ratingValue: '=',
            max: '=',
            onRatingSelected: '&'
        },
        link: function (scope, elem, attrs) {

            var updateStars = function () {
                scope.stars = [];
                for (var i = 0; i < scope.max; i++) {
                    scope.stars.push({
                        filled: i < scope.ratingValue
                    });
                }
            };

            scope.toggle = function (index) {
                scope.ratingValue = index + 1;
                scope.onRatingSelected({
                    rating: index + 1
                });
            };

            scope.$watch('ratingValue', function (oldVal, newVal) {
                if (newVal) {
                    updateStars();
                }
            });
        }
    }
});


