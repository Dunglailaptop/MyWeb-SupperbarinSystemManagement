load_data();
function load_data() {
    var url = $("#databody").data("url"); 
    $.ajax({
        url:url,
        type: 'GET',
        data: {

        },
        success: function (data) {
            console.log(data);
            // Xóa nội dung cũ của bảng
            $("#databody").empty();

            // Thêm dữ liệu mới vào bảng
            $.each(data, function (index, student) {
                var row = "<tr>"
                    + "<td>" + student.Code + "</td>"
                    + "<td>" + student.Name + "</td>"
                    + "<td>" + student.Email + "</td>"
                    + "<td>" + student.Phone + "</td>"
                    + "<td>" + generateListInline(student.Id) + "</td>"
                    + "</tr>";

                $("#databody").append(row);
            });

        },
        error: function (xhr, status, error) {
            console.error('Error saving cart item:', error);
        }
    });
}
function create() {
    var url = $("#datacreate").data("url");

    var item = {
        Name: $("#Name").val(),
        Code: $("#Code").val(),
        Email: $("#Email").val(),
        Phone: $("#Phone").val()
    };

    $.ajax({
        url: url,
        type: 'POST', // Change to POST method if you're sending data to server
        data: JSON.stringify(item), // Convert to JSON string
        contentType: 'application/json', // Set content type
        success: function (data) {
            // Assuming you want to do something after successfully saving data
            load_data();
        },
        error: function () {
            console.log("error data");
        }
    });
}


$('#searchtxt').on('input', function () {
    // Perform AJAX call when the value of the input field changes
    $.ajax({
        url: '/Students/Search',
        type: 'GET',
        data: {
            keyword: $(this).val() // Get the current value of the input field
        },
        dataType: 'json',
        success: function (data) {
            // Xóa nội dung cũ của bảng
            $("#databody").empty();

            // Thêm dữ liệu mới vào bảng
            $.each(data, function (index, student) {
                var row = "<tr>"
                    + "<td>" + student.Code + "</td>"
                    + "<td>" + student.Name + "</td>"
                    + "<td>" + student.Email + "</td>"
                    + "<td>" + student.Phone + "</td>"
                    + "<td>" + generateListInline(student.Id) + "</td>"
                    + "</tr>";

                $("#databody").append(row);
            });
        },
        error: function () {
            console.log('Error occurred while fetching data.');
        }
    });

});
function generateListInline(id) {
    return `
        <ul class="list-inline mb-0" style="width:200px;">
            <li class="list-inline-item">
                <a href="javascript:void(0);" data-bs-toggle="tooltip" data-bs-placement="top" title="Edit" class="px-2 text-primary">
                    <i class="bx bx-pencil font-size-18"></i>
                </a>
            </li>
            <li class="list-inline-item">
                <a href="javascript:void(0);" data-bs-toggle="tooltip" data-bs-placement="top" title="Delete" class="px-2 text-danger">
                    <i class="bx bx-trash-alt font-size-18"></i>
                </a>
            </li>
            <li class="list-inline-item dropdown">
                <a class="text-muted dropdown-toggle font-size-18 px-2" href="#" id="drop" data-bs-toggle="dropdown" aria-expanded="false">
                    <i class="bx bx-dots-vertical-rounded"></i>
                </a>
                <div class="dropdown-menu dropdown-menu-end" aria-labelledby="drop2" data-bs-popper="static">
                    <a class="dropdown-item" href="Registrations/Index/${id}">THỜI KHÓA BIỂU</a>
                </div>
            </li>
        </ul>
    `;
}


