var TCMB = {
    Handler: {
        ExchangeRates: function (data) {
            var myData = [{ "TestName": "3P" }, { "TestName": "6D" }, { "TestName": "1-"}];
            window.Table_ExchangeRates = $("#Table-ExchangeRates").DataTable({
                //"ajax": {
                //    "url": "/Ajax/JsonProvider?Method=DetailedTicketReport&GenericObject=true&Json=" + JSON.stringify(json),
                //    "dataSrc": ""
                //},
                data: myData,
                "columns": [
                    {
                        "data": "TestName",
                        "sTitle": "test name"
                    }
                ],
                "pageLength": 20,
                "lengthMenu": [
                    [20, 50, 100, -1],
                    [20, 50, 100, "All"]
                ],
                "info": false,
                bFilter: true,
                bInfo: false,
                "order": [[0, "asc"]],
                "scrollX": true,
                //"language": {
                //    "search": Util.Localize.Resource("942A5171-67CC-4971-AA43-C1346DBF6241", "{en: 'Search: ', tr: 'Arama: ', ar: 'بحث: '}")
                //},
                dom: 'Bfrtip'
            });
        }
    }
}