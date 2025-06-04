using System;

 var builder = WebApplication.CreateBuilder(args);
 var app = builder.Build();

 // unidade: "celsius", "kelvin" ou "fahrenheit"
 app.MapGet("/temperatura/{unidade}", (string unidade) =>
 {
 	// 1) obter hora atual em horas totais (ex.: 14.5 = 14h30m)
 	double t = DateTime.Now.TimeOfDay.TotalHours;

 	// 2) calcular temperatura em Celsius sem ruído
 	double tempCBase = 25.0 + 5.0 * Math.Sin((2.0 * Math.PI / 24.0) * t);

 	// 3) adicionar ruído uniforme [0,1)
 	double ruido = Random.Shared.NextDouble();
 	double tempC = tempCBase + ruido;

 	// 4) converter para a unidade pedida
 	double resultado;
 	string uni = unidade.ToLower();
 	if (uni == "kelvin")
 	{
     		resultado = tempC + 273.15;
 	}
 	else if (uni == "fahrenheit")
 	{
     		resultado = tempC * 9.0 / 5.0 + 32.0;
 	}
 	else if (uni == "celsius")
 	{
     		resultado = tempC;
 	}
 	else
 	{
     		return Results.BadRequest(new { erro = "Unidade inválida. Use celsius, kelvin ou fahrenheit." });
 	}

 	// 5) retornar JSON simples: { "unidade": "...", "valor": <número> }
 	return Results.Ok(new
 	{
     	unidade = uni,
     	valor = Math.Round(resultado, 2)
 	});
 });

 app.Run();