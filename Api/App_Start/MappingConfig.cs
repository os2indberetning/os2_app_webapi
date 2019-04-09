using AutoMapper;
using Core.DomainModel;
using Api.Models;

namespace Api.App_Start
{
    public static class MappingConfig
    {
        public static void Start()
        {

            Mapper.CreateMap<EmploymentViewModel, Employment>().ReverseMap();
            Mapper.CreateMap<RateViewModel, Rate>().ReverseMap();

            Mapper.CreateMap<DriveReportViewModel, DriveReport>();
            Mapper.CreateMap<RouteViewModel, Route>();
            Mapper.CreateMap<GPSCoordinateModel, GPSCoordinate>();

            Mapper.CreateMap<Core.DomainModel.Profile, ProfileViewModel>();

            Mapper.CreateMap<OrgUnit, OrgUnitViewModel>();
        }
    }
}