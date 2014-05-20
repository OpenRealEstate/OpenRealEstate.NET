function processText(isConvertingReaXmlToJson) {

    var reaXml;
    var json;

    $('#jsonCode').text('');
    $('#errorMessage').text('');

    if (isConvertingReaXmlToJson) {
        reaXml = $('#reaxmltext').val();
        json = null;
    } else {
        reaXml = null;
        json = $('#openREJson').val();
    }
    var url = isConvertingReaXmlToJson
        ? '/convert/ReaToORE'
        : '/convert/OREToReaXml';
    $.post(url,
        {
            'reaXml': reaXml,
            'json': json
        })
        .done(function(data) {
                if (isConvertingReaXmlToJson) {

                    //var jsonPretty = JSON.stringify(JSON.parse(data), null, 2);
                    $('#openREJson').text(JSON.stringify(data));
                    console.log(JSON.stringify(data));
                    $('#jsonCode').text(JSON.stringify(data));
                } else {
                    $('#reaxmltext').html(data);
                }
            }
        )
        .fail(function (qXhr, textStatus, errorThrown) {
            $('#errorMessage').text(qXhr.responseText);
        }
        );
}

function getSampleReaXml() {

    $.ajax({
        type: 'GET',
        url: 'content/ReaXmlSample.xml',
        dataType: 'text',
        success: function(xml) {
            $('#reaxmltext').val(xml);
        }
    });
}