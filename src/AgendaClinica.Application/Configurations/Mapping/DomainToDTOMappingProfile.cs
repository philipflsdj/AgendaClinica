using AgendaClinica.Application.Configurations.DTOs.Consultas;
using AgendaClinica.Application.Configurations.DTOs.Pacientes;
using AgendaClinica.Application.Configurations.DTOs.ProfissionaisSaude;
using AgendaClinica.Domain.Entities;
using AutoMapper;


namespace AgendaClinica.Application.Configurations.AutoMapper
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
         
            //Paciente
            CreateMap<Paciente, CriarPaciente>()
                .ForMember(dest => dest.nomeCompleto, opt => opt.MapFrom(src => src.NomeCompleto))
                .ForMember(dest => dest.documento, opt => opt.MapFrom(src => src.Documento))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.telefone, opt => opt.MapFrom(src => src.Telefone))
                .ForMember(dest => dest.criadoEm, opt => opt.MapFrom(src =>
                    src.CreatedOn.HasValue
                        ? src.CreatedOn.Value.ToString("yyyy-MM-dd HH:mm:ss")
                        : null
                ));

            CreateMap<CriarPaciente, Paciente>()
                .ConstructUsing(src => new Paciente(
                    src.nomeCompleto ?? string.Empty,
                    src.documento ?? string.Empty,
                    src.email ?? string.Empty,
                    src.telefone ?? string.Empty
                ));

            //Profissional Saude
            CreateMap<CriarProfissionalSaude, ProfissionalSaude>()
                .ConstructUsing(src => new ProfissionalSaude(
                    src.nomeCompleto ?? string.Empty,
                    src.especialidade ?? string.Empty,
                    src.numeroRegistro ?? string.Empty
                ));

            CreateMap<ProfissionalSaude, CriarProfissionalSaude>()
                .ForMember(dest => dest.nomeCompleto, opt => opt.MapFrom(src => src.NomeCompleto))
                .ForMember(dest => dest.especialidade, opt => opt.MapFrom(src => src.Especialidade))
                .ForMember(dest => dest.numeroRegistro, opt => opt.MapFrom(src => src.NumeroRegistro));

            //Consulta
            CreateMap<CriarConsultaRequest, Consulta>()
            .ConstructUsing(src => new Consulta(
                src.PacienteId,
                src.ProfissionalId,
                src.InicioEm
            ));


        }

    }
}
