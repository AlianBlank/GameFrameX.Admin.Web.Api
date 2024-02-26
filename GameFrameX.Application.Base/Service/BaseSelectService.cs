// 麻省理工学院许可证
// 
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
// 
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
// 
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using GameFrameX.Core.BaseInputOutput;
using GameFrameX.Core.SqlSugar;
using GameFrameX.Entity;
using Microsoft.AspNetCore.Mvc;

namespace GameFrameX.Application.Base.Service;

/// <summary>
/// 选择器基础服务
/// </summary>
/// <typeparam name="T"></typeparam>
[ApiDescriptionSettings(Order = 100)]
public class BaseSelectService<T> : BaseService<T> where T : EntityBase, ISelectFilter, new()
{
    public BaseSelectService(SqlSugarRepository<T> repository) : base(repository)
    {
    }

    /// <summary>
    /// 获取选择器列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Select")]
    public async Task<List<BaseSelectPageOutput>> Select(BaseSelectPageInput input)
    {
        var query = await Repository.AsQueryable()
                .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                    u.Name.Contains(input.SearchKey.Trim())
                ).ToListAsync()
            ;
        var list = new List<BaseSelectPageOutput>(query.Count);
        list.AddRange(query.Select(ts => new BaseSelectPageOutput { Id = ts.Id, Value = ts.Name, Label = ts.Description }));
        return list;
    }
}