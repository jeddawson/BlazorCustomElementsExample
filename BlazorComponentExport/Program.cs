﻿using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorComponents.Pages;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.RegisterCustomElement<Counter>("test-blazor-counter");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();