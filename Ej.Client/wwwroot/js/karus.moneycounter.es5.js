'use strict';

document.addEventListener('DOMContentLoaded', function () {

    var moneyCounterEl = document.getElementById("karus_moneycounter");
    if (!moneyCounterEl) {
        return;
    }

    // Parameters
    var startDateSmoking = new Date('2024-12-31T00:00:00'); // Your no-smoking start date
    var startDateDrinking = new Date('2024-11-24T19:00:00'); // Your no-drinking start date
    var dailyExpenseSmoking = 10; // Amount spent on smoking daily in Euros
    var dailyExpenseDrinking = 15; // Amount spent on drinking daily in Euros
    var animationDuration = 0.6; // Animation duration in seconds

    // Get elements
    var smokingImage = document.querySelector('#no-smoking img');
    var drinkingImage = document.querySelector('#no-drinking img');

    var smokingTimeElem = document.querySelector('#no-smoking .time');
    var smokingEuroElem = document.querySelector('#no-smoking .euro');

    var drinkingTimeElem = document.querySelector('#no-drinking .time');
    var drinkingEuroElem = document.querySelector('#no-drinking .euro');

    var totalEuroElem = document.querySelector('.total'); // Grand total element

    // Calculate total seconds and money saved
    var now = new Date();

    // Smoking
    var smokingData = getElapsedData(startDateSmoking, now, dailyExpenseSmoking);
    var totalSecondsSmoking = smokingData.totalSeconds;
    var totalMoneySmoking = smokingData.moneySaved;

    // Drinking
    var drinkingData = getElapsedData(startDateDrinking, now, dailyExpenseDrinking);
    var totalSecondsDrinking = drinkingData.totalSeconds;
    var totalMoneyDrinking = drinkingData.moneySaved;

    // Grand Total
    var totalMoneySaved = totalMoneySmoking + totalMoneyDrinking;

    // Animate image rotations (4 full rotations)
    gsap.to(smokingImage, {
        rotation: 360 * 4,
        duration: animationDuration,
        ease: "linear"
    });

    gsap.to(drinkingImage, {
        rotation: 360 * 4,
        duration: animationDuration,
        ease: "linear"
    });

    // Create counter objects
    var smokingTimeCounter = { val: 0 };
    var smokingMoneyCounter = { val: 0 };
    var drinkingTimeCounter = { val: 0 };
    var drinkingMoneyCounter = { val: 0 };
    var totalMoneyCounter = { val: 0 };

    // Animate smoking time counter
    gsap.to(smokingTimeCounter, {
        val: totalSecondsSmoking,
        duration: animationDuration,
        ease: "power1.out",
        onUpdate: function onUpdate() {
            var elapsedSmoking = formatElapsedTime(Math.floor(smokingTimeCounter.val));
            smokingTimeElem.textContent = elapsedSmoking;
        }
    });

    // Animate smoking money counter
    gsap.to(smokingMoneyCounter, {
        val: totalMoneySmoking,
        duration: animationDuration,
        ease: "power1.out",
        onUpdate: function onUpdate() {
            smokingEuroElem.textContent = '€ ' + formatNumber(smokingMoneyCounter.val);
        }
    });

    // Animate drinking time counter
    gsap.to(drinkingTimeCounter, {
        val: totalSecondsDrinking,
        duration: animationDuration,
        ease: "power1.out",
        onUpdate: function onUpdate() {
            var elapsedDrinking = formatElapsedTime(Math.floor(drinkingTimeCounter.val));
            drinkingTimeElem.textContent = elapsedDrinking;
        }
    });

    // Animate drinking money counter
    gsap.to(drinkingMoneyCounter, {
        val: totalMoneyDrinking,
        duration: animationDuration,
        ease: "power1.out",
        onUpdate: function onUpdate() {
            drinkingEuroElem.textContent = '€ ' + formatNumber(drinkingMoneyCounter.val);
        }
    });

    // Animate total money counter
    gsap.to(totalMoneyCounter, {
        val: totalMoneySaved,
        duration: animationDuration,
        ease: "power1.out",
        onUpdate: function onUpdate() {
            totalEuroElem.textContent = '€ ' + formatNumber(totalMoneyCounter.val);
        },
        onComplete: function onComplete() {
            // Start regular updates after the initial animation completes
            setInterval(updateCounter, 1000);
        }
    });

    function updateCounter() {
        now = new Date();

        // Smoking
        smokingData = getElapsedData(startDateSmoking, now, dailyExpenseSmoking);
        totalSecondsSmoking = smokingData.totalSeconds;
        totalMoneySmoking = smokingData.moneySaved;

        smokingTimeElem.textContent = formatElapsedTime(totalSecondsSmoking);
        smokingEuroElem.textContent = '€ ' + formatNumber(totalMoneySmoking);

        // Drinking
        drinkingData = getElapsedData(startDateDrinking, now, dailyExpenseDrinking);
        totalSecondsDrinking = drinkingData.totalSeconds;
        totalMoneyDrinking = drinkingData.moneySaved;

        drinkingTimeElem.textContent = formatElapsedTime(totalSecondsDrinking);
        drinkingEuroElem.textContent = '€ ' + formatNumber(totalMoneyDrinking);

        // Grand Total
        totalMoneySaved = totalMoneySmoking + totalMoneyDrinking;
        totalEuroElem.textContent = '€ ' + formatNumber(totalMoneySaved);
    }

    function getElapsedData(startDate, now, dailyExpense) {
        var totalSeconds = 0;
        var moneySaved = 0;

        if (now >= startDate) {
            totalSeconds = Math.floor((now - startDate) / 1000);
            var totalDays = (now - startDate) / (1000 * 3600 * 24);
            moneySaved = totalDays * dailyExpense;
        } else {
            // Start date is in the future
            totalSeconds = 0;
            moneySaved = 0;
        }

        return {
            totalSeconds: totalSeconds,
            moneySaved: moneySaved
        };
    }

    function formatElapsedTime(totalSeconds) {
        var days = Math.floor(totalSeconds / (3600 * 24));
        totalSeconds %= 3600 * 24;

        var hours = Math.floor(totalSeconds / 3600);
        totalSeconds %= 3600;

        var minutes = Math.floor(totalSeconds / 60);
        var seconds = totalSeconds % 60;

        return days + ' days ' + pad(hours) + ':' + pad(minutes) + ':' + pad(seconds);
    }

    function pad(num) {
        return num < 10 ? '0' + num : num;
    }

    function formatNumber(num) {
        return num.toFixed(2).replace('.', ',');
    }
});

