using Microsoft.EntityFrameworkCore;
using api.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using api.Filter;
using Serilog;
using Serilog.Core;


var builder = WebApplication.CreateBuilder(args);
// Thêm các service vào container.
// Thêm service để hiển thị tài liệu API sử dụng Swagger
builder.Services.AddEndpointsApiExplorer();

// Thêm service để tạo ra và cấu hình Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    // Cấu hình cho Swagger để sử dụng JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter into field the word 'Bearer' followed by a space and the JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
    c.OperationFilter<AuthorizeCheckOperationFilter>(); // Áp dụng OperationFilter
    // Thêm custom operation filter vào Swagger
    c.OperationFilter<CustomHeaderOperationFilter>();
});

// builder.Services.AddScoped<LogResultAttribute>();


//Thêm service cho builder tự lấy theo cấu trúc controller 
builder.Services.AddControllers();

// Thêm dịch vụ Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();

// Thêm và cấu hình JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // Xác nhận key ký
        IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])), // Key dùng để ký token
        ValidateIssuer = true, // Xác nhận issuer
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Issuer hợp lệ
        ValidateAudience = true, // Xác nhận audience
        ValidAudience = builder.Configuration["Jwt:Audience"], // Audience hợp lệ
        ValidateLifetime = true, // Xác nhận thời gian sống của token
        ClockSkew = TimeSpan.Zero // Đặt độ lệch thời gian cho token
    };
});


// Thêm hỗ trợ cho Authorization
builder.Services.AddAuthorization();





//Cấu hình EF Framework
builder.Services.AddDbContext<ApplicationContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionDBEbay"));
});

//http client :  dùng để gọi các action từ api khác
builder.Services.AddHttpClient();

// Tạo ứng dụng web từ builder
var app = builder.Build();
// Cấu hình HTTP request pipeline.

// Nếu ứng dụng đang chạy trong môi trường phát triển, sử dụng Swagger để hiển thị tài liệu API
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        // c.RoutePrefix = string.Empty; // Đặt UI ở trang gốc
    });
}

// Sử dụng HTTPS redirection để chuyển đổi tất cả các yêu cầu HTTP sang HTTPS
app.UseHttpsRedirection();
app.MapControllers();



// Cấu hình middleware cho xác thực và phân quyền
app.UseAuthentication();
app.UseAuthorization();


// Chạy ứng dụng web
app.Run();

