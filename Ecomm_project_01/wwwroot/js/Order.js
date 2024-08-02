var dataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tbldata').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAll"
        },
        "columns": [
            { "data": "id", "width": "15%", "class": "text-center" },
            { "data": "applicationUser.name", "width": "15%" },
            { "data": "orderDate", "width": "15%" },
            { "data": "orderStatus", "width": "15%" },
            { "data": "orderTotal", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                          <a class="btn btn-info" href="/Admin/Order/Details/${data}">
                              ViewDetails</a>
                        </div>
                    `;

                }
            }
        ]
    })
}