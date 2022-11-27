namespace Api.Mappers;

public class NoteMappingProfile : Profile
{
    public NoteMappingProfile()
    {
        CreateMap<Note, Models.Note>();
        CreateMap<Models.Note, Note>();
    }
}