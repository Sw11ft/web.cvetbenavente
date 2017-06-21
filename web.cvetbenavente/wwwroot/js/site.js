'use scrict';
/*
    TOC:
    $DOCUMENT.READY
        $NOTY
        $DADOS DE TABELAS
        $ANIMAÇÕES
        $LIMPEZA DE FORMS/INPUTS
        $AJAX
        $SELECT2
    $FUNCTIONS
        getParameterByName(name, url)
        removeParameterByName(name, url)
*/

//$DOCUMENT.READY
$(function () {

    //BOOTSTRAP POPOVER
    $('[data-toggle="popover"]').popover();
    //BOOTSTRAP TOOLTIP
    $('[data-toggle="tooltip"]').tooltip();

    /*****************************************************************/

    //$NOTY

    //nid: notification id
    //nt: notification type
    if (getParameterByName("nid") !== "" && getParameterByName("nid") !== null) {
        let id = getParameterByName("nid");
        let type = getParameterByName("nt");

        window.history.replaceState(null, null, window.location.pathname);

        let msg;

        let invalid = "$$$INVALID$$$";

        switch (id) {
            //CLIENTE
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
            //--
            //PASSWORD
            case "2":
                msg = "Password alterada com sucesso.";
                break;
            case "3":
                msg = "Ocorreu um erro ao alterar a password.";
                break;
            //--
            //ESPÉCIES
            case "4":
                msg = "Espécie criada com sucesso.";
                break;
            case "40":
                msg = "Espécie editada com sucesso.";
                break;
            case "41":
                msg = "A espécie não foi encontrada.";
                break;
            case "42":
                msg = "A espécie encontra-se inativa";
                break;
            case "43":
                msg = "Já existe um espécie com o nome introduzido";
                break;
            //--
            //ANIMAIS
            case "5":
                msg = "Animal criado com sucesso.";
                break;
            case "50":
                msg = "Animal editado com sucesso.";
                break;
            case "51":
                msg = "O animal não foi encontrado.";
                break;
            //--
            //DEFAULT
            default:
                msg = invalid;
                break;
        }

        switch (type) {
            case "a":                   //ALERT
                type = "alert";
                break;
            case "s":                   //SUCCESS
                type = "success";
                break;
            case "w":                   //WARNING
                type = "warning";
                break;
            case "e":                   //ERROR
                type = "error";
                break;
            case "i":                   //INFORMATION
                type = "information";
                break;
            default:                    //DEFAULT
                type = "information";
                break;
        }

        if (msg !== invalid) {
            new Noty({
                text: msg,
                type: type,
                layout: 'topRight',
                timeout: 2500,
                progressBar: false
            }).show();
        } else {
            console.error("ID noty não reconhecido (" & id & ")");
        }
    }

    /*****************************************************************/

    //$DADOS DE TABELAS
    /*
    //Clientes/Index Ordering
    $("#ClientesIndexTable .ClienteColumn").click(function () {
        if (!$(this).hasClass("disabled")) {
            let order = $(this).attr("data-order");
            let text = $("#ClientesIndexSearch").val().trim();

            let queryOrder;
            if (order !== "asc") {
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

                    if (order === "asc") {
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
    let pageTitle = $("#PageTitle").html();
    if (typeof pageTitle !== "undefined") {
        pageTitle = pageTitle.trim();
    }
    $("#ClientesIndexSearch").on("input", function () {
        //mostra o botão para limpar
        if ($("#ClientesIndexSearch").val().trim() !== "") {
            $("#ClientesIndexSearchFormGroup .clear").fadeIn(100);
        } else {
            $("#ClientesIndexSearchFormGroup .clear").fadeOut(100);
        }

        IndexSearch();
    });

    $("#ClientesIndexEstado").change(function () {
        IndexSearch();
    });

    var IndexSearch = function () {
        window.clearTimeout(searchTimeout);
        searchTimeout = window.setTimeout(function () {
            let text = $("#ClientesIndexSearch").val().trim();

            let order = $("#ClientesIndexTable .ClienteColumn").attr("data-order");
            let estado = $("#ClientesIndexEstado").val();

            $.ajax({
                url: "/Clientes/IndexTableData",
                type: "get",
                data: { field: "nome", order: order, query: text, estado: estado },
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
            if (text === "") {
                $("#PageTitle").html(pageTitle);
            } else {
                $("#PageTitle").html(pageTitle + " - Pesquisa por \x22" + "<i>" + text + "</i>" + "\x22");
            }
        }, 400);
    };
    */
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
    /*
    $("#ClientesIndexSearchFormGroup .clear").click(function (event) {
        $("#ClientesIndexSearch").val("").trigger("input");
        $(this).fadeOut(100);
    });
    */
    /*****************************************************************/

    //$AJAX

    //Desativar utilizador
    $(".disable-user").click(function () {
        let id = $(this).data("id");

        if (id !== null && id !== "") {
            swal({
                title: "Desativar Cliente",
                text: "Está prestes a desativar este cliente. <br/>" +
                "Um cliente inativo não pode ser alterado, não pode ser associado a eventos," +
                "não receberá notificações de eventos e quaisquer animais que lhe estejam " +
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
                        if (data === true) {
                            swal({
                                title: "Cliente desativado com sucesso.",
                                type: "success"
                            }, function () {
                                window.location.reload(true);
                            });
                        }
                        else {
                            swal({
                                title: "Ocorreu um erro. Se isto persistir, contacte a administração."  ,
                                type: "error"
                            });
                        }
                    }
                });
            });
        }
    });

    //Ativar utilizador
    $(".enable-user").click(function () {
        let id = $(this).data("id");

        if (id !== null && id !== "") {
            swal({
                title: "Ativar Cliente",
                text: "Está prestes a ativar este cliente. <br/>" +
                "Ao ser reativado, este cliente pode ser associado a eventos e receberá as suas notificações. <br/>" +
                "Qualquer animal associado a este cliente que não tenha sido desativado individualmente será reativado.",
                type: "warning",
                html: true,
                showCancelButton: true,
                closeOnConfirm: false,
                showLoaderOnConfirm: true,
                confirmButtonText: "Ativar",
                confirmButtonColor: "#6fc080",
                cancelButtonText: "Cancelar",
                cancelButtonColor: "#e2e2e2"
            }, function () { //on confirm
                $.ajax({
                    url: "/Clientes/EnableCliente",
                    type: "post",
                    data: { Id: id },
                    success: function (data) {
                        if (data === true) {
                            swal({
                                title: "Cliente ativado com sucesso.",
                                type: "success"
                            }, function () {
                                window.location.reload(true);
                            });
                        }
                        else {
                            swal({
                                title: "Ocorreu um erro. Se isto persistir, contacte a administração.",
                                type: "error"
                            });
                        }
                    }
                });
            });
        }
    });

    //Ativar utilizador
    $(".delete-especie").click(function () {
        let id = $(this).data("id");

        if (id !== null && id !== "") {
            swal({
                title: "Apagar Espécie",
                text: "Está prestes a apagar esta espécie. <br/>" +
                "Ao apagar a espécie, não lhe poderá associar nenhum animal. <br/>" +
                "Esta ação não pode ser revertida.",
                type: "warning",
                html: true,
                showCancelButton: true,
                closeOnConfirm: false,
                showLoaderOnConfirm: true,
                confirmButtonText: "Apagar",
                confirmButtonColor: "#dd6777",
                cancelButtonText: "Cancelar",
                cancelButtonColor: "#e2e2e2"
            }, function () { //on confirm
                $.ajax({
                    url: "/Especies/ApagarEspecie",
                    type: "post",
                    data: { Id: id },
                    success: function (data) {
                        if (data === true) {
                            swal({
                                title: "Espécie apagada com sucesso.",
                                type: "success"
                            }, function () {
                                window.location.href = "/Especies";
                            });
                        }
                        else {
                            swal({
                                title: "Ocorreu um erro. Se isto persistir, contacte a administração.",
                                type: "error"
                            });
                        }
                    }
                });
            });
        }
    });

    //MENU NÚMERO DE REGISTOS
    $.ajax({
        type: "get",
        url: "/XHR/Registos",
        data: { clientes: true, animais: true, especies: false },
        error: function () {
            console.error("Ocorreu um erro ao enviar o pedido AJAX (/XHR/Registos)");
            $(".menu-loader").fadeOut("fast");
        },
        success: function (data) {
            $(".menu-loader").fadeOut("fast", function () {
                if (data["clientes"] !== null) {
                    $("#ClientesBadge").html(data["clientes"]).fadeIn(750);
                }
                if (data["animais"] !== null) {
                    $("#AnimaisBadge").html(data["animais"]).fadeIn(750);
                }
                if (data["especies"] !== null) {
                    $("#EspeciesBadge").html(data["especies"]).fadeIn(750);
                }

                if (typeof data["error"] !== "undefined") {
                    console.error(data["message"]);
                }
            });
        }
    });

    /*****************************************************************/

    //$SELECT2

    $EspecieSelect = $("#EspeciesSelect").select2({
        placeholder: "Selecione uma espécie...",
        ajax: {
            language: "pt",
            delay: 250,
            url: "/Especies/GetEspecies",
            dataType: "json",
            type: "GET",
            data: function (params) {
                var query = {
                    q: params.term, /*query*/
                    mr: 10 /*max results*/
                };
                return query;
            },
            processResults: function (data) {
                return {
                    results: data
                    //results: $.map(data, function (item) {
                    //    return {
                    //        text: item.text + " <b>" + item.nrAnimais + "</b>",
                    //        id: item.id,
                    //    }
                    //})
                };
            }
        },
        templateResult: function (data) {
            markup = "<span class='title'>" + data.text + "</span> ";
            markup += "<span class='sub'>";
            if (typeof data.nrAnimais !== "undefined" && data.nrAnimais !== 0) {
                markup += data.nrAnimais;
                if (data.nrAnimais === 1) {
                    markup += " Animal";
                } else {
                    markup += " Animais";
                }
            }
            markup += "</span>";

            return $(markup);
        }
    });

    $("#GeneroSelect").select2({
        minimumResultsForSearch: -1,
        language: "pt",
        placeholder: "Selecione um género...",
        data: [
            { id: 0, text: "Macho" },
            { id: 1, text: "Fêmea" }
        ]
    });

    $ClienteSelect = $("#ClienteSelect").select2({
        placeholder: "Selecione um cliente...",
        ajax: {
            language: "pt",
            delay: 250,
            url: "/Clientes/GetClientes",
            dataType: "json",
            type: "GET",
            data: function (params) {
                var query = {
                    q: params.term, /*query*/
                    mr: 15, /*max results*/
                    page: params.page /*página*/
                };
                return query;
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data.items, /*$.map(data, function (item) {
                        return {
                            text: item.nome,
                            id: item.id,
                        }
                    })*/
                    pagination: {
                        more: params.page * 15 < data.total_items
                    }
                };
            },
            cache: true,
            initSelection: function (element, callback) {}
        },
        templateResult: function (data) {
            markup = "<span class='title'>" + data.text + "</span> ";

            if (typeof data.contacto !== "undefined") {
                markup += "<span class='sub'>" + data.contacto + "</span>";
            }
            if (typeof data.morada !== "undefined" && typeof data.codPostal !== "undefined" && typeof data.localidade !== "undefined") {
                markup += "<br />";
                markup += "<span class='morada'>" + data.morada + ", " + data.codPostal + " " + data.localidade + "</span>";
            }
            return $(markup);
        }
    });
    if (getParameterByName("cl")) {
        $ClienteSelect.val(getParameterByName("cl")).trigger("change");
    }

    $("#EventosCriarForm #ClienteSelect").change(function () {
        $("#EventosCriarForm #AnimalSelect").val("").trigger("change");
    });
    $AnimalSelect = $("#AnimalSelect").select2({
        placeholder: "Selecione um animal...",
        ajax: {
            language: "pt",
            delay: 250,
            url: "/Animais/GetAnimais",
            dataType: "json",
            type: "GET",
            data: function (params) {
                var query = {
                    q: params.term, /*query*/
                    cl: $("#ClienteSelect").val(), /*id do cliente*/
                    showWithoutCl: false, /*mostrar dados mesmo que não haja cliente definido*/
                    mr: 15, /*max results*/
                    page: params.page /*página*/
                };
                return query;
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data.items, /*$.map(data, function (item) {
                        return {
                            text: item.nome,
                            id: item.id,
                        }
                    })*/
                    pagination: {
                        more: params.page * 15 < data.total_items
                    }
                };
            },
            cache: true,
            initSelection: function (element, callback) { }
        },
        templateResult: function (data) {
            markup = "";

            if (typeof data.espImg !== "undefined") {
                if (data.espImg === null) {
                    markup += "<div class='img-wrapper' style='background-image: url(/images/paw.jpg)'></div>";
                } else {
                    markup += "<div class='img-wrapper' style='background-image: url(/upload/img/especies/" + data.espImg + ")'></div>";
                }
            }

            markup += "<div class='text'>";
            markup += "<span class='title'>" + data.text + "</span> ";

            if (typeof data.contacto !== "undefined") {
                markup += "<span class='sub'>" + data.contacto + "</span>";
            }
            if (typeof data.espNome !== "undefined" && typeof data.genero !== "undefined") {
                markup += "<br />";
                markup += "<span class='morada'>";
                if (data.genero === 0) {
                    markup += "<i class='colored hidden-print icon-symbol-male'></i> ";
                } else if (data.genero === 1) {
                    markup += "<i class='colored hidden-print icon-symbol-female'></i> ";
                }
                if (data.genero === 0 || typeof data.espNomeF === "undefined" || data.espNomeF === null) {
                    markup += data.espNome
                } else {
                    markup += data.espNomeF
                }
                markup += "</span>";
            }
            markup += "</div>";

            return $(markup);
        }
    })

    /*****************************************************************/
}); //document.ready

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
        queryString = url.indexOf("?") !== -1 ? url.split("?")[1] : "";
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

function insertParameter(key, value) {
    key = encodeURI(key); value = encodeURI(value);

    var kvp = document.location.search.substr(1).split('&');

    var i = kvp.length; var x; while (i--) {
        x = kvp[i].split('=');

        if (x[0] === key) {
            x[1] = value;
            kvp[i] = x.join('=');
            break;
        }
    }

    if (i < 0) { kvp[kvp.length] = [key, value].join('='); }

    //this will reload the page, it's likely better to store this until finished
    document.location.search = kvp.join('&');
}