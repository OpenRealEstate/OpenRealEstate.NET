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

        if (data.ValidationErrors) {
            
            var errors = jQuery.map(data.ValidationErrors, function (value, key) {
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
        var jsonListings = data.Listings == null ? '': JSON.stringify(data.Listings);
        var message = 'Residential Count: ' + data.ResidentialCount + '.<br/>Rental Count: ' + data.RentalCount + '.<br/>Rural Count: ' + data.RuralCount + '.<br/>Land Count: ' + data.LandCount + '.<br/><br/>';

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