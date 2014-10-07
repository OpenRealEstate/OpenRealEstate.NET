function processText(isConvertingReaXmlToJson) {

    var reaXml;
    var json;

    $('#jsonCode').text('');
    $('#errorMessage').text('');
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
            $('#errorMessage').text(qXhr.responseText);
        });
}

function displayListingResult(isConvertingReaXmlToJson, data) {
    if (isConvertingReaXmlToJson) {

        if (data.validationErrors) {
            
            var errors = jQuery.map(data.validationErrors, function (value, key) {
                return {
                    "Key": key,
                    "Value": value
                };
            });

            var errorMessage = '<br>Errors have occurred.<br/><br/>';
            $.each(errors, function(index, error) {
                errorMessage = errorMessage + error.Key + ' - error: ' + error.Value + '<br/>';
            });

            $('#errorMessage').html(errorMessage);
        } 

        // Split the pieces of the viewModel up.
        var jsonListings = data.listings == null ? '': JSON.stringify(data.listings);
        var message = 'Residential Count: ' + data.residentialCount + '.<br/>Rental Count: ' + data.rentalCount + '.<br/>Rural Count: ' + data.ruralCount + '.<br/>Land Count: ' + data.landCount + '.<br/><br/>';

        $('#openREJson').text(jsonListings);
        console.log(JSON.stringify(jsonListings));
        $('#jsonCode').text(JSON.stringify(jsonListings));

        $('#message').html(message);
        
    } else {
        $('#reaxmltext').html(data);
    }
}

function getSampleReaXml(fileName) {

    $.ajax({
        type: 'GET',
        url: 'content/REA/' + fileName,
        dataType: 'text',
        success: function(xml) {
            $('#reaxmltext').val(xml);
        }
    });
}