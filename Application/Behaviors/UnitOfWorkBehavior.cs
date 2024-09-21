using Application.Abstractions;
using MediatR;
using System.Transactions;

namespace Application.Behaviors;
internal sealed class UnitOfWorkBehavior<TRequest, TResponse>
    (IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (IsNotCommand())
        {
            return await next();
        }

        using TransactionScope transactionScope = new(TransactionScopeAsyncFlowOption.Enabled);
        TResponse? response = await next();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        transactionScope.Complete();

        return response;
    }

    private static bool IsNotCommand()
    {
        return !typeof(TRequest).Name.EndsWith("Command");
    }
}

