using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using CarManager.Areas.Admin.Models;


namespace CarManager.Infrastructure.Mapping
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }
        protected override void Configure()
        {
            // car diagram
            CreateMap<CarDiagram, CarDiagramItemModel>();
            CreateMap<CarDiagram, CarDiagramModel>();

            CreateMap<Car, CarModel>();
            CreateMap<Car, CarItemModel>();
        }
    }
}