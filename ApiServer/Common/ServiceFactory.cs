namespace ApiServer.Common
{
    public static class ServiceFactory
    {
        static public IServiceProvider ServiceProvider { get; set; } = default!;

        static public T GetService<T>()
        {
            return ServiceProvider.GetRequiredService<T>();
        }
    }
}
