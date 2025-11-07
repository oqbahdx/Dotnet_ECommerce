using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Auth;

namespace ECommerce.Application.Services;

public interface IAuthService
{
    Task<BaseResponse<string>> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<BaseResponse<string>> LoginAsync(LoginRequest request, CancellationToken ct = default);
}

