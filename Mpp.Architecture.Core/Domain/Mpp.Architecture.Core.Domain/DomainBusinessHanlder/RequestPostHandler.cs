namespace Mpp.Architecture.Core.Domain.DomainBusinessHanlder;

using MediatR;
using MediatR.Pipeline;
using Mpp.Architecture.Core.Domain.Persistence;
using System.Threading;
using System.Threading.Tasks;

public class RequestPostHandler<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    public RequestPostHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
        await _unitOfWork.SaveEntitiesAsync();
    }
}