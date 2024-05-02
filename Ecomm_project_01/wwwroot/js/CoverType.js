﻿var dataTable;
$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').dataTable({
        "ajax": {
            "url": "/Admin/CoverType/GetAll",
        },

        "columns": [
            { "data": "name", "width": "60%" },
            {
                "data":"id",
                "render": function (data) {
                    return `
                        <div class = "text-center">
                        <a href="/Admin/CoverType/Upsert/${data}" class= "btn btn-info">
                           <i class="fas fa-edit"></i>
                        <a class="btn btn-danger" onclick=Delete("/Admin/CoverType/Delete/${data}")>
                    <i class = "fas fa-trash-alt"></i></a></div>

                    `;
                }                
            }
        ]
    })
}

function Delete(url) {
    swal({
        title: "Want to Delete Data",
        text: "Delete Information",
        icon: "warning",
        buttons: true,
        dangerModel: true

    }).then((willdelete) => {
        if (willdelete) {
            $.ajax({
                url: url,
                type: "Delete",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}