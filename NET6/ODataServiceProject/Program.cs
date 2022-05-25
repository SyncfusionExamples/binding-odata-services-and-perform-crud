using Microsoft.EntityFrameworkCore;
using ODataServiceProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Microsoft.OData.Edm;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

static IEdmModel GetEdmModel()
{   
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    var books = builder.EntitySet<Orders>("Orders");
    FunctionConfiguration myFirstFunction = books.EntityType.Collection.Function("MyFirstFunction");
    myFirstFunction.ReturnsCollectionFromEntitySet<Orders>("Orders");
    return builder.GetEdmModel();
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ODataTutorial", Version = "v1" });
});
builder.Services.AddDbContext<OrdersDetailsContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("OrdersDetailsDatabase")));
builder.Services.AddControllers().AddOData(opt => opt.AddRouteComponents("odata", GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null));

var app = builder.Build();

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
