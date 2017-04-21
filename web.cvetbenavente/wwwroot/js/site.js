'use scrict';
/*
    TOC:
    $DOCUMENT.READY
        $NOTY
        $DADOS DE TABELAS
        $ANIMAÇÕES
        $LIMPEZA DE FORMS/INPUTS
        $AJAX
    $FUNCTIONS
        getParameterByName(name, url)
*/

//$DOCUMENT.READY
$(function () {

    //BOOTSTRAP POPOVER
    $('[data-toggle="popover"]').popover();

    /*****************************************************************/

    //$NOTY

    //nid: notification id
    //nt: notification type
    if (getParameterByName("nid") != "" && getParameterByName("nid") != null) {
        let id = getParameterByName("nid");
        let type = getParameterByName("nt");

        window.history.replaceState(null, null, window.location.pathname);

        let msg;

        let invalid = "$$$INVALID$$$";

        switch (id) {
            case "1":
                msg = "Cliente criado com sucesso.";
                break;
            case "10":
                msg = "Cliente editado com sucesso.";
                break;
            case "11":
                msg = "O cliente não foi encontrado.";
                break;
            case "12":
                msg = "O cliente encontra-se inativo.";
                break;
            case "2":
                msg = "Password alterada com sucesso.";
                break;
            case "3":
                msg = "Ocorreu um erro ao alterar a password.";
                break;
            default:
                msg = invalid;
                break;
        }

        switch (type) {
            case "a":
                type = "alert";
                break;
            case "s":
                type = "success";
                break;
            case "w":
                type = "warning";
                break;
            case "e":
                type = "error";
                break;
            case "i":
                type = "information";
                break;
            default:
                type = "information";
                break;
        }

        if (id != invalid) {
            new Noty({
                text: msg,
                type: type,
                layout: 'topRight',
                timeout: 2500,
                progressBar: false,
            }).show();
        }
    }

    /*****************************************************************/

    //$DADOS DE TABELAS

    //Clientes/Index Ordering
    $("#ClientesIndexTable .ClienteColumn").click(function () {
        if (!$(this).hasClass("disabled")) {
            let order = $(this).attr("data-order");
            let text = $("#ClientesIndexSearch").val().trim();

            let queryOrder;
            if (order != "asc") {
                queryOrder = "asc";
            } else {
                queryOrder = "desc";
            }

            $.ajax({
                url: "/Clientes/IndexTableData?field=nome&order=" + queryOrder + "&query=" + text,
                type: "get",
                beforeSend: function () {
                    $("#ClientesIndexTable tbody").addClass("loading");
                    $("#ClientesIndexTable .ClienteColumn").addClass("disabled");
                },
                success: function (data) {
                    $("#ClientesIndexTable tbody").html(data);

                    if (order == "asc") {
                        $("#ClientesIndexTable .ClienteColumn").attr("data-order", "desc");
                    } else {
                        $("#ClientesIndexTable .ClienteColumn").attr("data-order", "asc");
                    }

                    $("#ClientesIndexTable tbody").removeClass("loading");
                    $("#ClientesIndexTable .ClienteColumn").removeClass("disabled");

                    if ($("#ClientesIndexTable .ClienteColumn i").hasClass("fa-caret-up")) {
                        $("#ClientesIndexTable .ClienteColumn i").removeClass("fa-caret-up");
                        $("#ClientesIndexTable .ClienteColumn i").addClass("fa-caret-down");
                    } else {
                        $("#ClientesIndexTable .ClienteColumn i").removeClass("fa-caret-down");
                        $("#ClientesIndexTable .ClienteColumn i").addClass("fa-caret-up");
                    }
                }
            });
        }
    });

    //Clientes/Index Procura
    let searchTimeout;
    $("#ClientesIndexSearch").on("input", function () {
        //mostra o botão para limpar
        if ($("#ClientesIndexSearch").val().trim() != "") {
            $("#ClientesIndexSearchFormGroup .clear").fadeIn(100);
        } else {
            $("#ClientesIndexSearchFormGroup .clear").fadeOut(100);
        }

        window.clearTimeout(searchTimeout);
        searchTimeout = window.setTimeout(function () {
            let text = $("#ClientesIndexSearch").val().trim();

            let order = $("#ClientesIndexTable .ClienteColumn").attr("data-order");

            $.ajax({
                url: "/Clientes/IndexTableData?field=nome&order=" + order + "&query=" + text,
                type: "get",
                beforeSend: function () {
                    $("#ClientesIndexTable tbody").addClass("loading");
                    $("#ClientesIndexTable .ClienteColumn").addClass("disabled");
                },
                success: function (data) {
                    $("#ClientesIndexTable tbody").html(data);

                    $("#ClientesIndexTable tbody").removeClass("loading");
                    $("#ClientesIndexTable .ClienteColumn").removeClass("disabled");
                }
            });
        }, 400);
    });

    /*****************************************************************/

    //$ANIMAÇÕES

    //Texto Escondido
    $(".hidden-text-link").hover(function () { //handler in
        $(this).find(".text").stop().fadeIn(200);
    }, function () { //handler out
        $(this).find(".text").stop().fadeOut(200);
    });

    /*****************************************************************/

    //$LIMPEZA DE FORMS/INPUTS

    //Clientes/Index Procura
    $("#ClientesIndexSearchFormGroup .clear").click(function (event) {
        $("#ClientesIndexSearch").val("").trigger("input");
        $(this).fadeOut(100);
    });

    /*****************************************************************/

    //$AJAX

    //Desativar utilizador
    $(".disable-user").click(function () {
        let id = $(this).data("id");

        if (id != null && id != "") {
            swal({
                title: "Desativar Cliente",
                text: "Está prestes a desativar este cliente. <br/>" +
                      "Um cliente inativo não pode ser alterado, não pode ser associado a eventos," +
                      "não receberá notificações de eventos e quaisquer animais que lhe estejam" +
                      "associados estão igualmente inativos. É possivel reativar um cliente.",
                type: "warning",
                html: true,
                showCancelButton: true,
                closeOnConfirm: false,
                showLoaderOnConfirm: true,
                confirmButtonText: "Desativar",
                confirmButtonColor: "#dd6777",
                cancelButtonText: "Cancelar",
                cancelButtonColor: "#e2e2e2"
            }, function () { //on confirm
                $.ajax({
                    url: "/Clientes/DisableCliente",
                    type: "post",
                    data: { Id: id },
                    success: function (data) {
                        if (data == true) {
                            swal({
                                title: "Cliente desativado com sucesso.",
                                type: "success"
                            }, function () {
                                window.location.reload(true);
                            });
                        }
                        else {
                            swal({
                                title: "Ocorreu um erro",
                                type: "error"
                            });
                        }
                    }
                })
            });
        }
    })

    //Desativar utilizador
    $(".disable-user").click(function () {
        let id = $(this).data("id");

        if (id != null && id != "") {
            swal({
                title: "Ativar Cliente",
                text: "Está prestes a ativar este cliente. <br/>" +
                "Ao ser reativado, este cliente pode ser associado a eventos e receberá as suas notificações. <br/>" +

                "Um cliente inativo não pode ser alterado, não pode ser associado a eventos," +
                "não receberá notificações de eventos e quaisquer animais que lhe estejam" +
                "associados estão igualmente inativos. É possivel reativar um cliente.",
                type: "warning",
                html: true,
                showCancelButton: true,
                closeOnConfirm: false,
                showLoaderOnConfirm: true,
                confirmButtonText: "Desativar",
                confirmButtonColor: "#dd6777",
                cancelButtonText: "Cancelar",
                cancelButtonColor: "#e2e2e2"
            }, function () { //on confirm
                $.ajax({
                    url: "/Clientes/DisableCliente",
                    type: "post",
                    data: { Id: id },
                    success: function (data) {
                        if (data == true) {
                            swal({
                                title: "Cliente desativado com sucesso.",
                                type: "success"
                            }, function () {
                                window.location.reload(true);
                            });
                        }
                        else {
                            swal({
                                title: "Ocorreu um erro",
                                type: "error"
                            });
                        }
                    }
                })
            });
        }
    })

    /*****************************************************************/
}) //document.ready


//$FUNCTIONS
function getParameterByName(name, url) {
    /*
     * Esta função retorna o valor do
     * url parameter especificado no 'name',
     * no url especificado.
     * Caso o url não seja especificado,
     * é utilizado o atual.
     */

    if (!url) url = window.location.href;

    name = name.replace(/[\[\]]/g, "\\$&");

    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;

    if (!results[2]) return '';

    return decodeURIComponent(results[2].replace(/\+/g, " "));
}
function removeParameterByName(name, url) {

    if (!url) url = window.location.href;

    var rtn = url.split("?")[0],
        param,
        params_arr = [],
        queryString = (url.indexOf("?") !== -1) ? url.split("?")[1] : "";
    if (queryString !== "") {
        params_arr = queryString.split("&");
        for (var i = params_arr.length - 1; i >= 0; i -= 1) {
            param = params_arr[i].split("=")[0];
            if (param === name) {
                params_arr.splice(i, 1);
            }
        }
        rtn = rtn + "?" + params_arr.join("&");
    }
    return rtn;
}