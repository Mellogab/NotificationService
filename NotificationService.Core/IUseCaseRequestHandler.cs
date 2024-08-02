using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NotificationService.Core.IUseCaseRequest;

namespace NotificationService.Core
{
    public interface IUseCaseRequestHandler<in TUseCaseRequest, out TUseCaseResponse> where TUseCaseRequest : IUseCaseRequest<TUseCaseResponse>
    {
        Task<bool> Handle(TUseCaseRequest message, IOutputPort<TUseCaseResponse> outputPort);
    }

    public interface IUseCaseRequestHandler<out TUseCaseResponse>
    {
        Task<bool> Handle(IOutputPort<TUseCaseResponse> outputPort);
    }
}
