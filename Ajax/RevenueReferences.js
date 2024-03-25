function getDataFromServer() {
    $.ajax({
        url: '/RevenueReferences/getData', // Đường dẫn đến action trong controller
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // Xóa dữ liệu cũ trong bảng
            $('#dataTable tbody').empty();

            // Thêm dữ liệu mới vào bảng
            $.each(data, function (index, item) {
                var row = '<tr>' +
                    '<td>' + item.IdCourse + '</td>' +
                    '<td>' + item.Id + '</td>' +
                    '<td>' + item.Code + '</td>' +
                    '</tr>';
                $('#dataTable tbody').append(row);
            });
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
}
getDataFromServer();