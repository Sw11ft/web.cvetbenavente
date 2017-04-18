$(function () { //document.ready
    'use scrict';

    /*****************************************************************/

    //NOTY

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

    //DADOS DE TABELAS

    //Clientes/Index Ordering
    $("#ClientesIndexTable .ClienteColumn").click(function () {
        if (!$(this).hasClass("disabled")) {
            let order = $(this).attr("data-order");

            let queryOrder;

            if (order != "asc") {
                queryOrder = "asc";
            } else {
                queryOrder = "desc";
            }

            $.ajax({
                url: "/Clientes/IndexTableData?field=nome&order=" + queryOrder,
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
    $("#ClientesIndexSearch").keypress(function () {
        let text = $(this).val().trim();

        let order = $("#ClientesIndexTable .ClienteColumn").attr("data-order");

        $.ajax({
            url: "/Clientes/IndexTableData?field=nome&order=" + queryOrder + "&q=" + text,
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
    });

    /*****************************************************************/
}) //document.ready

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