namespace Api.Mappers;

public class ReportMappingProfile : Profile
{
    public ReportMappingProfile()
    {
        CreateMap<Report, Models.Report>();
        CreateMap<Models.Report, Report>();
    }
}