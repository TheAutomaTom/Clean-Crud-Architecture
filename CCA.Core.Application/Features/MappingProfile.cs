﻿using AutoMapper;

namespace CCA.Core.Application.Features
{
  public class MappingProfile : Profile
  {
    public MapperConfiguration InitAutoMapper()
    {
      MapperConfiguration config = new MapperConfiguration(cfg =>
      {
        //cfg.AddProfile(new GetXXXsProfile());


      });

      return config;
    }
  }
}
