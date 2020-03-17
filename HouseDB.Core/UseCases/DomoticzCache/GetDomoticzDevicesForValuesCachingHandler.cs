using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HouseDB.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HouseDB.Core.UseCases.DomoticzCache
{
    public class GetDomoticzDevicesForValuesCachingHandler : IRequestHandler<GetDomoticzDevicesForValuesCachingRequest, GetDomoticzDevicesForValuesCachingResponse>
    {

        public GetDomoticzDevicesForValuesCachingHandler()
        {
        }

        public async Task<GetDomoticzDevicesForValuesCachingResponse> Handle(GetDomoticzDevicesForValuesCachingRequest request, CancellationToken cancellationToken)
        {
            var validator = new GetDomoticzDevicesForValuesCachingRequestValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new MediatRValidationException(result.ToString());
            }

            throw new NotImplementedException();
        }
    }
}
