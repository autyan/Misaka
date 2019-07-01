using System;
using Microsoft.Extensions.DependencyInjection;

namespace Misaka.DpendencyInjection
{
    public interface IObjectProvider
    {
        IObjectProvider CreateScope();

        IObjectProvider CreateScope(IServiceCollection serviceCollection);

        T GetService<T>(string name) where T : class;

        T GetService<T>() where T : class;

        object GetService(Type type);

        object GetService(Type type, string name);

        T GetRequiredService<T>(string name) where T : class;

        T GetRequiredService<T>() where T : class;

        object GetRequiredService(Type type);

        object GetRequiredService(Type type, string name);
    }
}
