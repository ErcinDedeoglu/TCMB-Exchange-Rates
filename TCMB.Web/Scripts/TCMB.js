var TCMB = {
    Event: {
        Onload: function () {
            window.LanguageID = 1;
            window.Table_ExchangeRates = null;
            $("#DatePicker-ExchangeRate").datepicker({
                rtl: false,
                orientation: "left",
                autoclose: true,
                dateFormat: 'dd.mm.yy',
                onSelect: function (dateText) {
                    TCMB.SignalR.Request.Handler.ExchangeRates(this.value);
                }
            });

            TCMB.SignalR.Init("http://" + document.domain + ":8080/signalr/");
        }
    },
    Handler: {
        ExchangeRates: function (data) {
            if (window.Table_ExchangeRates) {
                window.Table_ExchangeRates.destroy();
                window.Table_ExchangeRates = null;
            }

            window.Table_ExchangeRates = $("#Table-ExchangeRates").DataTable({
                data: data,
                "columns": [
                    {
                        "data": "CurrencyCode",
                        "sTitle": TCMB.Language.UI.CurrencyCode[window.LanguageID],
                    },
                    {
                        "data": "CurrencyName",
                        "sTitle": TCMB.Language.UI.CurrencyType[window.LanguageID],
                        "render": function (data, type, full) {
                            try {
                                return TCMB.Language.Currency[full.CurrencyCode][window.LanguageID];
                            } catch (e) {}
                            return "N/A";
                        }
                    },
                    {
                        "data": "Unit",
                        "sTitle": TCMB.Language.UI.Unit[window.LanguageID]
                    },
                    {
                        "data": "BanknoteBuying",
                        "sTitle": TCMB.Language.UI.BanknoteBuying[window.LanguageID]
                    },
                    {
                        "data": "BanknoteSelling",
                        "sTitle": TCMB.Language.UI.BanknoteSelling[window.LanguageID]
                    },
                    {
                        "data": "ForexBuying",
                        "sTitle": TCMB.Language.UI.ForexBuying[window.LanguageID]
                    },
                    {
                        "data": "ForexSelling",
                        "sTitle": TCMB.Language.UI.ForexSelling[window.LanguageID]
                    },
                    {
                        "data": "CrossRateUSD",
                        "sTitle": TCMB.Language.UI.CrossRateUSD[window.LanguageID]
                    },
                    {
                        "data": "CrossRateOther",
                        "sTitle": TCMB.Language.UI.CrossRateOther[window.LanguageID]
                    },
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
                dom: 'Bfrtip'
            });
        },
        LanguageChange: function() {
            window.LanguageID = parseInt($("#Select-Language").val());

            if ($("#DatePicker-ExchangeRate").val() != "") {
                $("#DatePicker-ExchangeRate").change();
            }

            TCMB.Handler.ExchangeRates();
        }
    },
    SignalR: {
        State: false,
        ConnectionSlowInterval: null,
        Init: function (webSocketHubURL) {
            window.SignalR = $.signalR;
            window.HubConnection = $.hubConnection;
            window.HubConnection.prototype.createHubProxies = function () {
                var proxies = {};
                this.starting(function () {
                    TCMB.SignalR.RegisterHubProxies(proxies, true);
                    window.SignalR.hub._registerSubscribedHubs();
                }).disconnected(function () {
                    TCMB.SignalR.RegisterHubProxies(proxies, false);
                });
                proxies['signalRHub'] = window.SignalR.hub.createHubProxy('signalRHub');
                proxies['signalRHub'].client = {};
                proxies['signalRHub'].server = {
                    request: function () {
                        return proxies['signalRHub'].invoke.apply(proxies['signalRHub'], $.merge(["Request"], $.makeArray(arguments)));
                    }
                };
                return proxies;
            };

            window.SignalR.hub = window.HubConnection("/signalr", { useDefaultPath: false });
            $.extend(window.SignalR, window.SignalR.hub.createHubProxies());
            if (webSocketHubURL) {
                $.connection.hub.url = webSocketHubURL;
            } else {
                $.connection.hub.url = window.WebSocketHubURL;
            }

            var connection = $.connection.signalRHub;
            connection.client.response = function (methodName, jsonString) {
                TCMB.SignalR.Response.GenericCall(methodName, jsonString);
            };

            $.connection.hub.start().done(function () {
                TCMB.SignalR.State = true;
                window.KioskReady = true;
                TCMB.SignalR.Event.Connected();
            });
            $.connection.hub.reconnected(function () {
                $.connection.hub.stop();
                TCMB.SignalR.State = true;
                window.KioskReady = true;
                TCMB.SignalR.Event.Reconnected();
            });
            $.connection.hub.disconnected(function () {
                $.connection.hub.stop();
                TCMB.SignalR.State = false;
                window.KioskReady = false;
                TCMB.SignalR.Event.Disconnected();
                setTimeout(function () {
                    $.connection.hub.start().done(function () {
                        TCMB.SignalR.State = true;
                        window.KioskReady = true;
                        TCMB.SignalR.Event.Connected();
                    });
                }, 2000);
            });
            $.connection.hub.connectionSlow(function () {
                TCMB.SignalR.Event.ConnectionSlow();
            });
            $.connection.hub.reconnecting(function () {
                $.connection.hub.stop();
                TCMB.SignalR.State = false;
                window.KioskReady = false;
                TCMB.SignalR.Event.Reconnecting();
            });
        },
        RegisterHubProxies: function (instance, shouldSubscribe) {
            var key, hub, memberKey, memberValue, subscriptionMethod;
            for (key in instance) {
                if (instance.hasOwnProperty(key)) {
                    hub = instance[key];
                    if (!(hub.hubName)) {
                        continue;
                    }
                    if (shouldSubscribe) {
                        subscriptionMethod = hub.on;
                    } else {
                        subscriptionMethod = hub.off;
                    }
                    for (memberKey in hub.client) {
                        if (hub.client.hasOwnProperty(memberKey)) {
                            memberValue = hub.client[memberKey];
                            if (!$.isFunction(memberValue)) {
                                continue;
                            }
                            subscriptionMethod.call(hub, memberKey, function () {
                                memberValue.apply(hub, $.makeArray(arguments));
                            });
                        }
                    }
                }
            }
        },
        Request: {
            GenericCall: function (methodName, jsonString) {
                var connection = $.connection.signalRHub;
                console.log("Request Method: " + methodName);
                console.log("Request: " + jsonString);
                connection.server.request(methodName, jsonString);
            },
            Handler: {
                UpdateWebSocketConnection: function (webSocketConnectionGroup, intValue1, intValue2, intValue3, intValue4, intValue5, intValue6, webSocketConnectionInitialized, hardwareHostFingerPrintID, hardwareHostFingerPrint, webSocketConnectionVersion) {
                    var root = new Object();
                    root.ConnectionGUID = $.connection.hub.id;
                    root.WebSocketConnectionGroup = webSocketConnectionGroup;
                    root.WebSocketConnectionIntValue1 = intValue1;
                    root.WebSocketConnectionIntValue2 = intValue2;
                    root.WebSocketConnectionIntValue3 = intValue3;
                    root.WebSocketConnectionIntValue4 = intValue4;
                    root.WebSocketConnectionIntValue5 = intValue5;
                    root.WebSocketConnectionIntValue6 = intValue6;
                    root.HardwareHostFingerPrintID = hardwareHostFingerPrintID;
                    root.HardwareHostFingerPrint = hardwareHostFingerPrint;
                    root.WebSocketConnectionVersion = webSocketConnectionVersion;
                    root.WebSocketConnectionInitialized = webSocketConnectionInitialized;
                    TCMB.SignalR.Request.GenericCall("UpdateWebSocketConnection", JSON.stringify(root));
                },
                PingPong: function () {
                    TCMB.SignalR.Request.GenericCall("PingPong", JSON.stringify(new Object()));
                },
                ExchangeRates: function (date) {
                    TCMB.SignalR.Request.GenericCall("ExchangeRates", JSON.stringify(date));
                }
            }
        },
        Response: {
            GenericCall: function (methodName, jsonString) {
                var methodCall = "TCMB.SignalR.Response.Handler." + methodName + "(\'' + jsonString + '\')";
                eval(methodCall);
            },
            Handler: {
                PingPong: function (jsonString) {
                    TCMB.SignalR.Request.Handler.PingPong();
                },
                ExchangeRates: function(jsonString) {
                    TCMB.Handler.ExchangeRates(JSON.parse(jsonString));
                }
            }
        },
        Event: {
            Connected: function () {
                console.log("TCMB.SignalR.Event.Connected");
            },
            Reconnected: function () {
                console.log("TCMB.SignalR.Event.Reconnected");
            },
            Reconnecting: function () {
                console.log("TCMB.SignalR.Event.Reconnecting");
            },
            Disconnected: function () {
                console.log("TCMB.SignalR.Event.Disconnected");
            },
            ConnectionSlow: function () {
                console.log("TCMB.SignalR.Event.ConnectionSlow");
            }
        }
    },
    Language: {
        Currency: {
            'USD': { "1": "US DOLLAR", "2": "ABD DOLARI"},
            'TRY': { "1": "TURKISH LIRA", "2": "TÜRK LİRASI" },
            'AUD': { "1": "AUSTRALIAN DOLLAR", "2": "AVUSTRALYA DOLARI" },
            'DKK': { "1": "DANISH KRONE", "2": "DANİMARKA KRONU" },
            'EUR': { "1": "EURO", "2": "EURO" },
            'GBP': { "1": "POUND STERLING", "2": "İNGİLİZ STERLİNİ" },
            'CHF': { "1": "SWISS FRANK", "2": "İSVİÇRE FRANGI" },
            'SEK': { "1": "SWEDISH KRONA", "2": "İSVEÇ KRONU" },
            'CAD': { "1": "CANADIAN DOLLAR", "2": "KANADA DOLARI" },
            'KWD': { "1": "KUWAITI DINAR", "2": "KUVEYT DİNARI" },
            'NOK': { "1": "NORWEGIAN KRONE", "2": "NORVEÇ KRONU" },
            'SAR': { "1": "SAUDI RIYAL", "2": "SUUDİ ARABİSTAN RİYALİ" },
            'JPY': { "1": "JAPENESE YEN", "2": "JAPON YENİ" },
            'BGN': { "1": "BULGARIAN LEV", "2": "BULGAR LEVASI" },
            'RON': { "1": "NEW LEU", "2": "RUMEN LEYİ" },
            'RUB': { "1": "RUSSIAN ROUBLE", "2": "RUS RUBLESİ" },
            'IRR': { "1": "IRANIAN RIAL", "2": "İRAN RİYALİ" },
            'CNY': { "1": "CHINESE RENMINBI", "2": "ÇİN YUANI" },
            'PKR': { "1": "PAKISTANI RUPEE", "2": "PAKİSTAN RUPİSİ" },
            'XDR': { "1": "SPECIAL DRAWING RIGHT (SDR)", "2": "ÖZEL ÇEKME HAKKI (SDR)" },
        },
        UI: {
            'CurrencyType': {"1": "Currency Type", "2": "Döviz Cinsi" },
            'CurrencyCode': {"1": "Currency Code", "2": "Döviz Kodu" },
            'BanknoteBuying': { "1": "Banknote Buying", "2": "Döviz Alış" },
            'BanknoteSelling': { "1": "Banknote Selling", "2": "Döviz Satış" },
            'ForexBuying': { "1": "Forex Buying", "2": "Efektif Alış" },
            'ForexSelling': { "1": "Forex Selling", "2": "Efektif Satış" },
            'Unit': { "1": "Unit", "2": "Birim" },
            'CrossRateUSD': { "1": "Cross Rate - USD", "2": "Çapraz Kur - USD" },
            'CrossRateOther': { "1": "Cross Rate - Other", "2": "Çapraz Kur - Diğer" },
        }
    }
}