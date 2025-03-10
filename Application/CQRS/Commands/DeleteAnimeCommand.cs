﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Commands;

public class DeleteAnimeCommand(int id) : IRequest<bool>
{
    public int Id { get; set; } = id;
}