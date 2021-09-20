function clickacl(datetime) {
  let strtitle = `<br><div class="card" style="text-align:left;">
    <div class="card-body">
        <h5 class="card-title">備註</h5>
        <ul class="list-group list-group-flush">
            <li class="list-group-item">Cras justo odio</li>
            <li class="list-group-item">Dapibus ac facilisis in</li>
            <li class="list-group-item">Morbi leo risus</li>
            <li class="list-group-item">Porta ac consectetur ac</li>
            <li class="list-group-item">Vestibulum at eros</li>
        </ul>
    </div>
    </div>`;

  if (datetime != null) {
    Swal.fire({
      title: `${datetime}`,
      html:
        `    <div class="card" style="text-align:left;">
                                <div class="card-body">
                                    <h5 class="card-title">今日代辦事項</h5>
                                    <p class="card-text" id="strtitle">Some quick example text to build on the card title and make up the bulk of the card's content.</p>
                                    <button class="btn btn-dark float-right" id="Editdaytitlebtn" onclick="Editdaytitle()">編輯</button>
                                </div>
                            </div>` + strtitle,
      showConfirmButton: false,
      // focusConfirm: false,
    });
  }
}

function Editdaytitle() {
  const btn = $("#Editdaytitlebtn").text();

  if (btn == "編輯") {
    let x = $("#strtitle").text().trim();
    $("#strtitle").html(
      `<textarea cols='30' rows='5' maxlength='200' id='strtitletext'>${x}</textarea>`
    );
    console.log(x);
    $("#Editdaytitlebtn").text("確認");
  } else {
    $("#strtitle").html($("#strtitletext").text());
    $("#Editdaytitlebtn").text("編輯");
  }
}
