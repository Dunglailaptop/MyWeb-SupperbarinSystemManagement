
function formatVND(number) {
    // Convert number to string
    var strNumber = number.toString();

    // Split the number into integer and decimal parts
    var parts = strNumber.split(".");

    // Format the integer part with dots
    var formattedInteger = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ".");

    // Add the currency symbol and decimal part if exists
    var result = formattedInteger + " VND";
    if (parts.length > 1) {
        result += "." + parts[1];
    }

    return result;
}
function getdatacombox(id) {
    var courseSelect = document.getElementById('courseSelect');
    // Clear current options
    courseSelect.innerHTML = '';

    // Fetch course data based on the selected program using AJAX
    $.ajax({
        url: '/Registrations/GetDataCourse',
        type: 'GET',
        data: { id: id },
        success: function (data) {
            // Populate the course select options
            data.forEach(course => {
                var option = document.createElement('option');
                option.value = course.Id;
                option.textContent = course.Name;
                courseSelect.appendChild(option);
                document.getElementById("textInput").value = course.price;
                document.getElementById("textInput2").value = formatVND(parseFloat(course.price));
            });
        },
        error: function (xhr, status, error) {
            console.error('Error fetching course data:', error);
        }
    });
}
function loaddata(idRegistraion) {
    var randomParam = new Date().getTime(); // Tạo một tham số ngẫu nhiên
    $.ajax({
        url: '/Registrations/getData',
        type: 'GET',
        data: {
            IdRegistration: idRegistraion,
            randomParam: randomParam 
        },
        success: function (data) {
            console.log(data);
            var i = 0, total = 0;
            console.log(data)
            $('#dataBody').empty();

        

                // Tạo một hàng mới cho bảng
              
                
            // Thêm hàng mới vào phần tử <tbody> của bảng
            $('#dataBody').html(data.datalist);
            document.getElementById("IdRegistration").value = data.idRegistrations;
            document.getElementById("Bill").value = data.Code;
            document.getElementById("Datecreate").value = data.DateCreate;
            $(".total").text(formatVND(data.TotalAmount));

          
        },
        error: function (xhr, status, error) {
            alert("san pham da ton tai");
            console.error('Error saving cart item:', error);
        }
    })
}
function savecartitem() {
    // Get values from form inputs
    var programId = $('#programSelect').val();
    var courseId = $('#courseSelect').val();
    var promotionId = $('#KM').val();
    var price = $('#textInput').val(); // Assuming the price input has an id of "textInput"

    var select = document.getElementById("programSelect");
    var selectedIndex = select.selectedIndex;
    var selectedName = select.options[selectedIndex].getAttribute("data-name");
    var code = document.getElementById("IdRegistration").value;
    // You can add more variables as needed

    // Make AJAX request with collected data
    $.ajax({
        url: '/Registrations/saveCookie',
        type: 'GET',
        data: {
            idProgram: programId,
            IdCourse: courseId,
            Idpromotion: promotionId,
            price: price,
            nameprogram: selectedName,
            code: code
            // Add more data variables as needed
        },
        success: function (data) {
            loaddata(data);

        },
        error: function (xhr, status, error) {
            alert("san pham da ton tai");
            console.error('Error saving cart item:', error);
        }
    });
}

function deleteitem(courseId,registrationId) {
    

    var $row = $(this).closest('tr'); // Tìm hàng chứa nút xóa đã được click

    $.ajax({
        url: '/Registrations/Deletes', // Thay thế bằng URL xóa của bạn
        type: 'POST',
        data: {
            IdCourse: courseId,
            IdRegistration: registrationId
        },
        success: function (data) {
            loaddata(data);
            console.log(data);
            alert(data);
            // Xóa hàng từ bảng sau khi xóa thành công
            $row.remove();
       
        },
        error: function (xhr, status, error) {
            console.error('Error deleting cart item:', error);
            alert(error);
        }
    });
}