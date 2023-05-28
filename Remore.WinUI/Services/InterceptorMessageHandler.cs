using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remore.WinUI.Services;
public class InterceptorMessageHandler : DelegatingHandler
{
    public InterceptorMessageHandler()
    {
    }

    public event EventHandler<EventArgs> TokensDied;
    public event EventHandler<EventArgs> Disposing;
    protected override void Dispose(bool disposing)
    {
        Disposing?.Invoke(this, null);
        base.Dispose(disposing);
    }
    public InterceptorMessageHandler(HttpMessageHandler innerHandler) : base(innerHandler)
    {
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var resp = await base.SendAsync(request, cancellationToken);
        if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            TokensDied?.Invoke(this, null);
        }
        return resp;
    }
}
