window.onload = function () {
    initLocalClocks();
    setUpMinuteHands();
    moveSecondHands();
};

function initLocalClocks() {
    // Get the local time using JS
    var date = new Date();
    var seconds = date.getSeconds();
    var minutes = date.getMinutes();
    var hours = date.getHours();

    // Create an object with each hand and it's angle in degrees
    var hands = [
        {
            hand: "hours",
            angle: hours * (360 / 12) + minutes * (360 / 12 / 60), // 目前時間的時針角度
            // 時針一圈12小時，一圈360度，所以360度/12時=30度/時，也就是每小時旋轉30度
            // 對時針來說每分鐘旋轉角度為30度/60分=0.5度/分，也就是每分鐘旋轉0.5度
        },
        {
            hand: "minutes",
            angle: minutes * (360 / 60), // 目前時間的分針角度
            // 分針一圈60分，一圈360度，所以360度/60分=6度/分
        },
        {
            hand: "seconds",
            angle: seconds * (360 / 60), // 目前時間的秒針角度
            // 秒針一圈60秒，一圈360度，所以360度/60秒=6度/秒
        },
    ];

    for (var j = 0; j < hands.length; j++) {
        var elements = document.querySelectorAll("." + hands[j].hand); // 取得所有的指針(時針，分針，秒針)
        // 將指針依照目前時間來設定角度
        elements[0].style.webkitTransform = "rotateZ(" + hands[j].angle + "deg)"; // for Safari
        elements[0].style.transform = "rotateZ(" + hands[j].angle + "deg)";

        // 將秒針目前的角度註記在分針上
        if (hands[j].hand === "minutes") {
            elements[0].parentNode.setAttribute(
                "data-second-angle",
                hands[j + 1].angle
            ); // 秒針目前的角度
        }
    }
}

function setUpMinuteHands() {
    // 計算目前分針還剩多少時間走完目前的分鐘
    var containers = document.querySelectorAll(".minutes-container"); // 取得分針的container
    var secondAngle = containers[0].getAttribute("data-second-angle"); // 取得剛註記的秒針角度
    if (secondAngle > 0) {
        // 設定在目前的分鐘結束時，會觸發推動分針的延遲時間
        var delay = ((360 - secondAngle) / 6 + 0.1) * 1000;
        // 360度減秒針的角度即為目前分鐘剩下可跑的角度，在除以每秒6度，得到目前分鐘剩下可跑的時間
        // 例如秒針跑了30秒，則secondAngle為30秒*6度=180度，又(360度-180度)/6(度/秒)=30秒，代表目前分鐘還有30秒可以跑完。
        // 因為setTimeout的delay單位是以毫秒(千分之一秒)，所以要乘1000，也就是(30+0.1)*1000=30100毫秒=30.1秒(多的0.1秒是讓分針的移動比秒針慢一點，效果更擬真)
        // 也就是在30.1秒後觸發推動分針的方法moveMinuteHands()
        setTimeout(function () {
            moveMinuteHands(containers);
        }, delay);
    }
}

// 用來推動分針的方法
// 注意實際轉動是指針的container，指針本身是固定不動的
function moveMinuteHands(containers) {
    for (var i = 0; i < containers.length; i++) {
        containers[i].style.webkitTransform = "rotateZ(6deg)"; // for Safari
        containers[i].style.transform = "rotateZ(6deg)"; // 在30.1秒後讓分針轉到6度的位置
    }
    // 每間隔60秒執行一次
    setInterval(function () {
        for (var i = 0; i < containers.length; i++) {
            if (containers[i].angle === undefined) {
                containers[i].angle = 12; // 因為第一次已先移動了6度，所以下一次要多6度，所以是12度
            } else {
                containers[i].angle += 6; // 之後每一次的位置都是前一次的度數都多加6度
            }
            containers[i].style.webkitTransform =
                "rotateZ(" + containers[i].angle + "deg)";
            containers[i].style.transform = "rotateZ(" + containers[i].angle + "deg)";
        }
    }, 60000); // 60000毫秒=60秒
}

function moveSecondHands() {
    var containers = document.querySelectorAll(".seconds-container"); // 取得秒針的container

    // 每間隔1秒執行一次
    setInterval(function () {
        if (containers[0].angle === undefined) {
            containers[0].angle = 6;
        } else {
            containers[0].angle += 6;
        }
        containers[0].style.webkitTransform =
            "rotateZ(" + containers[0].angle + "deg)";
        containers[0].style.transform = "rotateZ(" + containers[0].angle + "deg)";
    }, 1000); // 1000毫秒=1秒
}
