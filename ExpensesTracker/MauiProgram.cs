﻿using ExpensesTracker.Services;
using Microsoft.Extensions.Logging;

namespace ExpensesTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            //registering User Services
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<AuthenticationStateService>();
            builder.Services.AddSingleton<TransactionService>();
            builder.Services.AddSingleton<DebtService>();
            builder.Services.AddScoped<TransactionService>();
            builder.Services.AddScoped<DebtService>();
            builder.Services.AddSingleton<DashboardService>();

            






#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
