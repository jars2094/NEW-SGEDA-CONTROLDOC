using ControlDoc.Components.Components.DropDown;
using ControlDoc.Components.Components.Modals;
using ControlDoc.ComponentViews.ComponentViews.Menu;
using ControlDoc.FrondEnd;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
// builder.Services.AddSingleton(new CallService(builder.Configuration.GetValue<string>("ServiceConfiguration:UrlApiGateway")));
builder.Services.AddScoped<ISessionStorage, SessionStorageService>();
builder.Services.AddScoped<ILocalStorage, LocalStorageService>();
builder.Services.AddScoped<DropDownLanguageComponent>();
builder.Services.AddSingleton<EventAggregatorService>();

builder.Services.AddScoped<ModalComponent>();
builder.Services.AddScoped<DropDownWithSearchComponent>();
builder.Services.AddScoped<EventAggregatorService>();
builder.Services.AddScoped<AuthenticationStateContainer>();
builder.Services.AddScoped<NavMenu>();

//Manejo de los call service
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("ServiceConfiguration:UrlApiGateway")) });
builder.Services.AddScoped<CallService>();
builder.Services.AddScoped<ICallService, CallService>(provider => provider.GetRequiredService<CallService>());
//

//Manejo del token de seguridad
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationJWTService>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationJWTService>(provider => provider.GetRequiredService<AuthenticationJWTService>());
builder.Services.AddScoped<IAuthenticationJWT, AuthenticationJWTService>(provider => provider.GetRequiredService<AuthenticationJWTService>());
builder.Services.AddScoped<RenewTokenService>();
//

builder.Services.AddSweetAlert2();
builder.Services.AddSpeechSynthesis();
builder.Services.AddTelerikBlazor();
builder.Logging.AddFilter("Microsoft.AspNetCore.Authorization.*", LogLevel.None);

await builder.Build().RunAsync();