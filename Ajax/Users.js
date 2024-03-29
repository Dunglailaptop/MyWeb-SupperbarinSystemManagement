//load data List User
// Function to load data dynamically

function updateCheckboxValue(checkbox) {
    // Kiểm tra xem checkbox có được chọn hay không
    var value = checkbox.checked ? 1 : 0;

    // Gán giá trị cho checkbox
    checkbox.value = value;

    // In giá trị của checkbox ra console để kiểm tra
    console.log("Checkbox value updated to: " + value);
}
function loaddataUser() {
    $.ajax({
        type: "GET",
        url: '/Users/LoadData',
        success: function (response) {
            console.log(response);
            $("#databody").html(response);
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}
loaddataUser();
function getDataPermission(idInput) {

    $('#datatable tbody').empty();
    // AJAX request to send the ID to the controller
    $.ajax({
        type: "GET",
        url: '/Users/getDataWithIdPermission',
        data: { idUserAccount: idInput },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            // Handle success response

            console.log(response);
            // Clear existing table rows


            // Xóa các hàng cũ khỏi bảng
            $('#datatable tbody').empty();
            var table = $('#datatable tbody');
            // Khi truy cập dữ liệu thành công, thêm dữ liệu vào bảng
            response.forEach(function (role) {


                var row = $('<tr>');
                row.append('<td><i class="fa fa-barcode"></i><b>' + role.Name + '</b></td>');
                row.append('<td style="text-align: left;padding-left: -10px;"><a ><i class="fa fa-check"></i></a><input type="hidden" id="checkall1_3" value="1"></td>');
                row.append('<td style="text-align: left;padding-left: -10px;"><a  ><i class="fa fa-check"></i></a><input type="hidden" id="checkall1_3" value="1"></td>');
                row.append('<td style="text-align: left;padding-left: -10px;"><a ><i class="fa fa-check"></i></a><input type="hidden" id="checkall1_3" value="1"></td>');
                row.append('<td style="text-align: left;padding-left:  -10px;"><a ><i class="fa fa-check"></i></a><input type="hidden" id="checkall1_3" value="1"></td>');

                table.append(row);

                role.Permissions.forEach(function (permission) {
                    /*  permissionRow.append('<td><input class="IdPermission" style="display: none;" type="number" value="' + permission.id + '"></td>');*/
                    var permissionRow = $('<tr class="permission">');
                    permissionRow.append('<td>+ ' + permission.Name + '<div class="blur_color">' + permission.code + '</div><input class="IdPermission" hidden type="number" value="' + permission.id + '"></td>');



                    var isReadCheckbox = '<td><input class="IsRead" type="checkbox" onchange="updateCheckboxValue(this) "' + (permission.IsRead ? ' checked' : '') + ' value="' + (permission.IsRead ? '1' : '0') + '"></td>';
                    var isCreateCheckbox = '<td><input class="IsCreate" type="checkbox" onchange="updateCheckboxValue(this)"' + (permission.IsCreate ? ' checked' : '') + ' value="' + (permission.IsCreate ? '1' : '0') + '"></td>';
                    var isEditCheckbox = '<td><input class="IsEdit" type="checkbox" onchange="updateCheckboxValue(this) "' + (permission.IsEdit ? ' checked' : '') + ' value="' + (permission.IsEdit ? '1' : '0') + '"></td>';
                    var isDeleteCheckbox = '<td><input class="IsDelete" type="checkbox" onchange="updateCheckboxValue(this) "' + (permission.IsDelete ? ' checked' : '') + ' value="' + (permission.IsDelete ? '1' : '0') + '"></td>';

                    // Thêm các checkbox vào hàng
                    permissionRow.append(isReadCheckbox);
                    permissionRow.append(isCreateCheckbox);
                    permissionRow.append(isEditCheckbox);
                    permissionRow.append(isDeleteCheckbox);


                    table.append(permissionRow);
                });

            });

        },
        error: function (xhr, status, error) {
            // Handle error response
            console.error("Error sending ID:", status, error);
            $('#datatable tbody').empty();
        }
    });
}

$("#submitButton").click(function (e) {
    e.preventDefault(); // Prevent form submission


    // Create a new instance of the Vehicle class and populate its properties

    // Create an array to hold permissions
    var Permissions = [];

    // Populate the permissions array with data from form inputs
    $(".permission").each(function () {
        var permission = {
            IdPermission: $(this).find(".IdPermission").val(),
            IsRead: $(this).find(".IsRead").val() == 1,
            IsCreate: $(this).find(".IsCreate").val() == 1,
            IsEdit: $(this).find(".IsEdit").val() == 1,
            IsDelete: $(this).find(".IsDelete").val() == 1
        };
        Permissions.push(permission);
    });



    // AJAX request to submit the data
    $.ajax({
        type: "POST",
        url: '/Users/SaveChange', // Replace with your controller's action method URL
        data: JSON.stringify(Permissions),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            // Handle the response from the controller if needed
            console.log("Data submitted successfully to the controller.");
            console.log("Response from the controller:", response);

        },
        error: function (xhr, status, error) {
            // Handle errors here
            console.error("Error submitting data to the controller:", status, error);
        }
    });

});

function ChangeStatus(button) {

    var id = parseInt(button.getAttribute("data-id")); // Chuyển đổi id thành một số nguyên
    var status = button.value; // Sử dụng button.value thay vì lấy từ tất cả các button

    $.ajax({
        type: "POST",
        url: '/Users/updateStatus', // Thay thế bằng URL của action trong controller của bạn
        data: JSON.stringify({ id: id, status: status }), // Chuyển đối dữ liệu thành JSON chuẩn
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            // Xử lý phản hồi từ controller nếu cần
            //// Thay đổi nội dung của button
            //if (response) {
            //    button.textContent = "ĐANG HOẠT ĐỘNG";
            //    button.classList.replace("bg-danger", "bg-success");
            //    button.setAttribute("value", "0"); // Thay đổi giá trị thuộc tính value
            //} else {

            //    button.textContent = "KHÔNG HOẠT ĐỘNG";
            //    button.classList.replace("bg-success", "bg-danger");
            //    button.setAttribute("value", "1"); // Thay đổi giá trị thuộc tính value
            //}


            loaddataUser();

        },
        error: function (xhr, status, error) {
            // Xử lý lỗi ở đây
            console.error("Lỗi khi gửi dữ liệu đến controller:", status, error);
        }
    });
}
$('#searchtxt').on('input', function () {
    // Perform AJAX call when the value of the input field changes
    $.ajax({
        url: '/Users/search',
        type: 'GET',
        data: {
            keyword: $(this).val() // Get the current value of the input field
        },
        dataType: 'json',
        success: function (data) {
            // Xóa nội dung cũ của bảng
            $("#databody").empty();
            $("#databody").html(data);
            if (data == null) {
                loaddataUser();
            }
     
        },
        error: function () {
            console.log('Error occurred while fetching data.');
        }
    });

});
//function get data of combobox
function getDataBranch(idBranch) {
    if (idBranch != 0) {
        $.ajax({
            type: "GET",
            url: "", // Provide the appropriate URL here
            data: { idBranch: idBranch }, // Corrected the assignment operator
            success: function (result) {
                // Handle the server response
                console.log(result);
            },
            error: function (xhr, status, error) {
                // Handle errors
                console.error(error);
            }
        });
    } else {
        $.ajax({
            type: "GET",
            url: "", // Provide the appropriate URL here
            success: function (result) {
                // Handle the server response
                console.log(result);
            },
            error: function (xhr, status, error) {
                // Handle errors
                console.error(error);
            }
        });
    }
}