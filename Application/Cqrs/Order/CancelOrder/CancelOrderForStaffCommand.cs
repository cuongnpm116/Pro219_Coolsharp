﻿using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Order.CancelOrder;
public record CancelOrderForStaffCommand(Guid Id) : IRequest<Result>;