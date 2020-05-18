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
    //Works with localstorage
    var tokenFromStorage = localStorage.getItem('token');
    var currentUserContext = {
        token: tokenFromStorage
    };

        console.log(currentUserContext);
        IzendaSynergy.setCurrentUserContext(currentUserContext);
        IzendaSynergy.render(document.getElementById('izenda-root'));

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

    var tokenFromStorage = localStorage.getItem('token');
    var currentUserContext = {
        token: tokenFromStorage
    };

    console.log(currentUserContext);
    IzendaSynergy.setCurrentUserContext(currentUserContext);
    IzendaSynergy.renderReportPage(document.getElementById('izenda-root'));
};

// Render report viewer to a <div> tag by report id
var izendaInitReportViewer = function (reportId, filters) {

    var tokenFromStorage = localStorage.getItem('token');
    var currentUserContext = {
        token: tokenFromStorage
    };

    console.log(currentUserContext);
    IzendaSynergy.setCurrentUserContext(currentUserContext);
    IzendaSynergy.renderReportViewerPage(document.getElementById('izenda-root'), reportId, filters);
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

    var tokenFromStorage = localStorage.getItem('token');
    var currentUserContext = {
        token: tokenFromStorage
    };

    console.log(currentUserContext);
    IzendaSynergy.setCurrentUserContext(currentUserContext);
    IzendaSynergy.renderDashboardPage(document.getElementById('izenda-root'));
};

// Render dashboard viewer to a <div> tag by dashboard id
var izendaInitDashboardViewer = function (dashboardId, filters) {

    var tokenFromStorage = localStorage.getItem('token');
    var currentUserContext = {
        token: tokenFromStorage
    };

    console.log(currentUserContext);
    IzendaSynergy.setCurrentUserContext(currentUserContext);
    IzendaSynergy.renderDashboardViewerPage(document.getElementById('izenda-root'), dashboardId, filters);
};

var izendaInitReportDesigner = function () {

    var tokenFromStorage = localStorage.getItem('token');
    var currentUserContext = {
        token: tokenFromStorage
    };

    console.log(currentUserContext);
    IzendaSynergy.setCurrentUserContext(currentUserContext);
    IzendaSynergy.renderReportDesignerPage(document.getElementById('izenda-root'));
};

var izendaInitNewDashboard = function () {

    var tokenFromStorage = localStorage.getItem('token');
    var currentUserContext = {
        token: tokenFromStorage
    };

    console.log(currentUserContext);
    IzendaSynergy.setCurrentUserContext(currentUserContext);
    IzendaSynergy.renderNewDashboardPage(document.getElementById('izenda-root'));
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

var izendaInitReportPartDemo = function() {

  var tokenFromStorage = localStorage.getItem('token');
  var currentUserContext = {
      token: tokenFromStorage
  };

  // You can add report parts after creating reports using the context below
  // Add the report part ID's in the <reportPartIds> array
  var reportPartIds = new Array(
    "D2647E59-AEB8-48B9-A228-28E8CD96D622",
    "D2647E59-AEB8-48B9-A228-28E8CD96D622",
    "D2647E59-AEB8-48B9-A228-28E8CD96D622",
    "D2647E59-AEB8-48B9-A228-28E8CD96D622"
  );

  IzendaSynergy.setCurrentUserContext(currentUserContext);

  for (var i = 0; i < reportPartIds.length; i++) {
    //create report part container
    var reportPartNode = document.createElement("div");
    reportPartNode.className = "report-part-chart";

    //Render report part
    IzendaSynergy.renderReportPart(reportPartNode, {
      "id": reportPartIds[i]
    });

    //Append to izenda-root container
    document.getElementById('izenda-root').appendChild(reportPartNode);
  }
};
