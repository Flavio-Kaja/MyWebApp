namespace MyWebApp
{
    public interface IRenderViewService
    {
        Task RenderViewAsync(string viewName, object model = null);
    }
}