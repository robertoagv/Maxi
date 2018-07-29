$(document).ready(function () {
    getInformationTrancing();
    configDT();
});

var btnDetailTracing = $('#tb-categoria .btn-detail-categoria');
btnDetailTracing.attr('value');
var btnEditTracing = $('#tb-categoria .btn-edit-categoria');
btnDetailTracing.attr('value');

var configColumsDef = [
    {
        targets: 0,
        visible: false,
        searchable: false,
        orderable: false
    },
    {
        targets: 2,
        searchable: false,
        orderable: false,
    },
];
var configColumnsData = [
    { data: 'CategoryId' },
    { data: 'Descripcion' },
    { data: null,
       render: function (data, type, row, meta) {
         return '<button class="btn btn-info btn-xs btn-detail-categoria" value="' + data.CategoryId + '"><span class="glyphicon glyphicon-eye-open"></span></button>' +
                '<button class="btn btn-success btn-xs btn-edit-categoria" value="' + data.CategoryId + '"><span class="glyphicon glyphicon-pencil"></span></button>';
        }
    },


];


function getInformationTrancing() {
    $.ajax({
        type: 'get',
        datatype: 'json',
        data: {
            //username: cliente.val()
        },
        url: '/categories/getCategory'
    })
    .done(function (data) {
        console.log(data);
        fillTable('#tb-categoria', data.data, configColumsDef, configColumnsData);
        configDT();
    })
    .fail(function (data) {
        console.log(data);
    });
}

$('#tb-categoria').on('click', '.btn-detail-categoria', function () {
    console.log($('#tb-categoria .btn-detail-categoria').val());
    alert($('#tb-categoria .btn-detail-categoria').val());
});

$('#tb-categoria').on('click', '.btn-edit-categoria', function () {
    console.log($('#tb-categoria .btn-edit-categoria').val());
    alert($('#tb-categoria .btn-edit-categoria').val());
});