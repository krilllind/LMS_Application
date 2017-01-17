(function () {
    var ScheduleProvider = function ($interval) {
        var canvas,
            ctx,
            textHeight;

        var canvasCurrentWidthCheck = 0;

        var numberOfColumns,
            dayStartsAtSunday,
            dayStartsAt,
            dayEndsAt,
            allLessons,
            titles;

        var canvasContentWidth,
            canvasContentHeight,
            canvasContentOffset,
            canvasHeadlineHeight;

        var headlineFont,
            timelineFont,
            lessonFont,
            timestampFont;

        var lineColor = "#000000",
            textColor = "#000000",
            bgColor = "#CCCCCC",
            timeBoxbg = "#FFFFFF";

        // Used for clearing the canvas //
        var clear = function () {
            ctx.clearRect(0, 0, canvas.width, canvas.height);
        }

        var setBgColor = function (color) {
            ctx.fillStyle = color;
            ctx.fillRect(canvasContentOffset, canvasHeadlineHeight, canvasContentWidth, canvasContentHeight);
        }

        var setSize = function () {
            if (canvasCurrentWidthCheck == canvas.clientWidth)
                return;

            canvasCurrentWidthCheck = canvas.clientWidth
            ctx.canvas.width = canvas.clientWidth;
            ctx.canvas.height = canvas.clientHeight;

            canvasContentOffset = (canvas.clientWidth / 10);
            canvasContentWidth = canvas.clientWidth - canvasContentOffset;
            canvasContentHeight = canvas.clientHeight;
            canvasHeadlineHeight = canvas.clientHeight / 16;

            if (canvas.clientWidth <= 480)
                textHeight = ctx.measureText("_").width / 2;
            else
                textHeight = ctx.measureText("_").width;

            // Set text font size relative canvas width //
            if (canvas.clientWidth >= 780) {
                headlineFont = (canvasContentWidth / numberOfColumns) / 9;
                timelineFont = canvasContentOffset / 7;
                lessonFont = (canvasContentWidth / numberOfColumns) / 12;
                timestampFont = 12;
            }
            if (canvas.clientWidth < 780) {
                headlineFont = (canvasContentWidth / numberOfColumns) / 8;
                timelineFont = canvasContentOffset / 6;
                lessonFont = (canvasContentWidth / numberOfColumns) / 10;
                timestampFont = 10;
            }
            if (canvas.clientWidth < 480) {
                headlineFont = (canvasContentWidth / numberOfColumns) / 6;
                timelineFont = canvasContentOffset / 4;
                lessonFont = (canvasContentWidth / numberOfColumns) / 8;
                timestampFont = 8;
            }

            // Clear the canvas //
            clear();

            // Set background color //
            setBgColor(bgColor);

            // Call draw method to sync //
            draw();
        }

        // Draw all the data to the canvas //
        var draw = function () {

            // Draw Timeline //
            (function Timeline() {

                // Show every 30min //
                var interval = 30;

                // Space between timestamps and first column //
                var space = 5;

                ctx.font = timelineFont + "px Arial";

                var index = 0;
                var dayLength = GetTimespanInMinutes(dayStartsAt, dayEndsAt);
                var height = Math.ceil((canvasContentHeight - canvasHeadlineHeight) / (dayLength / interval));

                for (var i = 0; i < dayLength; i += interval) {

                    var time = GetTimeInHoursMinutes(i);
                    var w = canvasContentOffset;
                    var h = height * index + canvasHeadlineHeight;

                    ctx.fillStyle = textColor;
                    ctx.fillText(time, w - ctx.measureText(time).width - (space * 1.5), h - 2);

                    ctx.fillStyle = lineColor;
                    ctx.moveTo(w, h);
                    ctx.lineTo(w - ctx.measureText(time).width - (space * 1.5), h);

                    index += 1;
                }

                ctx.stroke();
            }());

            // Draw headers //
            (function Headers() {
                ctx.lineWidth = 1;
                ctx.strokeStyle = lineColor;

                var columnWidth = canvasContentWidth / numberOfColumns;

                ctx.moveTo(canvasContentOffset, canvasHeadlineHeight);
                ctx.lineTo(canvas.width + canvasContentOffset, canvasHeadlineHeight);

                ctx.fillStyle = textColor;
                ctx.font = headlineFont + "px Georgia";

                for (var i = 0; i < titles.length; i++) {
                    var textWidth = ctx.measureText(titles[i]).width;

                    var w = (columnWidth * i + (columnWidth / 2) + canvasContentOffset) - (textWidth / 2);
                    var h = (canvasHeadlineHeight / 2) + textHeight;

                    ctx.fillText(titles[i], w, h);
                }

                ctx.stroke();
            }());

            // Draw the lessons //
            (function Lessons() {
                var columnWidth = canvasContentWidth / numberOfColumns;
                var dayLength = GetTimespanInMinutes(dayStartsAt, dayEndsAt) + 60;

                for (var i = 0; i < titles.length; i++) {
                    angular.forEach(allLessons, function (value, key) {
                        if (String(value.day).toLowerCase() == String(titles[i]).toLowerCase()) {

                            // Draw lesson box //
                            ctx.fillStyle = value.color;

                            var x = (columnWidth * i) + canvasContentOffset;
                            var y = ((ConvertHoursToMinutes(value.from) - ConvertHoursToMinutes(dayStartsAt)) / dayLength) * canvasContentHeight + canvasHeadlineHeight;
                            var h = (GetTimespanInMinutes(value.from, value.to) / dayLength) * canvasContentHeight;

                            ctx.fillRect(x, y, columnWidth, h);

                            ctx.strokeStyle = "rgba(0, 0, 0, 0.15)";
                            ctx.strokeRect(x, y, columnWidth, h);

                            // Fill box with information //
                            ctx.fillStyle = lineColor;
                            ctx.font = lessonFont + "px Arial";
                            ctx.textAlign = "center";

                            var x1 = x + (columnWidth / 2);
                            var y1 = y + (h / 2) - textHeight;
                            var x2 = x + (columnWidth / 2);
                            var y2 = y1 + (textHeight * 3.5);

                            var lessonTypeTeacher = value.subject + " " + value.courseLevel + ", " + value.teacher;
                            var classroom = value.classroom;

                            ctx.fillText(lessonTypeTeacher, x1, y1);
                            ctx.fillText(classroom, x2, y2);

                            // Draw timestamps on top of box //
                            ctx.font = timestampFont + "px Arial";
                            ctx.textAlign = "left";

                            var h = textHeight + 3;
                            var w = ctx.measureText(value.from).width;

                            ctx.fillStyle = timeBoxbg;
                            ctx.fillRect(x, y, w, h);
                            ctx.fillStyle = textColor;
                            ctx.fillText(value.from, x, (y + h));

                            // Draw timestamps on bottom of box //
                            var h = ((GetTimespanInMinutes(value.from, value.to) / dayLength) * canvasContentHeight);
                            var w = ctx.measureText(value.to).width;

                            ctx.fillStyle = timeBoxbg;
                            ctx.fillRect(x + columnWidth - w, (y + h) - (textHeight + 3), w, (textHeight + 3));
                            ctx.fillStyle = textColor;
                            ctx.fillText(value.to, (x + columnWidth - w), (y + h));
                        }
                    });
                }
            }());

            // Draw board //
            (function Board() {
                ctx.lineWidth = 1;
                ctx.strokeStyle = lineColor;

                var columnWidth = canvasContentWidth / numberOfColumns;

                for (var i = 0; i < numberOfColumns; i++) {
                    ctx.moveTo(columnWidth * i + canvasContentOffset, 0);
                    ctx.lineTo(columnWidth * i + canvasContentOffset, canvas.height);
                }

                ctx.stroke();
            })();
        }

        // Add lessons to the schedule //
        var addLessons = function (lessons) {
            angular.forEach(lessons, function (value, key) {
                allLessons.push(value);
            });
        }

        // Initialize the canvas settings //
        var init = function (canvasObj, boardColumns, startAtSunday, dayStarts, dayEnds, lessons) {
            titles = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

            canvasObj = canvasObj || null;

            if (canvasObj) {
                canvas = canvasObj;
                ctx = canvas.getContext("2d");
                textHeight = ctx.measureText("_").width;
            } else return null;

            if (boardColumns > 0 && boardColumns <= 7)
                numberOfColumns = boardColumns;
            else
                numberOfColumns = 7;

            dayStartsAtSunday = ((startAtSunday === true) ? true : false);
            dayStartsAt = dayStarts;
            dayEndsAt = dayEnds;
            allLessons = lessons || Array(0);

            if (dayStartsAtSunday) {
                var day = titles.splice(-1, 1);
                titles.unshift(day);
            }

            // Add eventlistener for canvas resize //
            $interval(setSize, 500);
        }

        // Get number of minutes between two times //
        function GetTimespanInMinutes(from, to) {
            var dayStartsAtMinutes = ConvertHoursToMinutes(from);
            var dayEndssAtArrMinutes = ConvertHoursToMinutes(to);

            return Math.abs(Math.floor(dayEndssAtArrMinutes - dayStartsAtMinutes));
        }

        // Convert time of type string to minutes type int //
        function ConvertHoursToMinutes(hoursToConvert) {
            var tmp = (hoursToConvert != null) ? String(hoursToConvert).split(":") : String("00:00").split(":");

            if ((+tmp[0]) > 0) {
                tmp[0] *= 60;
            }

            return (+tmp[0]) + (+tmp[1]);
        }

        // Convert minutes type int to time type string //
        function GetTimeInHoursMinutes(minutesToConvert) {
            minutesToConvert += ConvertHoursToMinutes(dayStartsAt);

            var realMin = minutesToConvert % 60;
            var hours = Math.floor(minutesToConvert / 60);

            return ((String(hours).length == 2) ? hours : "0" + hours) + ":" + ((String(realMin).length == 2) ? realMin : realMin + "0");
        }

        return {
            Initialize: init,
            UpdateCanvasSize: setSize,
            Draw: draw,
            Clear: clear,
            element: canvas,
            AddLessons: addLessons
        };
    }

    LMSApp.factory("Schedule", [
        "$interval",
        ScheduleProvider
    ]);

}());