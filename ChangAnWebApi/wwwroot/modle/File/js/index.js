(function () {
    $(function () {
        GetAppScreenKey();
        $("#Refresh").click(function () {
            GetAlistApps();
        })

        //搜索
        $("#search").click(function () {
            let scree = $("#scree").val();
            SetAppScreenKey(scree);
        })
    });

    function SetAppScreenKey(scree) {
        $("#cool").html("");
        let ret = zlGet(`/api/App/SetAppScreenKey?scree=${scree}`, "");
        let data = ret;
        if (data.length > 0) {
            let html = $.map(data, item => {
                return `<div class="file-info-li">
                            <div class="info-name">名称：${item.app_name}</div>
                            <div class="info-name">大小：${item.package_size}</div>
                            <div class="info-name">包名：${item.package_name}</div>
                        </div>`;
            })
            $("#cool").html(html);
        }
    }

    function GetAppScreenKey() {
        let ret = zlGet(`/api/App/GetAppScreenKey`, "");
        $("#scree").val(ret);
    }

    function GetAlistApps() {
        let ret = zlPost(`/api/App/GetAListApps`, "", "");
        let data = ret;
        if (data.length > 0) {
            let html = $.map(data, item => {
                return `<div class="file-info-li">
                            <div class="info-name">名称：${item.app_name}</div>
                            <div class="info-name">大小：${item.package_size}</div>
                            <div class="info-name">包名：${item.package_name}</div>
                        </div>`;
            })
            $("#alist").html(html);
        }
    }
})();