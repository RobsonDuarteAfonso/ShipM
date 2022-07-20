using Microsoft.EntityFrameworkCore;
using ShipM.Data;
using ShipM.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ContextDb>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//Post User
app.MapPost("/user", async (
    ContextDb context,
    User user) =>
{
    context.Users.Add(user);
    var result = await context.SaveChangesAsync();

    return result > 0 ? Results.Created($"user/{user.Id}", user)
                      : Results.BadRequest("There was a problem saving the record.");
})
    .Produces<Ship>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("PostUser")
    .WithTags("User");



//Get All
app.MapGet("/ship", async (
    ContextDb context) => 

    await context.Ships.ToListAsync())
    .WithName("GetShip")
    .WithTags("Ship");

//Get Id
app.MapGet("/ship/{number}", async (
    int number,
    ContextDb context) =>

    await context.Ships.FindAsync(number)
        is Ship ship ? Results.Ok(ship) : Results.NotFound())

    .Produces<Ship>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("GetShipForNumber")
    .WithTags("Ship");

//Post
app.MapPost("/ship", async (
    ContextDb context,
    Ship ship) =>
    {
        context.Ships.Add(ship);
        var result = await context.SaveChangesAsync();

        return result > 0 ? Results.CreatedAtRoute("GetShipForNumber", new { number = ship.Number}, ship) 
                          : Results.BadRequest("There was a problem saving the record.");
    })
    .Produces<Ship>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("PostShip")
    .WithTags("Ship");

//Put
app.MapPut("/ship/{number}", async (
    int number,
    ContextDb context,
    Ship ship) =>
{
    var shipDatabase = await context.Ships.AsNoTracking<Ship>().FirstOrDefaultAsync(f => f.Number == number);
    if (shipDatabase == null) return Results.NotFound();

    context.Ships.Update(ship);
    var result = await context.SaveChangesAsync();

    return result > 0 ? Results.NoContent()
                      : Results.BadRequest("There was a problem updating the record.");
})
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("PutShip")
    .WithTags("Ship");

//Delete
app.MapDelete("/ship/{number}", async (
    int number,
    ContextDb context) =>
{
    var ship = await context.Ships.FindAsync(number);
    if (ship == null) return Results.NotFound();

    context.Ships.Remove(ship);
    var result = await context.SaveChangesAsync();

    return result > 0 ? Results.NoContent()
                      : Results.BadRequest("There was a problem removing the record.");
})
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("DeleteShip")
    .WithTags("Ship");

app.Run();
