using Dommain.Entities;
using Model;

namespace API.Mappers
{
    public class ExerciseMapper : AutoMapper.Profile
    {
        public ExerciseMapper()
        {
            CreateMap<Exercise, ExerciseEntity>();
            CreateMap<ExerciseEntity, Exercise>();
        }
    }
}
