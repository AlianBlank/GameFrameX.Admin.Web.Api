﻿// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using System.Linq;
using Furion.DatabaseAccessor;
using GameFrameX.Core.Enum;
using GameFrameX.Core.Service.Role.Dto;
using GameFrameX.Core.SqlSugar;
using GameFrameX.Entity.System;

namespace GameFrameX.Core.Service.Role;

/// <summary>
/// 系统角色机构服务
/// </summary>
public class SysRoleOrgService : ITransient
{
    private readonly SqlSugarRepository<SysRoleOrg> _sysRoleOrgRep;

    public SysRoleOrgService(SqlSugarRepository<SysRoleOrg> sysRoleOrgRep)
    {
        _sysRoleOrgRep = sysRoleOrgRep;
    }

    /// <summary>
    /// 授权角色机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [UnitOfWork]
    public async Task GrantRoleOrg(RoleOrgInput input)
    {
        await _sysRoleOrgRep.DeleteAsync(u => u.RoleId == input.Id);
        if (input.DataScope == (int)DataScopeEnum.Define)
        {
            var roleOrgs = input.OrgIdList.Select(u => new SysRoleOrg
            {
                RoleId = input.Id,
                OrgId = u
            }).ToList();
            await _sysRoleOrgRep.InsertRangeAsync(roleOrgs);
        }
    }

    /// <summary>
    /// 根据角色Id集合获取角色机构Id集合
    /// </summary>
    /// <param name="roleIdList"></param>
    /// <returns></returns>
    public async Task<List<long>> GetRoleOrgIdList(List<long> roleIdList)
    {
        return await _sysRoleOrgRep.AsQueryable()
            .Where(u => roleIdList.Contains(u.RoleId))
            .Select(u => u.OrgId).ToListAsync();
    }

    /// <summary>
    /// 根据机构Id集合删除角色机构
    /// </summary>
    /// <param name="orgIdList"></param>
    /// <returns></returns>
    public async Task DeleteRoleOrgByOrgIdList(List<long> orgIdList)
    {
        await _sysRoleOrgRep.DeleteAsync(u => orgIdList.Contains(u.OrgId));
    }

    /// <summary>
    /// 根据角色Id删除角色机构
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task DeleteRoleOrgByRoleId(long roleId)
    {
        await _sysRoleOrgRep.DeleteAsync(u => u.RoleId == roleId);
    }
}