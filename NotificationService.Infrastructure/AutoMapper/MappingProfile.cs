using AutoMapper;
using NotificationService.Core.Entities;
using NotificationService.Core.UseCases.Notification;

namespace NotificationService.Infrastructure.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Notification, NotificationUseCaseInput>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.From))
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.To))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
                .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body))
                .ForMember(dest => dest.NotificationType, opt => opt.MapFrom(src => src.NotificationType))
                .ReverseMap();
        }
    }
}
