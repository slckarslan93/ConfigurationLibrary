using System.Linq.Expressions;
using System.Net;
using ConfigurationLibrary.Data;
using ConfigurationLibrary.Models;
using ConfigurationLibrary.UI.Models.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationLibrary.UI.Services.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly ConfigurationDbContext _configurationDbContext;

        public ConfigurationService(ConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;
        }

        public async Task<ServiceResponse<ConfigurationPaginationModel>> GetPaginationConfigurationAsync(ConfigurationFilterModel filter)
        {
            try
            {
                var query = _configurationDbContext.ConfigurationSettings
                            .AsQueryable();

                #region Filter

                if (!string.IsNullOrEmpty(filter.Name))
                {
                    query = query.Where(u => u.Name.ToLower().Contains(filter.Name.ToLower()));
                }

                if (!string.IsNullOrEmpty(filter.ApplicationName))
                {
                    query = query.Where(u => u.ApplicationName.ToLower().Contains(filter.ApplicationName.ToLower()));
                }

                #endregion Filter

                #region Pagination

                var totalRecords = await query.CountAsync();

                if (!string.IsNullOrEmpty(filter.OrderBy))
                {
                    query = ApplySorting(query, filter.OrderBy, filter.IsDesc);
                }

                var settings = await query
                    .Skip((filter.Page - 1) * filter.Size)
                    .Take(filter.Size)
                    .ToListAsync();

                var settingDtos = settings.Select(s => new ConfigurationModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Type = s.Type,
                    Value = s.Value,
                    IsActive = s.IsActive,
                    ApplicationName = s.ApplicationName,
                    CreatedDate = s.CreatedDate,
                }).ToList();

                var paginationDto = new ConfigurationPaginationModel
                {
                    Page = filter.Page,
                    Size = filter.Size,
                    TotalPages = (int)Math.Ceiling(totalRecords / (double)filter.Size),
                    RecordsFiltered = settingDtos.Count,
                    RecordsTotal = totalRecords,
                    Records = settingDtos
                };

                return new ServiceResponse<ConfigurationPaginationModel>
                {
                    IsSuccess = true,
                    Data = paginationDto
                };

                #endregion Pagination
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ConfigurationPaginationModel>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<ServiceResponse> UpdateConfigurationAsync(ConfigurationModel model)
        {
            try
            {
                var config = await _configurationDbContext.ConfigurationSettings.FindAsync(model.Id);
                if (config == null)
                {
                    return new ServiceResponse
                    {
                        IsSuccess = false,
                        Message = "Configuration not found.",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                config.Name = model.Name;
                config.Type = model.Type;
                config.Value = model.Value;
                config.IsActive = model.IsActive;
                config.ApplicationName = model.ApplicationName;

                _configurationDbContext.ConfigurationSettings.Update(config);
                await _configurationDbContext.SaveChangesAsync();

                return new ServiceResponse
                {
                    IsSuccess = true,
                    Message = "Configuration updated successfully.",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<ServiceResponse> ToggleActiveStatusAsync(int id)
        {
            try
            {
                var config = await _configurationDbContext.ConfigurationSettings.FindAsync(id);
                if (config == null)
                {
                    return new ServiceResponse
                    {
                        IsSuccess = false,
                        Message = "Configuration not found.",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                config.IsActive = !config.IsActive;
                _configurationDbContext.ConfigurationSettings.Update(config);
                await _configurationDbContext.SaveChangesAsync();

                return new ServiceResponse
                {
                    IsSuccess = true,
                    Message = "Configuration status updated successfully.",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<ServiceResponse> DeleteConfigurationAsync(int id)
        {
            try
            {
                var config = await _configurationDbContext.ConfigurationSettings.FindAsync(id);
                if (config == null)
                {
                    return new ServiceResponse
                    {
                        IsSuccess = false,
                        Message = "Configuration not found.",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                _configurationDbContext.ConfigurationSettings.Remove(config);
                await _configurationDbContext.SaveChangesAsync();

                return new ServiceResponse
                {
                    IsSuccess = true,
                    Message = "Configuration deleted successfully.",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<ServiceResponse> AddConfigurationAsync(ConfigurationAddModel model)
        {
            try
            {
                var config = new ConfigurationSetting
                {
                    Name = model.Name,
                    Type = model.Type,
                    Value = model.Value,
                    IsActive = model.IsActive,
                    ApplicationName = model.ApplicationName,
                    CreatedDate = DateTime.UtcNow
                };

                _configurationDbContext.ConfigurationSettings.Add(config);
                await _configurationDbContext.SaveChangesAsync();

                return new ServiceResponse
                {
                    IsSuccess = true,
                    Message = "Configuration added successfully.",
                    StatusCode = HttpStatusCode.Created
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        private IQueryable<ConfigurationSetting> ApplySorting(IQueryable<ConfigurationSetting> query, string orderBy, bool isDesc)
        {
            var propertyInfo = typeof(ConfigurationSetting).GetProperty(orderBy);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"'{orderBy}' adlı özellik bulunamadı.");
            }

            var parameter = Expression.Parameter(typeof(ConfigurationSetting), "u");
            var property = Expression.Property(parameter, propertyInfo);
            var orderByExpression = Expression.Lambda(property, parameter);

            var methodName = isDesc ? "OrderByDescending" : "OrderBy";

            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(ConfigurationSetting), propertyInfo.PropertyType },
                query.Expression,
                orderByExpression);

            return query.Provider.CreateQuery<ConfigurationSetting>(resultExpression);
        }
    }
}