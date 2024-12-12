using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using DalamudBasics.Chat.Output;
using DalamudBasics.DependencyInjection;
using DalamudBasics.Interop;
using DalamudBasics.Logging;
using DeadRinger;
using ECommons;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ProjectTemplate;

public sealed class Plugin : IDalamudPlugin
{
    private IServiceProvider serviceProvider { get; init; }
    private ILogService logService { get; set; }

    public Plugin(IDalamudPluginInterface pluginInterface)
    {
        ECommonsMain.Init(pluginInterface, this);

        serviceProvider = BuildServiceProvider(pluginInterface);
        logService = serviceProvider.GetRequiredService<ILogService>();

        InitializeServices(serviceProvider);

        pluginInterface.UiBuilder.OpenConfigUi += Noop;
        pluginInterface.UiBuilder.OpenMainUi += Noop;
    }

    private void Noop() { }
    public void Dispose()
    {
        serviceProvider.GetRequiredService<HookManager>().Dispose();          
    }

    private IServiceProvider BuildServiceProvider(IDalamudPluginInterface pluginInterface)
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddAllDalamudBasicsServices<Configuration>(pluginInterface);
        serviceCollection.AddSingleton<TargetringManager>();

        return serviceCollection.BuildServiceProvider();
    }

    private void InitializeServices(IServiceProvider serviceProvider)
    {
        IFramework framework = serviceProvider.GetRequiredService<IFramework>();
        serviceProvider.GetRequiredService<IChatOutput>().InitializeAndAttachToGameLogicLoop(framework);
        serviceProvider.GetRequiredService<TargetringManager>().Attach(framework);
    }
}
