var apiURL = "http://localhost:6360";
var authApplicationURL = "http://localhost:19657";

var DoIzendaConfig = function () {
    var hostApi = apiURL + "/api/";
    var configJson = {
        "WebApiUrl": hostApi,
        "BaseUrl": "/izenda",
        "RootPath": "/izenda",
        "CssFile": "izenda-ui.css",
        "Routes": {
            "Settings": "settings",
            "New": "new",
            "Dashboard": "dashboard",
            "Report": "report",
            "ReportViewer": "reportviewer",
            "ReportViewerPopup": "reportviewerpopup",
            "Viewer": "viewer"
        },
        "Timeout": 3600
    };
    IzendaSynergy.config(configJson);

};

function errorFunc() {
    alert('Token was not generated correctly. Please login.');

    // confirm dialog
    alertify.confirm("Your token was not generated correctly, please login.", function () {
        // user clicked "ok"
    }, function () {
        // user clicked "cancel"
    });
}

var DoRender = function (successFunc) {
    $.ajax({
        type: "GET",
        url: authApplicationURL + "/api/user/GenerateToken",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: successFunc,
        error: errorFunc
    });
};

var izendaInit = function () {
    //function successFunc(data, status) {
    //var currentUserContext = {
    //    //token: data.token
    //    token: localStorage.getItem("token")
    //};
    //Works with localstorage
    var tokenFromStorage = localStorage.getItem('token');
    //var currentUserContext = document.cookie;
    var currentUserContext = {
        token: tokenFromStorage
    };

        console.log(currentUserContext);
        IzendaSynergy.setCurrentUserContext(currentUserContext);
        IzendaSynergy.render(document.getElementById('izenda-root'));
    //}

    //this.DoRender(successFunc);

};

var izendaInitReport = function () {

    function successFunc(data, status) {
        var currentUserContext = {
            token: data.token
        };

        IzendaSynergy.setCurrentUserContext(currentUserContext);
        IzendaSynergy.renderReportPage(document.getElementById('izenda-root'));
    }

    this.DoRender(successFunc);

};

var izendaInitSetting = function () {
  //function successFunc(data, status) {
    //    console.info(data);
    //    var currentUserContext = {
    //        token: data.token
    //    };

    //    IzendaSynergy.setCurrentUserContext(currentUserContext);
    //    IzendaSynergy.renderSettingPage(document.getElementById('izenda-root'));
    //}

    //this.DoRender(successFunc);

    var tokenFromStorage = localStorage.getItem('token');
    var currentUserContext = {
        token: tokenFromStorage
    };

    console.log(currentUserContext);
    IzendaSynergy.setCurrentUserContext(currentUserContext);
    IzendaSynergy.renderSettingPage(document.getElementById('izenda-root'));
};

var izendaInitReportPart = function (reportParts) {

    function successFunc(data, status) {
        console.info(data);
        var currentUserContext = {
            token: data.token
        };

        IzendaSynergy.setCurrentUserContext(currentUserContext);
        for (var i = 0; i < reportParts.length; i++) {
            if (reportParts[i].overridingFilterValue) {
                IzendaSynergy.renderReportPart(document.getElementById(reportParts[i].selector), {
                    "id": reportParts[i].id,
                    "overridingFilterValue": reportParts[i].overridingFilterValue,
                });
            }
            else {
                IzendaSynergy.renderReportPart(document.getElementById(reportParts[i].selector), {
                    "id": reportParts[i].id
                });
            }

        }
    }

    this.DoRender(successFunc);
};

var izendaInitReportPartUpdateResult = function (reportPartId, overridingFilterValue, container) {

    function successFunc(data, status) {
        console.info(data);
        var currentUserContext = {
            token: data.token
        };

        IzendaSynergy.setCurrentUserContext(currentUserContext);
        IzendaSynergy.renderReportPart(document.getElementById(container), {
            "id": reportPartId,
            "overridingFilterValue": overridingFilterValue,
        });
    }

    this.DoRender(successFunc);
};

var izendaRenderReportPart = function (reportPartId, container) {

    function successFunc(data, status) {
        console.info(data);
        var currentUserContext = {
            token: data.token
        };

        IzendaSynergy.setCurrentUserContext(currentUserContext);
        IzendaSynergy.renderReportPart(document.getElementById(container), {
            "id": reportPartId
        });
    }

    this.DoRender(successFunc);
};

var izendaInitReport = function () {

    function successFunc(data, status) {
        var currentUserContext = {
            token: data.token
        };

        IzendaSynergy.setCurrentUserContext(currentUserContext);
        IzendaSynergy.renderReportPage(document.getElementById('izenda-root'));
    }

    this.DoRender(successFunc);

};

// Render report viewer to a <div> tag by report id
var izendaInitReportViewer = function (reportId, filters) {
    function successFunc(data, status) {
        var currentUserContext = {
            token: data.token
        };
        IzendaSynergy.setCurrentUserContext(currentUserContext);
        IzendaSynergy.renderReportViewerPage(document.getElementById('izenda-root'), reportId, filters);
    }

    this.DoRender(successFunc);

};

var izendaInitReportCustomFilters = function (reportObject) {

    function successFunc(data, status) {
        var currentUserContext = {
            token: data.token
        };

        IzendaSynergy.setCurrentUserContext(currentUserContext);
        if (reportObject.filtersObj) {
            IzendaSynergy.renderReportViewerPage(document.getElementById(reportObject.selector), reportObject.id, reportObject.filtersObj);
        }
        else {
            IzendaSynergy.renderReportViewerPage(document.getElementById(reportObject.selector), reportObject.id);
        }
    }

    this.DoRender(successFunc);

};

var izendaInitDashboard = function () {

    function successFunc(data, status) {
        var currentUserContext = {
            token: data.token
        };

        IzendaSynergy.setCurrentUserContext(currentUserContext);
        IzendaSynergy.renderDashboardPage(document.getElementById('izenda-root'));
    }

    this.DoRender(successFunc);

};

// Render dashboard viewer to a <div> tag by dashboard id
var izendaInitDashboardViewer = function (dashboardId, filters) {
    function successFunc(data, status) {
        var currentUserContext = {
            token: data.token
        };
        IzendaSynergy.setCurrentUserContext(currentUserContext);
        IzendaSynergy.renderDashboardViewerPage(document.getElementById('izenda-root'), dashboardId, filters);
    }

    this.DoRender(successFunc);
};

var izendaInitReportDesigner = function () {

    function successFunc(data, status) {
        var currentUserContext = {
            token: data.token
        };

        IzendaSynergy.setCurrentUserContext(currentUserContext);
        IzendaSynergy.renderReportDesignerPage(document.getElementById('izenda-root'));

    }

    this.DoRender(successFunc);

};

var izendaInitNewDashboard = function () {

    function successFunc(data, status) {
        var currentUserContext = {
            token: data.token
        };

        IzendaSynergy.setCurrentUserContext(currentUserContext);
        IzendaSynergy.renderNewDashboardPage(document.getElementById('izenda-root'));
    }

    this.DoRender(successFunc);

};

var izendaInitReportPartExportViewer = function (reportPartId, token) {
    var currentUserContext = {
        token: token
    };
    IzendaSynergy.setCurrentUserContext(currentUserContext);
    IzendaSynergy.renderReportPart(document.getElementById('izenda-root'), {
        id: reportPartId,
        useQueryParam: true,
        useHash: false
    });
};
