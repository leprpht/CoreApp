using System.ComponentModel.DataAnnotations;
using System.Net;
using AppCore.Dto;
using AppCore.Entities;
using AppCore.Repositories;
using AppCore.Mapper;

namespace AppCore.Services;

public class OrganizationService(IOrganizationRepository orgRepository, IHttpClientFactory httpClientFactory)
{
    private const string KrsApiUrl = "https://api-krs.ms.gov.pl/api/krs/OdpisAktualny/{krs}?rejestr=P&format=json";

    public async Task<OrganizationDto> CreateOrganizationAsync(CreateOrganizationDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Krs))
        {
            var isValidKrs = await ValidateKrsAsync(dto.Krs);
            if (!isValidKrs) throw new ValidationException("Invalid Crs number");
        }
        
        if (!string.IsNullOrEmpty(dto.Website))
        {
            var isValidWebsite = await ValidateWebsiteAsync(dto.Website);
            if (!isValidWebsite) throw new ValidationException("Website returned non-200 status");
        }

        var organization = new Organization 
        { 
            Name = dto.Name,
            Type = dto.Type,
            KRS = dto.Krs,
            Website = dto.Website 
        };
        
        await orgRepository.CreateAsync(organization);
        return organization.MapToDto();
    }

    private async Task<bool> ValidateKrsAsync(string krs)
    {
        try
        {
            var client = httpClientFactory.CreateClient();
            var response = await client.GetAsync(KrsApiUrl.Replace("{krs}", krs));
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private async Task<bool> ValidateWebsiteAsync(string url)
    {
        try
        {
            var client = httpClientFactory.CreateClient();
            var response = await client.GetAsync(url);
            return response.StatusCode == HttpStatusCode.OK;
        }
        catch
        {
            return false;
        }
    }
}