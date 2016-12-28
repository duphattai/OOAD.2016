using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using CarManager.Areas.Admin.Models;

namespace CarManager.Infrastructure.Mapping
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            CreateMap<CarDiagramModel, CarDiagram>();
            CreateMap<CarModel, Car>();
        }
    }
}