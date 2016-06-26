function CotGBrowser_RefreshRaidReports(t) {
    //lista raportów z raidów

    $.post("overview/rreps.php", function (data)
    {
        hobbita.raids_repors(data);
    });
}

function CotGBrowser_GetRaidReport(reportId) {
    //kokretny raport z raidu

    $.post("overview/gFrep.php", "a=" + reportId, function (data)
    {
        hobbita.raid_report(data);
    });
}

function CotGBrowser_RefreshTroopsOverview(t) {
    $.post("overview/trpover.php", function (data) {
        hobbita.troops_overview(data);
    });
}
