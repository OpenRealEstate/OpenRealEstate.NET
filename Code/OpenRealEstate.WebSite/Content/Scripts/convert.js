function processText(isConvertingReaXmlToJson) {

    var reaXml;
    var json;

    $('#jsonCode').text('');
    $('#message').text('');

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
        .done(function(data) { displayListingResult(isConvertingReaXmlToJson, data); })
        .fail(function (qXhr, textStatus, errorThrown) {
            $('#message').text(qXhr.responseText);
        });
}

function displayListingResult(isConvertingReaXmlToJson, data) {
    if (isConvertingReaXmlToJson) {

        // Split the pieces of the viewModel up.
        var jsonListings = JSON.stringify(data.listings);
        var message = 'Residential Count: ' + data.residentialCount + '.<br/>Rental Count: ' + data.rentalCount + '.<br/><br/>';

        if (data.validationErrors) {
            
            var errors = jQuery.map(data.validationErrors, function (value, key) {
                return {
                    "Key": key,
                    "Value": value
                };
            });

            var errorMessage = '<br>Errors have occurred.<br/><br/>';
            $.each(errors, function(index, error) {
                errorMessage = errorMessage + 'key: ' + error.Key + ' - error: ' + error.Value + '<br/>';
            });

            $('#message').html(errorMessage);
        } else {
            //var jsonPretty = JSON.stringify(JSON.parse(data), null, 2);
            $('#openREJson').text(jsonListings);
            console.log(JSON.stringify(jsonListings));
            $('#jsonCode').text(JSON.stringify(jsonListings));

            $('#message').html(message);
        }
    } else {
        $('#reaxmltext').html(data);
    }
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