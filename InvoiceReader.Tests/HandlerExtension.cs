using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace InvoiceReader.Tests
{
    public static class HandlerExtension
    {
        public static async Task<object> Handle<T>(this IRequestHandler<T> handler, T request, CancellationToken cancellationToken = default(CancellationToken)) where T : IRequest
        {
            var castedHandler = (IRequestHandler<T, MediatR.Unit>)handler;
            return await castedHandler.Handle(request, cancellationToken);
        }

        public static async Task<TResponse> Handle<T, TResponse>(this IRequestHandler<T, TResponse> handler, T request, CancellationToken cancellationToken = default(CancellationToken)) where T : IRequest<TResponse>
        {
            var castedHandler = (IRequestHandler<T, TResponse>)handler;
            return await castedHandler.Handle(request, cancellationToken);
        }
    }
}