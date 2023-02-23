﻿using Dss.Application.Kafka.Messages.MRM;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dss.Application.Queries
{
    public record RequestSASQuery():IRequest<SASUrlRequest>;
 

}
