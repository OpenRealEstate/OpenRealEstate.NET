
function cleanUpUI() {
    $('#jsonCode').text('');
    $('#errorMessage').html('');
    $('#message').text('');
}


function processText(isConvertingReaXmlToJson) {

    cleanUpUI();

    var reaXml;
    var json;

    if (isConvertingReaXmlToJson) {
        reaXml = $('#reaxmltext').val();
        json = null;
    } else {
        reaXml = null;
        json = $('#openREJson').val();
    }
    var url = isConvertingReaXmlToJson
        ? '/parse/Rea'
        : '/parse/OREToReaXml';
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

        if (!jQuery.isEmptyObject(data.validationErrors)) {
            
            var errors = jQuery.map(data.validationErrors, function (value, key) {
                return {
                    "Key": key,
                    "Value": value
                };
            });

            var errorMessage = '<br>Errors have occurred.<br/><br/>';
            $.each(errors, function(index, error) {
                errorMessage = errorMessage + escapeHTML(error.Key) + ' - error: ' + escapeHTML(error.Value) + '<br/>';
            });

            $('#errorMessage').html(errorMessage);
        } 

        // Split the pieces of the viewModel up.
        var json = JSON.parse(data.listingsJson);
        
        var jsonListings = json == null ||
                           json.length <= 0
                           ? ''
                           : json;
        var message = 'Residential Count: ' + data.residentialCount + '.<br/>Rental Count: ' + data.rentalCount + '.<br/>Rural Count: ' + data.ruralCount + '.<br/>Land Count: ' + data.landCount + '.<br/><br/>';

        $('#openREJson').text(data.listingsJson);
        $('#jsonCode').text(JSON.stringify(jsonListings));
        $('#message').html(message);
        
    } else {
        $('#reaxmltext').html(data);
    }
}

function escapeHTML(html) {
    return html.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
}

function getSampleReaXml(fileName) {

    $.ajax({
        type: 'GET',
        url: 'xml/REA/' + fileName,
        dataType: 'text',
        success: function(xml) {
            $('#reaxmltext').val(xml);
        }
    });
}

function uploadFiles() {

    cleanUpUI();

    var files = $('#upload')[0].files;

    // Grab all of the file data.
    var formData = new FormData();
    $.each(files, function(key, value) {
        formData.append(key, value);
    });

    $.ajax({
        url: '/parse/files',
        type: 'POST',
        data: formData,
        cache: false,
        dataType: 'json',
        processData: false, // Don't process the files
        contentType: false, // Set content type to false as jQuery will tell the server its a query string request
        success: function (data, textStatus, qXhr) {
            if (typeof data.error === 'undefined') {
                // Success so call function to process the form
                displayListingResult(true, data);
            }
            else {
                // Handle errors here
                $('#errorMessage').text(qXhr.responseText);
            }
        },
        error: function (qXhr, textStatus, errorThrown) {
            // Handle errors here
            $('#errorMessage').text(qXhr.responseText);
            // STOP LOADING SPINNER
        }
    });
}