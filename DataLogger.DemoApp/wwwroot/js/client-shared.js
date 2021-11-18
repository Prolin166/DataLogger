

window.downloadFileFromUrl = function (downloadUrl) {
    var link = document.createElement('a');
    link.href = downloadUrl;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
    