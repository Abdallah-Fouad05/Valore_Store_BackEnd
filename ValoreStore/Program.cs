var builder = WebApplication.CreateBuilder(args);


// React Add Cors

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // «·”„«Õ ·√Ì „’œ— (›Ì «·»Ì∆… «· ‰„ÊÌ… ›ﬁÿ)
              .AllowAnyMethod()  // «·”„«Õ »Ã„Ì⁄ ÿ—ﬁ HTTP (GET, POST, etc.)
              .AllowAnyHeader(); // «·”„«Õ »Ã„Ì⁄ «·‹ Headers
    });
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
