using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using System.Threading.Tasks;
namespace MyWebApp;

public class RenderViewService : IRenderViewService
{
    private readonly ICompositeViewEngine _viewEngine;
    private readonly ITempDataProvider _tempDataProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RenderViewService(ICompositeViewEngine viewEngine, ITempDataProvider tempDataProvider, IHttpContextAccessor httpContextAccessor)
    {
        _viewEngine = viewEngine;
        _tempDataProvider = tempDataProvider;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task RenderViewAsync(string viewName, object model = null)
    {
        var context = _httpContextAccessor.HttpContext;
        var actionContext = new ActionContext(context, context.GetRouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

        var viewResult = _viewEngine.FindView(actionContext, viewName, false);
        if (viewResult.View == null)
        {
            throw new FileNotFoundException("View not found");
        }

        var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
        {
            Model = model
        };

        var tempData = new TempDataDictionary(context, _tempDataProvider);

        using var output = new StringWriter();
        var viewContext = new ViewContext(
            actionContext,
            viewResult.View,
            viewData,
            tempData,
            output,
            new HtmlHelperOptions());

        await viewResult.View.RenderAsync(viewContext);

        var html = output.ToString();

        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync(html);
    }
}
