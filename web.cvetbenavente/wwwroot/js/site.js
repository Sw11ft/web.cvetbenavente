/*
    TOC:
    $DOCUMENT.READY
        $NOTY
        $DADOS DE TABELAS
        $ANIMAÇÕES
        $LIMPEZA DE FORMS/INPUTS
    $FUNCTIONS
        getParameterByName(name, url)
*/

//$DOCUMENT.READY
$(function () {
    'use scrict';

    /*****************************************************************/

    //$NOTY

    //Sucesso ao criar um cliente
    if (getParameterByName("clSuccess") == "True") {
        new Noty({
            text: 'Cliente criado com sucesso!',
            type: 'success',
            layout: 'topRight',
            timeout: 2500,
            progressBar: false,
        }).show();
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

    //Clientes/Index Novo Cliente Link
    $("#ClientesIndexCriar").hover(function () { //handler in
        $(this).find(".text").stop().fadeIn(200);
    }, function () { //handler out
        $(this).find(".text").stop().fadeOut(200);
        });

    //Clientes/Criar Voltar Atrás Link
    $("#ClientesCriarVoltar").hover(function () { //handler in
        $(this).find(".text").stop().fadeIn(200);
    }, function () { //handler out
        $(this).find(".text").stop().fadeOut(200);
    });

    /*****************************************************************/

    //$LIMPEZA DE FORMS/INPUTS

    //Clientes/Index Procura
    $("#ClientesIndexSearchFormGroup .clear").click(function (event) {
        $("#ClientesIndexSearch").val("").trigger("keydown");
        $(this).fadeOut(100);
    });

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