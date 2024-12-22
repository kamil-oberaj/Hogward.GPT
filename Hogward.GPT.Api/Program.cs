using Hogward.GPT.Api.Data;
using Hogward.GPT.Api.Services;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var openAiOptions = builder.Configuration.GetSection("Azure").Get<AzureSettings>();
ArgumentNullException.ThrowIfNull(openAiOptions, nameof(openAiOptions));

builder.Services.AddAzureOpenAIChatCompletion(
    openAiOptions.DeploymentName,
    openAiOptions.OpenAIUrl,
    openAiOptions.OpenAIKey,
    modelId: openAiOptions.OpenAIModel);

builder.Services.AddTransient(sp => new Kernel(sp));

builder.Services.AddSingleton<ChatService>(sp => new ChatService(
    new ChatHistory(),
    sp.GetRequiredService<Kernel>(),
    sp.GetRequiredService<ILogger<ChatService>>()));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();