


function createTable(idTable, configColumnsDef) {
    $(idTable).DataTable({
        destroy: true,
        language: {
            search: ""
        },
        dom: 't',
        paging: false,
        columnDefs: configColumsDef,
    });
    configDT();
}


function configDT() {
    $('.dataTables_filter input[type="search"]').
    addClass('form-control').
    css({ "font-size": "16px", "width": "250px" }).
    attr('placeholder', 'Search');
}

function fillTable(idTable, datos, configColumnsDef, configColumnsData) {
    tabla = $(idTable).DataTable({
        destroy: true,
        processing: true,
        pagingType: "full",
        language: {
            processing: "Waiting for response...",
            search: "",
            zeroRecords: "Nothing found",
            infoEmpty: "No records available"
        },
        dom: "<'row'<'col-sm-12'<'pull-left'f>>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-7'i>>" +
                "<'row'<'col-sm-12'<'pull-rigth'p>>>",
        data: datos,
        columnDefs: configColumsDef,
        columns: configColumnsData
    });
}

function createNoty(texto, tipo) {

    return new Noty(
        {
            text: "<p style=\"text-transform:uppercase;text-align:center\">" + texto + ".</p>",
            type: tipo,
            layout: 'topCenter',
            theme: 'relax',
            timeout: 2000
        }
    ).show();

}

