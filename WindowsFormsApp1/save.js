function getQueryParamNew(param) {
    // ��ȡ��ǰҳ���URL
    alert(window.location)
    const url = new URL(window.location.href);
    alert(url)
    // ʹ��URLSearchParams������ѯ�ַ���
    const params = new URLSearchParams(url.search);

    // ��ȡָ��������ֵ
    return params.get(param);
}

function getQueryParam(param) {
    var queryString = window.location.search.substring(1);
    var params = queryString.split("&");
    for (var i = 0; i < params.length; i++) {
        var pair = params[i].split("=");
        if (decodeURIComponent(pair[0]) === param) {
            return decodeURIComponent(pair[1]);
        }
    }
    return null;
}

//

$(document).ready(function () {

    // alert(getQueryParam("id"))
    var id = getQueryParam("id");
    if (!id)
        return;
    $("#id").val(getQueryParam("id"))
    //window.id.value = getQueryParam("id");

    var obj = window.external.find(getQueryParam("id"));
    obj = JSON.parse(obj);
   // alert(obj)
    populateForm(obj)

});

function populateForm(data) {
    $.each(data, function (key, value) {
        // Find the form input with the matching name attribute
        var input = $('[name=' + key + ']');

        // Check if input exists and set its value
        if (input.length) {
            input.val(value);
        }
    });
}