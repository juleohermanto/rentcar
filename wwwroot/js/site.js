function toggleMenu() {
    var sidebar = document.getElementById("sideMenu");
    if (sidebar.style.width === "250px") {
        sidebar.style.width = "0";
    } else {
        sidebar.style.width = "250px";
    }
}
function cekTanggal() {
    
    var inputAmbil = document.getElementById("tglAmbil");
    var inputKembali = document.getElementById("tglKembali");
    if (inputAmbil && inputKembali) {
        var ambil = inputAmbil.value;
        var kembali = inputKembali.value;
        if (ambil === "" || kembali === "") {
            alert("Isi tanggal terlebih dahulu");
            return false; 
        }
    }
    return true; 
}

document.addEventListener("DOMContentLoaded", function () {

    var formCari = document.getElementById("formPencarian");

    if (formCari) {
        formCari.addEventListener("submit", function (event) {

            var tglAmbil = document.getElementById("tglAmbil").value;
            var tglKembali = document.getElementById("tglKembali").value;

            if (!tglAmbil || !tglKembali) {
                event.preventDefault();
                alert(" Harap isi Tanggal Pickup dan Return terlebih dahulu!");
                return; 
            }
            var dateAmbil = new Date(tglAmbil);
            var dateKembali = new Date(tglKembali);

            if (dateKembali <= dateAmbil) {
                event.preventDefault(); 
                alert("Tanggal Kembali harus lebih besar dari Tanggal Pickup!");
                return;
            }
        });
    }
});