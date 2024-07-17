// Để làm việc với các lớp và phương thức giúp xây dựng ứng dụng web
using Microsoft.EntityFrameworkCore;
using api.Models;

var builder = WebApplication.CreateBuilder(args);
// Thêm các service vào container.
// Thêm service để hiển thị tài liệu API sử dụng Swagger
builder.Services.AddEndpointsApiExplorer();
// Thêm service để tạo ra và cấu hình Swagger
builder.Services.AddSwaggerGen();
//Thêm service cho builder tự lấy theo cấu trúc controller 
builder.Services.AddControllers();


//Cấu hình EF Framework
builder.Services.AddDbContext<ApplicationContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});






// Tạo ứng dụng web từ builder
var app = builder.Build();
// Cấu hình HTTP request pipeline.
// Nếu ứng dụng đang chạy trong môi trường phát triển, sử dụng Swagger để hiển thị tài liệu API
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Sử dụng HTTPS redirection để chuyển đổi tất cả các yêu cầu HTTP sang HTTPS
app.UseHttpsRedirection();
app.MapControllers();
// Chạy ứng dụng web
app.Run();


