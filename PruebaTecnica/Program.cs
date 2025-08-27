    using FluentValidation;
    using FluentValidation.AspNetCore;
    using Microsoft.EntityFrameworkCore;
    using PruebaTecnica.Data;
    using PruebaTecnica.Services;


    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<ICardService, CardService>();
    builder.Services.AddControllers()
    .AddFluentValidation(fv =>{
    fv.RegisterValidatorsFromAssemblyContaining<CardValidator>();
    });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

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
