using System.Threading;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MyWebApp;
namespace MyWebApp;
public class MyRequest
{

}
public class MyResponse
{
    public MyResponse(string text)
    {
        Text = text;
    }
    public string Text { get; set; } = String.Empty;
}

public class GetEndpoint : Endpoint<MyRequest, MyResponse>
{
    private readonly IRenderViewService _renderViewService;

    public GetEndpoint(IRenderViewService renderViewService)
    {
        _renderViewService = renderViewService;
    }

    public override void Configure()
    {
        Get("/api/test");
        AllowAnonymous();
    }

    public override async Task HandleAsync(MyRequest request, CancellationToken canc)
    {
        var model = new MyResponse("Hello World");
        await _renderViewService.RenderViewAsync("Index", model);
    }
}
