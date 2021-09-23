// 呼叫API的涵式
function AJAXcall(data, clock, Tname) {
    const xhttp = new XMLHttpRequest();
    xhttp.open("POST", "/api/WorklogListAPI" + encodeURI(`?Num=${data}&clock=${clock}&Text=${Tname}`), true);
    xhttp.send();
    // 要取得JSON要等他load完才拿得到
    xhttp.onload = () => {
        // var data = JSON.parse(xhttp.responseText);
        console.log(xhttp.responseText);
        // console.log(data);
    }
}


const mylist = $(".mylist");

    // 監聽 mylist 的點擊事件，製作點窗格就執行check
    mylist.click((e) => {

        // 如果沒點擊到就不做
        if (e.target.getAttribute("class") != "mycheck del" && e.target.getAttribute("class") != "mycheck" && e.target.getAttribute("class") != "check-box") {
            console.log(e.target.nodeName)
            return;
        }

        // 取得預藏的data-cnt來尋找checked的id
        let chk = document.querySelector(`#list_${e.target.getAttribute("data-cnt")}`);
        let y = document.querySelector(`#Tname_${e.target.getAttribute("data-cnt")}`);
        let chkTF;

        // 如果點進來是點 check-box 近來，則轉換一下布林值
        if (e.target.getAttribute("class") == "check-box") {
            chkTF = !chk.checked;
        } else {
            chkTF = chk.checked;
            // console.log(chk);
            // console.log(chk.checked);
        }
        // 更改點選狀態
        if (chkTF) {
            chk.checked = false;
            y.setAttribute("class", "mycheck");
            // console.log(e.target.getAttribute("data-tid"));
            AJAXcall(e.target.getAttribute("data-TID"), 0, '');
        } else {
            chk.checked = true;
            y.setAttribute("class", "mycheck del");
            // console.log(e.target.getAttribute("data-tid"));
            AJAXcall(e.target.getAttribute("data-TID"), 1, '');
        }
    })

function myEdit(e) {
    // 取得按鈕
    // console.log(e.target);
    let btne = e.target;
    // 取得文字
    let x = document.querySelector(`#Tname_${e.target.getAttribute("data-cnt")}`);
    let xvu = x.innerHTML;

    // 判斷如果是編輯就轉確認並加入input
    if (btne.innerHTML == "編輯") {
        x.innerHTML = `<input type='text' size='50' maxlength='30' value='${xvu.trim()}' id='Edit_${e.target.getAttribute("data-cnt")}'>`;
        btne.innerHTML = '確認';
        btne.className = "btn btn-light"
        // console.log(btne);
    } else if (btne.innerHTML == "確認") {
        let Evu = document.querySelector(`#Edit_${e.target.getAttribute("data-cnt")}`).value;
        // console.log(Evu)
        btne.className = "btn btn-FB"
        x.innerHTML = Evu;
        btne.innerHTML = '編輯';
        // console.log(e.target.getAttribute("data-TID"));
        // console.log(Evu);
        AJAXcall(e.target.getAttribute("data-TID"), 0, Evu);
    }
}
