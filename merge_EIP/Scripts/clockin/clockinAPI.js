// 流程
// 取得時間 > 判斷點按打卡 > 第一次按是上班打卡 > 下一次按是下班打卡
// 上班打卡使用post(新增)，下班打卡使用put(修改)

var clockinorout;
var x;
var EID;
// var x = document.getElementById("clickname").innerHTML;

$(document).ready(() => {
    x = $("#clickname").text();
    EID = $("#EID").text();

    // 如果偵測到今日已打過卡，則顯示下班打卡
    if (x == "上班打卡") {
        clockinorout = true;
        console.log(clockinorout);
    } else if (x == "下班打卡") {
        clockinorout = false;
        $("#bodyTemp").hide();
        console.log(clockinorout);
    }
})

function test() {
    // 取得現在時間
    let mytime = new Date();
    // 取得日期yyyy-mm-dd
    let myday = `${mytime.getFullYear()}-${String(mytime.getMonth() + 1).padStart(2, "0")}-${String(mytime.getDate()).padStart(2, "0")}`;
    console.log(myday);
    // 取得完整時間hh:mm:ss
    let mystime = `${String(mytime.getHours()).padStart(2, "0")}:${String(mytime.getMinutes()).padStart(2, "0")}:${String(mytime.getSeconds()).padStart(2, "0")}`;



    if (clockinorout) {
        // 取得體溫
        var bodyTemp = $("#bodyTemp").val();
        if (bodyTemp == "" || bodyTemp < 35 || bodyTemp > 42) {
            Swal.fire({
                heightAuto: false,
                title: "體溫輸入錯誤",
                icon: "error"
            })
            return
        }
        var bodyTempStr = `<br>${bodyTemp}度，體溫正常`;
        if (parseFloat(bodyTemp) > 37.5) {
            bodyTempStr = `<br>${bodyTemp}度，有發燒狀況，請多留意`
        }

        $("#clickname").text("下班打卡");
        $("#bodyTemp").hide();
        clockinorout = false;

        const xhttp = new XMLHttpRequest();
        xhttp.open("POST", "/api/ClockinAPI" + encodeURI(`?EID=${EID}&day=${myday}&clockin=${mystime}&bodyTemp=${bodyTemp}`), true);
        xhttp.send();
        // 要取得JSON要等他load完才拿得到
        xhttp.onload = () => {
            var data = JSON.parse(xhttp.responseText);
            console.log(xhttp.responseText);
            console.log(data);

            if (data == "打卡成功") {
                Swal.fire({
                    heightAuto: false,
                    title: "上班" + data,
                    html: `打卡時間: ${myday}  ${mystime}` + bodyTempStr,
                    icon: "success"
                })
            } else if (data == "打卡失敗") {
                Swal.fire({
                    heightAuto: false,
                    title: "上班" + data,
                    icon: "error"
                })
            }
        }
    } else {
        //document.getElementById("clickname").innerHTML = "上班打卡";
        //clockinorout = true;

        const xhttp = new XMLHttpRequest();
        xhttp.open("PUT", "/api/ClockinAPI" + encodeURI(`?EID=${EID}&day=${myday}&clockout=${mystime}`), true);
        xhttp.send();
        // 要取得JSON要等他load完才拿得到
        xhttp.onload = () => {
            var data = JSON.parse(xhttp.responseText);
            console.log(xhttp.responseText);
            console.log(data);
            if (data == "打卡成功") {
                Swal.fire({
                    heightAuto: false,
                    title: "下班" + data,
                    html: `打卡時間: ${myday}  ${mystime}`,
                    icon: "success"
                })
            } else if (data == "打卡失敗") {
                Swal.fire({
                    heightAuto: false,
                    title: "下班" + data,
                    icon: "error"
                })
            }
        }
    }
}