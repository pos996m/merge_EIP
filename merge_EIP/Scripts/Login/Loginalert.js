$("#demo3").click(() => {

    var thePwd = document.getElementById("userPWD").value;
    var flag1 = false, flag2 = false;
    if (thePwd == "") {
        Swal.fire({
            heightAuto: false,
            title: "登入失敗",
            text: "帳號密碼不可空白",
            icon: "error",
            showClass: {
                popup: 'animate__animated animate__pulse'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            },
            textAlign: "left"
        })
        //標題 
        //訊息內容(可省略)
        //圖示(可省略) success/info/warning/error/question
        //圖示範例：https://sweetalert2.github.io/#icons
    } else {
        document.form1.submit();
    }
    // 密碼長度為>=6
    //else if (thePwd.length >= 6) {
    //    for (i = 0; i < thePwd.length; i++) {
    //        var theChar = thePwd.substr(i, 1).toUpperCase();
    //        //var theChar = thePwd[i].toUpperCase();
    //        if ("A" <= theChar && theChar <= "Z")
    //            flag1 = true;
    //        else if (0 <= theChar && theChar <= 9)
    //            flag2 = true;
    //        if (flag1 == true && flag2 == true)
    //            break;
    //    }

    //    if (flag1 && flag2) {
    //        Swal.fire({
    //            heightAuto: false,
    //            title: "歡迎登入",
    //            text: "可以使用",
    //            icon: "success"
    //        })
    //        document.form1.submit();
    //    } else {
    //        Swal.fire({
    //            heightAuto: false,
    //            title: "查詢作業失敗",
    //            text: "密碼須包含至少一個字母與一個數字",
    //            icon: "error"
    //        })
    //    }
    //}
    //else {
    //    Swal.fire({
    //        heightAuto: false,
    //        title: "查詢作業失敗",
    //        text: "密碼長度不正確",
    //        icon: "error"
    //    })
    //}

})
