﻿using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Role.GetRoles;
public readonly record struct GetRolesQuery() : IRequest<Result>;
