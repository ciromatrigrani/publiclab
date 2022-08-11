using gRPCApiClient.GrpcGateway;
using gRPCDiscountAPI.Protos;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine(">>>>>>>>>>" + builder.Configuration["Grpc:GrpcServer"]);
builder.Services.AddAutoMapper(mapper => mapper.AddMaps(typeof(DiscountProfile).Assembly));
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt => opt.Address = new Uri("http://localhost:5003")); 
builder.Services.AddScoped<DiscountGrpcGateway>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

