﻿// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using GameFrameX.Core.Const;
using GameFrameX.Core.Enum;
using Microsoft.AspNetCore.Http;

namespace GameFrameX.Core.Service.User;

/// <summary>
/// 当前登录用户
/// </summary>
public class UserManager : IScoped
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private long _tenantId;

    public long UserId
    {
        get => long.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.UserId)?.Value);
    }

    public long TenantId
    {
        get
        {
            var tId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.TenantId)?.Value;
            return string.IsNullOrWhiteSpace(tId) ? _tenantId : long.Parse(tId);
        }
        set => _tenantId = value;
    }

    public string Account
    {
        get => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.Account)?.Value;
    }

    public string RealName
    {
        get => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.RealName)?.Value;
    }

    public bool SuperAdmin
    {
        get => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.AccountType)?.Value == ((int)AccountTypeEnum.SuperAdmin).ToString();
    }

    public long OrgId
    {
        get
        {
            var orgId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.OrgId)?.Value;
            return string.IsNullOrWhiteSpace(orgId) ? 0 : long.Parse(orgId);
        }
    }

    public string OpenId
    {
        get => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.OpenId)?.Value;
    }

    public UserManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}