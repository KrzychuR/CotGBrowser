function InitAjaxHandlers(t)
{

    //lista raportów z raidów
    /*
    $.post("overview/rreps.php", function (data)
    {
        alert('res...');
        hobbita.ajax_success("rreps.php", data);
    });
    */

    $.post("overview/gFrep.php", "a=6543691871", function (data) {
        alert('res...');
        hobbita.ajax_success("gFrep.php", data);
    });

    /*
    var jqxhr = $.post("rreps.php", function () {
        alert("success");
    })
      .done(function () {
          alert("second success");
      })
      .fail(function () {
          alert("error");
      })
      .always(function () {
          alert("finished");
      });
      */

    /*
    if (hobbita == null || hobbita == undefined)
        alert('brak hobbity');
    else {
        alert('Jest Hob');
        hobbita.ajax_success("Test", "Test2");
    }

    $(document).ajaxSuccess(function (event, xhr, settings, data) {
        hobbita.ajax_success("OK", "OK");

        if (settings)
        {
            hobbita.ajax_success(JSON.stringify(settings), "");
        }

        if (settings.url != "/includes/poll2.php") {
            hobbita.ajax_success(JSON.stringify(settings), data);
        }
    });
    */
}